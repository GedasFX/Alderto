import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AccountService } from './account.service';
import { AldertoWebUserApi } from './web/alderto/user.api';
import { IGuild } from "src/app/models/guild";

export class User {
  public id: number;
  public username: string;

  public avatar: string;
  public discriminator: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  public user$ = new BehaviorSubject<User>(undefined);
  public userGuilds$ = new BehaviorSubject<IGuild[]>(undefined);

  constructor(accountService: AccountService, userApi: AldertoWebUserApi) {
    accountService.accessToken$.subscribe(t => {
      if (t == null) {
        accountService.updateToken();
        return;
      }

      userApi.fetchUser().subscribe(u => { this.user$.next(u); console.log(u); });
      userApi.fetchGuilds().subscribe(u => {
        this.userGuilds$.next(u);
        console.log(u);
      });
    });
  }

  public get user() { return this.user$.getValue(); }
}
