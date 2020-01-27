import { Component, OnDestroy, OnInit } from '@angular/core';
//import { OidcSecurityService } from 'angular-auth-oidc-client';
import { AuthService } from './core/auth.service'
import { LoaderService } from './shared/services/loader.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {

  constructor(
    public authService: AuthService,
    public loaderSvc: LoaderService
  ) {
    this.loaderSvc.stopLoader();
  }

  ngOnInit() {
    this.authService.initAuthentication();
  }

  ngOnDestroy(): void {
    this.authService.ngOnDestroy();
  }

}
