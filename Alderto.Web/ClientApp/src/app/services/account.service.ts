import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export class User {
  public id: number;
  public token: string;

  public username: string;
}

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  public readonly userSubject: BehaviorSubject<User>;

  constructor() {
    this.userSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('user')));
  }

  public loginDiscord() {
    window.location.href = '/api/account/login';
  }

  public logout() {
    localStorage.removeItem('user');
    this.userSubject.next(null);
  }

  public getUser() { return this.userSubject.value; }

  public isLoggedIn() { return this.getUser() !== null; }
}
