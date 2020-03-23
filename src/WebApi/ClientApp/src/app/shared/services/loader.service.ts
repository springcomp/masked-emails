import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

  public _dataLoaded: boolean = false;
  constructor() { }

  get dataLoaded(): boolean {
    return this._dataLoaded;
  }

  set dataLoaded(value: boolean) {
    this._dataLoaded = value;
  }

  public startLoader(): void {
    this._dataLoaded = false;
  }

  public stopLoader(): void {
    this._dataLoaded = true;
  }
}
