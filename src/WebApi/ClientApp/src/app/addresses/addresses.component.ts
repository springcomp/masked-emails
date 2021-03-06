import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { AddressService } from '../shared/services/address.service';
import { LoaderService } from '../shared/services/loader.service'
import { MatTableDataSource } from '@angular/material/table';
import { NewMaskedEmailAddressDialogComponent } from './new-masked-email-address-dialog/new-masked-email-address-dialog.component'
import { RemoveMaskedEmailAddressDialogComponent } from './remove-masked-email-address-dialog/remove-masked-email-address-dialog.component'
import { UpdateMaskedEmailAddressDialogComponent } from './update-masked-email-address-dialog/update-masked-email-address-dialog.component'
import { MaskedEmail, AddressPages } from '../shared/models/model';
import { ScrollService } from '../shared/services/scroll.service';
import { MediaMatcher } from '@angular/cdk/layout';
import {
  debounceTime,
  distinctUntilChanged
} from "rxjs/operators";
import { Subject } from 'rxjs';

@Component({
  selector: 'app-addresses',
  templateUrl: './addresses.component.html',
  styleUrls: ['./addresses.component.scss']
})

export class AddressesComponent implements OnInit {

  public pageResult: AddressPages;
  public searchValue: string;

  public addresses: MaskedEmail[] = [];
  public dataSource: MatTableDataSource<MaskedEmail>;
  public expandedElement: MaskedEmail | null;
  public searchChanged: Subject<string> = new Subject<string>();

  private lock: boolean;
  private sortingMode: string;
  private isSearching: boolean;
  private numberOfRow: number;
  private lockAddresses: boolean;

  mobileQuery: MediaQueryList;
  constructor(
    private addressService: AddressService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private loaderSvc: LoaderService,
    private scrollService: ScrollService,
    private media: MediaMatcher
  ) {
    this.loaderSvc.startLoading();
    //Search method: wait 400ms after the last event before emitting next event
    this.searchChanged.pipe(
      debounceTime(400),
      distinctUntilChanged())
      .subscribe(model => {
        if (!this.isSearching) {
          this.isSearching = true;
          this.searchValue = model;
          this.clearDatasource();
          this.loadAddresses();
        }
      });

    //Listen to event "resize" of the screen to calculate again the number of data that can be displayed and get the good amount of data.
    window.addEventListener('resize', this.initializeData.bind(this));
  }

  ngOnInit() {
    this.initializeData();
  }

  ngOnDestroy() {
    window.removeEventListener('resize', this.initializeData.bind(this));
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
    return this.loaderSvc.loading;
  }

  changedSearchField(text: string) {
    this.searchChanged.next(text);
  }

  sorting(sortValue: string) {
    this.pageResult = null;
    this.sortingMode = sortValue;

    this.loadAddresses();
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

  onToggleChecked($event: { address: MaskedEmail, $event }): void {
    this.addressService.toggleAddressForwarding($event.address.emailAddress)
      .subscribe(_ => {
        $event.address.forwardingEnabled = $event.$event.checked;
        this.snackBar.open(`Successfully ${$event.address.forwardingEnabled ? 'enabled' : 'disabled'} the masked email ${$event.address.emailAddress}.`, 'Undo', {
          duration: 2000
        });
      });
  }

  openRemoveDialog(address: MaskedEmail): void {
    const dialogRef = this.dialog.open(RemoveMaskedEmailAddressDialogComponent, {
      data: { removingAddress: address }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log("openRemoveDialog:afterClosed()");
      console.log(result);
      if (result && result.event == 'Confirm') {
        this.addresses = this.dataSource.data.filter(a => a.emailAddress !== address.emailAddress);
        this.updateDatasource();
      }
    });
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
    this.changedSearchField('');
  }

  openUpdateDialog(address: MaskedEmail): void {
    this.dialog.open(UpdateMaskedEmailAddressDialogComponent, {
      data: { updatingAddress: address }
    });
  }

  private loadAddresses(): void {
    var queries = this.setQueries();
    if (!this.lockAddresses) {
      this.lockAddresses = true;
      if (queries.search) {
        this.addressService.getSearchedAddresses(queries.top, null, queries.search, queries.sort).subscribe(page => {
          this.handleDatasourceData(queries.cursor, page);
        }, () => {
            this.isSearching = false;
            this.lockAddresses = false;
        });
      } else {
        this.addressService.getAddressesPages(queries.top, queries.cursor, queries.sort).subscribe(page => {
          this.handleDatasourceData(queries.cursor, page);
        });
      }
    }
  }

  private handleDatasourceData(cursor: string, page: AddressPages) {
    this.loaderSvc.stopLoading();
    this.pageResult = page;

    const data: MaskedEmail[] = this.dataSource && cursor
      ? [...this.dataSource.data, ...page.addresses.map(a => MaskedEmail.fromAddress(a))]
      : page.addresses.map(a => MaskedEmail.fromAddress(a));
    // Assign the data to the data source for the table to render
    this.dataSource = new MatTableDataSource(data);

    this.scrollService.scrollToBottom = false;
    this.lock = false;
    this.isSearching = false;
    this.lockAddresses = false;
  }

  private setQueries() {
    return {
      top: this.numberOfRow,
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
    if (this.dataSource)
      this.dataSource.data = [];
  }

  private setNumberOfDataDisplayed() {
    let mobileQuery = this.media.matchMedia('(max-width: 768px)');
    let headerHeight = 0;
    let rowHeight = 0;

    if (mobileQuery.matches) {
      headerHeight = 123; //Size of all the headers in mobile mode.
      rowHeight = 90;
    } else {
      headerHeight = 206; //Size of all the headers.
      rowHeight = 40;
    }

    let availableHeight = window.innerHeight - headerHeight;

    let possibleNumberOfRow = availableHeight / rowHeight;
    let row = Math.round(possibleNumberOfRow);
    this.numberOfRow = row + 1;
    this.loaderSvc.startLoading();

    this.clearDatasource();
    this.loadAddresses();
  }

  private initializeData() {
    this.setNumberOfDataDisplayed();
    this.clearDatasource();
    this.loadAddresses();
  }
}
