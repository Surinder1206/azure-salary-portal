# Azure Salary Slip Portal - Quick Setup Script
# PowerShell version for Windows

Write-Host "🚀 Azure Salary Slip Portal - Development Setup" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Green

# Check prerequisites
Write-Host "✅ Checking prerequisites..." -ForegroundColor Yellow

# Check Node.js
try {
    $nodeVersion = node --version
    $versionNumber = [int]($nodeVersion.Substring(1).Split('.')[0])
    if ($versionNumber -lt 18) {
        Write-Host "❌ Node.js version is too old. Please install Node.js 18+" -ForegroundColor Red
        exit 1
    }
    Write-Host "✅ Node.js $nodeVersion found" -ForegroundColor Green
}
catch {
    Write-Host "❌ Node.js is not installed. Please install Node.js 18+" -ForegroundColor Red
    exit 1
}

# Check .NET
try {
    $dotnetVersion = dotnet --version
    $versionNumber = [int]($dotnetVersion.Split('.')[0])
    if ($versionNumber -lt 8) {
        Write-Host "❌ .NET version is too old. Please install .NET 8 SDK" -ForegroundColor Red
        exit 1
    }
    Write-Host "✅ .NET $dotnetVersion found" -ForegroundColor Green
}
catch {
    Write-Host "❌ .NET is not installed. Please install .NET 8 SDK" -ForegroundColor Red
    exit 1
}

# Check Azure CLI
try {
    az --version | Out-Null
    Write-Host "✅ Azure CLI found" -ForegroundColor Green
}
catch {
    Write-Host "⚠️  Azure CLI not found. Install it for deployment: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli" -ForegroundColor Yellow
}

# Check Azure Functions Core Tools
try {
    func --version | Out-Null
    Write-Host "✅ Azure Functions Core Tools found" -ForegroundColor Green
}
catch {
    Write-Host "⚠️  Azure Functions Core Tools not found. Install for local development:" -ForegroundColor Yellow
    Write-Host "   npm install -g azure-functions-core-tools@4 --unsafe-perm true" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "🔧 Setting up development environment..." -ForegroundColor Yellow

# Install frontend dependencies
Write-Host "📦 Installing frontend dependencies..." -ForegroundColor Blue
Set-Location frontend
npm install
Set-Location ..

# Restore backend dependencies
Write-Host "📦 Restoring backend dependencies..." -ForegroundColor Blue
Set-Location backend
dotnet restore
Set-Location ..

Write-Host ""
Write-Host "🛠️  Development Commands" -ForegroundColor Green
Write-Host "======================" -ForegroundColor Green
Write-Host ""
Write-Host "Frontend Development:" -ForegroundColor Cyan
Write-Host "  cd frontend; npm start         # Start Angular dev server"
Write-Host "  cd frontend; npm run build     # Build for production"
Write-Host "  cd frontend; npm test          # Run tests"
Write-Host ""
Write-Host "Backend Development:" -ForegroundColor Cyan
Write-Host "  cd backend; func start         # Start Functions locally"
Write-Host "  cd backend; dotnet build       # Build Functions"
Write-Host "  cd backend; dotnet test        # Run tests"
Write-Host ""
Write-Host "Full Stack:" -ForegroundColor Cyan
Write-Host "  npm run dev                    # Start both frontend and backend"
Write-Host "  npm run build                  # Build both projects"
Write-Host "  npm test                       # Run all tests"
Write-Host ""
Write-Host "🚀 Deployment" -ForegroundColor Green
Write-Host "=============" -ForegroundColor Green
Write-Host ""
Write-Host "1. Create Azure resources:"
Write-Host "   az group create --name rg-payslip-portal --location uksouth"
Write-Host "   az deployment group create --resource-group rg-payslip-portal --template-file infrastructure/main.bicep"
Write-Host ""
Write-Host "2. Configure GitHub repository:"
Write-Host "   - Connect to Azure Static Web Apps"
Write-Host "   - Add AZURE_STATIC_WEB_APPS_API_TOKEN secret"
Write-Host ""
Write-Host "3. Configure authentication:"
Write-Host "   - Set up Entra External ID app registration"
Write-Host "   - Configure Static Web App authentication"
Write-Host ""
Write-Host "📚 Documentation" -ForegroundColor Green
Write-Host "===============" -ForegroundColor Green
Write-Host ""
Write-Host "- README.md                      # Main project documentation"
Write-Host "- docs/API.md                    # API documentation"
Write-Host "- docs/DEPLOYMENT.md             # Deployment guide"
Write-Host "- frontend/README.md             # Frontend documentation"
Write-Host "- backend/README.md              # Backend documentation"
Write-Host "- infrastructure/README.md       # Infrastructure documentation"
Write-Host ""
Write-Host "✅ Setup complete! Happy coding! 🎉" -ForegroundColor Green