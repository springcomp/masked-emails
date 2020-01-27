import { Injectable, OnDestroy } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable, Subscription, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnDestroy {
  public isAuthorized: boolean = false;
  private isAuthorizedSubscription: Subscription = new Subscription;

  constructor(private oidcSecurityService: OidcSecurityService) { }

  ngOnDestroy(): void {
    if (this.isAuthorizedSubscription) {
      this.isAuthorizedSubscription.unsubscribe();
    }
  }

  public initAuthentication() {
    if (this.oidcSecurityService.moduleSetup) {
      this.doCallbackLogicIfRequired();
    } else {
      this.oidcSecurityService.onModuleSetup.subscribe(() => {
        this.doCallbackLogicIfRequired();
      });
    }

    this.isAuthorizedSubscription = this.oidcSecurityService.getIsAuthorized().subscribe((isAuthorized => {
      this.isAuthorized = isAuthorized;
    }));
  }

  public login(): void {
    this.oidcSecurityService.authorize();
  }

  public logout(): void {
    this.oidcSecurityService.logoff();
  }

  public getIsAuthorized(): Observable<boolean> {
    return this.oidcSecurityService.getIsAuthorized();
  }

  private doCallbackLogicIfRequired() {
    var url = window.location.toString();
    this.oidcSecurityService.authorizedCallbackWithCode(url);
  }
}
