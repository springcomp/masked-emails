import { Injectable, OnDestroy } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable, Subscription } from 'rxjs';

import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnDestroy {
  public isAuthorized: boolean = false;
  private isAuthorizedSubscription: Subscription = new Subscription;

  constructor(private oidcSecurityService: OidcSecurityService, private router: Router) { }

  ngOnDestroy(): void {
    if (this.isAuthorizedSubscription) {
      this.isAuthorizedSubscription.unsubscribe();
    }
  }

  public initAuthentication() {
    this.oidcSecurityService
      .checkAuth()

      .subscribe((isAuthenticated) => {
        if (!isAuthenticated) {
          if ('/login' !== window.location.pathname) {
            this.router.navigate(['/login']);
          }
        }
        if (isAuthenticated) {
          this.router.navigate(['/masked-emails']);
        }
      });
  }

  public login(): void {
    this.oidcSecurityService.authorize();
  }

  public logout(): void {
    this.oidcSecurityService.logoff();
  }

  public getIsAuthorized(): Observable<boolean> {
    return this.oidcSecurityService.isAuthenticated$;
  }
}
