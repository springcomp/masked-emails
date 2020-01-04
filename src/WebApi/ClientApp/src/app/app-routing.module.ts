import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { MaskedEmailsComponent } from './masked-emails/masked-emails.component';

import { AuthorizationGuard } from './core/authorization-guard';

const routes: Routes = [
    { path: '', component: MaskedEmailsComponent, canActivate: [ AuthorizationGuard ] },
    { path: 'forbidden', component: AppComponent },
    { path: 'unauthorized', component: AppComponent },
    { path: "masked-emails", component: MaskedEmailsComponent, canActivate: [ AuthorizationGuard ] },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
