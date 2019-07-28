import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  constructor() {
    
  }

  public ngOnInit() { }

  public loginDiscord() {
    window.open(window.location.origin + '/api/account/login', null, 'width=600,height=800');
    if (window.addEventListener) {
      window.addEventListener("message", this.handleMessage.bind(this), false);
    } else {
      (window as any).attachEvent("onmessage", this.handleMessage.bind(this));
    }
  }

  private handleMessage: any = (event: any) => {
    const message = event as MessageEvent;

    // Only trust messages from the below origin.
    if (message.origin !== window.location.origin)
      return;

    console.log(message.data);
  }
}
