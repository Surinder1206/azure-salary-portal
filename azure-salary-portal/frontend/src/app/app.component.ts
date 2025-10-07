import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    RouterModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    MatListModule
  ],
  template: `
    <mat-sidenav-container class="sidenav-container">
      <mat-sidenav #drawer class="sidenav" fixedInViewport
          [attr.role]="'dialog'"
          [mode]="'over'"
          [opened]="false">
        <mat-toolbar>Menu</mat-toolbar>
        <mat-nav-list>
          <a mat-list-item routerLink="/dashboard">
            <mat-icon>dashboard</mat-icon>
            <span>Dashboard</span>
          </a>
          <a mat-list-item routerLink="/payslips">
            <mat-icon>receipt</mat-icon>
            <span>Payslips</span>
          </a>
          <a mat-list-item routerLink="/documents">
            <mat-icon>description</mat-icon>
            <span>Documents</span>
          </a>
          <a mat-list-item routerLink="/admin" *ngIf="isAdmin">
            <mat-icon>settings</mat-icon>
            <span>Admin</span>
          </a>
        </mat-nav-list>
      </mat-sidenav>

      <mat-sidenav-content>
        <mat-toolbar color="primary">
          <button
            type="button"
            aria-label="Toggle sidenav"
            mat-icon-button
            (click)="drawer.toggle()">
            <mat-icon aria-label="Side nav toggle icon">menu</mat-icon>
          </button>
          <span>Payslip Portal</span>

          <div class="spacer"></div>

          <button mat-button *ngIf="!isAuthenticated" (click)="login()">
            <mat-icon>login</mat-icon>
            Login
          </button>

          <div *ngIf="isAuthenticated" class="user-menu">
            <span>{{ userDisplayName }}</span>
            <button mat-button (click)="logout()">
              <mat-icon>logout</mat-icon>
              Logout
            </button>
          </div>
        </mat-toolbar>

        <div class="main-content">
          <router-outlet></router-outlet>
        </div>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: [`
    .sidenav-container {
      height: 100%;
    }

    .sidenav {
      width: 200px;
    }

    .sidenav .mat-toolbar {
      background: inherit;
    }

    .mat-toolbar.mat-primary {
      position: sticky;
      top: 0;
      z-index: 1;
    }

    .spacer {
      flex: 1 1 auto;
    }

    .user-menu {
      display: flex;
      align-items: center;
      gap: 16px;
    }
  `]
})
export class AppComponent {
  title = 'Payslip Portal';
  isAuthenticated = false;
  isAdmin = false;
  userDisplayName = '';

  constructor() {
    // TODO: Initialize authentication state
    this.checkAuthStatus();
  }

  private checkAuthStatus() {
    // TODO: Check if user is authenticated via SWA
    // This will be implemented when we add the auth service
  }

  login() {
    // TODO: Redirect to SWA auth endpoint
    window.location.href = '/.auth/login/aad';
  }

  logout() {
    // TODO: Logout via SWA
    window.location.href = '/.auth/logout';
  }
}