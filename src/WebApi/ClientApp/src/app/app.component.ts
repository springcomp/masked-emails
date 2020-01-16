import { Component, OnDestroy, OnInit } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { LoaderService } from './shared/services/loader.service';

@Component({
    selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {

    public isAuthenticated: boolean;

    constructor(
      public oidcSecurityService: OidcSecurityService,
      public loaderSvc: LoaderService
    ) {
      this.loaderSvc.stopLoader();
        if (this.oidcSecurityService.moduleSetup) {
            this.doCallbackLogicIfRequired();
        } else {
            this.oidcSecurityService.onModuleSetup.subscribe(() => {
                this.doCallbackLogicIfRequired();
            });
        }

      
    }

    ngOnInit() {
        this.oidcSecurityService.getIsAuthorized().subscribe(auth => {
            this.isAuthenticated = auth;
        });
    }

    ngOnDestroy(): void { }

    login() {
        this.oidcSecurityService.authorize();
    }

  get dataLoaded(): boolean {
    return this.loaderSvc.dataLoaded;
  }

    private doCallbackLogicIfRequired() {
        var url = window.location.toString();
        this.oidcSecurityService.authorizedCallbackWithCode(url);
    }
}
