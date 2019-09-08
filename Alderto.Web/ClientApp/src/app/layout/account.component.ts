import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService, DiscordWebApi } from '../services';
import { IUser } from '../models/user';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {
  private user: Observable<IUser>;

  public loggedIn: boolean;
  public userImg: Observable<string>;

  constructor(
    private readonly account: AccountService,
    private readonly discord: DiscordWebApi) { }

  public ngOnInit() {
    this.loggedIn = this.account.isLoggedIn();

    if (this.loggedIn) {
      this.user = this.discord.fetchUser();
      this.userImg = this.user.pipe(map((user: IUser) => {
        if (user.avatar == null)
          return this.getDefaultAvatar(user.discriminator);
        return `https://cdn.discordapp.com/avatars/${user.id}/${user.avatar}.jpg?size=64`
      }));
    }
  }

  public loginDiscord() {
    this.account.loginDiscord().subscribe((u: any) => {
      if (u !== null)
        location.reload(true);
    });
  }

  public logout() {
    this.account.logout();
    location.reload(true);
  }

  private getDefaultAvatar(discriminator): string {
    switch (discriminator % 5) {
      case 0:
        return 'https://discordapp.com/assets/6debd47ed13483642cf09e832ed0bc1b.png';
      case 1:
        return 'https://discordapp.com/assets/322c936a8c8be1b803cd94861bdfa868.png';
      case 2:
        return 'https://discordapp.com/assets/dd4dbc0016779df1378e7812eabaa04d.png';
      case 3:
        return 'https://discordapp.com/assets/0e291f67c9274a1abdddeb3fd919cbaa.png';
      case 4:
        return 'https://discordapp.com/assets/1cbd08c76f8af6dddce02c5138971129.png';
    }
  }
}
