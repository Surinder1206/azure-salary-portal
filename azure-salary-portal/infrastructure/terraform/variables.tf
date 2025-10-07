variable "environment" {
  description = "The environment (dev, staging, prod)"
  type        = string
  default     = "dev"

  validation {
    condition     = contains(["dev", "staging", "prod"], var.environment)
    error_message = "Environment must be one of: dev, staging, prod."
  }
}

variable "location" {
  description = "The Azure region where resources will be created"
  type        = string
  default     = "uksouth"
}

variable "project_name" {
  description = "The name of the project"
  type        = string
  default     = "payslip-portal"
}

variable "github_repo_url" {
  description = "The GitHub repository URL for the Static Web App"
  type        = string
  default     = ""
}

variable "github_branch" {
  description = "The GitHub branch for deployment"
  type        = string
  default     = "main"
}