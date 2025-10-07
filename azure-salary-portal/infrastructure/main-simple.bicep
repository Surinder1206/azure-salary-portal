@description('The location for all resources')
param location string = resourceGroup().location

@description('The environment (dev, staging, prod)')
@allowed(['dev', 'staging', 'prod'])
param environment string = 'dev'

// Generate unique suffix for resource names
var uniqueSuffix = take(uniqueString(resourceGroup().id), 8)

// Storage Account
resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: 'stpayslip${uniqueSuffix}${environment}'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: false
    allowSharedKeyAccess: true
  }
}

// Blob Containers
resource payslipContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-01-01' = {
  name: '${storageAccount.name}/default/payslips'
  properties: {
    publicAccess: 'None'
  }
}

resource documentsContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-01-01' = {
  name: '${storageAccount.name}/default/documents'
  properties: {
    publicAccess: 'None'
  }
}

// Static Web App
resource staticWebApp 'Microsoft.Web/staticSites@2023-01-01' = {
  name: 'swa-payslip-${uniqueSuffix}-${environment}'
  location: location
  sku: {
    name: 'Free'
    tier: 'Free'
  }
  properties: {
    buildProperties: {
      skipGithubActionWorkflowGeneration: false
      appLocation: '/frontend'
      apiLocation: '/backend'
      outputLocation: 'dist/payslip-portal'
    }
  }
}

// Outputs
output storageAccountName string = storageAccount.name
output staticWebAppName string = staticWebApp.name
output staticWebAppUrl string = staticWebApp.properties.defaultHostname
output resourceGroupName string = resourceGroup().name