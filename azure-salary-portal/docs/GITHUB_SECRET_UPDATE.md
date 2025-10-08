# Static Web App Token Update Guide

## Current Issue
The deployment is failing because the GitHub Actions workflow is using the API token from the old Static Web App (wrong subscription). We need to update the GitHub secret with the API token from the new Static Web App.

## Step 1: Get New API Token from Azure Portal

1. **Go to Azure Portal**: https://portal.azure.com
2. **Navigate to your new Static Web App**:
   - Subscription: `129fda08-c053-44b9-809a-c540880aae75`
   - Look for Static Web App with name containing: `brave-pond-03f918e0f`
3. **In the Static Web App Overview page**:
   - Click **"Manage deployment token"** 
   - **Copy the API Token** (long string starting with deployment token)

## Step 2: Update GitHub Secret

1. **Go to GitHub**: https://github.com/Surinder1206/azure-salary-portal/settings/secrets/actions
2. **Find**: `AZURE_STATIC_WEB_APPS_API_TOKEN`
3. **Click "Update"**
4. **Replace** the value with the new API token from Step 1
5. **Save** the updated secret

## Step 3: Alternative - Create New Workflow

If the above doesn't work, you can create a new workflow for the new Static Web App:

1. **In Azure Portal** → Your new Static Web App
2. **Click "Browse"** to see if there's a working URL
3. **If working**, use that URL instead
4. **If not working**, go to **"Configuration" → "General"**
5. **Click "Download publish profile"** 
6. **Set up GitHub Actions** through the Azure Portal integration

## Expected New URLs

Your new Static Web App should be accessible at:
- Main URL: `https://brave-pond-03f918e0f.1.azurestaticapps.net`
- API Health: `https://brave-pond-03f918e0f.1.azurestaticapps.net/api/health`

## Current Status

The deployment keeps failing because:
1. **Wrong API Token**: Points to old subscription Static Web App
2. **File Location**: While index.html exists, the wrong deployment target can't find it

## Next Steps

1. ✅ **Update the GitHub secret** (most likely fix)
2. 🔄 **Test deployment** after secret update
3. 🔄 **Verify URLs work** with new deployment

---

**Once you update the GitHub secret with the correct API token, the deployment should work immediately!**