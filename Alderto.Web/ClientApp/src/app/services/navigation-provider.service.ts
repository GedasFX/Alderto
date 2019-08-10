import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { INavData, homeNav, guildNav } from '../_nav';

@Injectable({
  providedIn: 'root'
})
export class NavigationProviderService {
  private readonly navItemsSubject$: BehaviorSubject<INavData[]>;
  public readonly navItems$: Observable<INavData[]>;

  constructor() {
    this.navItemsSubject$ = new BehaviorSubject<INavData[]>(homeNav);
    this.navItems$ = this.navItemsSubject$.asObservable();
  }

  /**
   * Updates nav items to use the home layout.
   */
  public navigateToHome() {
    this.navItemsSubject$.next(homeNav);
  }

  /**
   * Updates nav items to use the guild management layout.
   * @param guildId Id of guild to manage.
   */
  public navigateToGuild(guildId: number) {
    // Replace :id with actual guildId.
    const nav = JSON.parse(JSON.stringify(guildNav).replace(new RegExp(':id', 'g'), guildId as any)) as INavData[];
    this.navItemsSubject$.next(nav);
  }
}
