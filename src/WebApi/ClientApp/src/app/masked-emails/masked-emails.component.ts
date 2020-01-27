import { Component, OnInit } from '@angular/core';
import { AuthService } from '../core/auth.service';
import { Router, Route, CanActivate, CanLoad, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Component({
  selector: 'app-masked-emails',
  templateUrl: './masked-emails.component.html',
  styleUrls: ['./masked-emails.component.scss']
})
export class MaskedEmailsComponent implements OnInit {

  public isAuthenticated: boolean = false;

  constructor(private authService: AuthService,
    private router: Router) { }

  ngOnInit(): void {
    this.isAuthenticated = this.authService.isAuthorized;

  }
}
