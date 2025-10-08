# Azure Subscription Migration Guide

## Current Problem
- Infrastructure deployed to: `45f5cc66-56d1-42c0-acfc-80cc0461731f`  
- Your correct subscription: `129fda08-c053-44b9-809a-c540880aae75`
- This causes 404 errors on API endpoints

## Solution: Redeploy to Correct Subscription

### Step 1: Authenticate with Correct Account
1. **Open Azure Portal**: https://portal.azure.com
2. **Login** with account that has access to subscription `129fda08-c053-44b9-809a-c540880aae75`
3. **Verify** you can see the subscription in the portal

### Step 2: Clean Up Old Infrastructure (Optional)
You can leave the old resources in the other subscription or delete them:
- Resource Group: `rg-payslip-portal` in subscription `45f5cc66-56d1-42c0-acfc-80cc0461731f`

### Step 3: Prepare New Terraform Deployment
1. **Create new Terraform state** for the correct subscription
2. **Update provider configuration** if needed

### Step 4: Azure CLI Login
```powershell
# Login with the correct account
az login --tenant CORRECT_TENANT_ID

# Verify subscription
az account list --output table

# Set correct subscription  
az account set --subscription "129fda08-c053-44b9-809a-c540880aae75"
```

### Step 5: Deploy Infrastructure
```powershell
cd infrastructure/terraform

# Initialize Terraform (will create new state)
terraform init -reconfigure

# Plan deployment to new subscription
terraform plan

# Apply to create resources in correct subscription
terraform apply
```

### Step 6: Update GitHub Secrets
After successful deployment:
1. **Get new Static Web App API token** from Azure Portal
2. **Update GitHub secret**: `AZURE_STATIC_WEB_APPS_API_TOKEN`
3. **Trigger new deployment**

## Expected New Resources
- Resource Group: `rg-payslip-portal` (in correct subscription)
- Storage Account: `stpayslip[random]` (new name)
- Static Web App: `swa-payslip-portal-[random]-dev` (new name)

## Post-Deployment Steps
1. **Update authentication** settings with new Static Web App
2. **Test API endpoints** with new URLs
3. **Verify GitHub Actions** deployment works

---

## Alternative: Quick Manual Creation

If Terraform continues to have issues, you can manually create resources in Azure Portal:

1. **Create Resource Group**: `rg-payslip-portal-new`
2. **Create Storage Account**: Any unique name
3. **Create Static Web App**: Connect to your GitHub repo
4. **Update GitHub secrets** with new API token

This will get you working immediately while we troubleshoot Terraform.