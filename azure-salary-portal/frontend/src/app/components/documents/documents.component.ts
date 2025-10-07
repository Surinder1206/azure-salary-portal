import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';

interface Document {
  id: string;
  name: string;
  type: string;
  date: Date;
  size: string;
}

@Component({
  selector: 'app-documents',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule, MatTableModule],
  template: `
    <div class="container">
      <h1>HR Documents</h1>

      <mat-card>
        <mat-card-content>
          <table mat-table [dataSource]="documents" class="mat-elevation-z8">
            <ng-container matColumnDef="name">
              <th mat-header-cell *matHeaderCellDef>Document Name</th>
              <td mat-cell *matCellDef="let doc">{{ doc.name }}</td>
            </ng-container>

            <ng-container matColumnDef="type">
              <th mat-header-cell *matHeaderCellDef>Type</th>
              <td mat-cell *matCellDef="let doc">{{ doc.type }}</td>
            </ng-container>

            <ng-container matColumnDef="date">
              <th mat-header-cell *matHeaderCellDef>Date</th>
              <td mat-cell *matCellDef="let doc">{{ doc.date | date }}</td>
            </ng-container>

            <ng-container matColumnDef="size">
              <th mat-header-cell *matHeaderCellDef>Size</th>
              <td mat-cell *matCellDef="let doc">{{ doc.size }}</td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef>Actions</th>
              <td mat-cell *matCellDef="let doc">
                <button mat-icon-button color="primary" (click)="downloadDocument(doc.id)">
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
export class DocumentsComponent implements OnInit {
  displayedColumns: string[] = ['name', 'type', 'date', 'size', 'actions'];
  documents: Document[] = [];

  ngOnInit() {
    this.loadDocuments();
  }

  loadDocuments() {
    // TODO: Load from API
    this.documents = [
      {
        id: '1',
        name: 'P60 2024-25',
        type: 'P60',
        date: new Date('2025-04-05'),
        size: '245 KB'
      },
      {
        id: '2',
        name: 'P45 2024',
        type: 'P45',
        date: new Date('2024-12-15'),
        size: '180 KB'
      }
    ];
  }

  downloadDocument(id: string) {
    // TODO: Call API to get download URL
    console.log('Downloading document:', id);
  }
}