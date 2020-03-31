import { Component, OnInit, OnDestroy } from '@angular/core';
import { LoaderService } from '../shared/services/loader.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth-callback',
  template: ''
})
export class AuthCallbackComponent implements OnInit, OnDestroy {
 
  constructor(private loadingScreenService: LoaderService, private router: Router) {

  }

  ngOnInit() {
    this.loadingScreenService.startLoading();

    this.router.navigate(['/masked-emails']);    
  }

  ngOnDestroy() {
    this.loadingScreenService.stopLoading();
  }
}
