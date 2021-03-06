import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable} from 'rxjs';

import { HttpService } from './http.service';
import { Profile } from '../models/model';

export class Claim { type: string; value: string; }

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(
    private helpers: HttpService,
    private http: HttpClient
  ) { }

  public getProfile(): Observable<Profile> {
    var headers = { headers: this.helpers.getHeaders() };
    var requestUri = this.helpers.getRequestUri("/profiles/my");
    return this.http.get<Profile>(requestUri, headers);
  }

  public updateProfile(profile: Profile): Observable<Profile>{
    var headers = { headers: this.helpers.getHeaders() };
    var requestUri = this.helpers.getRequestUri("/profiles/my");
    return this.http.patch<Profile>(requestUri, profile, headers);
  }

  public getClaims(): Observable<Claim[]> {
    var headers = { headers: this.helpers.getHeaders() };
    var requestUri = this.helpers.getRequestUri("/api/claims");
    return this.http.get<Claim[]>(requestUri, headers);
  }
}
