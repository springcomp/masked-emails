import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
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
  MatSlideToggleModule,
  MatSnackBarModule,
  MatFormFieldModule
} from '@angular/material/';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    BrowserAnimationsModule,
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
    MatSlideToggleModule,
    MatSnackBarModule,
    MatFormFieldModule
  ], exports: [
    BrowserAnimationsModule,
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
    MatSlideToggleModule,
    MatSnackBarModule,
    MatFormFieldModule
  ],
  declarations: []
})

export class MaterialModule { }
