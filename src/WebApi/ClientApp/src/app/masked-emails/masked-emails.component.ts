import { Component, OnInit } from '@angular/core';
import { AddressesComponent } from '../addresses/addresses.component';
import { MyProfileComponent } from '../my-profile/my-profile.component';

@Component({
  selector: 'app-masked-emails',
  templateUrl: './masked-emails.component.html',
  styleUrls: ['./masked-emails.component.scss']
})
export class MaskedEmailsComponent implements OnInit {
  constructor() { }
  ngOnInit(): void {
  }
}
