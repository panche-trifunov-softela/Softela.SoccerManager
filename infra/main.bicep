// Keycloak infrastructure for SoccerManager.
//
// Creates a container registry, a Container Apps environment and the Keycloak
// container app. The SQL server is pre-existing and shared with the
// application; only Keycloak's own database is declared here. Keycloak is
// served on a custom domain from the existing DNS zone, with a free managed
// certificate.

@description('Azure region for new resources. Defaults to the resource group location.')
param location string = resourceGroup().location

@description('Existing SQL server hosting the Keycloak database.')
param sqlServerName string = 'soccerteambuilder'

@description('Database holding Keycloak\'s own store. Separate from the application database.')
param keycloakDatabaseName string = 'keycloak'

@description('SQL login Keycloak connects with.')
param sqlAdministratorLogin string

@secure()
@description('Password for the SQL login.')
param sqlAdministratorPassword string

@description('Bootstrap admin username for the Keycloak console. Created on first start only.')
param keycloakAdminUsername string

@secure()
@description('Bootstrap admin password for the Keycloak console.')
param keycloakAdminPassword string

@description('Container registry name. Globally unique, alphanumeric only.')
param registryName string = 'soccermanageracr${uniqueString(resourceGroup().id)}'

@description('Tag of the Keycloak image to deploy. Required, so a redeploy cannot fall back to an older image.')
param keycloakImageTag string

@description('Existing Azure DNS zone that holds the public host names.')
param dnsZoneName string = 'the11master.com'

@description('Host label for Keycloak inside the DNS zone.')
param keycloakSubdomain string = 'auth'

var keycloakAppName = 'soccermanager-keycloak'
var keycloakImageName = 'soccermanager-keycloak'
var environmentName = 'soccermanager-env'
var logAnalyticsName = 'soccermanager-logs'
var realmName = 'soccermanager'

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: logAnalyticsName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
  }
}

resource registry 'Microsoft.ContainerRegistry/registries@2023-11-01-preview' = {
  name: registryName
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    // Admin credentials keep the first deployment simple. A system-assigned
    // identity with an AcrPull role assignment is the better hardening step.
    adminUserEnabled: true
  }
}

resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' existing = {
  name: sqlServerName
}

// S0 rather than Basic: Keycloak holds a persistent connection pool that
// Basic's concurrent-worker ceiling cannot carry.
resource keycloakDatabase 'Microsoft.Sql/servers/databases@2023-08-01-preview' = {
  parent: sqlServer
  name: keycloakDatabaseName
  location: location
  sku: {
    name: 'S0'
    tier: 'Standard'
  }
}

// Without this the Container App cannot reach the SQL server at all.
resource allowAzureServices 'Microsoft.Sql/servers/firewallRules@2023-08-01-preview' = {
  parent: sqlServer
  name: 'AllowAllWindowsAzureIps'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource containerAppEnvironment 'Microsoft.App/managedEnvironments@2024-03-01' = {
  name: environmentName
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalytics.properties.customerId
        sharedKey: logAnalytics.listKeys().primarySharedKey
      }
    }
  }
}

// The app's own host name is known once the environment exists, so DNS can
// point at it in the same deployment.
var keycloakDefaultHostname = '${keycloakAppName}.${containerAppEnvironment.properties.defaultDomain}'
var keycloakHostname = '${keycloakSubdomain}.${dnsZoneName}'

resource dnsZone 'Microsoft.Network/dnsZones@2018-05-01' existing = {
  name: dnsZoneName
}

// A managed certificate needs the CNAME to point straight at the app's own
// host name; an intermediate CNAME blocks issuance and renewal.
resource keycloakCname 'Microsoft.Network/dnsZones/CNAME@2018-05-01' = {
  parent: dnsZone
  name: keycloakSubdomain
  properties: {
    TTL: 3600
    CNAMERecord: {
      cname: keycloakDefaultHostname
    }
  }
}

