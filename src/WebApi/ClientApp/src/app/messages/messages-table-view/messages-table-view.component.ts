import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { MessageSpec } from '../../shared/models/model';
import { MatTableDataSource } from '@angular/material';


@Component({
  selector: 'app-messages-table-view',
  templateUrl: './messages-table-view.component.html',
  styleUrls: ['./messages-table-view.component.scss']
})
export class MessagesTableViewComponent implements OnInit {
  @Input() dataSource: MatTableDataSource<MessageSpec>;

  @Output() openMessage = new EventEmitter<MessageSpec>();
  public displayedColumns: string[] = ['received', 'sender', 'subject', 'actions'];

  constructor() { }

  ngOnInit() {
  }

  public showMessage(messageSpec: MessageSpec) {
    this.openMessage.emit(messageSpec);
  }

}
