# Generate random suffix for unique resource names
resource "random_string" "suffix" {
  length  = 8
  upper   = false
  special = false
}

# Data source for current client config
data "azurerm_client_config" "current" {}

# Resource Group
data "azurerm_resource_group" "main" {
  name = "rg-payslip-portal"
}

# Storage Account
resource "azurerm_storage_account" "main" {
  name                     = "stpayslip${random_string.suffix.result}"
  resource_group_name      = data.azurerm_resource_group.main.name
  location                = data.azurerm_resource_group.main.location
  account_tier            = "Standard"
  account_replication_type = "LRS"
  account_kind            = "StorageV2"

  min_tls_version               = "TLS1_2"
  https_traffic_only_enabled    = true
  allow_nested_items_to_be_public = false
  shared_access_key_enabled     = true

  tags = {
    Environment = var.environment
    Project     = var.project_name
  }
}

# Storage Containers
resource "azurerm_storage_container" "payslips" {
  name                  = "payslips"
  storage_account_name  = azurerm_storage_account.main.name
  container_access_type = "private"
}

resource "azurerm_storage_container" "documents" {
  name                  = "documents"
  storage_account_name  = azurerm_storage_account.main.name
  container_access_type = "private"
}

# Storage Table for metadata
resource "azurerm_storage_table" "employees" {
  name                 = "employees"
  storage_account_name = azurerm_storage_account.main.name
}

resource "azurerm_storage_table" "payslips" {
  name                 = "payslips"
  storage_account_name = azurerm_storage_account.main.name
}

resource "azurerm_storage_table" "documents" {
  name                 = "documents"
  storage_account_name = azurerm_storage_account.main.name
}

# Static Web App
resource "azurerm_static_web_app" "main" {
  name                = "swa-${var.project_name}-${random_string.suffix.result}-${var.environment}"
  resource_group_name = data.azurerm_resource_group.main.name
  location           = "West Europe"  # Static Web Apps have limited region availability
  sku_tier           = "Free"
  sku_size           = "Free"

  app_settings = {
    "AZURE_STORAGE_ACCOUNT_NAME" = azurerm_storage_account.main.name
    "AZURE_STORAGE_ACCOUNT_KEY"  = azurerm_storage_account.main.primary_access_key
    "ENVIRONMENT"                = var.environment
  }

  tags = {
    Environment = var.environment
    Project     = var.project_name
  }
}