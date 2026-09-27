---
name: run-soccermanager
description: Build, run, start, stop and smoke-test the SoccerManager backend API (ASP.NET Core, Evolve migrations, Keycloak auth). Use when asked to run, start, launch, restart or stop the API, check it is healthy, hit an endpoint, or confirm a change works in the running app.
---

Drive `SoccerManager.API` with `.claude/skills/run-soccermanager/smoke.sh`: it
builds, starts, stops and smoke-tests it. Paths below are relative to the repo
root.

## WARNING

Starting the API in `Development` runs **Evolve migrations on startup**
against whatever database `ConnectionStrings__soccermanager` points at —
currently the **shared Azure SQL dev database**, not a local instance. Once a
`V*` script has been applied anywhere it is frozen: Evolve checksums the
whole file, comments included, and refuses to start on any change.
**Never edit an applied `V*` script under
`SoccerManager.Infrastructure/Database/Scripts` — write a new one.**

## Prerequisites

- .NET SDK matching `SoccerManager.API`'s `TargetFramework` (`net10.0`).
  Check what's actually installed and used:
  ```bash
  dotnet --version
  ```
- `.env` at the repo root with `ConnectionStrings__soccermanager` set to a
  working Azure SQL ADO connection string. Never edit or print this file's
  contents — it's developer-owned and carries a live credential.

## Run (agent path)

This is the path to use. All commands below are run from the repo root (they
also work from any other cwd — see Gotchas).

Start (or reuse an already-running instance) and smoke-test:
```bash
.claude/skills/run-soccermanager/smoke.sh
```
Skips launching if `https://localhost:7265/health` already answers.
Otherwise it builds, launches `dotnet run --project SoccerManager.API
--launch-profile https` in the background, waits for it to come up (first
build can take a minute or two), leaves it running, and prints one
PASS/FAIL line per check:
```text
PASS: GET /health -> 200 Healthy
PASS: GET /swagger/v1/swagger.json -> 200 (40 routes)
PASS: GET /api/leagues (no token) -> 401
```
Exit code is 0 only if every check passed.

Check only, against an API that's already running:
```bash
.claude/skills/run-soccermanager/smoke.sh check
```

Stop it (frees both 7265 and 5005):
```bash
.claude/skills/run-soccermanager/smoke.sh stop
```

### Calling other endpoints

Once it's up, hit any route from the printed swagger.json with `curl -sk`
(the dev cert is self-signed, hence `-k`):
```bash
curl -sk -o /dev/null -w '%{http_code}\n' https://localhost:7265/api/competitions
```
It prints `401` (empty body), as every protected route does without a token.
Keycloak's authority is the **deployed Azure realm in every environment** — there's nothing local to
stand up for auth. This skill has no way yet to obtain a real token, so an
authenticated call can't be exercised end to end here; the unauthenticated
401 check is what stands in for "auth is wired" for now.

## Run (human path)

```bash
dotnet run --project SoccerManager.API --launch-profile https
```
Plain `dotnet run` with no `--launch-profile` picks the first profile
(`http`), which is port 5005 only. `SoccerManager.UI` calls :7265, so the
`https` profile is required.

## Test

There is no test project:
```bash
git ls-files '*.csproj'
```
lists only `SoccerManager.API`, `.Application`, `.Domain` and
`.Infrastructure` — no test project exists to run.

## Gotchas

- **Don't source `.env` directly** (`. ./.env`, or the README's
  `set -a; . ./.env; set +a`). It's CRLF, and
  `ConnectionStrings__soccermanager`'s value is an *unquoted* ADO string
  (`Server=...;Initial Catalog=...;User ID=...;...`) — bash splits on `;` and
  tries to run `Initial`, `User`, etc. as commands (`Initial: command not
  found`, exit 127, `dotnet` never starts). `smoke.sh` instead greps just
  that one line and strips `\r` before exporting it.
- `smoke.sh stop` kills whatever is LISTENING on 7265/5005; killing the
  `dotnet run` wrapper also took the child down in testing here.
- New `V*` scripts separate statements with `GO`.
- `V1_0_0_15__add_match_attendance.sql` carries a "GO is required…" comment
  line, the only script that does. It was applied to Azure with that line,
  so removing it breaks Evolve's checksum. Leave it.
- `$TEMP`/`$TMP` in Git Bash are Windows backslash paths; the script uses
  `$TMPDIR` instead, which Git Bash gives as a POSIX path.
- The background `dotnet run` needs both `nohup` and `disown` to survive
  past the shell that launched it.

## Troubleshooting

Only errors actually hit while building this skill:

- **`Initial: command not found` / `User: command not found`, exit 127,
  `dotnet` never starts.** Cause: sourcing `.env` directly. Fix: use
  `smoke.sh` - it extracts just the one variable it needs instead.
- **`EvolveDb.EvolveException: Error executing script:
  V1_0_0_15__add_match_attendance.sql … Invalid column name 'Attendance'`.**
  Cause: a `CHECK` constraint (or a filtered index `WHERE`) on a column
  added earlier in the *same* batch - SQL Server compiles the whole batch
  before running it. Fix: separate the statements with `GO`. If the broken
  script was already applied anywhere, write a new script instead of
  editing it (see the WARNING above); if it was never applied anywhere
  (the failure is deterministic, so this is easy to tell), it can still be
  edited.
- **Build failure** (a compile error after editing code) - the most likely
  failure a future agent will hit:
  ```text
  FAIL: build failed. Compiler errors:
  Program.cs(1,35): error CS1002: ; expected [...]
  ```
  `smoke.sh` watches the log for `The build failed` and prints the
  `error CS` lines as soon as they appear, instead of waiting out the full
  startup timeout.
