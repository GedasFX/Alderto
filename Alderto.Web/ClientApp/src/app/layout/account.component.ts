import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService, DiscordApiService } from '../services';
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
    private readonly discordService: DiscordApiService) { }

  public ngOnInit() {
    this.loggedIn = this.account.isLoggedIn();

    if (this.loggedIn) {
      this.user = this.discordService.fetchUser();
      this.userImg = this.user.pipe(map((user: IUser) =>`https://cdn.discordapp.com/avatars/${user.id}/${user.avatar}.jpg?size=64`));
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
