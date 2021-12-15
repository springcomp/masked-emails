import { Injectable, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticatedResult, LoginResponse, OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable, Subscription } from 'rxjs';
import { map } from 'rxjs/internal/operators/map';

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
      .subscribe((response: LoginResponse) => {
        if (!response.isAuthenticated) {
          if ('/login' !== window.location.pathname) {
            this.router.navigate(['/login']);
          }
        }
        if (response.isAuthenticated) {
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
    return this.oidcSecurityService.isAuthenticated$.pipe(
      map(
        (authenticatedResult: AuthenticatedResult) => 
          authenticatedResult.isAuthenticated
      )
    );
  }
}
