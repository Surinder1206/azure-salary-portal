# Frontend README

## Angular 17 Payslip Portal Frontend

This is the frontend application for the Azure Salary Slip Portal, built with Angular 17 and Angular Material.

## Features

- **Modern Angular 17**: Standalone components with latest features
- **Material Design**: Clean, professional UI using Angular Material
- **Responsive Layout**: Works on desktop and mobile devices
- **Authentication Integration**: Seamless integration with Azure Static Web Apps authentication
- **Role-Based Access**: Different views for regular users and administrators

## Project Structure

```
src/
├── app/
│   ├── components/
│   │   ├── dashboard/          # Main dashboard
│   │   ├── payslips/           # Payslip list and management
│   │   ├── documents/          # HR documents (P60, P45, etc.)
│   │   └── admin/              # Admin panel (for authorized users)
│   ├── app.component.ts        # Main app component with navigation
│   └── app.routes.ts           # Application routing
├── assets/                     # Static assets
└── styles.scss                 # Global styles
```

## Getting Started

### Prerequisites

- Node.js 18 or higher
- npm or yarn
- Angular CLI (`npm install -g @angular/cli`)

### Installation

1. **Install Dependencies**
   ```bash
   npm install
   ```

2. **Development Server**
   ```bash
   ng serve
   ```
   Navigate to `http://localhost:4200`

3. **Build for Production**
   ```bash
   ng build --prod
   ```

### Development Guidelines

#### Components

All components are **standalone** (Angular 17 feature):
- No need for NgModules
- Direct import of dependencies in component decorator
- Improved tree-shaking and performance

#### Routing

- Lazy-loaded components for better performance
- Route guards for admin-only sections (to be implemented)
- Clean URLs with Angular Router

#### Authentication

Authentication is handled by Azure Static Web Apps:
- Login/logout redirects to `/.auth/login/aad` and `/.auth/logout`
- User information retrieved from `/api/me` endpoint
- Role-based UI rendering

#### State Management

Currently using simple component state. For future enhancements, consider:
- Angular Signals (Angular 17+ feature)
- RxJS for complex async operations
- NgRx for complex state management

## API Integration

The frontend integrates with the Azure Functions backend:

- **Authentication**: `/api/me`
- **Payslips**: `/api/payslips`, `/api/payslips/{id}/download`
- **Documents**: `/api/docs`, `/api/docs/{id}/download`
- **Admin**: `/api/admin/*` (admin role required)

## Deployment

The application is automatically deployed via GitHub Actions to Azure Static Web Apps when code is pushed to the main branch.

Build configuration:
- **App Location**: `/frontend`
- **Output Location**: `dist/payslip-portal`
- **Skip API Build**: `false` (builds backend too)

## Future Enhancements

### Planned Features
- [ ] Advanced filtering and search
- [ ] Bulk download functionality
- [ ] Email notifications integration
- [ ] PWA support for mobile
- [ ] Dark theme support
- [ ] Multi-language support

### Technical Improvements
- [ ] Add comprehensive unit tests
- [ ] Implement E2E testing with Cypress
- [ ] Add performance monitoring
- [ ] Implement proper error handling
- [ ] Add loading states and skeleton screens
- [ ] Implement caching strategies

## Component Details

### Dashboard Component
- Overview cards for quick navigation
- Recent activity summary
- Admin-specific cards for admin users

### Payslips Component
- Tabular display of all payslips
- Filtering by tax year and period
- Direct download functionality
- Responsive table design

### Documents Component
- Display of HR documents (P60, P45, etc.)
- Document type categorization
- Download tracking

### Admin Component
- **Upload Tab**: Batch upload of payslips with CSV metadata
- **Tax Config Tab**: Tax year configuration management
- **Employee Tab**: Employee management (future)
- Role-based access control

## Styling

- **Angular Material Theme**: Indigo-Pink theme
- **Custom SCSS**: Global styles and component-specific styles
- **Responsive Design**: Mobile-first approach
- **Consistent Spacing**: Material Design spacing guidelines

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)

## Contributing

1. Follow Angular style guide
2. Use TypeScript strict mode
3. Write unit tests for new components
4. Follow Material Design principles
5. Ensure responsive design