import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Profile } from '../../shared/models/model';
import { ProfileDialogComponent } from '../profile-dialog/profile-dialog.component';

@Component({
  selector: 'app-edit-forwarding-address',
  templateUrl: './edit-forwarding-address.component.html',
  styleUrls: ['./edit-forwarding-address.component.scss']
})
export class EditForwardingAddressComponent implements OnInit {
  @Input() userIsAuthenticated: boolean;
  @Input() user: Profile;

  @Output() updateUserModel = new EventEmitter<Profile>();

  constructor(private dialog: MatDialog) { }

  ngOnInit() {
  }

  public openDialog(): void {
    //Open dialog window to update profile
    const dialogRef = this.dialog.open(ProfileDialogComponent, {
      data: { profile: this.user }
    });

    //Action to handle after closing the dialog window
    dialogRef.afterClosed().subscribe(result => {
      if (result && result.event == 'UpdateProfile') {
        this.updateUserModel.emit(result.data);
      }
    });
  }
}
