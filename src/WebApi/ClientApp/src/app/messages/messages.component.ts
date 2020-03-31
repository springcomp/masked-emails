import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { InboxService } from '../shared/services/inbox.service';
import { LoaderService } from '../shared/services/loader.service';
import { MessageSpec, Message } from '../shared/models/model';
import { MatTableDataSource } from '@angular/material/table';
import { MediaMatcher } from '@angular/cdk/layout';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit, OnDestroy {
  public selection = new SelectionModel<MessageSpec>(true, []);
  opened: boolean;
  message: Message | null;
  messageContent: MessageSpec = new MessageSpec();
  messages: MessageSpec[] = [];
  dataSource: MatTableDataSource<MessageSpec>;
  loadingMessage: boolean;

  mobileQuery: MediaQueryList;
  private _mobileQueryListener: () => void;

  constructor(
    private inboxService: InboxService,
    private loaderSvc: LoaderService,
    private media: MediaMatcher,
    private changeDetectorRef: ChangeDetectorRef
  ) {
    this.loaderSvc.startLoading();

    //Used to modify mat-sidenav mode in mobile mode or desktop mode
    this.mobileQuery = this.media.matchMedia('(max-width: 768px)');
    this._mobileQueryListener = () => this.changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);

  }

  ngOnInit() {
    this.loadMessages();
  }

  ngOnDestroy(): void {
    this.mobileQuery.removeListener(this._mobileQueryListener);
  }

  public getMessageBody(): string {
    if (this.message != null) {
      return this.message.htmlBody;
    }
    return "";
  }

  public showMessage(message: MessageSpec) {
    this.opened = true;
    this.loadingMessage = true;
    this.messageContent = message;
    const location = message.location;
    this.inboxService.getMessage(location)
      .subscribe(msg => {
        this.message = msg;
        this.loadingMessage = false;
      });
  }

  public closeMessageContent() {
    this.opened = !this.opened;
    this.selection.clear();
  }

  private loadMessages(): void {
    this.inboxService.getMessages()
      .subscribe(messages => {
        this.loaderSvc.stopLoading();
        this.messages = messages;

        // Assign the data to the data source for the table to render
        this.dataSource = new MatTableDataSource(this.messages);

      });
  }


}
