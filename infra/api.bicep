// API hosting for SoccerManager.
//
// Runs the API as a second container app in the existing Keycloak environment,
// serves it on a custom domain with a free managed certificate, and creates the
// identity GitHub Actions signs in with (OpenID Connect, no stored secret).

@description('Azure region for new resources. Defaults to the resource group location.')
param location string = resourceGroup().location

@description('Existing Container Apps environment that also runs Keycloak.')
param environmentName string = 'soccermanager-env'

@description('Existing container registry the API image is pulled from.')
param registryName string = 'soccermanageracr'

@description('Existing Azure DNS zone that holds the public host names.')
param dnsZoneName string = 'the11master.com'

@description('Host label for the API inside the DNS zone.')
param apiSubdomain string = 'api'

@description('Full reference of the image to run. A redeploy runs exactly this image, so pass the live one.')
param image string

@secure()
@description('SQL connection string for the API, using its own database user.')
param dbConnectionString string

@description('GitHub repository allowed to deploy, written the way GitHub names it in the OIDC token subject: owner@ownerId/name@repositoryId. The plain owner/name form does not match.')
param githubRepository string = 'panche-trifunov-softela@226723225/Softela.SoccerManager@1353429205'

var appName = 'soccermanager-api'
var apiHostname = '${apiSubdomain}.${dnsZoneName}'

// Built-in Contributor role.
var contributorRoleId = subscriptionResourceId('Microsoft.Authorization/roleDefinitions', 'b24988ac-6180-42a0-ab88-20f7382dd24c')

resource containerAppEnvironment 'Microsoft.App/managedEnvironments@2025-07-01' existing = {
  name: environmentName
}

resource registry 'Microsoft.ContainerRegistry/registries@2023-11-01-preview' existing = {
  name: registryName
}

resource dnsZone 'Microsoft.Network/dnsZones@2018-05-01' existing = {
  name: dnsZoneName
}

// A managed certificate needs the CNAME to point straight at the app's own
// host name, which is known before the app exists.
resource apiCname 'Microsoft.Network/dnsZones/CNAME@2018-05-01' = {
  parent: dnsZone
  name: apiSubdomain
  properties: {
    TTL: 3600
    CNAMERecord: {
      cname: '${appName}.${containerAppEnvironment.properties.defaultDomain}'
    }
  }
}

resource apiDomainVerification 'Microsoft.Network/dnsZones/TXT@2018-05-01' = {
  parent: dnsZone
  name: 'asuid.${apiSubdomain}'
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

resource api 'Microsoft.App/containerApps@2025-07-01' = {
  name: appName
  location: location
  properties: {
    managedEnvironmentId: containerAppEnvironment.id
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: true
        targetPort: 8080
        transport: 'auto'
        allowInsecure: false
        // Auto binds the managed certificate below as soon as it is issued.
        customDomains: [
          {
            name: apiHostname
            bindingType: 'Auto'
          }
        ]
      }
      // Same admin-credential pull as the Keycloak app in main.bicep.
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
          name: 'db-connection'
          value: dbConnectionString
        }
      ]
    }
    template: {
      containers: [
        {
          name: 'api'
          image: image
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: [
            {
              name: 'ConnectionStrings__soccermanager'
              secretRef: 'db-connection'
            }
            // The database is shared with local development, so the API applies
            // pending migrations on start, the same as a local run does.
            {
              name: 'Database__RunMigrationsOnStartup'
              value: 'true'
            }
            // The ingress terminates TLS; trusting X-Forwarded-Proto lets the app
            // see the original https scheme.
            {
              name: 'ASPNETCORE_FORWARDEDHEADERS_ENABLED'
              value: 'true'
            }
          ]
          // Kestrel only listens after the migrations finish, so the startup
          // probe allows up to five minutes.
          probes: [
            {
              type: 'Startup'
              tcpSocket: {
                port: 8080
              }
              periodSeconds: 5
              failureThreshold: 60
            }
            {
              type: 'Readiness'
              tcpSocket: {
                port: 8080
              }
              periodSeconds: 10
            }
            {
              type: 'Liveness'
              tcpSocket: {
                port: 8080
              }
              periodSeconds: 30
            }
          ]
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 1
      }
    }
  }
  dependsOn: [
    apiCname
    apiDomainVerification
  ]
}

resource apiCertificate 'Microsoft.App/managedEnvironments/managedCertificates@2025-07-01' = {
  parent: containerAppEnvironment
  name: replace(apiHostname, '.', '-')
  location: location
  properties: {
    subjectName: apiHostname
    domainControlValidation: 'CNAME'
  }
  dependsOn: [
    api
  ]
}

resource githubIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: 'soccermanager-github'
  location: location
}

// Only workflow runs on main can exchange their GitHub token for this identity.
resource githubFederatedCredential 'Microsoft.ManagedIdentity/userAssignedIdentities/federatedIdentityCredentials@2023-01-31' = {
  parent: githubIdentity
  name: 'github-main'
  properties: {
    issuer: 'https://token.actions.githubusercontent.com'
    subject: 'repo:${githubRepository}:ref:refs/heads/main'
    audiences: [
      'api://AzureADTokenExchange'
    ]
  }
}

// az acr build runs as a task inside the registry.
resource githubRegistryRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(registry.id, githubIdentity.id, contributorRoleId)
  scope: registry
  properties: {
    roleDefinitionId: contributorRoleId
    principalId: githubIdentity.properties.principalId
    principalType: 'ServicePrincipal'
  }
}

// az containerapp update swaps the image on this app only.
resource githubApiRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(api.id, githubIdentity.id, contributorRoleId)
  scope: api
  properties: {
    roleDefinitionId: contributorRoleId
    principalId: githubIdentity.properties.principalId
    principalType: 'ServicePrincipal'
  }
}

@description('Public URL of the API.')
output apiUrl string = 'https://${apiHostname}'

@description('Default Container Apps host name, reachable before the custom domain resolves.')
output apiDefaultHostname string = api.properties.configuration.ingress.fqdn

@description('Value for the AZURE_CLIENT_ID GitHub secret.')
output githubClientId string = githubIdentity.properties.clientId

@description('Value for the AZURE_TENANT_ID GitHub secret.')
output githubTenantId string = tenant().tenantId

@description('Value for the AZURE_SUBSCRIPTION_ID GitHub secret.')
output githubSubscriptionId string = subscription().subscriptionId
