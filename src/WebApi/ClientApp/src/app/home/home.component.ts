import { Component, OnInit } from '@angular/core';

import { Router } from '@angular/router';
import { AuthService } from '../core/auth.service'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(private authService: AuthService,
    private router: Router) {
    this.router.navigate(['/login']);

    if (this.authService.isAuthorized) {
      this.router.navigate(['/masked-emails']);
    }
  }

  ngOnInit() {

  }

}
