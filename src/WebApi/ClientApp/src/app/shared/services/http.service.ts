import { Injectable } from "@angular/core";
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
    providedIn: 'root'

})
export class HttpService {

    constructor(
        private openid: OidcSecurityService,
        private http: HttpClient
    ) { }

    public getRequestUri(path: string): string {
        const scheme = window.location.protocol;
        const host = window.location.host;
        const url = `${scheme}//${host}${path}`;
        return url;
    }

    public getHeaders(): HttpHeaders {
        var headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Accept', 'application/json');

        const token = this.openid.getAccessToken();
        if (token !== '') {
            const tokenValue = 'Bearer ' + token;
            headers = headers.set('Authorization', tokenValue);
        }

        return headers;
    }
}