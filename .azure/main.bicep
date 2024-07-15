@description('The name of the environment. This must be dev, or prod.')
@allowed(['dev', 'prod'])
param environmentName string = 'dev'

