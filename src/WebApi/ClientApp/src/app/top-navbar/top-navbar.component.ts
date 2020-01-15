import { Component, OnInit, Inject } from '@angular/core';
import { ProfileService, Claim } from '../profile.service';
import { Profile } from '../model';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { MatDialog } from '@angular/material';
import { ProfileDialogComponent } from './profile-dialog/profile-dialog.component';


@Component({
  selector: 'top-navbar',
  templateUrl: './top-navbar.component.html',
  styleUrls: ['./top-navbar.component.scss']
})
export class TopNavbarComponent implements OnInit {

  private UPDATE_FORWARDING_ADDRESS_DIALOG: string = "update-forwarding-address-dialog";

  public isAuthenticated: boolean;

  public my: Profile = undefined;
  public claims: Claim[] = [];

  constructor(
    private profileService: ProfileService,
    private dialog: MatDialog,
    public oidcSecurityService: OidcSecurityService
  ) {
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
      if (auth) {
        this.loadProfile();
      }
    });

  }

  get forwardingAddress(): string {
    return this.my && this.my.forwardingAddress ? this.my.forwardingAddress : '';
  }

  get userIsAuthenticated(): boolean {
    return this.isAuthenticated && this.my != undefined;
  }

  public login() {
    this.oidcSecurityService.authorize();
  }

  public openDialog(): void {
    this.dialog.open(ProfileDialogComponent, {
      data: { profile: this.my }
    });
  }

  public logout() {
    this.oidcSecurityService.logoff();
  }

  private loadProfile(): void {
    this.profileService.getProfile()
      .subscribe(profile => this.my = profile);
    this.profileService.getClaims()
      .subscribe(claims => this.claims = claims);
  }

  private doCallbackLogicIfRequired() {
    var url = window.location.toString();
    this.oidcSecurityService.authorizedCallbackWithCode(url);
  }

}
