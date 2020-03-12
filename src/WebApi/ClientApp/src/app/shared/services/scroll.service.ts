import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ScrollService {

  private _scrollToBottom: boolean;
  constructor() {
    this.scrollToBottom = false;
  }

  get scrollToBottom(): boolean {
    return this._scrollToBottom;
  }

  set scrollToBottom(value: boolean) {
    this._scrollToBottom = value;
  }

  public isScrolledToBottom($event): void {
    if ($event && $event.target.offsetHeight + $event.target.scrollTop >= $event.target.scrollHeight) {
      this.scrollToBottom = true;
     
    } else {
      this.scrollToBottom = false;
    }
  }

}
