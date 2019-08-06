import { Component } from '@angular/core';

import { AccountService, DiscordService } from '../services';
import { DiscordUser } from '../models/discord_user';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent {
  public readonly loggedIn: boolean;
  public userImg: string = "";

  constructor(private readonly account: AccountService, private readonly discordService: DiscordService) {
    this.loggedIn = account.isLoggedIn();

    if (this.loggedIn) {
      this.discordService.getUser().subscribe((dUser: DiscordUser) => {
        this.userImg = `https://cdn.discordapp.com/avatars/${dUser.id}/${dUser.avatar}.jpg?size=64`;
      });
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
}
