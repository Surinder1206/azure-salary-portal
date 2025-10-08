# Azure Salary Portal - Authentication Setup Guide

## Current Status
✅ **Infrastructure Deployed**: All Azure resources are live
✅ **Basic Deployment**: Application is accessible at https://ashy-ocean-01ba99f03.1.azurestaticapps.net
🔄 **Next Step**: Configure Entra ID authentication

## Step 1: Create App Registration in Azure Portal

1. **Go to Azure Portal**: https://portal.azure.com
2. **Navigate to**: Azure Active Directory → App registrations → New registration
3. **Fill in the registration form**:
   ```
   Name: Azure Salary Portal
   Supported account types: Accounts in any organizational directory and personal Microsoft accounts
   Redirect URI:
     - Platform: Web
     - URI: https://ashy-ocean-01ba99f03.1.azurestaticapps.net/.auth/login/aad/callback
   ```
4. **Click Register**

## Step 2: Configure App Registration

### A. Copy Important Values
After registration, **copy and save** these values:
- **Application (client) ID**: `2856cc32-b786-431b-8d1b-14940d08b2c6`
- **Directory (tenant) ID**: `0f5d282c-f167-4e5a-898c-2f18a1435bd5`

### B. Configure Authentication
1. Go to **Authentication** tab
2. **Enable ID tokens** (check the checkbox)
3. **Add additional redirect URIs**:
   - `https://ashy-ocean-01ba99f03.1.azurestaticapps.net/.auth/login/microsoftprovider/callback`
4. **Save changes**

### C. Create Client Secret
1. Go to **Certificates & secrets** tab
2. Click **New client secret**
3. **Description**: `Azure Salary Portal Secret`
4. **Expires**: `24 months` (recommended)
5. **Copy the secret value immediately** (you won't see it again)

### D. Configure API Permissions
1. Go to **API permissions** tab
2. Verify **User.Read** permission exists
3. If needed, click **Grant admin consent**

## Step 3: Configure Static Web App Settings

1. **Go to Azure Portal**: https://portal.azure.com
2. **Navigate to**: Resource groups → rg-payslip-portal → swa-payslip-portal-f26tw8p8-dev
3. **Go to**: Configuration (in the left sidebar)
4. **Add the following Application settings**:

   | Name | Value |
   |------|--------|
   | `AAD_CLIENT_ID` | `[Your Application (client) ID from Step 2A]` |
   | `AAD_CLIENT_SECRET` | `[Your client secret value from Step 2C]` |

5. **Save** the configuration

## Step 4: Deploy Updated Configuration

After completing the Azure Portal configuration, we need to deploy the updated `staticwebapp.config.json`:

```powershell
# Commit and push the updated configuration
git add staticwebapp.config.json
git commit -m "Add Entra ID authentication configuration"
git push
```

## Step 5: Test Authentication

1. **Wait for deployment** (2-3 minutes after push)
2. **Visit your app**: https://ashy-ocean-01ba99f03.1.azurestaticapps.net
3. **Navigate to protected routes** (will trigger login)
4. **Test login flow** with Microsoft account

## Step 6: Deploy Backend Functions (Next Phase)

Once authentication is working, we'll deploy the .NET Azure Functions for:
- Employee management
- Payslip generation and retrieval
- Document upload and management
- Admin operations

## Security Features Enabled

✅ **Route Protection**: API routes require authentication
✅ **Automatic Redirect**: Unauthenticated users redirect to login
✅ **Multi-tenant Support**: Works with any Microsoft account
✅ **Secure Headers**: XSS, clickjacking, and content-type protection

## Troubleshooting

### Common Issues:
1. **"Application not found"**: Check client ID is correct
2. **"Invalid redirect URI"**: Verify redirect URLs match exactly
3. **"Authentication failed"**: Check client secret is valid
4. **"403 Forbidden"**: Ensure user has correct role assignments

### Debug URLs:
- **Auth info**: `https://ashy-ocean-01ba99f03.1.azurestaticapps.net/.auth/me`
- **Logout**: `https://ashy-ocean-01ba99f03.1.azurestaticapps.net/.auth/logout`

## Cost Impact
- **App Registration**: Free
- **Authentication calls**: Included in Static Web App free tier
- **Total additional cost**: £0/month

---

**Next**: After completing these steps, we'll set up the backend Azure Functions and initialize the storage tables for employee data.