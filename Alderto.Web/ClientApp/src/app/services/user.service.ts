import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AccountService } from './account.service';
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

  constructor(accountService: AccountService) {
    accountService.user$.subscribe(u => {
      this.user$.next(u ? u.profile['user'] : undefined);
      this.userGuilds$.next(u ? this.mapGuilds(u.profile['user_guilds']) : undefined);
    });
  }

  // Dirty hack for getting data from datastring
  private mapGuilds(guilds) {
    const mapGuild = (name: string, data: string) => {
      const dat = data.split(':');
      return {
        name: name,
        id: dat[0],
        permissions: Number(dat[1]),
        owner: Boolean(dat[2]),
        icon: dat[3]
      } as IGuild;
    };

    return Object.keys(guilds).map(k => mapGuild(k, guilds[k]));
  }

  public get user() { return this.user$.getValue(); }
  public get userGuilds() { return this.userGuilds$.getValue(); }
}
