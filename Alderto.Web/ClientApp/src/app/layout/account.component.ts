import { Component, OnInit } from '@angular/core';

import { AccountService, DiscordService } from '../services';
import { IUser } from '../models/user';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {

  constructor(
    private readonly account: AccountService,
    private readonly discordService: DiscordService) { }

  public ngOnInit() {
    this.loggedIn = this.account.isLoggedIn();

    if (this.loggedIn) {
      this.discordService.getUser().subscribe((dUser: IUser) => {
        this.userImg = `https://cdn.discordapp.com/avatars/${dUser.id}/${dUser.avatar}.jpg?size=64`;
      });
    }
  }

  public loggedIn: boolean;
  public userImg: string = "/assets/img/unknown.svg";

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