resource keycloakDomainVerification 'Microsoft.Network/dnsZones/TXT@2018-05-01' = {
  parent: dnsZone
  name: 'asuid.${keycloakSubdomain}'
  properties: {
    TTL: 3600
    TXTRecords: [
      {
        value: [
          containerAppEnvironment.properties.customDomainConfiguration.customDomainVerificationId
        ]
      }
    ]
  }
}

resource keycloak 'Microsoft.App/containerApps@2025-07-01' = {
  name: keycloakAppName
  location: location
  properties: {
    managedEnvironmentId: containerAppEnvironment.id
    configuration: {
      ingress: {
        external: true
        targetPort: 8080
        transport: 'auto'
        allowInsecure: false
        // Auto binds the managed certificate below as soon as it is issued.
        customDomains: [
          {
            name: keycloakHostname
            bindingType: 'Auto'
          }
        ]
      }
      registries: [
        {
          server: registry.properties.loginServer
          username: registry.listCredentials().username
          passwordSecretRef: 'registry-password'
        }
      ]
      secrets: [
        {
          name: 'registry-password'
          value: registry.listCredentials().passwords[0].value
        }
        {
          name: 'sql-password'
          value: sqlAdministratorPassword
        }
        {
          name: 'keycloak-admin-password'
          value: keycloakAdminPassword
        }
      ]
    }
    template: {
      containers: [
        {
          name: 'keycloak'
          image: '${registry.properties.loginServer}/${keycloakImageName}:${keycloakImageTag}'
          resources: {
            cpu: json('1.0')
            memory: '2Gi'
          }
          env: [
            {
              name: 'KC_DB'
              value: 'mssql'
            }
            {
              name: 'KC_DB_URL'
              value: 'jdbc:sqlserver://${sqlServerName}${environment().suffixes.sqlServerHostname};databaseName=${keycloakDatabaseName};encrypt=true;trustServerCertificate=false;loginTimeout=30'
            }
            {
              name: 'KC_DB_USERNAME'
              value: sqlAdministratorLogin
            }
            {
              name: 'KC_DB_PASSWORD'
              secretRef: 'sql-password'
            }
            {
              name: 'KC_BOOTSTRAP_ADMIN_USERNAME'
              value: keycloakAdminUsername
            }
            {
              name: 'KC_BOOTSTRAP_ADMIN_PASSWORD'
              secretRef: 'keycloak-admin-password'
            }
            {
              name: 'KC_HOSTNAME'
              value: 'https://${keycloakHostname}'
            }
            // Container Apps terminates TLS at the ingress and forwards plain
            // HTTP, so Keycloak must trust the forwarded headers.
            {
              name: 'KC_HTTP_ENABLED'
              value: 'true'
            }
            {
              name: 'KC_PROXY_HEADERS'
              value: 'xforwarded'
            }
          ]
        }
      ]
      // Pinned to one replica: multi-replica Keycloak needs Infinispan cache
      // clustering, which is not configured here.
      scale: {
        minReplicas: 1
        maxReplicas: 1
      }
    }
  }
  dependsOn: [
    keycloakDatabase
    allowAzureServices
    keycloakCname
    keycloakDomainVerification
  ]
}

resource keycloakCertificate 'Microsoft.App/managedEnvironments/managedCertificates@2025-07-01' = {
  parent: containerAppEnvironment
  name: replace(keycloakHostname, '.', '-')
  location: location
  properties: {
    subjectName: keycloakHostname
    domainControlValidation: 'CNAME'
  }
  dependsOn: [
    keycloak
  ]
}

@description('Public URL of the deployed Keycloak instance.')
output keycloakUrl string = 'https://${keycloakHostname}'

@description('Value for Keycloak:Authority in the API appsettings.')
output keycloakAuthority string = 'https://${keycloakHostname}/realms/${realmName}'

@description('Registry to push the Keycloak image to before deploying.')
output registryLoginServer string = registry.properties.loginServer

@description('Registry name, for az acr login.')
output registryName string = registry.name
