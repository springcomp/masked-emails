import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { MessageSpec } from '../../shared/models/model';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-messages-table-mobile-view',
  templateUrl: './messages-table-mobile-view.component.html',
  styleUrls: ['../messages.component.scss', './messages-table-mobile-view.component.scss']
})
export class MessagesTableMobileViewComponent implements OnInit {
  @Input() dataSource: MatTableDataSource<MessageSpec>;

  @Output() openMessage = new EventEmitter<MessageSpec>();

  public mobileColumnsToDisplay: string[] = ['informations', 'actions'];

  constructor() {
  }

  ngOnInit() {
  }

  public showMessage(messageSpec: MessageSpec) {
    this.openMessage.emit(messageSpec);
  }

}
