import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { InboxService } from '../shared/services/inbox.service';
import { LoaderService } from '../shared/services/loader.service';
import { MessageSpec, Message } from '../shared/models/model';
import { MatTableDataSource } from '@angular/material';
import { MediaMatcher } from '@angular/cdk/layout';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {
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
    this.loaderSvc.startLoader();

    //Used to modify mat-sidenav mode in mobile mode or desktop mode
    this.mobileQuery = media.matchMedia('(max-width: 768px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);

  }

  ngOnInit() {
    this.loadMessages();
  }

  ngOnDestroy(): void {
    this.mobileQuery.removeListener(this._mobileQueryListener);
  }

  get dataLoaded(): boolean {
    return this.loaderSvc.dataLoaded;
  }

  public getMessageBody(): string {
    if (this.message != null) {
      return this.message.htmlBody;
    }
    return "";
  }

  showMessage(message: MessageSpec) {
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

  closeMessageContent() {
    this.opened = !this.opened;
  }

  private loadMessages(): void {
    this.inboxService.getMessages()
      .subscribe(messages => {
        this.loaderSvc.stopLoader();
        this.messages = messages;

        // Assign the data to the data source for the table to render
        this.dataSource = new MatTableDataSource(this.messages);

      });
  }


}
