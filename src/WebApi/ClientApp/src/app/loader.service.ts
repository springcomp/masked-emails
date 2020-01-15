import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

  public dataLoaded: boolean = false;
  constructor() { }

  public stopLoader(): void {
    this.dataLoaded = true;
  }
}
