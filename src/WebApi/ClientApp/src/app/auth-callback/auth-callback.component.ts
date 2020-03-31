import { Component, OnInit, OnDestroy } from '@angular/core';
import { LoaderService } from '../shared/services/loader.service';

@Component({
  selector: 'app-auth-callback',
  template: ''
})
export class AuthCallbackComponent implements OnInit, OnDestroy {
 
  constructor(private loadingScreenService: LoaderService) {

  }

  ngOnInit() {
    this.loadingScreenService.startLoading();
  }

  ngOnDestroy() {
    this.loadingScreenService.stopLoading();
  }
}
