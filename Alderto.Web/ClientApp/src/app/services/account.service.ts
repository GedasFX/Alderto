import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SessionWebApi } from './web';
import { ITokenResponse } from "src/app/services/web/session.api";

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  public readonly accessToken$: BehaviorSubject<string>;
  public readonly refreshToken$: BehaviorSubject<string>;

  constructor(private readonly sessionApi: SessionWebApi) {
    this.accessToken$ = new BehaviorSubject<string>(localStorage["access_token"]);
    this.refreshToken$ = new BehaviorSubject<string>(localStorage["refresh_token"]);
  }

  public authorize() {
    this.sessionApi.authorize();
  }

  public login(code: string) {
    this.sessionApi.login(code).subscribe(t => {
      this.storeTokens(t);
    });
  }

  public logout() {
    this.sessionApi.logout();
  }

  public updateToken() {
    //return this.sessionApi.refreshToken(this.refreshToken).pipe(tap(t => {
    //  this.accessToken$.next(t as string);
    //}));
  }

  public storeTokens(t: ITokenResponse) {
    this.accessToken$.next(t.access_token);
    this.refreshToken$.next(t.refresh_token);

    localStorage.setItem('access_token', t.access_token);
    localStorage.setItem('refresh_token', t.refresh_token);
  }

  public get accessToken() { return this.accessToken$.getValue() }
  public get refreshToken() { return this.refreshToken$.getValue() }

  public isLoggedIn() { return this.accessToken != null; }
}
