import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

import { HttpService } from './http.service';
import { Address, AddressPages, MaskedEmailRequest, UpdateMaskedEmailRequest } from '../models/model'
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

  public getAddressesPages(top: number, cursor: string, sort_by: string): Observable<AddressPages> {
    var headers = { headers: this.helpers.getHeaders() };

    var requestUri = this.urlBuilder("/profiles/my/address-pages", top, cursor, sort_by, null);

    return this.http.get<AddressPages>(requestUri, headers);
  }

  public createAddress(request: MaskedEmailRequest): Observable<Address> {
    var headers = { headers: this.helpers.getHeaders() };
    var requestUri = this.helpers.getRequestUri("/profiles/my/addresses");
    return this.http.post<Address>(requestUri, request, headers)
  }

  public updateAddress(email: string, request: UpdateMaskedEmailRequest): Observable<any> {
    var headers = { headers: this.helpers.getHeaders() };
    var requestUri = this.helpers.getRequestUri(`/profiles/my/addresses/${email}`);
    return this.http.patch(requestUri, request, headers)
  }

  public deleteAddress(email: string): Observable<any> {
    var headers = { headers: this.helpers.getHeaders() };
    var requestUri = this.helpers.getRequestUri(`/profiles/my/addresses/${email}`);
    return this.http.delete(requestUri, headers)
  }

  public toggleAddressForwarding(email: string): Observable<any> {
    var headers = { headers: this.helpers.getHeaders() };
    var requestUri = this.helpers.getRequestUri(`/profiles/my/addresses/${email}/enableForwarding`);
    return this.http.patch(requestUri, {}, headers)
  }

  public getSearchedAddresses(top:number, cursor: string, contains: string, sort_by: string): Observable<AddressPages> {
    var headers = { headers: this.helpers.getHeaders() };

    var requestUri = this.urlBuilder("/profiles/my/search", top, cursor, sort_by, contains);

    return this.http.get<AddressPages>(requestUri, headers);
  }

  private urlBuilder(url: string, top: number, cursor: string, sort_by: string, search: string): string {
    var query_params: string[] = [];
    if (top) {
      query_params.push("top=" + top);
    }

    if (cursor) {
      query_params.push("cursor=" + cursor);
    }

    if (sort_by) {
      query_params.push("sort_by=" + sort_by);
    }

    if (search) {
      query_params.push("contains=" + search);
    }

    if (query_params.length === 0)
      return this.helpers.getRequestUri(url);

    var query_string = query_params.join('&');

    return this.helpers.getRequestUri(url + "?" + query_string);
  }
}
