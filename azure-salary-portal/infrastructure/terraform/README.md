# Terraform Infrastructure for Azure Salary Portal

This directory contains Terraform configuration files to deploy the Azure infrastructure for the Salary Portal application.

## Prerequisites

1. **Azure CLI** - Install and login to Azure
   ```powershell
   az login
   az account set --subscription "your-subscription-name"
   ```

2. **Terraform** - Install Terraform CLI
   - Download from: https://www.terraform.io/downloads.html
   - Add to PATH environment variable

3. **Resource Group** - Ensure the resource group exists
   ```powershell
   az group create --name rg-payslip-portal --location uksouth
   ```

## Configuration

1. **Copy the example variables file:**
   ```powershell
   cp terraform.tfvars.example terraform.tfvars
   ```

2. **Edit terraform.tfvars** with your desired values:
   ```hcl
   environment = "dev"
   location = "uksouth"
   project_name = "payslip-portal"
   ```

## Deployment

1. **Initialize Terraform:**
   ```powershell
   terraform init
   ```

2. **Plan the deployment:**
   ```powershell
   terraform plan
   ```

3. **Apply the configuration:**
   ```powershell
   terraform apply
   ```

4. **View outputs:**
   ```powershell
   terraform output
   ```

## Resources Created

- **Storage Account** - For storing payslip PDFs and metadata
- **Storage Containers** - `payslips` and `documents` blob containers
- **Storage Tables** - `employees`, `payslips`, and `documents` tables
- **Static Web App** - Frontend hosting with built-in authentication

## Outputs

After deployment, Terraform will output:
- Storage Account name and connection details
- Static Web App URL and API key
- Resource Group information

## Cleanup

To destroy all resources:
```powershell
terraform destroy
```

## Cost Estimation

The deployed resources should cost approximately £1/month:
- Storage Account (LRS): ~£0.50/month
- Static Web App (Free tier): £0/month
- Table Storage: ~£0.50/month (based on minimal usage)

## Security Notes

- Storage Account has HTTPS-only traffic enabled
- Public blob access is disabled
- Minimum TLS version is 1.2
- Access keys are managed as sensitive outputs