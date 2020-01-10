import { Component, OnInit } from '@angular/core';

import { AddressService } from '../address.service';
import { ModalService } from '../core/_modal/modal.service';
import { HashService } from '../hash.service';

import { Address, MaskedEmail, MaskedEmailRequest, UpdateMaskedEmailRequest } from '../model';

@Component({
  selector: 'app-addresses',
  templateUrl: './addresses.component.html',
  styleUrls: ['./addresses.component.scss']
})
export class AddressesComponent implements OnInit {

  NEW_MASKED_EMAIL_ADDRESS_DIALOG_ID: string = 'new-masked-email-address-dialog';
  UPDATE_MASKED_EMAIL_ADDRESS_DIALOG_ID: string = 'update-masked-email-address-dialog';

  addresses: MaskedEmail[] = [];
  searchAddress: string = "";

  newAddressName: string = "";
  newAddressDescription: string = "";
  newAddressPassword: string = "";

  updatingAddress: MaskedEmail = undefined;

  constructor(
    private addressService: AddressService,
    private modalService: ModalService,
    private hashService: HashService
  ) { }

  ngOnInit() {
    this.loadAddresses();
  }

  onClearSearchAddress(): void {
    this.searchAddress = "";
  }

  onNewAddress(): void {
    var request: MaskedEmailRequest = {
      name: this.newAddressName,
      description: this.newAddressDescription,
      forwardingEnabled: true
    };
    if (this.newAddressPassword.length > 0){
      const passwordHash = this.hashService.hashPassword(this.newAddressPassword);
      request.passwordHash = passwordHash;
    }
    this.addressService.createAddress(request)
      .subscribe(address => {
        this.addresses.push(MaskedEmail.fromAddress(address));
        this.newAddressName = "";
        this.newAddressName = "";
        this.newAddressPassword = "";
      });
  }

  copyToClipboard(text: string): void {
    const selBox = document.createElement('textarea');
    selBox.style.position = 'fixed';
    selBox.style.left = '0';
    selBox.style.top = '0';
    selBox.style.opacity = '0';
    selBox.value = text;
    document.body.appendChild(selBox);
    selBox.focus();
    selBox.select();
    document.execCommand('copy');
    document.body.removeChild(selBox);
  }

  onToggleChecked(address: MaskedEmail): void {
    this.addressService.toggleAddressForwarding(address.emailAddress)
      .subscribe(_ => { });
  }

  onUpdate(address: MaskedEmail): void {
    const updateRequest: UpdateMaskedEmailRequest = {
      name: address.name,
      description: address.description,
    };
    this.addressService.updateAddress(address.emailAddress, updateRequest)
      .subscribe(_ => { });
  }

  onDelete(address: MaskedEmail): void {
    this.addressService.deleteAddress(address.emailAddress)
      .subscribe(_ => {
        this.addresses = this.addresses.filter(a => a.emailAddress !== address.emailAddress);
      });
  }

  openDialog(): void {
    this.newAddressName = "";
    this.newAddressDescription = "";
    this.modalService.open(this.NEW_MASKED_EMAIL_ADDRESS_DIALOG_ID);
  }
  closeDialog(status: boolean): void {
    this.modalService.close(this.NEW_MASKED_EMAIL_ADDRESS_DIALOG_ID);
    if (status) {
      this.onNewAddress();
    }
  }
  openUpdateDialog(address: MaskedEmail): void {
    this.newAddressName = address.name;
    this.newAddressDescription = address.description;
    this.updatingAddress = address;

    this.modalService.open(this.UPDATE_MASKED_EMAIL_ADDRESS_DIALOG_ID);
  }
  closeUpdateDialog(status: boolean): void {
    this.modalService.close(this.UPDATE_MASKED_EMAIL_ADDRESS_DIALOG_ID);
    if (status) {
      if (
        this.updatingAddress.name !== this.newAddressName ||
        this.updatingAddress.description !== this.newAddressDescription
      ) {
        this.updatingAddress.name = this.newAddressName;
        this.updatingAddress.description = this.newAddressDescription;

        this.onUpdate(this.updatingAddress);
      }
    }

    this.updatingAddress = undefined;
  }

  private loadAddresses(): void {
    this.addressService.getAddresses()
      .subscribe(addresses => this.addresses = addresses.map(a => MaskedEmail.fromAddress(a)));
  }
}
