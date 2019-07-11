import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';

import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

const loginUrl = "/api/account/login";
const logoutUrl = "/api/account/logout";

@Injectable({
    providedIn: 'root'
})
export class AccountService {
    constructor(@Inject(DOCUMENT) private readonly document: Document, private readonly httpClient: HttpClient) { }

    private userLoggedIn: boolean = null;

    public login() {
        location.href = location.origin + loginUrl;
    }

    public logout() {
        return this.httpClient.post(logoutUrl, null, null).subscribe(_ => {
            //redirect the user to a page that does not require authentication
        });
    }

    public isLoggedIn() {
        if (this.userLoggedIn === null) {

        }

        return this.userLoggedIn;
    }
}

export class SecurityService {
    private isUserAuthenticatedSubject = new BehaviorSubject<boolean>(false);
    public isUserAuthenticated = this.isUserAuthenticatedSubject.asObservable();

    constructor(@Inject(DOCUMENT) private readonly document: Document, private readonly httpClient: HttpClient) { }

    public updateUserAuthenticationStatus() {
        return this.httpClient.get<boolean>(`/api/account/isAuthenticated`, { withCredentials: true }).pipe(tap(
            isAuthenticated => {
                this.isUserAuthenticatedSubject.next(isAuthenticated);
            }));
    }
}