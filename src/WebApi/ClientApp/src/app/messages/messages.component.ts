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
        console.log("new instance of MessagesComponent.");
    }

    ngOnInit() {
        console.log("Loading inbox messages...");
        this.loadMessages();
    }

    get dataLoaded(): boolean {
        return this.loaderSvc.dataLoaded;
    }

    private loadMessages(): void {
        this.inboxService.getMessages()
            .subscribe(messages => {
                this.loaderSvc.stopLoader();
                console.log("inbox messages retrieved successfully");
                this.messages = messages;
                console.log(messages[0].subject);
            });
    }
}