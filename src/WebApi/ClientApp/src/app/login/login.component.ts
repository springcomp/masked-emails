import { Component, OnInit } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';

import { LoaderService } from '../shared/services/loader.service'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(public oidcSecurityService: OidcSecurityService, private loaderSvc : LoaderService) { }

  ngOnInit() {
    this.loaderSvc.startLoading();
    this.oidcSecurityService.checkAuth().subscribe(() => this.oidcSecurityService.authorize());
  }

}
