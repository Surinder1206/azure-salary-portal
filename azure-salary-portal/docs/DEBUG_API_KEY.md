# 🔧 Static Web App API Key Debug Guide

## Current Issue
The deployment keeps failing with:
```
Reason: No matching Static Web App was found or the api key was invalid.
```

## 🔍 Debugging Steps

### Step 1: Verify Static Web App Details

**Expected Static Web App:**
- **Name**: `swa-payslip-portal-f26tw8p8-dev`  
- **Resource Group**: `rg-payslip-portal`
- **URL**: https://ashy-ocean-01ba99f03.1.azurestaticapps.net

### Step 2: Get API Key from Azure Portal

**Option A - Direct URL:**
1. Go to: https://portal.azure.com/#@sita.aero/resource/subscriptions/45f5cc66-56d1-42c0-acfc-80cc0461731f/resourceGroups/rg-payslip-portal/providers/Microsoft.Web/staticSites/swa-payslip-portal-f26tw8p8-dev/overview
2. Look for "Manage deployment token" button
3. Copy the token

**Option B - Manual Navigation:**
1. Azure Portal → All Resources
2. Search: `swa-payslip-portal-f26tw8p8-dev`
3. Click on it → Overview → Manage deployment token

### Step 3: Update GitHub Secret

1. **Go to**: https://github.com/Surinder1206/azure-salary-portal/settings/secrets/actions
2. **Find**: `AZURE_STATIC_WEB_APPS_API_TOKEN`
3. **Update** with the token from Azure Portal
4. **Save**

### Step 4: Alternative - Manual Deployment

If GitHub Actions keeps failing, you can deploy manually:

1. **Download GitHub Desktop** or use git bash
2. **Clone your repo locally** (if not already done)
3. **Use Azure Static Web Apps CLI**:
   ```bash
   npm install -g @azure/static-web-apps-cli
   swa deploy --deployment-token="YOUR_TOKEN_HERE" --app-location="/" 
   ```

## 🚨 If All Else Fails

**Create New Static Web App:**
1. Delete current Static Web App in Azure Portal
2. Run Terraform again to recreate it
3. Get fresh API key from new Static Web App

## 🎯 Expected Success

Once API key is correct, GitHub Actions should show:
```
✅ App Directory Location: '/' was found.
✅ Finished building app  
✅ Uploading build artifacts
✅ Deployment successful
```

And your site will be live at: https://ashy-ocean-01ba99f03.1.azurestaticapps.net

## 📞 Need Help?

If you're still stuck:
1. Share a screenshot of the Azure Portal Static Web App page
2. Confirm the exact name of your Static Web App in Azure
3. Let me know if you see "Manage deployment token" button