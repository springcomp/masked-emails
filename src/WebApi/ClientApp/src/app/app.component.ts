import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from './core/auth.service'
import { LoaderService } from './shared/services/loader.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {

  constructor(
    public authService: AuthService,
    public loaderSvc: LoaderService
  ) {
    this.loaderSvc.stopLoader();
  }

  get dataLoaded(): boolean {
    return this.loaderSvc.dataLoaded;
  }

  ngOnInit() {
    this.authService.initAuthentication();
  }

  ngOnDestroy(): void {
    this.authService.ngOnDestroy();
  }

}
