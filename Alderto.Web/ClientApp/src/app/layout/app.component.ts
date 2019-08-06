import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../services';
import { navItems } from '../_nav';

@Component({
  selector: 'body',
  templateUrl: 'app.component.html'
})
export class AppComponent {
  constructor(private readonly http: HttpClient,
    private readonly accountService: AccountService) { }

  public navItems = navItems;

  public somee() {
    this.http.post('/api/account/some', null).subscribe((data: any) => {
      console.log(data);
    });
  }

  public someee() {
    this.http.get('https://discordapp.com/api/users/@me').subscribe((data: any) => {
      console.log(data);
    });
  }

  public printUser() {
    console.log(this.accountService.getUser());
  }
}
