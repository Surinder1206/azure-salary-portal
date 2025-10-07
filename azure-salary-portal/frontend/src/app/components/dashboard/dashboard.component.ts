import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule],
  template: `
    <div class="container">
      <h1>Welcome to Payslip Portal</h1>

      <div class="dashboard-cards">
        <mat-card class="dashboard-card">
          <mat-card-header>
            <mat-card-title>
              <mat-icon>receipt</mat-icon>
              Recent Payslips
            </mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <p>View your latest salary slips and payment history.</p>
          </mat-card-content>
          <mat-card-actions>
            <button mat-raised-button color="primary" routerLink="/payslips">
              View Payslips
            </button>
          </mat-card-actions>
        </mat-card>

        <mat-card class="dashboard-card">
          <mat-card-header>
            <mat-card-title>
              <mat-icon>description</mat-icon>
              HR Documents
            </mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <p>Access P60s, P45s, and other important documents.</p>
          </mat-card-content>
          <mat-card-actions>
            <button mat-raised-button color="primary" routerLink="/documents">
              View Documents
            </button>
          </mat-card-actions>
        </mat-card>

        <mat-card class="dashboard-card" *ngIf="isAdmin">
          <mat-card-header>
            <mat-card-title>
              <mat-icon>settings</mat-icon>
              Administration
            </mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <p>Manage employees, upload payslips, and configure settings.</p>
          </mat-card-content>
          <mat-card-actions>
            <button mat-raised-button color="accent" routerLink="/admin">
              Admin Panel
            </button>
          </mat-card-actions>
        </mat-card>
      </div>
    </div>
  `,
  styles: [`
    .container {
      padding: 20px;
    }

    .dashboard-cards {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
      gap: 20px;
      margin-top: 20px;
    }

    .dashboard-card {
      min-height: 200px;
    }

    .dashboard-card mat-card-header {
      margin-bottom: 16px;
    }

    .dashboard-card mat-card-title {
      display: flex;
      align-items: center;
      gap: 8px;
    }
  `]
})
export class DashboardComponent {
  isAdmin = false; // TODO: Get from auth service
}