# 🔑 Fix GitHub Actions API Key Issue

## Current Problem
```
The content server has rejected the request with: BadRequest
Reason: No matching Static Web App was found or the api key was invalid.
```

## ✅ Quick Fix Steps

### Step 1: Get Correct API Key from Azure Portal

1. **Open Azure Portal**: https://portal.azure.com
2. **Navigate to your Static Web App**:
   - Resource Groups → `rg-payslip-portal` 
   - Static Web App: `swa-payslip-portal-f26tw8p8-dev`
3. **Get deployment token**:
   - Click **"Overview"** tab
   - Click **"Manage deployment token"** 
   - **Copy the token** (long string starting with numbers/letters)

### Step 2: Update GitHub Repository Secret

1. **Go to repository**: https://github.com/Surinder1206/azure-salary-portal
2. **Navigate to Settings**:
   - Click **"Settings"** tab (top right)
   - Click **"Secrets and variables"** → **"Actions"**
3. **Update the secret**:
   - Find: `AZURE_STATIC_WEB_APPS_API_TOKEN`
   - Click **"Update"** 
   - **Paste the deployment token** from Step 1
   - Click **"Update secret"**

### Step 3: Trigger New Deployment

After updating the secret, trigger a new deployment:

```powershell
cd c:\safe\azure-salary-portal
echo "# API key updated $(Get-Date)" >> README.md
git add README.md
git commit -m "trigger: Update deployment after API key fix"
git push
```

## 🔍 How to Verify Success

### GitHub Actions Should Show:
- ✅ **Green checkmark** instead of ❌ red X
- **No more "api key was invalid" errors**
- **Successful deployment** message

### Live Site Should Load:
- **URL**: https://ashy-ocean-01ba99f03.1.azurestaticapps.net
- **Shows**: "🏢 Azure Salary Portal" page
- **No more** Azure default page

## 📋 Troubleshooting

### If you still get API key errors:

1. **Check Static Web App name**: Ensure it's exactly `swa-payslip-portal-f26tw8p8-dev`
2. **Verify resource group**: Should be `rg-payslip-portal`
3. **Multiple workflow files**: Check if there are multiple `.yml` files in `.github/workflows/`

### Alternative: Get API Key via Azure CLI (if login works):

```powershell
az login
az staticwebapp secrets list --name swa-payslip-portal-f26tw8p8-dev --resource-group rg-payslip-portal --query "properties.apiKey" --output tsv
```

## 🎯 Expected Result

After fixing the API key:
- ✅ **GitHub Actions**: Green checkmark ✅
- ✅ **Live site**: https://ashy-ocean-01ba99f03.1.azurestaticapps.net works
- ✅ **Ready for authentication setup**: Can proceed with Entra External ID

The deployment infrastructure is working perfectly - just need the correct API key!