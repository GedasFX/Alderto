import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';

const loginUrl = "/api/account/login";
const logoutUrl = "/api/account/logout";

@Injectable({
    providedIn: 'root'
})
export class AccountService {
    constructor(@Inject(DOCUMENT) private readonly document: Document, private readonly httpClient: HttpClient) { }

    public login() {
        location.href = location.origin + loginUrl;
    }

    public logout() {
        return this.httpClient.post(logoutUrl, null, null).subscribe(_ => {
            //redirect the user to a page that does not require authentication
        });
    }
}
