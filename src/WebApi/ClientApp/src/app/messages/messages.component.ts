import { Component, OnInit } from '@angular/core';
import { InboxService } from '../shared/services/inbox.service';
import { LoaderService } from '../shared/services/loader.service';
import { MessageSpec, Message } from '../shared/models/model';
import { MatTableDataSource } from '@angular/material';

@Component({
    selector: 'app-messages',
    templateUrl: './messages.component.html',
    styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {

    message: Message | null;
    messages: MessageSpec[] = [];
    dataSource: MatTableDataSource<MessageSpec>;

    public displayedColumns: string[] = ['received', 'sender', 'subject', 'actions'];
    public mobileColumnToDisplay: string[] = ['received', 'sender', 'subject', 'actions'];

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

    public getMessageBody(): string {
        if (this.message != null){
            return this.message.htmlBody;
        }
        return "";
    }

    showMessage(message: MessageSpec) {
        const location = message.location;
        this.inboxService.getMessage(location)
            .subscribe(msg => { 
                this.message = msg;
            });
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