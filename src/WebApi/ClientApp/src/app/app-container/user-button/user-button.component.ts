import { Component, OnInit, Input } from '@angular/core';
import { Profile } from '../../shared/models/model';

@Component({
  selector: 'app-user-button',
  templateUrl: './user-button.component.html',
  styleUrls: ['./user-button.component.scss']
})
export class UserButtonComponent implements OnInit {

  @Input() icon: string;
  @Input() user: Profile;

  constructor() { }

  ngOnInit() {
  }

  get forwardingAddress(): string {
    return this.user && this.user.forwardingAddress ? this.user.forwardingAddress : '';
  }
}
