import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

import { User } from '../models/user';
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private readonly userSubject: BehaviorSubject<User>;
  public user: Observable<User>;

  constructor() {
    this.userSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('user')));
    this.user = this.userSubject.asObservable();
  }

  public loginDiscord(): Observable<User> {
    window.open(window.location.origin + '/api/account/login', null, 'width=600,height=800');
    window.addEventListener('message', this.loginDiscordCallback);

    return this.user;
  }

  private loginDiscordCallback = (message: MessageEvent) => {
    if (message.origin !== window.location.origin) return;

    try {
      const decodedJwt = jwt_decode(message.data);
      const user = new User(decodedJwt.nameid, message.data, decodedJwt.unique_name, decodedJwt.role);

      this.userSubject.next(user);
      localStorage.setItem('user', JSON.stringify(user));
    } catch (e) {
      if (e instanceof jwt_decode.InvalidTokenError) {
        console.log('Invalid token was provided.');

        // TODO: Add some form of failure logic.
      }
    }

    // Unregister event after it is done executing.
    window.removeEventListener('message', this.loginDiscordCallback);
  };

  // TODO: More through logout. Roles change in-between changes?
  public logout() {
    localStorage.removeItem('user');
    this.userSubject.next(null);
  }

  public getUser() { return this.userSubject.value; }

  public isLoggedIn() { return this.getUser() !== null; }
}
