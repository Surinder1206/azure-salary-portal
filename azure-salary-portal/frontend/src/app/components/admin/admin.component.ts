import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [
    CommonModule,
    MatTabsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule
  ],
  template: `
    <div class="container">
      <h1>Administration Panel</h1>

      <mat-tab-group>
        <mat-tab label="Upload Payslips">
          <div class="tab-content">
            <mat-card>
              <mat-card-header>
                <mat-card-title>Batch Upload Payslips</mat-card-title>
              </mat-card-header>
              <mat-card-content>
                <p>Upload CSV file with employee data and corresponding PDF payslips.</p>

                <div class="upload-section">
                  <input type="file" accept=".csv" (change)="onCsvFileSelected($event)" #csvFile>
                  <button mat-raised-button color="primary" (click)="csvFile.click()">
                    <mat-icon>upload_file</mat-icon>
                    Select CSV File
                  </button>
                </div>

                <div class="upload-section">
                  <input type="file" accept=".pdf" multiple (change)="onPdfFilesSelected($event)" #pdfFiles>
                  <button mat-raised-button color="primary" (click)="pdfFiles.click()">
                    <mat-icon>upload_file</mat-icon>
                    Select PDF Files
                  </button>
                </div>

                <button mat-raised-button color="accent" (click)="uploadPayslips()" [disabled]="!canUpload()">
                  <mat-icon>cloud_upload</mat-icon>
                  Upload Payslips
                </button>
              </mat-card-content>
            </mat-card>
          </div>
        </mat-tab>

        <mat-tab label="Tax Configuration">
          <div class="tab-content">
            <mat-card>
              <mat-card-header>
                <mat-card-title>Tax Year Configuration</mat-card-title>
              </mat-card-header>
              <mat-card-content>
                <mat-form-field>
                  <mat-label>Tax Year</mat-label>
                  <input matInput [(ngModel)]="taxYear" placeholder="2024-25">
                </mat-form-field>

                <mat-form-field>
                  <mat-label>PAYE Tax Threshold</mat-label>
                  <input matInput type="number" [(ngModel)]="payeThreshold" placeholder="12570">
                </mat-form-field>

                <mat-form-field>
                  <mat-label>NI Threshold</mat-label>
                  <input matInput type="number" [(ngModel)]="niThreshold" placeholder="12570">
                </mat-form-field>

                <div class="form-actions">
                  <button mat-raised-button color="primary" (click)="saveTaxConfig()">
                    Save Configuration
                  </button>
                  <button mat-raised-button (click)="previewTaxCalculation()">
                    Preview Calculation
                  </button>
                </div>
              </mat-card-content>
            </mat-card>
          </div>
        </mat-tab>

        <mat-tab label="Employee Management">
          <div class="tab-content">
            <mat-card>
              <mat-card-header>
                <mat-card-title>Employee Management</mat-card-title>
              </mat-card-header>
              <mat-card-content>
                <p>Manage employee access and roles.</p>
                <!-- TODO: Add employee list and management UI -->
                <button mat-raised-button color="primary">
                  <mat-icon>person_add</mat-icon>
                  Add Employee
                </button>
              </mat-card-content>
            </mat-card>
          </div>
        </mat-tab>
      </mat-tab-group>
    </div>
  `,
  styles: [`
    .container {
      padding: 20px;
    }

    .tab-content {
      padding: 20px 0;
    }

    .upload-section {
      margin: 16px 0;
      display: flex;
      align-items: center;
      gap: 16px;
    }

    .upload-section input[type="file"] {
      display: none;
    }

    mat-form-field {
      width: 100%;
      margin-bottom: 16px;
    }

    .form-actions {
      display: flex;
      gap: 16px;
      margin-top: 16px;
    }
  `]
})
export class AdminComponent {
  taxYear = '2024-25';
  payeThreshold = 12570;
  niThreshold = 12570;

  selectedCsvFile: File | null = null;
  selectedPdfFiles: File[] = [];

  onCsvFileSelected(event: any) {
    this.selectedCsvFile = event.target.files[0];
  }

  onPdfFilesSelected(event: any) {
    this.selectedPdfFiles = Array.from(event.target.files);
  }

  canUpload(): boolean {
    return this.selectedCsvFile != null && this.selectedPdfFiles.length > 0;
  }

  uploadPayslips() {
    // TODO: Call API to upload files
    console.log('Uploading payslips...', {
      csv: this.selectedCsvFile,
      pdfs: this.selectedPdfFiles
    });
  }

  saveTaxConfig() {
    // TODO: Call API to save tax configuration
    console.log('Saving tax config...', {
      taxYear: this.taxYear,
      payeThreshold: this.payeThreshold,
      niThreshold: this.niThreshold
    });
  }

  previewTaxCalculation() {
    // TODO: Call API to preview tax calculation
    console.log('Previewing tax calculation...');
  }
}