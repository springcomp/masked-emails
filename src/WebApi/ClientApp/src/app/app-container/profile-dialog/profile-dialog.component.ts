import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Profile } from '../../shared/models/model';
import { ProfileService } from '../../shared/services/profile.service';

@Component({
  selector: 'app-profile-dialog',
  templateUrl: './profile-dialog.component.html',
  styleUrls: ['./profile-dialog.component.scss']
})
export class ProfileDialogComponent {

  public newForwardingAddress: string = '';

  constructor(public dialogRef: MatDialogRef<ProfileDialogComponent>,
    private profileService: ProfileService,
    @Inject(MAT_DIALOG_DATA) private data: { profile: Profile } ) {

  }

  public close() {
    this.dialogRef.close();
  }

  public save() {
      this.onUpdateForwardingAddress();
  }

  private onUpdateForwardingAddress(): void {
    var profile: Profile = {
      displayName: this.data.profile.displayName,
      forwardingAddress: this.newForwardingAddress
    };

    this.profileService.updateProfile(profile).subscribe(updated => {
      this.dialogRef.close({ event: 'UpdateProfile', data: updated });
    });
  }

}
