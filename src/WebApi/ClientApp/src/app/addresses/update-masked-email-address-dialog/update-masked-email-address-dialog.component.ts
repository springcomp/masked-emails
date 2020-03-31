import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AddressService } from '../../shared/services/address.service';
import { HashService } from 'src/app/shared/services/hash.service';
import { MaskedEmail, UpdateMaskedEmailRequest } from '../../shared/models/model';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { pristineOrminLength } from './pristineOrMinLength.validator';

@Component({
  selector: 'app-update-masked-email-address-dialog',
  templateUrl: './update-masked-email-address-dialog.component.html',
  styleUrls: ['./update-masked-email-address-dialog.component.scss']
})
export class UpdateMaskedEmailAddressDialogComponent {
  public addressForm = new FormGroup({
    name: new FormControl(''),
    description: new FormControl(''),
    password: new FormControl('')
  });

  public newAddressName: string;
  public newAddressDescription: string;
  public newPassword: string;

  private updatingAddress: MaskedEmail;

  private minPasswordLength: number = 10;

  constructor(public dialogRef: MatDialogRef<UpdateMaskedEmailAddressDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { updatingAddress: MaskedEmail },
    private addressService: AddressService,
    private hashService: HashService,
    private formBuilder: FormBuilder) {

    this.addressForm = this.formBuilder.group({
      name: [this.data.updatingAddress.name, Validators.required],
      description: [this.data.updatingAddress.description],
      password: ['', [pristineOrminLength(this.minPasswordLength)]]
    });

    this.updatingAddress = this.data.updatingAddress;
  }

  ngOnInit() {
  }

  public close(): void {
    this.dialogRef.close();
  }

  public update(): void {

    console.log(this.addressForm);
    console.log(this.addressForm.get('name'));

    this.newAddressName = this.addressForm.get('name').value;
    this.newAddressDescription = this.addressForm.get('description').value;
    if (this.addressForm.get('password') !== null) {
      this.newPassword = this.addressForm.get('password').value;
    }

    console.log(this.newAddressName);
    console.log(this.newAddressDescription);
    console.log(this.newPassword);
    console.log(this.newPassword.length);

    if (
      this.updatingAddress.name !== this.newAddressName ||
      this.updatingAddress.description !== this.newAddressDescription ||
      this.newPassword.length > 0
    ) {
      console.log('updating...');
      this.updatingAddress.name = this.newAddressName;
      this.updatingAddress.description = this.newAddressDescription;

      this.onUpdate(this.updatingAddress, this.newPassword);
    }

    this.updatingAddress = undefined;
    this.close();
  }

  public getErrorMessageForName() {
    return this.addressForm.get('name').hasError('required')
      ? 'You must enter a value'
      : ''
      ;
  }
  public getErrorMessageForPassword() {
    return this.addressForm.get('password').hasError('pristineOrMinLength')
      ? `The password must be at least ${this.minPasswordLength} characters long`
      : ''
      ;
  }

  private onUpdate(address: MaskedEmail, password: string): void {
    const updateRequest: UpdateMaskedEmailRequest = {
      name: address.name,
      description: address.description,
    };
    if (password.length > 0) {
      const passwordHash = this.hashService.hashPassword(password);
      updateRequest.passwordHash = passwordHash;
    }
    this.addressService.updateAddress(address.emailAddress, updateRequest)
      .subscribe(_ => { this.close() });
  }
}
