import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormGroup, FormControl } from '@angular/forms';
import { Profile } from '../../model';
import { ProfileService } from '../../profile.service';

@Component({
  selector: 'app-profile-dialog',
  templateUrl: './profile-dialog.component.html',
  styleUrls: ['./profile-dialog.component.scss']
})
export class ProfileDialogComponent {

  public newForwardingAddress: string = '';

  constructor(public dialogRef: MatDialogRef<ProfileDialogComponent>,
    private profileService: ProfileService,
    @Inject(MAT_DIALOG_DATA) private profile) {

  }

  public close() {
    this.dialogRef.close();
  }

  public save() {
      this.onUpdateForwardingAddress();
  }

  private onUpdateForwardingAddress(): void {
    console.log("updating forwarding address...");
    var profile: Profile = {
      ...this.profile,
      forwardingAddress: this.newForwardingAddress
    };

    this.profileService.updateProfile(profile).subscribe(updated => {
      this.profile = updated;
      this.close();
    });
  }

}
