import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { MaskedEmailsComponent } from './masked-emails/masked-emails.component';

import { AuthorizationGuard } from './core/authorization-guard';

const routes: Routes = [
  { path: '', component: MaskedEmailsComponent, canActivate: [AuthorizationGuard] },
  { path: 'login', component: LoginComponent },

  { path: 'masked-emails', component: MaskedEmailsComponent, canActivate: [AuthorizationGuard]},

  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
