// Reads credentials from the environment so nothing secret lands on disk.
// Source them from your .env before deploying, e.g. in PowerShell:
//   Get-Content .env | Where-Object { $_ -match '^\w' } | ForEach-Object {
//     $k, $v = $_ -split '=', 2; [Environment]::SetEnvironmentVariable($k, $v)
//   }

using './main.bicep'

param sqlAdministratorLogin = readEnvironmentVariable('SQL_USERNAME')
param sqlAdministratorPassword = readEnvironmentVariable('SQL_PASSWORD')
param keycloakAdminUsername = readEnvironmentVariable('KEYCLOAK_ADMIN_USERNAME')
param keycloakAdminPassword = readEnvironmentVariable('KEYCLOAK_ADMIN_PASSWORD')

// A .bicepparam cannot be combined with inline -p arguments, so the registry
// name is read here too rather than passed on the command line.
param registryName = readEnvironmentVariable('ACR_NAME', 'soccermanageracr')

// The tag to run, normally the one live now:
//   az containerapp show -n soccermanager-keycloak -g soccerteambuilder --query "properties.template.containers[0].image" -o tsv
param keycloakImageTag = readEnvironmentVariable('KEYCLOAK_IMAGE_TAG')
