import { Component, OnInit, OnDestroy } from '@angular/core';
import { LoaderService } from '../shared/services/loader.service';
import { Subscription } from "rxjs";

@Component({
  selector: 'app-loading-screen',
  templateUrl: './loading-screen.component.html',
  styleUrls: ['./loading-screen.component.scss']
})
export class LoadingScreenComponent implements OnInit, OnDestroy {
  public loading: boolean = false;
  public loadingSubscription: Subscription;

  constructor(private loadingScreenService: LoaderService) { }

  ngOnInit() {
    this.loadingSubscription = this.loadingScreenService.loadingStatus.subscribe((value) => {
      this.loading = value;
    });
  }

  ngOnDestroy() {
    this.loadingSubscription.unsubscribe();
  }

}
