import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';
import { MaskedEmail, MaskedEmailRequest } from '../../model';
import { AddressService } from '../../address.service';
import { HashService } from '../../hash.service';

@Component({
  selector: 'app-new-masked-email-address-dialog',
  templateUrl: './new-masked-email-address-dialog.component.html',
  styleUrls: ['./new-masked-email-address-dialog.component.scss']
})
export class NewMaskedEmailAddressDialogComponent implements OnInit {
  public addressForm = new FormGroup({
    name: new FormControl(''),
    description: new FormControl(''),
    password: new FormControl(''),
  });

  constructor(public dialogRef: MatDialogRef<NewMaskedEmailAddressDialogComponent>,
    private addressService: AddressService,
    private hashService: HashService,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: { addresses: MaskedEmail[] },) {
    this.addressForm = this.formBuilder.group({
      name: ['', Validators.required],
      description: [''],
      password:['']
    });
  }

  ngOnInit() {
  }

  public close(): void {
    this.dialogRef.close();
  }

  public createAddress(): void {
    var request: MaskedEmailRequest = {
      name: this.addressForm.get('name').value,
      description: this.addressForm.get('description').value,
      forwardingEnabled: true
    };
    if (this.addressForm.get('password').value.length > 0) {
      const passwordHash = this.hashService.hashPassword(this.addressForm.get('password').value);
      request.passwordHash = passwordHash;
    }
    this.addressService.createAddress(request)
      .subscribe(address => {
        this.dialogRef.close({ event: 'Create', data: MaskedEmail.fromAddress(address)});
      });

  }

  public getErrorMessage() {
    return this.addressForm.get('name').hasError('required') ? 'You must enter a value' :
      '';
  }
}
