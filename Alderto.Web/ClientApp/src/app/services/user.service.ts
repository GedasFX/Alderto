import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AccountService } from './account.service';
import { IGuild } from "src/app/models/guild";
import { SessionWebApi } from './web';

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

  constructor(accountService: AccountService, sessionApi: SessionWebApi) {
    accountService.accessToken$.subscribe(t => {
      if (t == null)
        return;

      // Access token exists. Fetch user data.
      sessionApi.userInfo().subscribe(s => {
        this.user$.next(s.user);
        this.userGuilds$.next(s.user_guilds);
      });
    });
  }

  public get user() { return this.user$.getValue(); }
}
