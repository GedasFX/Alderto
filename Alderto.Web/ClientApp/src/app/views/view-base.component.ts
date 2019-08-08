import { OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';

export abstract class ViewBaseComponent implements OnInit, OnDestroy {
  protected constructor(private readonly route: ActivatedRoute) { }

  private routeSub: Subscription;
  protected guildId: number = 0;

  public ngOnInit() {
    this.routeSub = this.route.params.subscribe((params: Params) => {
      this.guildId = params['id'];
    });
  }

  public ngOnDestroy(): void {
    this.routeSub.unsubscribe();
  }
}
