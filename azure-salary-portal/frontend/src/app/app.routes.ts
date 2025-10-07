import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./components/dashboard/dashboard.component').then(c => c.DashboardComponent)
  },
  {
    path: 'payslips',
    loadComponent: () => import('./components/payslips/payslips.component').then(c => c.PayslipsComponent)
  },
  {
    path: 'documents',
    loadComponent: () => import('./components/documents/documents.component').then(c => c.DocumentsComponent)
  },
  {
    path: 'admin',
    loadComponent: () => import('./components/admin/admin.component').then(c => c.AdminComponent),
    // canActivate: [AdminGuard] // TODO: Add admin guard
  },
  {
    path: '**',
    redirectTo: '/dashboard'
  }
];