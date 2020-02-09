import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

import { HttpService } from './http.service';
import { Address, MaskedEmailRequest, UpdateMaskedEmailRequest } from '../models/model'
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AddressService {

  constructor(
    private helpers: HttpService,
    private http: HttpClient
  ) { }

  public getAddresses(): Observable<Address[]> {
    var headers = { headers: this.helpers.getHeaders() };
    var requestUri = this.helpers.getRequestUri("/profiles/my/addresses");
    return this.http.get<Address[]>(requestUri, headers);
  }

  public createAddress(request: MaskedEmailRequest): Observable<Address>{
    var headers = { headers: this.helpers.getHeaders() };
    var requestUri = this.helpers.getRequestUri("/profiles/my/addresses");
    return this.http.post<Address>(requestUri, request, headers)
  }

  public updateAddress(email: string, request: UpdateMaskedEmailRequest): Observable<any>{
    var headers = { headers: this.helpers.getHeaders() };
    var requestUri = this.helpers.getRequestUri(`/profiles/my/addresses/${email}`);
    return this.http.patch(requestUri, request, headers)
  }

  public deleteAddress(email: string): Observable<any>{
    var headers = { headers: this.helpers.getHeaders() };
    var requestUri = this.helpers.getRequestUri(`/profiles/my/addresses/${email}`);
    return this.http.delete(requestUri, headers)
  }

  public toggleAddressForwarding(email: string): Observable<any>{
    var headers = { headers: this.helpers.getHeaders() };
    var requestUri = this.helpers.getRequestUri(`/profiles/my/addresses/${email}/enableForwarding`);
    return this.http.patch(requestUri, {}, headers)
  }
}
