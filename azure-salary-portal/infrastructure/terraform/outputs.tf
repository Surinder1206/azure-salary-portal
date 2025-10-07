# Storage Account outputs
output "storage_account_name" {
  description = "The name of the Storage Account"
  value       = azurerm_storage_account.main.name
}

output "storage_account_key" {
  description = "The primary access key for the Storage Account"
  value       = azurerm_storage_account.main.primary_access_key
  sensitive   = true
}

output "storage_account_connection_string" {
  description = "The connection string for the Storage Account"
  value       = azurerm_storage_account.main.primary_connection_string
  sensitive   = true
}

# Static Web App outputs
output "static_web_app_name" {
  description = "The name of the Static Web App"
  value       = azurerm_static_web_app.main.name
}

output "static_web_app_url" {
  description = "The default URL of the Static Web App"
  value       = "https://${azurerm_static_web_app.main.default_host_name}"
}

output "static_web_app_api_key" {
  description = "The API key for the Static Web App"
  value       = azurerm_static_web_app.main.api_key
  sensitive   = true
}

# Resource Group output
output "resource_group_name" {
  description = "The name of the Resource Group"
  value       = data.azurerm_resource_group.main.name
}

output "location" {
  description = "The Azure region where resources are deployed"
  value       = data.azurerm_resource_group.main.location
}