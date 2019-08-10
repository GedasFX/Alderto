import { Injectable } from '@angular/core';
import { Router, Event, NavigationEnd } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { INavData, homeNav, guildNav } from '../_nav';
import { IGuild } from '../models/guild';

@Injectable({
  providedIn: 'root'
})
export class NavigationProviderService {
  public readonly currentRoute$: Observable<string[]>;

  private readonly navItemsSubject$: BehaviorSubject<INavData[]>;
  public readonly navItems$: Observable<INavData[]>;

  private readonly currentGuildSubject$: BehaviorSubject<IGuild>;
  public readonly currentGuild$: Observable<IGuild>;

  constructor(router: Router, ) {
    this.navItemsSubject$ = new BehaviorSubject<INavData[]>(homeNav);
    this.navItems$ = this.navItemsSubject$.asObservable();

    this.currentGuildSubject$ = new BehaviorSubject<IGuild>(undefined);
    this.currentGuild$ = this.currentGuildSubject$.asObservable();

    // Gets a NavigationEnd event and gets the route off of it.
    // There has to be a better way of doing this.
    this.currentRoute$ = router.events.pipe(
      filter((e: Event) => e instanceof NavigationEnd),
      map((e: NavigationEnd) => {
        const arr = e.urlAfterRedirects.split('/'); arr.shift(); return arr;
      })
    );

    this.currentRoute$.subscribe((e: string[]) => {
      switch (e[0]) {
        case "guild":
          this.onNavigatedToGuild(+e[1]);
          break;
        case "home":
          this.onNavigatedToHome();
          break;
      }
    });
  }

  /**
   * Updates nav items to use the home layout.
   */
  private onNavigatedToHome() {
    this.navItemsSubject$.next(homeNav);
    this.currentGuildSubject$.next(undefined);
  }

  /**
   * Updates nav items to use the guild management layout.
   * @param guildId Id of guild to manage.
   */
  private onNavigatedToGuild(guildId: number) {
    // Replace :id with actual guildId.
    const nav = JSON.parse(JSON.stringify(guildNav).replace(new RegExp(':id', 'g'), guildId as any)) as INavData[];
    this.navItemsSubject$.next(nav);
    this.currentGuildSubject$.next({ id: guildId, icon: null, name: null, owner: null, permissions: null });
  }
}
