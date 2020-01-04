import { Component, OnInit } from '@angular/core';
import { ModalService } from '../core/_modal/modal.service';
import { ProfileService, Claim } from '../profile.service';
import { Profile } from '../model';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css']
})
export class MyProfileComponent implements OnInit {

  private UPDATE_FORWARDING_ADDRESS_DIALOG: string = "update-forwarding-address-dialog";

  public my: Profile = undefined;
  public claims: Claim[] = [];

  public newForwardingAddress: string = '';

  constructor(
    private profileService: ProfileService,
    private modalService: ModalService
  ) { }

  private onUpdateForwardingAddress(): void {
    console.log("updating forwarding address...");
    var profile: Profile = {
      ...this.my,
      forwardingAddress: this.newForwardingAddress
    };
    this.profileService.updateProfile(profile).subscribe(updated =>
      this.my = updated);
  }

  ngOnInit() {
    this.loadProfile();
  }

  openDialog(): void {
    this.newForwardingAddress = "";
    this.modalService.open(this.UPDATE_FORWARDING_ADDRESS_DIALOG);
  }
  closeDialog(status: boolean): void {
    this.modalService.close(this.UPDATE_FORWARDING_ADDRESS_DIALOG);
    if (status) {
      this.onUpdateForwardingAddress();
    }
  }

  private loadProfile(): void {
    this.profileService.getProfile()
      .subscribe(profile => this.my = profile);
    this.profileService.getClaims()
      .subscribe(claims => this.claims = claims);
  }
}
