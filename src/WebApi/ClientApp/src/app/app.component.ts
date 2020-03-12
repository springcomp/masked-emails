import { Component, OnDestroy, OnInit, ElementRef, ViewChild } from '@angular/core';
import { AuthService } from './core/auth.service'
import { LoaderService } from './shared/services/loader.service';
import { ScrollService } from './shared/services/scroll.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  @ViewChild('scrollMe', { static: true }) private myScrollContainer: ElementRef;

  public lock: boolean;
  constructor(
    public authService: AuthService,
    public loaderService: LoaderService,
    private scrollService: ScrollService
  ) {
    this.loaderService.stopLoader();
  }

  get dataLoaded(): boolean {
    return this.loaderService.dataLoaded;
  }

  ngOnInit() {
    this.authService.initAuthentication();
  }

  ngOnDestroy(): void {
    this.authService.ngOnDestroy();
  }

  onScroll($event: any) {
    if (!this.lock) {
      this.lock = true;
      this.scrollService.isScrolledToBottom($event);

      setTimeout(() => {
        if (this.scrollService.scrollToBottom)
          this.myScrollContainer.nativeElement.scrollTop = this.myScrollContainer.nativeElement.scrollHeight;
      });

      this.lock = false;
    }

  }

}
