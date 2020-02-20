import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AccountService } from "src/app/services/account.service";

@Component({
  template: ''
})
export class LoginComponent implements OnInit, OnDestroy {

  private queryParamsSub: Subscription;

  constructor(private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly accountService: AccountService) { }

  public ngOnInit() {
    this.queryParamsSub = this.route.queryParams.subscribe(p => {
      this.accountService.login(p["code"]);
      this.router.navigateByUrl("/");
    });
  }

  public ngOnDestroy(): void {
    this.queryParamsSub.unsubscribe();
  }
}
