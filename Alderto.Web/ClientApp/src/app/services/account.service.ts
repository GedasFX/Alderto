import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export class User {
  public id: number;
  public token: string;
  public discord: string;

  public username: string;
}

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  public user$: BehaviorSubject<User>;

  constructor() {
    this.user$ = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('user')));
  }

  public login() {
    // No need to modify subject or localstorage as they are going to be changed either way.
    window.location.href = '/api/account/login';
  }
  
  public logout() {
    localStorage.removeItem('user');
    this.user$.next(null);
  }

  public get user() { return this.user$.value; }

  public isLoggedIn() { return this.user !== null; }
}
