#!/usr/bin/env bash
# smoke.sh - start/stop/check the SoccerManager.API backend for local dev.
#
#   smoke.sh          start the API if it isn't already up, then run checks
#   smoke.sh check    run the checks only, against an already-running API
#   smoke.sh stop     stop whatever is listening on 7265/5005
#
# Works from any cwd: the repo root is derived from this script's own path.

set -uo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/../../.." && pwd)"

HTTPS_URL="https://localhost:7265"
HTTPS_PORT=7265
HTTP_PORT=5005
READY_LINE="Now listening on: $HTTPS_URL"
START_TIMEOUT=180 # seconds - first build can take a minute or two

# Overridable only for testing (e.g. pointing at a dummy env file, or a
# throwaway broken project to prove the build-failure path); normal use
# takes all three defaults.
ENV_FILE="${SOCCERMANAGER_ENV_FILE:-$REPO_ROOT/.env}"
LOG_FILE="${SOCCERMANAGER_LOG_FILE:-${TMPDIR:-/tmp}/soccermanager-api.log}"
PROJECT="${SOCCERMANAGER_PROJECT:-SoccerManager.API}"

# Print a path the way a Windows user can open it, falling back to the raw
# path if cygpath isn't around.
win_path() {
  if command -v cygpath >/dev/null 2>&1; then
    cygpath -w "$1"
  else
    echo "$1"
  fi
}

# PIDs actually LISTENING on a given port, one per line.
pids_on_port() {
  local port="$1"
  netstat -ano | tr -d '\r' | grep -E ":${port}[[:space:]].*LISTENING" | awk '{print $NF}' | sort -u
}

port_free() {
  [ -z "$(pids_on_port "$1")" ]
}

cmd_stop() {
  local pids
  pids="$( { pids_on_port "$HTTPS_PORT"; pids_on_port "$HTTP_PORT"; } | sort -u )"

  if [ -z "$pids" ]; then
    echo "Nothing listening on $HTTPS_PORT or $HTTP_PORT."
  else
    local pid
    for pid in $pids; do
      echo "Killing PID $pid"
      taskkill //F //PID "$pid" >/dev/null 2>&1 || true
    done
    sleep 1
  fi

  if port_free "$HTTPS_PORT" && port_free "$HTTP_PORT"; then
    echo "Ports $HTTPS_PORT and $HTTP_PORT are free."
    return 0
  fi

  echo "FAIL: a port is still in use after stop." >&2
  netstat -ano | tr -d '\r' | grep -E ":(${HTTPS_PORT}|${HTTP_PORT})[[:space:]]" >&2
  return 1
}

# Pull just the one connection-string var out of .env. Sourcing the whole
# file (as the README shows) breaks: the value is an unquoted ADO string
# full of semicolons, so bash tries to run "Initial", "User", etc. as
# commands. CRLF also leaves a trailing \r on every value, hence tr -d.
start_api() {
  if [ ! -f "$ENV_FILE" ]; then
    echo "FAIL: env file not found: $ENV_FILE" >&2
    exit 1
  fi

  local line
  line="$(grep -m1 '^ConnectionStrings__soccermanager=' "$ENV_FILE" | tr -d '\r')"
  if [ -z "$line" ]; then
    echo "FAIL: ConnectionStrings__soccermanager= not found in $ENV_FILE" >&2
    exit 1
  fi

  export "$line"
  if [ -z "${ConnectionStrings__soccermanager:-}" ]; then
    echo "FAIL: ConnectionStrings__soccermanager is set but empty in $ENV_FILE" >&2
    exit 1
  fi

  : > "$LOG_FILE"
  echo "Starting API (log: $(win_path "$LOG_FILE"))"
  # nohup + disown: without both, the dotnet process can die with this shell.
  ( cd "$REPO_ROOT" && nohup dotnet run --project "$PROJECT" --launch-profile https </dev/null >>"$LOG_FILE" 2>&1 & disown )

  echo "Waiting for the API to come up (first build can take a minute or two)..."
  local waited=0
  while [ "$waited" -lt "$START_TIMEOUT" ]; do
    # Build errors show up in seconds - check first so a compile error
    # (the most likely failure after editing code) doesn't wait out the
    # whole timeout.
    if grep -q "The build failed" "$LOG_FILE" 2>/dev/null; then
      echo "FAIL: build failed. Compiler errors:" >&2
      grep "error CS" "$LOG_FILE" >&2
      exit 1
    fi
    if grep -q "Unhandled exception" "$LOG_FILE" 2>/dev/null; then
      echo "FAIL: API crashed on startup. First lines of the exception:" >&2
      grep -A 15 "Unhandled exception" "$LOG_FILE" | head -16 >&2
      exit 1
    fi
    if grep -qF "$READY_LINE" "$LOG_FILE" 2>/dev/null; then
      echo "API is up."
      return 0
    fi
    sleep 2
    waited=$((waited + 2))
  done

  echo "FAIL: timed out after ${START_TIMEOUT}s waiting for the API to come up." >&2
  echo "See $(win_path "$LOG_FILE")" >&2
  exit 1
}

check_swagger() {
  local tmp status routes
  tmp="$(mktemp)"
  status="$(curl -sk -o "$tmp" -w '%{http_code}' "$HTTPS_URL/swagger/v1/swagger.json")"

  if [ "$status" != "200" ]; then
    echo "FAIL: GET /swagger/v1/swagger.json -> $status"
    rm -f "$tmp"
    return 1
  fi

  routes="$(grep -cE '^    "/[^"]+": \{' "$tmp")"
  rm -f "$tmp"
  echo "PASS: GET /swagger/v1/swagger.json -> 200 ($routes routes)"
}

run_checks() {
  local failed=0 body status

  body="$(curl -sk "$HTTPS_URL/health")"
  status="$(curl -sk -o /dev/null -w '%{http_code}' "$HTTPS_URL/health")"
  if [ "$status" = "200" ] && [ "$body" = "Healthy" ]; then
    echo "PASS: GET /health -> 200 Healthy"
  else
    echo "FAIL: GET /health -> $status body='$body'"
    failed=1
  fi

  check_swagger || failed=1

  status="$(curl -sk -o /dev/null -w '%{http_code}' "$HTTPS_URL/api/leagues")"
  if [ "$status" = "401" ]; then
    echo "PASS: GET /api/leagues (no token) -> 401"
  else
    echo "FAIL: GET /api/leagues (no token) -> $status"
    failed=1
  fi

  return "$failed"
}

already_up() {
  [ "$(curl -sk -o /dev/null -w '%{http_code}' "$HTTPS_URL/health" 2>/dev/null)" = "200" ]
}

case "${1:-}" in
  stop)
    cmd_stop
    exit $?
    ;;
  check)
    run_checks
    exit $?
    ;;
  "")
    if already_up; then
      echo "API already answering at $HTTPS_URL - skipping launch."
    else
      start_api
    fi
    run_checks
    exit $?
    ;;
  *)
    echo "Usage: smoke.sh [stop|check]" >&2
    exit 2
    ;;
esac
