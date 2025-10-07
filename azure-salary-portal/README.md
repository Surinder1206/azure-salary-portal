# Azure Salary Slip Portal

A serverless salary slip management portal built with Angular 17 and Azure Functions.

## 🏗️ Architecture

- **Frontend**: Angular 17 SPA hosted on Azure Static Web Apps
- **Backend**: Azure Functions (.NET 8 isolated)
- **Storage**: Azure Blob Storage (PDFs) + Table Storage (metadata)
- **Authentication**: Entra External ID via SWA built-in auth

## 💰 Cost Target: ~£1/month

## 📁 Project Structure

```
├── frontend/          # Angular 17 SPA
├── backend/           # Azure Functions (.NET 8)
├── infrastructure/    # ARM/Bicep templates
├── docs/             # Documentation
└── .github/          # GitHub Actions workflows
```

## 🚀 Quick Start

### Prerequisites
- Node.js 18+
- .NET 8 SDK
- Azure CLI
- Angular CLI

### Development Setup

1. **Frontend Setup**
   ```bash
   cd frontend
   npm install
   ng serve
   ```

2. **Backend Setup**
   ```bash
   cd backend
   dotnet restore
   func start
   ```

3. **Infrastructure Deploy**
   ```bash
   cd infrastructure
   az deployment group create --resource-group rg-payslip --template-file main.bicep
   ```

## 📋 Features

### User Features
- ✅ Login/Logout via Entra ID
- ✅ View payslip history
- ✅ Download payslips (PDF)
- ✅ View HR documents (P60, P45)

### Admin Features
- ✅ Batch upload payslips
- ✅ Employee management
- ✅ Tax year configuration
- ✅ Tax calculation preview

## 🔒 Security

- **Authentication**: Entra External ID (OIDC)
- **Authorization**: Role-based access via SWA routes
- **File Access**: Short-lived SAS tokens
- **Data**: Encrypted at rest in Azure Storage

## 🎯 Roadmap

- **Week 1**: Core scaffold + auth + basic payslip flow
- **Week 2**: Admin features + storage integration
- **Week 3**: UI polish + testing + deployment

## 📊 Monitoring

- Storage metrics for bandwidth/transactions
- Function logs via SWA portal
- Audit logs in Table Storage
- Budget alert at £2/month

## 🛠️ Development

See individual README files in each directory for detailed setup instructions.