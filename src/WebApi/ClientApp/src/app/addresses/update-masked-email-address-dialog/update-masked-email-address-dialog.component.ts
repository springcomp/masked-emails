import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AddressService } from '../../address.service';
import { Address, MaskedEmail, MaskedEmailRequest, UpdateMaskedEmailRequest } from '../../model';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-update-masked-email-address-dialog',
  templateUrl: './update-masked-email-address-dialog.component.html',
  styleUrls: ['./update-masked-email-address-dialog.component.scss']
})
export class UpdateMaskedEmailAddressDialogComponent {
  public addressForm = new FormGroup({
    name: new FormControl(''),
    description: new FormControl('')
  });

  public newAddressName: string;
  public newAddressDescription: string;
  private updatingAddress: MaskedEmail;

  constructor(public dialogRef: MatDialogRef<UpdateMaskedEmailAddressDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { updatingAddress: MaskedEmail },
    private addressService: AddressService,
    private formBuilder: FormBuilder) {

    this.addressForm = this.formBuilder.group({
      name: [this.data.updatingAddress.name, Validators.required],
      description: [this.data.updatingAddress.description],
    });

    this.updatingAddress = this.data.updatingAddress;
  }

  ngOnInit() {
  }

  public close(): void {
    this.dialogRef.close();
  }

  public update(): void {

    if (
      this.updatingAddress.name !== this.newAddressName ||
      this.updatingAddress.description !== this.newAddressDescription
    ) {
      this.updatingAddress.name = this.newAddressName;
      this.updatingAddress.description = this.newAddressDescription;

      this.onUpdate(this.updatingAddress);
    }

    this.updatingAddress = undefined;
  }

  public getErrorMessage() {
    return this.addressForm.get('name').hasError('required') ? 'You must enter a value' :
      '';
  }

  private onUpdate(address: MaskedEmail): void {
    const updateRequest: UpdateMaskedEmailRequest = {
      name: address.name,
      description: address.description,
    };
    this.addressService.updateAddress(address.emailAddress, updateRequest)
      .subscribe(_ => { this.close() });
  }

}
