# GitHub Integration Guide for Azure Salary Portal

This guide will help you set up GitHub integration with your Azure Static Web App for automatic deployments.

## Prerequisites

- ✅ Azure resources deployed via Terraform
- ✅ Local git repository with all code committed
- ⚠️ GitHub account and repository (to be created)

## Step 1: Create GitHub Repository

1. **Go to GitHub**: https://github.com
2. **Create New Repository**:
   - Repository name: `azure-salary-portal`
   - Description: `Azure Salary Slip Portal - Serverless payslip management system`
   - Set to **Public** or **Private** (your choice)
   - **DO NOT** initialize with README, .gitignore, or license (we have existing code)

## Step 2: Connect Local Repository to GitHub

After creating the repository on GitHub, you'll get a URL like:
`https://github.com/yourusername/azure-salary-portal.git`

Run these commands to connect your local repository:

```powershell
# Add GitHub as remote origin
git remote add origin https://github.com/yourusername/azure-salary-portal.git

# Push your code to GitHub
git branch -M main
git push -u origin main
```

## Step 3: Connect Static Web App to GitHub

### Option A: Using Azure Portal (Recommended)

1. **Open Azure Portal**: https://portal.azure.com
2. **Navigate to Static Web App**:
   - Resource Group: `rg-payslip-portal`
   - Static Web App: `swa-payslip-portal-f26tw8p8-dev`
3. **Configure Deployment**:
   - Click **"Manage deployment token"** in the overview
   - Copy the deployment token (keep it safe)
   - Go to **"Configuration"** → **"Deployment"**
   - Click **"Connect to GitHub"**
   - Authorize Azure to access your GitHub account
   - Select repository: `yourusername/azure-salary-portal`
   - Branch: `main`
   - Build configuration:
     - App location: `/frontend`
     - Api location: `/backend`
     - Output location: `dist/payslip-portal`

### Option B: Using Azure CLI

```powershell
# Get the API key for your Static Web App
az staticwebapp secrets list --name swa-payslip-portal-f26tw8p8-dev --resource-group rg-payslip-portal

# Configure GitHub integration
az staticwebapp source set --name swa-payslip-portal-f26tw8p8-dev --resource-group rg-payslip-portal --source https://github.com/yourusername/azure-salary-portal --branch main --app-location "/frontend" --api-location "/backend" --output-location "dist/payslip-portal"
```

## Step 4: Verify GitHub Actions Workflow

After connecting to GitHub, Azure will automatically:
1. Create a GitHub Actions workflow file in `.github/workflows/`
2. Set up deployment secrets in your GitHub repository
3. Trigger the first build and deployment

Check:
- GitHub repository → **Actions** tab for build status
- Azure Portal → Static Web App for deployment status

## Step 5: Test the Deployment

1. **Visit your Static Web App URL**: 
   - https://ashy-ocean-01ba99f03.1.azurestaticapps.net
   
2. **Make a test change**:
   - Edit a file in your repository
   - Commit and push to main branch
   - Watch GitHub Actions rebuild and redeploy automatically

## Troubleshooting

### Common Issues:

1. **Build fails**: Check the build configuration paths in the workflow
2. **API not working**: Ensure backend Functions are in `/backend` directory
3. **Routing issues**: Add `routes.json` file to `/frontend` for SPA routing

### Build Configuration:
```json
{
  "app_location": "/frontend",
  "api_location": "/backend", 
  "output_location": "dist/payslip-portal",
  "skip_github_action_workflow_generation": false
}
```

## Next Steps After GitHub Integration

Once GitHub integration is complete:
1. ✅ Configure Entra External ID authentication
2. ✅ Test the complete application flow
3. ✅ Set up monitoring and logging

## Important Notes

- **Free Tier Limits**: GitHub Actions has free tier limits (2000 minutes/month)
- **Build Time**: Initial build may take 5-10 minutes
- **Automatic Deployments**: Every push to main branch triggers deployment
- **Preview Deployments**: Pull requests create preview environments (if configured)