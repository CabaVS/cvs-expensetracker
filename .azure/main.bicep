param environmentPostfix string

@secure()
param sqlAdminLogin string
@secure()
param sqlAdminPassword string
@secure()
param keycloakAdminLogin string
@secure()
param keycloakAdminPassword string

// Provided later once we know a hostname of Container App
param keycloakHostname string = ''

var appServicePlanName = 'asp-cvs-solutions${environmentPostfix}'
var appNameForExpenseTrackerApi = 'app-cvs-expensetracker-api${environmentPostfix}'
var sqlServerName = 'sql-cvs-solutions${environmentPostfix}'
var sqlDbNameForIdentityServer = 'sqldb-cvs-identityserver${environmentPostfix}'
var sqlDbNameForExpenseTracker = 'sqldb-cvs-expensetracker${environmentPostfix}'
var containerEnvironmentName = 'cae-cvs-solutions${environmentPostfix}'
var keycloakAppName = 'ca-cvs-identityserver${environmentPostfix}'
var keycloakImage = 'quay.io/keycloak/keycloak:26.1.4'

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: resourceGroup().location
  sku: {
    name: 'B1'
    tier: 'Basic'
    size: 'B1'
    capacity: 1
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource webApp 'Microsoft.Web/sites@2022-03-01' = {
  name: appNameForExpenseTrackerApi
  location: resourceGroup().location
  kind: 'app'
  properties: {
    serverFarmId: appServicePlan.id
  }
}

resource allowAzure 'Microsoft.Sql/servers/firewallRules@2022-02-01-preview' = {
  parent: sqlServer
  name: 'AllowAzureServices'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource sqlServer 'Microsoft.Sql/servers@2022-02-01-preview' = {
  name: sqlServerName
  location: resourceGroup().location
  properties: {
    administratorLogin: sqlAdminLogin
    administratorLoginPassword: sqlAdminPassword
  }
}

resource sqlDbForIdentityServer 'Microsoft.Sql/servers/databases@2022-02-01-preview' = {
  parent: sqlServer
  name: sqlDbNameForIdentityServer
  location: resourceGroup().location
  properties: {
    readScale: 'Disabled'
  }
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 5
  }
}

resource sqlDbForExpenseTracker 'Microsoft.Sql/servers/databases@2022-02-01-preview' = {
  parent: sqlServer
  name: sqlDbNameForExpenseTracker
  location: resourceGroup().location
  properties: {
    readScale: 'Disabled'
  }
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 5
  }
}

resource containerEnv 'Microsoft.App/managedEnvironments@2023-05-01' = {
  name: containerEnvironmentName
  location: resourceGroup().location
  properties: {}
}

resource keycloakApp 'Microsoft.App/containerApps@2023-05-01' = {
  name: keycloakAppName
  location: resourceGroup().location
  properties: {
    managedEnvironmentId: containerEnv.id
    configuration: {
      ingress: {
        external: true
        targetPort: 8080
        transport: 'auto'
        allowInsecure: false
      }
      secrets: [
        {
          name: 'sql-password'
          value: sqlAdminPassword
        }
        {
          name: 'keycloak-password'
          value: keycloakAdminPassword
        }
      ]
      activeRevisionsMode: 'Single'
    }
    template: {
      containers: [
        {
          name: 'keycloak'
          image: keycloakImage
          command: [
            '/opt/keycloak/bin/kc.sh'
            'start'
          ]
          env: [
            { name: 'KC_HTTP_ENABLED', value: 'true' }
            { name: 'KC_HTTP_PORT', value: '8080' }
            { name: 'KC_HOSTNAME', value: keycloakHostname }
            { name: 'KC_DB', value: 'mssql' }
            { name: 'KC_DB_URL', value: 'jdbc:sqlserver://sql-cvs-solutions${environmentPostfix}.database.windows.net:1433;database=sqldb-cvs-identityserver${environmentPostfix};encrypt=true;trustServerCertificate=false;hostNameInCertificate=*.database.windows.net;loginTimeout=30;' }
            { name: 'KC_DB_USERNAME', value: sqlAdminLogin }
            { name: 'KC_DB_PASSWORD', secretRef: 'sql-password' }
            { name: 'KC_BOOTSTRAP_ADMIN_USERNAME', value: keycloakAdminLogin }
            { name: 'KC_BOOTSTRAP_ADMIN_PASSWORD', secretRef: 'keycloak-password' }
          ]
          resources: {
            cpu: 1
            memory: '2Gi'
          }
        }
      ]
      scale: {
        minReplicas: 0
        maxReplicas: 1
      }
    }
  }
}
