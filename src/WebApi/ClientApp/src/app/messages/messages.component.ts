import { Component, OnInit } from '@angular/core';
import { InboxService } from '../shared/services/inbox.service';
import { LoaderService } from '../shared/services/loader.service';
import { MessageSpec } from '../shared/models/model';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {

    messages: MessageSpec[] = [];

    constructor(
        private inboxService: InboxService,
        private loaderSvc: LoaderService
    ) {
    }

    ngOnInit() {
        this.loadMessages();
    }

    get dataLoaded(): boolean {
        return this.loaderSvc.dataLoaded;
    }

    private loadMessages(): void {
        this.inboxService.getMessages()
            .subscribe(messages => {
                this.loaderSvc.stopLoader();
                this.messages = messages;
                console.log(messages[0].subject);
            });
    }
}