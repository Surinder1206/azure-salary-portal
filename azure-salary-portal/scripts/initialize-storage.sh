#!/bin/bash
# Azure Storage Tables Initialization Script (Azure CLI version)
# This script creates the required tables for the Azure Salary Portal

echo "🔄 Initializing Azure Storage Tables..."

# Navigate to terraform directory
cd "$(dirname "$0")/../infrastructure/terraform" || exit 1

# Get storage account details from Terraform
STORAGE_ACCOUNT_NAME=$(terraform output -raw storage_account_name 2>/dev/null)
RESOURCE_GROUP_NAME=$(terraform output -raw resource_group_name 2>/dev/null)

if [ -z "$STORAGE_ACCOUNT_NAME" ] || [ -z "$RESOURCE_GROUP_NAME" ]; then
    echo "❌ Error: Could not get storage account details from Terraform"
    echo "Please ensure you're in the project root and Terraform has been applied"
    exit 1
fi

echo "📦 Storage Account: $STORAGE_ACCOUNT_NAME"
echo "🏗️  Resource Group: $RESOURCE_GROUP_NAME"

# Get storage account key
STORAGE_KEY=$(az storage account keys list \
    --resource-group "$RESOURCE_GROUP_NAME" \
    --account-name "$STORAGE_ACCOUNT_NAME" \
    --query '[0].value' \
    --output tsv)

if [ -z "$STORAGE_KEY" ]; then
    echo "❌ Error: Could not get storage account key"
    exit 1
fi

# Define tables to create
TABLES=("employees" "payslips" "documents" "auditlogs" "taxconfigs")

# Create tables
echo ""
echo "📊 Creating Tables..."
for table in "${TABLES[@]}"; do
    if az storage table create \
        --name "$table" \
        --account-name "$STORAGE_ACCOUNT_NAME" \
        --account-key "$STORAGE_KEY" \
        --output none 2>/dev/null; then
        echo "✅ Created table '$table'"
    else
        # Check if table already exists
        if az storage table exists \
            --name "$table" \
            --account-name "$STORAGE_ACCOUNT_NAME" \
            --account-key "$STORAGE_KEY" \
            --query exists \
            --output tsv | grep -q "true"; then
            echo "✅ Table '$table' already exists"
        else
            echo "❌ Failed to create table '$table'"
        fi
    fi
done

# Define containers to create  
CONTAINERS=("payslips" "documents" "employee-photos")

# Create blob containers
echo ""
echo "📁 Creating Blob Containers..."
for container in "${CONTAINERS[@]}"; do
    if az storage container create \
        --name "$container" \
        --account-name "$STORAGE_ACCOUNT_NAME" \
        --account-key "$STORAGE_KEY" \
        --public-access blob \
        --output none 2>/dev/null; then
        echo "✅ Created container '$container'"
    else
        # Check if container already exists
        if az storage container exists \
            --name "$container" \
            --account-name "$STORAGE_ACCOUNT_NAME" \
            --account-key "$STORAGE_KEY" \
            --query exists \
            --output tsv | grep -q "true"; then
            echo "✅ Container '$container' already exists"
        else
            echo "❌ Failed to create container '$container'"
        fi
    fi
done

echo ""
echo "🎉 Storage initialization complete!"
echo ""
echo "📊 Tables: ${TABLES[*]}"
echo "📁 Containers: ${CONTAINERS[*]}"