import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  MatIconModule,
  MatButtonModule,
  MatDialogModule,
  MatInputModule,
  MatToolbarModule,
  MatSidenavModule,
  MatCardModule,
  MatMenuModule,
  MatDividerModule
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
    MatSidenavModule,
    MatCardModule,
    MatMenuModule,
    MatDividerModule
  ], exports: [
    BrowserAnimationsModule,
    FormsModule,
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    MatInputModule,
    MatToolbarModule,
    MatSidenavModule,
    MatCardModule,
    MatMenuModule,
    MatDividerModule
  ],
  declarations: []
})

export class MaterialModule { }
