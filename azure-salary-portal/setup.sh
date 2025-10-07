#!/bin/bash

# Azure Salary Slip Portal - Quick Setup Script
# This script helps set up the development environment

set -e

echo "🚀 Azure Salary Slip Portal - Development Setup"
echo "================================================"

# Check prerequisites
echo "✅ Checking prerequisites..."

# Check Node.js
if ! command -v node &> /dev/null; then
    echo "❌ Node.js is not installed. Please install Node.js 18+"
    exit 1
fi

NODE_VERSION=$(node --version | cut -d'v' -f2 | cut -d'.' -f1)
if [ "$NODE_VERSION" -lt 18 ]; then
    echo "❌ Node.js version is too old. Please install Node.js 18+"
    exit 1
fi

echo "✅ Node.js $(node --version) found"

# Check .NET
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET is not installed. Please install .NET 8 SDK"
    exit 1
fi

DOTNET_VERSION=$(dotnet --version | cut -d'.' -f1)
if [ "$DOTNET_VERSION" -lt 8 ]; then
    echo "❌ .NET version is too old. Please install .NET 8 SDK"
    exit 1
fi

echo "✅ .NET $(dotnet --version) found"

# Check Azure CLI
if ! command -v az &> /dev/null; then
    echo "⚠️  Azure CLI not found. Install it for deployment: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli"
fi

# Check Azure Functions Core Tools
if ! command -v func &> /dev/null; then
    echo "⚠️  Azure Functions Core Tools not found. Install for local development:"
    echo "   npm install -g azure-functions-core-tools@4 --unsafe-perm true"
fi

echo ""
echo "🔧 Setting up development environment..."

# Install frontend dependencies
echo "📦 Installing frontend dependencies..."
cd frontend
npm install
cd ..

# Restore backend dependencies
echo "📦 Restoring backend dependencies..."
cd backend
dotnet restore
cd ..

echo ""
echo "🛠️  Development Commands"
echo "======================"
echo ""
echo "Frontend Development:"
echo "  cd frontend && npm start         # Start Angular dev server"
echo "  cd frontend && npm run build     # Build for production"
echo "  cd frontend && npm test          # Run tests"
echo ""
echo "Backend Development:"
echo "  cd backend && func start         # Start Functions locally"
echo "  cd backend && dotnet build       # Build Functions"
echo "  cd backend && dotnet test        # Run tests"
echo ""
echo "Full Stack:"
echo "  npm run dev                      # Start both frontend and backend"
echo "  npm run build                    # Build both projects"
echo "  npm test                         # Run all tests"
echo ""
echo "🚀 Deployment"
echo "============="
echo ""
echo "1. Create Azure resources:"
echo "   az group create --name rg-payslip-portal --location uksouth"
echo "   az deployment group create --resource-group rg-payslip-portal --template-file infrastructure/main.bicep"
echo ""
echo "2. Configure GitHub repository:"
echo "   - Connect to Azure Static Web Apps"
echo "   - Add AZURE_STATIC_WEB_APPS_API_TOKEN secret"
echo ""
echo "3. Configure authentication:"
echo "   - Set up Entra External ID app registration"
echo "   - Configure Static Web App authentication"
echo ""
echo "📚 Documentation"
echo "==============="
echo ""
echo "- README.md                      # Main project documentation"
echo "- docs/API.md                    # API documentation"
echo "- docs/DEPLOYMENT.md             # Deployment guide"
echo "- frontend/README.md             # Frontend documentation"
echo "- backend/README.md              # Backend documentation"
echo "- infrastructure/README.md       # Infrastructure documentation"
echo ""
echo "✅ Setup complete! Happy coding! 🎉"