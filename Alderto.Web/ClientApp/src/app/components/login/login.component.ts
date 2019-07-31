import { Component } from '@angular/core';

import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  constructor(private readonly account: AccountService) {  }

  public loginDiscord() {
    this.account.loginDiscord();
  }
}
