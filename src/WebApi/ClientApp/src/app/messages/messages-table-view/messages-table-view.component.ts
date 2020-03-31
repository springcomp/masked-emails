import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { MessageSpec } from '../../shared/models/model';
import { MatTableDataSource } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-messages-table-view',
  templateUrl: './messages-table-view.component.html',
  styleUrls: ['../messages.component.scss', './messages-table-view.component.scss']
})
export class MessagesTableViewComponent implements OnInit {
  @Input() dataSource: MatTableDataSource<MessageSpec>;
  @Input() selection: SelectionModel<MessageSpec>;

  @Output() openMessage = new EventEmitter<MessageSpec>();

  public selectedRowIndex: any = null;
  public displayedColumns: string[] = ['received', 'sender', 'subject', 'actions'];

  constructor() { }

  ngOnInit() {
  }

  public showMessage(messageSpec: MessageSpec) {
    this.selection.clear();
    this.selection.select(messageSpec);

    this.openMessage.emit(messageSpec);
  }

}
