import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MessageSpec, Message } from '../../shared/models/model';

@Component({
  selector: 'app-message-content-mobile-view',
  templateUrl: './message-content-mobile-view.component.html',
  styleUrls: ['./message-content-mobile-view.component.scss']
})
export class MessageContentMobileViewComponent implements OnInit {
  @Input() messageSpec: MessageSpec;
  @Input() messageContent: Message;
  @Input() loadingMessage: boolean;

  @Output() closeSidenav = new EventEmitter();

  constructor() {}

  ngOnInit() {
  }

  public getMessageBody(): string {
    if (this.messageContent != null) {
      return this.messageContent.htmlBody;
    }
    return "";
  }

  public close() {
    this.closeSidenav.emit();
  }

}
