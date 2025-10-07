# Backend README

## Azure Functions Backend (.NET 8)

This is the backend API for the Azure Salary Slip Portal, built with .NET 8 isolated Azure Functions.

## Features

- **.NET 8 Isolated Functions**: Latest Azure Functions runtime
- **Azure Table Storage**: Metadata storage for payslips, employees, etc.
- **Azure Blob Storage**: Secure PDF file storage with SAS tokens
- **Built-in Authentication**: Integration with Azure Static Web Apps authentication
- **Role-Based Authorization**: Admin and user role support
- **Audit Logging**: Comprehensive audit trail for all operations

## Project Structure

```
├── Functions/
│   ├── AuthenticationFunctions.cs    # User authentication endpoints
│   ├── PayslipFunctions.cs          # Payslip management
│   ├── DocumentFunctions.cs         # HR document access
│   └── AdminFunctions.cs            # Admin-only operations
├── Models/
│   ├── Employee.cs                  # Employee entity
│   ├── Payslip.cs                   # Payslip entity
│   ├── Document.cs                  # HR document entity
│   ├── TaxYearConfig.cs             # Tax configuration
│   └── AuditLog.cs                  # Audit logging
├── Services/                        # Future: Business logic services
├── Program.cs                       # Function app configuration
├── host.json                        # Function runtime configuration
└── local.settings.json              # Local development settings
```

## API Endpoints

### Authentication
- `GET /api/me` - Get current user information

### Payslips
- `GET /api/payslips` - List user's payslips
- `GET /api/payslips/{id}/download` - Get payslip download URL

### Documents
- `GET /api/docs` - List user's HR documents
- `GET /api/docs/{id}/download` - Get document download URL

### Admin (Requires admin role)
- `POST /api/admin/payslips/batch` - Batch upload payslips
- `GET /api/admin/config/tax-year/{year}` - Get tax configuration
- `PUT /api/admin/config/tax-year/{year}` - Update tax configuration
- `POST /api/admin/tax/preview` - Preview tax calculation

## Data Models

### Employee
- Stores user information and roles
- Partition Key: "Employee"
- Row Key: Employee ID

### Payslip
- Stores payslip metadata and blob references
- Partition Key: Employee ID
- Row Key: Payslip ID
- Links to PDF files in blob storage

### Document
- Stores HR document metadata (P60, P45, etc.)
- Partition Key: Employee ID
- Row Key: Document ID

### TaxYearConfig
- Tax calculation configuration per year
- Partition Key: "TaxYear"
- Row Key: Tax Year (e.g., "2024-25")

### AuditLog
- Comprehensive audit logging
- Partition Key: Date (YYYY-MM-DD)
- Row Key: Timestamp + GUID

## Security

### Authentication
- Uses Azure Static Web Apps built-in authentication
- User information passed via headers:
  - `X-MS-CLIENT-PRINCIPAL-ID`: User ID
  - `X-MS-CLIENT-PRINCIPAL-NAME`: User email
  - `X-MS-CLIENT-PRINCIPAL-ROLES`: User roles

### Authorization
- Role-based access control
- Admin functions require "admin" role
- User functions require "authenticated" role

### File Access
- Blob storage with private access only
- Short-lived SAS tokens (1 hour expiration)
- User can only access their own files

## Development

### Prerequisites
- .NET 8 SDK
- Azure Functions Core Tools v4
- Azure Storage Emulator (for local development)

### Local Development

1. **Install Dependencies**
   ```bash
   dotnet restore
   ```

2. **Start Storage Emulator**
   ```bash
   # Windows (Azurite recommended)
   azurite --silent --location c:\azurite --debug c:\azurite\debug.log
   ```

3. **Run Functions Locally**
   ```bash
   func start
   ```

### Configuration

#### Local Settings
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "StorageConnectionString": "UseDevelopmentStorage=true",
    "StorageBlobContainerName": "payslips"
  }
}
```

#### Production Settings
- Configured automatically via Bicep template
- Settings stored in Azure Static Web App configuration
- Connection strings and secrets managed securely

## Database Schema

### Table Storage Tables

1. **employees**: Employee master data
2. **payslips**: Payslip metadata and blob references
3. **documents**: HR document metadata
4. **taxconfigs**: Tax year configuration
5. **auditlogs**: Audit trail for all operations

### Blob Storage Containers

1. **payslips**: PDF payslip files
2. **documents**: HR document files (P60, P45, etc.)

## Error Handling

- Comprehensive try-catch blocks in all functions
- Structured logging using ILogger
- Standard HTTP status codes
- Detailed error messages in development
- Generic error messages in production

## Logging and Monitoring

### Audit Logging
All sensitive operations are logged:
- File downloads
- Admin configuration changes
- User authentication events

### Application Logging
- Function execution logs
- Error logging with stack traces
- Performance metrics

### Storage Metrics
- Blob storage access patterns
- Table storage query performance
- Cost monitoring

## Performance Considerations

### Efficient Queries
- Partition key design for optimal performance
- Minimal table scans
- Proper indexing strategy

### Blob Storage
- SAS token caching (future enhancement)
- CDN integration for static content (future)
- Compression for large files (future)

### Function Performance
- Cold start optimization with isolated worker
- Minimal dependencies
- Async/await best practices

## Testing

### Unit Testing
```bash
dotnet test
```

### Integration Testing
- Test against Azure Storage Emulator
- Mock authentication headers
- End-to-end API testing

## Deployment

### Automatic Deployment
- GitHub Actions automatically builds and deploys
- No manual deployment steps required

### Manual Deployment
```bash
# Build
dotnet build --configuration Release

# Publish
dotnet publish --configuration Release

# Deploy (via Azure Functions Core Tools)
func azure functionapp publish <function-app-name>
```

## Future Enhancements

### Planned Features
- [ ] Enhanced tax calculation engine
- [ ] Email notification service
- [ ] Bulk operations optimization
- [ ] File validation and virus scanning
- [ ] Data encryption at application level

### Technical Improvements
- [ ] Implement service layer pattern
- [ ] Add comprehensive unit tests
- [ ] Implement caching layer (Redis)
- [ ] Add health check endpoints
- [ ] Implement retry policies
- [ ] Add performance monitoring

## Troubleshooting

### Common Issues

1. **Storage Connection Issues**
   - Verify connection string format
   - Check storage account access keys
   - Ensure storage emulator is running (local dev)

2. **Authentication Problems**
   - Verify SWA authentication configuration
   - Check user role assignments
   - Validate header values

3. **File Access Issues**
   - Check blob container permissions
   - Verify SAS token generation
   - Ensure files exist in storage

### Debugging
- Use Azure Functions Core Tools logs
- Enable detailed logging in local.settings.json
- Use Azure Storage Explorer for data inspection
- Check Azure Portal for Function execution logs

## Cost Optimization

- Use consumption plan for Functions (pay per execution)
- Optimize storage queries to minimize RU usage
- Implement proper blob storage lifecycle policies
- Monitor and set up budget alerts