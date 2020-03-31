import { Injectable } from '@angular/core';
import { Subject } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

  public _loading: boolean = false;
  public loadingStatus: Subject<boolean> = new Subject();

  constructor() { }

  get loading(): boolean {
    return this._loading;
  }

  set loading(value: boolean) {
    this._loading = value;
    this.loadingStatus.next(value);
  }

  public startLoading(): void {
    this.loading  = true;
  }

  public stopLoading(): void {
    this.loading  = false;
  }
}
