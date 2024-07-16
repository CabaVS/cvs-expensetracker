@description('The name of the environment. This must be dev, or prod.')
@allowed(['dev', 'prod'])
param environmentName string = 'dev'

param location string = resourceGroup().location
param appServicePlanSku string = 'F1'
param linuxFxVersion string

var solutionName = 'cvs-et'

var appServicePlanName = '${solutionName}-asp-${environmentName}'
var webAppBackendName = '${solutionName}-backend-app-${environmentName}'

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

output webAppBackendUrl string = 'https://${webAppBackend.properties.defaultHostName}'