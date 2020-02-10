import { Component } from '@angular/core';
import { ProfileService, Claim } from '../shared/services/profile.service';
import { Profile } from '../shared/models/model';
import { AuthService } from '../core/auth.service';

@Component({
  selector: 'app-container',
  templateUrl: './app-container.component.html',
  styleUrls: ['./app-container.component.scss']
})
export class AppContainerComponent {

  public isAuthenticated: boolean;

  public my: Profile = undefined;
  public claims: Claim[] = [];

  constructor(
    private profileService: ProfileService,
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

  public logout() {
    this.authService.logout();
  }

  public updateUserModel(user: Profile) {
    this.my = user;
  }

  private loadProfile(): void {
   this.profileService.getProfile()
      .subscribe(profile => this.my = profile);
    this.profileService.getClaims()
      .subscribe(claims => this.claims = claims);
  }

}
