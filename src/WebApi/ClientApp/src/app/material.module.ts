import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatIconModule,
  MatButtonModule,
  MatDialogModule,
  MatInputModule,
  MatToolbarModule,
  MatCardModule,
  MatMenuModule,
  MatDividerModule,
  MatTableModule,
  MatSidenavModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatFormFieldModule,
  MatProgressSpinnerModule,
  MatExpansionModule
} from '@angular/material/';
import { CdkTableModule } from '@angular/cdk/table';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CdkTableModule,
    FormsModule,
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    MatInputModule,
    MatToolbarModule,
    MatCardModule,
    MatMenuModule,
    MatDividerModule,
    MatTableModule,
    MatSidenavModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    MatExpansionModule,
    MatSortModule
  ], exports: [
    CdkTableModule,
    FormsModule,
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    MatInputModule,
    MatToolbarModule,
    MatCardModule,
    MatMenuModule,
    MatDividerModule,
    MatTableModule,
    MatSidenavModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    MatExpansionModule,
    MatSortModule
  ],
  declarations: []
})

export class MaterialModule { }
