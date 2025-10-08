# Azure Storage Tables Initialization Script
# This script creates the required tables for the Azure Salary Portal

# Get storage account details from Terraform
Write-Host "🔄 Initializing Azure Storage Tables..." -ForegroundColor Yellow

# Get storage account name and connection string from Terraform
$storageAccountName = terraform output -raw storage_account_name
$connectionString = terraform output -raw storage_account_connection_string

Write-Host "📦 Storage Account: $storageAccountName" -ForegroundColor Green

# Install Azure PowerShell module if not present
if (-not (Get-Module -ListAvailable -Name Az.Storage)) {
    Write-Host "Installing Azure Storage PowerShell module..." -ForegroundColor Yellow
    Install-Module -Name Az.Storage -Force -AllowClobber
}

# Import the module
Import-Module Az.Storage

# Create storage context
$ctx = New-AzStorageContext -ConnectionString $connectionString

# Define tables to create
$tables = @(
    "employees",
    "payslips", 
    "documents",
    "auditlogs",
    "taxconfigs"
)

# Create tables
foreach ($tableName in $tables) {
    try {
        $table = Get-AzStorageTable -Name $tableName -Context $ctx -ErrorAction SilentlyContinue
        if ($table) {
            Write-Host "✅ Table '$tableName' already exists" -ForegroundColor Green
        } else {
            New-AzStorageTable -Name $tableName -Context $ctx
            Write-Host "✅ Created table '$tableName'" -ForegroundColor Green
        }
    }
    catch {
        Write-Host "❌ Error creating table '$tableName': $($_.Exception.Message)" -ForegroundColor Red
    }
}

# Create blob containers
$containers = @(
    "payslips",
    "documents", 
    "employee-photos"
)

foreach ($containerName in $containers) {
    try {
        $container = Get-AzStorageContainer -Name $containerName -Context $ctx -ErrorAction SilentlyContinue
        if ($container) {
            Write-Host "✅ Container '$containerName' already exists" -ForegroundColor Green
        } else {
            New-AzStorageContainer -Name $containerName -Context $ctx -Permission Blob
            Write-Host "✅ Created container '$containerName'" -ForegroundColor Green
        }
    }
    catch {
        Write-Host "❌ Error creating container '$containerName': $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "🎉 Storage initialization complete!" -ForegroundColor Green
Write-Host ""
Write-Host "📊 Created Tables:" -ForegroundColor Cyan
$tables | ForEach-Object { Write-Host "  • $_" -ForegroundColor White }
Write-Host ""
Write-Host "📁 Created Containers:" -ForegroundColor Cyan  
$containers | ForEach-Object { Write-Host "  • $_" -ForegroundColor White }