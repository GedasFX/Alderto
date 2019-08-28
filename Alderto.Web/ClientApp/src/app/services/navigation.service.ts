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

  private readonly navItemsSubject$: BehaviorSubject<INavData[]>;
  public readonly navItems$: Observable<INavData[]>;

  private readonly currentGuildIdSubject$: BehaviorSubject<string>;
  public readonly currentGuildId$: Observable<string>;

  constructor(router: Router) {
    this.navItemsSubject$ = new BehaviorSubject<INavData[]>(homeNav);
    this.navItems$ = this.navItemsSubject$.asObservable();

    this.currentGuildIdSubject$ = new BehaviorSubject<string>(undefined);
    this.currentGuildId$ = this.currentGuildIdSubject$.asObservable();

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
    this.navItemsSubject$.next(homeNav);
  }

  /**
   * Updates nav items to use the guild management layout.
   * @param guildId Id of guild to manage.
   */
  private onNavigatedToGuild(guildId: string) {
    // Replace :id with actual guildId.
    const nav = JSON.parse(JSON.stringify(guildNav).replace(new RegExp(':id', 'g'), guildId)) as INavData[];
    this.navItemsSubject$.next(nav);
    this.currentGuildIdSubject$.next(guildId);
  }

  /**
   * Gets the current guild Id.
   */
  public getCurrentGuildId(): string {
    return this.currentGuildIdSubject$.value;
  }
}
