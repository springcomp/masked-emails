import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material';
import { AddressService } from '../shared/services/address.service';
import { LoaderService } from '../shared/services/loader.service'
import { MatTableDataSource } from '@angular/material/table';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UpdateMaskedEmailAddressDialogComponent } from './update-masked-email-address-dialog/update-masked-email-address-dialog.component'
import { NewMaskedEmailAddressDialogComponent } from './new-masked-email-address-dialog/new-masked-email-address-dialog.component'
import { MaskedEmail } from '../shared/models/model';

@Component({
  selector: 'app-addresses',
  templateUrl: './addresses.component.html',
  styleUrls: ['./addresses.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ])
  ]
})
export class AddressesComponent implements OnInit {

  public searchValue: string;
  public displayedColumns: string[] = ['name', 'emailAddress', 'description', 'enabled', 'actions'];
  public mobileColumnsToDisplay: string[] = ['informations', 'actions'];
  addresses: MaskedEmail[] = [];
  dataSource: MatTableDataSource<MaskedEmail>;
  expandedElement: MaskedEmail | null;

  constructor(
    private addressService: AddressService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private loaderSvc: LoaderService
  ) { }

  ngOnInit() {
    this.loadAddresses();
  }

  get dataLoaded(): boolean {
    return this.loaderSvc.dataLoaded;
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

    this.snackBar.open("Address successfully copied!", 'Undo', {
      duration: 2000
    });
  }

  onToggleChecked(address: MaskedEmail, $event): void {
    this.addressService.toggleAddressForwarding(address.emailAddress)
      .subscribe(_ => {
        address.forwardingEnabled = $event.checked;
        this.snackBar.open(`Successfully ${address.forwardingEnabled ? 'enabled' : 'disabled'} the masked email ${address.emailAddress}.`, 'Undo', {
          duration: 2000
        });
      });
  }

  onDelete(address: MaskedEmail): void {
    this.addressService.deleteAddress(address.emailAddress)
      .subscribe(_ => {
        this.addresses = this.addresses.filter(a => a.emailAddress !== address.emailAddress);
        this.updateDatasource();
      });
  }

  applyFilter() {
    this.dataSource.filter = this.searchValue.trim().toLowerCase();
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open(NewMaskedEmailAddressDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.event == 'Create') {
        this.addresses.push(result.data);
        this.updateDatasource();
      }
    });
  }

  clearSearchField(): void {
    this.searchValue = '';
    this.dataSource.filter = '';
  }

  openUpdateDialog(address: MaskedEmail): void {
    this.dialog.open(UpdateMaskedEmailAddressDialogComponent, {
      data: { updatingAddress: address }
    });
  }


  private loadAddresses(): void {
    this.addressService.getAddresses()
      .subscribe(addresses => {
        this.loaderSvc.stopLoader();
        this.addresses = addresses.map(a => MaskedEmail.fromAddress(a));

        // Assign the data to the data source for the table to render
        this.dataSource = new MatTableDataSource(this.addresses);

      });

  }

  private updateDatasource() {
    this.dataSource.data = this.addresses;
  }
}
