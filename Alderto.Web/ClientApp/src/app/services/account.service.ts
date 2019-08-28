import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

import * as jwt_decode from 'jwt-decode';
import { IGuild } from 'src/app/models';

export class User {
  public id: number;
  public token: string;
  public discordToken: string;

  public username: string;
  public role: string;

  public guilds: Map<string, IGuild>;

  constructor(id?: number, token?: string, discordToken?: string, username?: string, role?: string) {
    this.id = id;
    this.token = token;
    this.discordToken = discordToken;
    this.username = username;
    this.role = role;
  }
}

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
    window.open('/api/account/login', null, 'width=600,height=800');
    window.addEventListener('message', this.loginDiscordCallback);

    return this.user;
  }

  private loginDiscordCallback = (message: MessageEvent) => {
    if (message.origin !== window.location.origin) return;

    try {
      const decodedJwt = jwt_decode(message.data);
      const user = new User(decodedJwt.nameid, message.data, decodedJwt.discord, decodedJwt.unique_name, decodedJwt.role);

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

  public logout() {
    localStorage.removeItem('user');
    this.userSubject.next(null);
  }

  public getUser() { return this.userSubject.value; }

  public isLoggedIn() { return this.getUser() !== null; }
}
