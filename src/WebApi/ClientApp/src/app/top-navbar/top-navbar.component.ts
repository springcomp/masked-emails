import { Component, OnInit, Inject } from '@angular/core';
import { ProfileService, Claim } from '../shared/services/profile.service';
import { Profile } from '../shared/models/model';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { MatDialog } from '@angular/material';
import { ProfileDialogComponent } from './profile-dialog/profile-dialog.component';


@Component({
  selector: 'top-navbar',
  templateUrl: './top-navbar.component.html',
  styleUrls: ['./top-navbar.component.scss']
})
export class TopNavbarComponent implements OnInit {

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
    const dialogRef = this.dialog.open(ProfileDialogComponent, {
      data: { profile: this.my }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.event == 'UpdateProfile') {
        this.my = result.data;
      }
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
