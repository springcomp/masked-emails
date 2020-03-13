import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material';
import { AddressService } from '../shared/services/address.service';
import { LoaderService } from '../shared/services/loader.service'
import { MatTableDataSource } from '@angular/material/table';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UpdateMaskedEmailAddressDialogComponent } from './update-masked-email-address-dialog/update-masked-email-address-dialog.component'
import { NewMaskedEmailAddressDialogComponent } from './new-masked-email-address-dialog/new-masked-email-address-dialog.component'
import { MaskedEmail, AddressPages } from '../shared/models/model';
import { ScrollService } from '../shared/services/scroll.service';

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
  public pageResult: AddressPages;
  public searchValue: string;
  public displayedColumns: string[] = ['name', 'address', 'description', 'enabled', 'actions'];
  public mobileColumnsToDisplay: string[] = ['informations', 'actions'];
  addresses: MaskedEmail[] = [];
  dataSource: MatTableDataSource<MaskedEmail>;
  expandedElement: MaskedEmail | null;
  private lock: boolean;
  private sortingMode: string;

  constructor(
    private addressService: AddressService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private loaderSvc: LoaderService,
    private scrollService: ScrollService
  ) { }

  ngOnInit() {
    this.loadAddresses();
  }

  sorting(sort: { active: string, direction: string }) {
    this.pageResult = null;
    if (sort.direction === "")
      this.sortingMode = null;
    else
      this.sortingMode = sort.active + "-" + sort.direction;

    this.loadAddresses();
  }

  get showLoadingSpinner(): boolean {
    if (!this.lock && this.scrollService.scrollToBottom) {
      this.lock = true;

      if (this.pageResult && this.dataSource.data.length >= this.pageResult.total) {
        this.lock = false;
        return false;
      }

      setTimeout(() => {
        this.loadAddresses();
      }, 2000);
    }
    return this.scrollService.scrollToBottom;
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
        this.addresses = this.dataSource.data.filter(a => a.emailAddress !== address.emailAddress);
        this.updateDatasource();
      });
  }

  applyFilter() {
    this.dataSource.data = [];
    this.loadAddresses();
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open(NewMaskedEmailAddressDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.event == 'Create') {
        this.clearDatasource();
        this.scrollService.scrollToBottom = true;
        this.loadAddresses();
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
    var queries = this.setQueries();
    if (queries.search) {
      this.addressService.getSearchedAddresses(queries.cursor, queries.search, queries.sort).subscribe(page => {
        this.handleDatasourceData(queries.cursor, page);
      });
    } else {
      this.addressService.getAddressesPages(queries.cursor, queries.sort).subscribe(page => {
        this.handleDatasourceData(queries.cursor, page);
      });
    }
  }

  private handleDatasourceData(cursor: string, page: AddressPages) {
    this.loaderSvc.stopLoader();
    this.pageResult = page;

    const data: MaskedEmail[] = this.dataSource && cursor
      ? [...this.dataSource.data, ...page.addresses.map(a => MaskedEmail.fromAddress(a))]
      : page.addresses.map(a => MaskedEmail.fromAddress(a));
    // Assign the data to the data source for the table to render
    this.dataSource = new MatTableDataSource(data);

    this.scrollService.scrollToBottom = false;
    this.lock = false;
  }

  private setQueries() {
    return {
      cursor: this.pageResult ? this.pageResult.cursor : null,
      sort: this.sortingMode ? this.sortingMode : null,
      search: this.searchValue ? this.searchValue.trim().toLowerCase() : null
    }
  }

  private updateDatasource() {
    this.dataSource.data = this.addresses;
  }

  private clearDatasource(): void {
    this.pageResult = null;
    this.dataSource.data = [];
  }
}
