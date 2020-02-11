import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthCallbackComponent } from './auth-callback/auth-callback.component'
import { HomeComponent } from './home/home.component'
import { LoginComponent } from './login/login.component';
import { MaskedEmailsComponent } from './masked-emails/masked-emails.component';
import { MessagesComponent } from './messages/messages.component';

import { AuthorizationGuard } from './core/authorization-guard';

const routes: Routes = [
  //{ path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'auth-callback', component: AuthCallbackComponent },
  { path: 'masked-emails', component: MaskedEmailsComponent, canActivate: [AuthorizationGuard]},
  { path: 'inbox', component: MessagesComponent, canActivate: [AuthorizationGuard]},

  { path: '**', redirectTo: 'masked-emails' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
