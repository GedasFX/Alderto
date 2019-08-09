import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../services';
import { navItems, INavData } from '../_nav';

@Component({
  selector: 'body',
  templateUrl: 'app.component.html'
})
export class AppComponent implements OnInit {
  public currentGuildId: Observable<number>;
  public navItems: Observable<INavData[]>;

  constructor(private readonly route: ActivatedRoute) { }

  public ngOnInit() {
    this.currentGuildId = this.route.params.pipe(map((params: Params) => params['id'] as number));
    this.navItems =
      this.currentGuildId.pipe(map((guildId: number) =>
        JSON.parse(JSON.stringify(navItems).replace(new RegExp(':id', 'g'), guildId as any)) as INavData[]));
  }
}
