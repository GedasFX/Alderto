import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SessionWebApi } from './web';

@Injectable({
    providedIn: 'root'
})
export class AccountService {
    public readonly access_token$: BehaviorSubject<string>;

    constructor(private readonly sessionApi: SessionWebApi) {
        this.access_token$ = new BehaviorSubject<string>(undefined);
    }

    public login() {
        this.sessionApi.login();
    }

    public logout() {
        this.sessionApi.logout();
    }

    public updateToken() {
        return new Promise<string>(p =>
            this.sessionApi.refreshToken().subscribe(token => {
                console.log(token);
                this.access_token$.next(token);
                p(token);
            }));
    }

    public get access_token() { return this.access_token$.getValue() }

    public isLoggedIn() { return this.access_token !== null; }
}
