@description('The name of the environment. This must be dev, or prod.')
@allowed(['dev', 'prod'])
param environmentName string = 'dev'

param location string = resourceGroup().location
param appServicePlanSku string
param linuxFxVersion string
param databaseSku object

@secure()
param adminUser string
@secure()
param adminPassword string

var solutionName = 'cvs-et'

var appServicePlanName = '${solutionName}-asp-${environmentName}'
var webAppBackendName = '${solutionName}-backend-app-${environmentName}'
var sqlServerName = '${solutionName}-sql-${environmentName}'
var sqlDatabaseName = '${solutionName}-sqldb-${environmentName}'

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: appServicePlanSku
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource webAppBackend 'Microsoft.Web/sites@2022-03-01' = {
  name: webAppBackendName
  location: location
  kind: 'app'
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: linuxFxVersion
      ftpsState: 'FtpsOnly'
    }
    httpsOnly: true
  }
}

resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: adminUser
    administratorLoginPassword: adminPassword
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  parent: sqlServer
  name: sqlDatabaseName
  location: location
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
  }
  sku: {
    name: databaseSku.name
    tier: databaseSku.tier
  }
}

output webAppBackendUrl string = 'https://${webAppBackend.properties.defaultHostName}'