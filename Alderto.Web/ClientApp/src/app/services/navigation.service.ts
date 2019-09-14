import { Injectable } from '@angular/core';
import { Router, Event, NavigationEnd } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { INavData, homeNav, guildNav } from '../_nav';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {
  public readonly currentRoute$: Observable<string>;

  public readonly navItems$: BehaviorSubject<INavData[]>;
  public readonly currentGuildId$: BehaviorSubject<string>;

  private hNav = homeNav;

  constructor(router: Router) {
    this.navItems$ = new BehaviorSubject<INavData[]>(homeNav);
    this.currentGuildId$ = new BehaviorSubject<string>(undefined);

    // Gets a NavigationEnd event and gets the route off of it.
    // There has to be a better way of doing this.
    this.currentRoute$ = router.events.pipe(
      filter((e: Event) => e instanceof NavigationEnd),
      map((e: NavigationEnd) => e.urlAfterRedirects)
    );

    this.currentRoute$.pipe(map(e => e.split('/')))
      .subscribe((e: string[]) => {
        switch (e[1]) {
          case "guild":
            this.onNavigatedToGuild(e[2]);
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
    this.navItems$.next(this.hNav);
  }

  /**
   * Updates nav items to use the guild management layout.
   * @param guildId Id of guild to manage.
   */
  private onNavigatedToGuild(guildId: string) {
    // Replace :id with actual guildId.
    const gNav = JSON.stringify(guildNav).replace(new RegExp(':id', 'g'), guildId) + ',';
    const nav = JSON.stringify(homeNav).replace('{},', gNav);
    this.navItems$.next(JSON.parse(nav));
    this.currentGuildId$.next(guildId);
  }

  /**
   * Gets the current guild Id.
   */
  public getCurrentGuildId(): string {
    return this.currentGuildId$.value;
  }
}
