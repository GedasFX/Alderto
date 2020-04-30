import { Component, OnInit } from '@angular/core';
import { GuildService } from 'src/app/services';
import { Observable, from, of } from 'rxjs';
import { GuildConfiguration } from 'src/app/models';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './overview.component.html'
})
export class OverviewComponent implements OnInit {
  public guildConfiguration$: Observable<GuildConfiguration>;

  constructor(
    private readonly guildService: GuildService
  ) { }

  public ngOnInit() {
    this.guildConfiguration$ = this.guildService.currentGuild$.pipe(
      switchMap(g => g
        ? from(g.preferences)
        : of(null)
      ));

    this.guildConfiguration$.subscribe(g => console.log(g));
  }
}
