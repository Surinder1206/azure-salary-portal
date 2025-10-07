# API Documentation

## Authentication

All API endpoints require authentication via Azure Static Web Apps built-in authentication. The following headers are automatically provided:

- `X-MS-CLIENT-PRINCIPAL-ID`: User ID
- `X-MS-CLIENT-PRINCIPAL-NAME`: User email
- `X-MS-CLIENT-PRINCIPAL-ROLES`: Comma-separated list of roles

## Endpoints

### Authentication

#### GET /api/me
Get current user information.

**Response:**
```json
{
  "id": "user-id",
  "email": "user@example.com",
  "displayName": "User Name",
  "roles": ["authenticated"],
  "isAdmin": false,
  "isAuthenticated": true
}
```

### Payslips

#### GET /api/payslips
Get payslips for the current user.

**Query Parameters:**
- `year` (optional): Filter by tax year
- `period` (optional): Filter by period

**Response:**
```json
[
  {
    "id": "payslip-id",
    "taxYear": "2024-25",
    "period": "March 2025",
    "grossPay": 3000.00,
    "netPay": 2400.00,
    "payDate": "2025-03-31T00:00:00Z",
    "fileName": "payslip-march-2025.pdf"
  }
]
```

#### GET /api/payslips/{id}/download
Get download URL for a specific payslip.

**Response:**
```json
{
  "downloadUrl": "https://storage.blob.core.windows.net/...",
  "fileName": "payslip-march-2025.pdf",
  "contentType": "application/pdf",
  "fileSize": 245760,
  "expiresAt": "2025-03-07T13:00:00Z"
}
```

### Documents

#### GET /api/docs
Get HR documents for the current user.

**Response:**
```json
[
  {
    "id": "doc-id",
    "name": "P60 2024-25",
    "type": "P60",
    "description": "Annual tax summary",
    "taxYear": "2024-25",
    "createdDate": "2025-04-05T00:00:00Z",
    "fileSize": 245760,
    "fileName": "p60-2024-25.pdf"
  }
]
```

#### GET /api/docs/{id}/download
Get download URL for a specific document.

**Response:**
```json
{
  "downloadUrl": "https://storage.blob.core.windows.net/...",
  "fileName": "p60-2024-25.pdf",
  "contentType": "application/pdf",
  "fileSize": 245760,
  "expiresAt": "2025-03-07T13:00:00Z"
}
```

### Admin Endpoints

*Requires `admin` role*

#### POST /api/admin/payslips/batch
Batch upload payslips with CSV metadata and PDF files.

**Request:** Multipart form data
- `csv`: CSV file with employee data
- `files[]`: PDF files

**Response:**
```json
{
  "message": "Batch upload completed",
  "status": "Success",
  "processedCount": 25,
  "errors": []
}
```

#### GET /api/admin/config/tax-year/{year}
Get tax configuration for a specific year.

#### PUT /api/admin/config/tax-year/{year}
Update tax configuration for a specific year.

**Request:**
```json
{
  "taxYear": "2024-25",
  "payeThreshold": 12570.00,
  "niThreshold": 12570.00,
  "basicRate": 0.20,
  "higherRate": 0.40,
  "higherRateThreshold": 50270.00
}
```

#### POST /api/admin/tax/preview
Preview tax calculation for given salary.

**Request:**
```json
{
  "grossSalary": 30000.00,
  "taxYear": "2024-25"
}
```

**Response:**
```json
{
  "grossSalary": 30000.00,
  "taxDeducted": 3486.00,
  "niDeducted": 2088.00,
  "netSalary": 24426.00,
  "calculationBreakdown": {
    "payeThreshold": 12570.00,
    "taxableAmount": 17430.00,
    "basicRateTax": 3486.00,
    "niAmount": 2088.00
  }
}
```

## Error Responses

All endpoints return standard HTTP status codes:

- `200`: Success
- `400`: Bad Request
- `401`: Unauthorized
- `403`: Forbidden (insufficient permissions)
- `404`: Not Found
- `500`: Internal Server Error

Error response format:
```json
{
  "error": "Error message",
  "details": "Additional error details"
}
```