import { Component } from '@angular/core';

import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent {
  private readonly loggedIn: boolean;
  constructor(private readonly account: AccountService) {
    this.loggedIn = account.isLoggedIn();
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
