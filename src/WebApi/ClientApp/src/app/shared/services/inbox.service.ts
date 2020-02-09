import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

import { HttpService } from './http.service';
import { Message, MessageSpec, } from '../models/model'
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class InboxService {

  constructor(
    private helpers: HttpService,
    private http: HttpClient
  ) { }

  public getMessages(): Observable<MessageSpec[]> {
    console.log("Retrieving inbox messages...");
    var headers = { headers: this.helpers.getHeaders() };
    var requestUri = this.helpers.getRequestUri("/messages/my");
    console.log(requestUri);
    console.log(headers);
    return this.http.get<MessageSpec[]>(requestUri, headers);
  }
}