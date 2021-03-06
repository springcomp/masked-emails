import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AddressService } from '../../shared/services/address.service';
import { MaskedEmail } from '../../shared/models/model';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-remove-masked-email-address-dialog',
  templateUrl: './remove-masked-email-address-dialog.component.html',
  styleUrls: ['./remove-masked-email-address-dialog.component.scss']
})
export class RemoveMaskedEmailAddressDialogComponent {
  public addressForm = new FormGroup({
    address: new FormControl(''),
  });

  private removingAddress: MaskedEmail;

  constructor(public dialogRef: MatDialogRef<RemoveMaskedEmailAddressDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { removingAddress: MaskedEmail },
    private addressService: AddressService,
    private formBuilder: FormBuilder) {

    this.addressForm = this.formBuilder.group({
      address: [this.data.removingAddress.emailAddress],
    });

    this.removingAddress = this.data.removingAddress;

    console.log(this.data.removingAddress);
  }

  ngOnInit() {
  }

  public close(): void {
    this.dialogRef.close();
  }

  public confirm(): void {

    console.log(this.addressForm);
    console.log(this.addressForm.get('address'));

    this.onDelete(this.removingAddress);
  }

  private onDelete(address: MaskedEmail): void {
    this.addressService.deleteAddress(address.emailAddress)
      .subscribe(_ => {
        this.dialogRef.close({ event: 'Confirm' });
        this.removingAddress = undefined;
      });
  }
}
