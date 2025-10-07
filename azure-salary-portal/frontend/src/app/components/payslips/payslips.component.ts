import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';

interface Payslip {
  id: string;
  taxYear: string;
  period: string;
  grossPay: number;
  netPay: number;
  date: Date;
}

@Component({
  selector: 'app-payslips',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule, MatTableModule],
  template: `
    <div class="container">
      <h1>My Payslips</h1>

      <mat-card>
        <mat-card-content>
          <table mat-table [dataSource]="payslips" class="mat-elevation-z8">
            <ng-container matColumnDef="taxYear">
              <th mat-header-cell *matHeaderCellDef>Tax Year</th>
              <td mat-cell *matCellDef="let payslip">{{ payslip.taxYear }}</td>
            </ng-container>

            <ng-container matColumnDef="period">
              <th mat-header-cell *matHeaderCellDef>Period</th>
              <td mat-cell *matCellDef="let payslip">{{ payslip.period }}</td>
            </ng-container>

            <ng-container matColumnDef="grossPay">
              <th mat-header-cell *matHeaderCellDef>Gross Pay</th>
              <td mat-cell *matCellDef="let payslip">{{ payslip.grossPay | currency:'GBP' }}</td>
            </ng-container>

            <ng-container matColumnDef="netPay">
              <th mat-header-cell *matHeaderCellDef>Net Pay</th>
              <td mat-cell *matCellDef="let payslip">{{ payslip.netPay | currency:'GBP' }}</td>
            </ng-container>

            <ng-container matColumnDef="date">
              <th mat-header-cell *matHeaderCellDef>Date</th>
              <td mat-cell *matCellDef="let payslip">{{ payslip.date | date }}</td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef>Actions</th>
              <td mat-cell *matCellDef="let payslip">
                <button mat-icon-button color="primary" (click)="downloadPayslip(payslip.id)">
                  <mat-icon>download</mat-icon>
                </button>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
          </table>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .container {
      padding: 20px;
    }

    table {
      width: 100%;
    }
  `]
})
export class PayslipsComponent implements OnInit {
  displayedColumns: string[] = ['taxYear', 'period', 'grossPay', 'netPay', 'date', 'actions'];
  payslips: Payslip[] = [];

  ngOnInit() {
    this.loadPayslips();
  }

  loadPayslips() {
    // TODO: Load from API
    this.payslips = [
      {
        id: '1',
        taxYear: '2024-25',
        period: 'March 2025',
        grossPay: 3000,
        netPay: 2400,
        date: new Date('2025-03-31')
      },
      {
        id: '2',
        taxYear: '2024-25',
        period: 'February 2025',
        grossPay: 3000,
        netPay: 2400,
        date: new Date('2025-02-28')
      }
    ];
  }

  downloadPayslip(id: string) {
    // TODO: Call API to get download URL
    console.log('Downloading payslip:', id);
  }
}