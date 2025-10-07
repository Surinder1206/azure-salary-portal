# Azure Infrastructure Deployment

This directory contains the Azure infrastructure as code using Bicep templates.

## Resources Created

- **Storage Account**: Standard LRS for blob and table storage
- **Blob Containers**: Separate containers for payslips and documents
- **Static Web App**: Free tier for hosting the Angular frontend and Functions backend

## Deployment Instructions

### Prerequisites
- Azure CLI installed and logged in
- Appropriate permissions to create resources in the target subscription

### Deploy Infrastructure

1. **Create Resource Group**
   ```bash
   az group create --name rg-payslip-portal --location uksouth
   ```

2. **Deploy Bicep Template**
   ```bash
   az deployment group create \
     --resource-group rg-payslip-portal \
     --template-file main.bicep \
     --parameters environment=dev
   ```

3. **Get Output Values**
   ```bash
   az deployment group show \
     --resource-group rg-payslip-portal \
     --name main \
     --query properties.outputs
   ```

### Configuration

After deployment, you'll need to:

1. **Configure Entra External ID** in the Static Web App authentication settings
2. **Set up GitHub repository connection** for automatic deployments
3. **Configure role mappings** for admin users in the Static Web App

### Environment Variables

The following environment variables will be automatically configured:

- `AzureWebJobsStorage`: Connection string for Azure Functions
- `StorageConnectionString`: Connection string for blob and table storage
- `StorageBlobContainerName`: Name of the blob container for payslips

## Cost Estimation

- **Storage Account**: ~£0.20-£1.00/month (depending on usage)
- **Static Web App**: £0 (Free tier)
- **Data Transfer**: Minimal for typical usage

**Total Expected Cost**: ~£1/month