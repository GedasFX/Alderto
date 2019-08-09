import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { map } from 'rxjs/operators'; 
import { INavData, navItems } from '../../_nav';

@Component({
  templateUrl: 'guild.component.html'
})
export class GuildLayoutComponent implements OnInit {
  public navItems: Observable<INavData[]>;
  public constructor(
    private readonly route: ActivatedRoute) { }

  public ngOnInit() {
    this.navItems =
      this.route.params.pipe(map((params: Params) =>
        JSON.parse(JSON.stringify(navItems).replace(new RegExp(':id', 'g'), params['id'])) as INavData[]));
  }
}
