import { Injectable } from '@angular/core';
import { BehaviorSubject, from } from 'rxjs';
import { UserManagerSettings, UserManager, User } from 'oidc-client';
import { tap, map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

const settings = {
  authority: `${window.location.origin}/api`,
  client_id: 'js',
  redirect_uri: `${window.location.origin}/oauth/signin-callback.html`,
  post_logout_redirect_uri: `${window.location.origin}/oauth/signout-callback.html`,
  response_type: 'id_token token',
  scope: 'openid api',

  silent_redirect_uri: `${window.location.origin}/oauth/refresh-callback.html`,
  automaticSilentRenew: true,

  popupWindowFeatures: 'location=no,toolbar=no,width=500,height=800,left=100,top=100'
} as UserManagerSettings;

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  public user$ = new BehaviorSubject<User>(undefined);
  private userManager = new UserManager(settings);

  constructor(private readonly http: HttpClient) {
    this.renewToken().subscribe(() => { }, () => { });
  }

  public signIn() {
    return from(this.userManager.signinPopup()).pipe(tap((u: User) => {
      this.user$.next(u);
    }));
  }

  public signOut() {
    return this.http.post('/api/account/logout', null);
  }

  public renewToken() {
    return from(this.userManager.signinSilent()).pipe(tap((u: User) => this.user$.next(u)));
  }

  public get user() { return this.user$.getValue(); }

  public get userLoggedIn() { return !!this.user; }

  public get userLoggedIn$() { return this.user$.pipe(map(u => !!u)); }

  public get accessToken() { return this.user ? this.user.access_token : null; }
}
