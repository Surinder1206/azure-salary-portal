# Deployment Guide

## Prerequisites

Before deploying the Azure Salary Slip Portal, ensure you have:

1. **Azure Subscription** with appropriate permissions
2. **Azure CLI** installed and logged in
3. **GitHub Repository** for source code
4. **Entra External ID** tenant configured
5. **Node.js 18+** and **.NET 8 SDK** for local development

## Step-by-Step Deployment

### 1. Clone and Setup Repository

```bash
git clone <your-repo-url>
cd azure-salary-portal
```

### 2. Deploy Infrastructure

```bash
# Create resource group
az group create --name rg-payslip-portal --location uksouth

# Deploy Bicep template
az deployment group create \
  --resource-group rg-payslip-portal \
  --template-file infrastructure/main.bicep \
  --parameters environment=prod

# Get deployment outputs
az deployment group show \
  --resource-group rg-payslip-portal \
  --name main \
  --query properties.outputs
```

### 3. Configure Static Web App

#### 3.1 Connect GitHub Repository

1. Go to Azure Portal → Static Web Apps
2. Find your deployed Static Web App
3. Click **Manage deployment token**
4. Copy the deployment token

#### 3.2 Add GitHub Secrets

In your GitHub repository, add these secrets:
- `AZURE_STATIC_WEB_APPS_API_TOKEN`: The deployment token from above

#### 3.3 Configure Build Settings

The GitHub Actions workflow is already configured to:
- Build Angular frontend from `/frontend`
- Build .NET Functions from `/backend`
- Deploy to Azure Static Web Apps

### 4. Configure Authentication

#### 4.1 Create Entra External ID Application

1. Go to **Entra External ID** in Azure Portal
2. Create new **App Registration**:
   - Name: `Payslip Portal`
   - Supported account types: `Accounts in this organizational directory only`
   - Redirect URI: `https://<your-swa-url>/.auth/login/aad/callback`

3. Note the **Application (client) ID**

#### 4.2 Configure Static Web App Authentication

1. Go to your Static Web App in Azure Portal
2. Navigate to **Authentication**
3. Add **Azure Active Directory** provider:
   - Client ID: Your app registration client ID
   - Client secret: Create in app registration → Certificates & secrets
   - Issuer URL: `https://login.microsoftonline.com/<tenant-id>/v2.0`

#### 4.3 Configure Role Mappings

1. In Static Web App **Role management**
2. Add role mappings:
   - **admin**: Assign to specific users who need admin access
   - **authenticated**: Default for all authenticated users

### 5. Initialize Storage

#### 5.1 Create Table Storage Tables

Run these commands to create required tables:

```bash
# Get storage connection string from deployment outputs
STORAGE_CONN="<your-storage-connection-string>"

# Create tables (using Azure CLI or Azure Storage Explorer)
az storage table create --name employees --connection-string $STORAGE_CONN
az storage table create --name payslips --connection-string $STORAGE_CONN
az storage table create --name documents --connection-string $STORAGE_CONN
az storage table create --name taxconfigs --connection-string $STORAGE_CONN
az storage table create --name auditlogs --connection-string $STORAGE_CONN
```

#### 5.2 Create Blob Containers

Containers are automatically created by the Bicep template:
- `payslips`: For PDF payslip files
- `documents`: For HR documents (P60, P45, etc.)

### 6. Test Deployment

1. **Access the Application**
   - URL: `https://<your-swa-name>.azurestaticapps.net`

2. **Test Authentication**
   - Click **Login** button
   - Should redirect to Entra ID login

3. **Test API Endpoints**
   - GET `/api/me` should return user info
   - Other endpoints should return appropriate responses

### 7. Configure Admin Users

1. **Add Admin Role**:
   ```bash
   # In Azure Portal → Static Web App → Role management
   # Add user email to 'admin' role
   ```

2. **Test Admin Functions**:
   - Login as admin user
   - Navigate to `/admin`
   - Should see admin panel

### 8. Production Configuration

#### 8.1 Configure Custom Domain (Optional)

1. Go to Static Web App → **Custom domains**
2. Add your domain
3. Configure DNS CNAME record

#### 8.2 Set Up Monitoring

1. **Enable Application Insights** (if not using free tier):
   ```bash
   az monitor app-insights component create \
     --app payslip-portal-insights \
     --location uksouth \
     --resource-group rg-payslip-portal
   ```

2. **Configure Budget Alerts**:
   ```bash
   az consumption budget create \
     --budget-name payslip-portal-budget \
     --amount 5 \
     --resource-group rg-payslip-portal \
     --time-grain Monthly
   ```

#### 8.3 Backup Strategy

- **Table Storage**: Enable soft delete and versioning
- **Blob Storage**: Configure lifecycle management
- **Configuration**: Export ARM templates regularly

## Post-Deployment Checklist

- [ ] Application loads successfully
- [ ] Authentication works with Entra ID
- [ ] Users can view payslips (with sample data)
- [ ] Admin users can access admin panel
- [ ] File downloads work correctly
- [ ] All API endpoints respond correctly
- [ ] Budget alerts are configured
- [ ] Backup strategy is in place

## Troubleshooting

### Common Issues

1. **Authentication not working**
   - Check redirect URIs in app registration
   - Verify client ID and tenant ID
   - Ensure user has appropriate role assignments

2. **API endpoints returning 500 errors**
   - Check Function logs in Azure Portal
   - Verify storage connection string
   - Ensure tables are created

3. **File downloads not working**
   - Check blob container permissions
   - Verify SAS token generation
   - Ensure files exist in storage

### Logs and Monitoring

- **Function Logs**: Azure Portal → Static Web App → Functions → Monitor
- **Storage Metrics**: Azure Portal → Storage Account → Monitoring
- **Application Insights**: If enabled, provides detailed telemetry

## Cost Monitoring

Expected monthly costs:
- Storage Account: £0.20-£1.00
- Static Web App: £0 (Free tier)
- Data Transfer: Minimal

Set up budget alerts to monitor actual usage and costs.