import { Component } from '@angular/core';
import { ProfileService, Claim } from '../shared/services/profile.service';
import { Profile } from '../shared/models/model';
import { AuthService } from '../core/auth.service';
import { MatDialog } from '@angular/material';
import { ProfileDialogComponent } from './profile-dialog/profile-dialog.component';


@Component({
  selector: 'top-navbar',
  templateUrl: './top-navbar.component.html',
  styleUrls: ['./top-navbar.component.scss']
})
export class TopNavbarComponent {

  public isAuthenticated: boolean;

  public my: Profile = undefined;
  public claims: Claim[] = [];

  constructor(
    private profileService: ProfileService,
    private dialog: MatDialog,
    public authService: AuthService
  ) {
    this.authService.getIsAuthorized().subscribe(auth => {
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
    this.authService.login();
  }

  public openDialog(): void {
    //Open dialog window to update profile
    const dialogRef = this.dialog.open(ProfileDialogComponent, {
      data: { profile: this.my }
    });

    //Action to handle after closing the dialog window
    dialogRef.afterClosed().subscribe(result => {
      if (result && result.event == 'UpdateProfile') {
        this.my = result.data;
      }
    });
  }

  public logout() {
    this.authService.logout();
  }

  private loadProfile(): void {
   this.profileService.getProfile()
      .subscribe(profile => this.my = profile);
    this.profileService.getClaims()
      .subscribe(claims => this.claims = claims);
  }

}
