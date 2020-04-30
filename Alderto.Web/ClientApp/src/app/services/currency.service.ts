import { Injectable } from '@angular/core';
import { GuildService } from './guild.service';
import { AldertoWebCurrencyApi } from './web/alderto/currency.api';
import { of } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ILeaderboardEntry } from '../models/leaderboard-entry';

@Injectable({
  providedIn: 'root'
})
export class CurrencyService {
  private cache = {} as any;

  constructor(
    private readonly guildService: GuildService,
    private readonly currencyApi: AldertoWebCurrencyApi
  ) { }

  public getTop50() {
    if (this.cache[this.guildService.currentGuildId]) {
      return of(this.cache[this.guildService.currentGuildId] as ILeaderboardEntry[]);
    }

    return this.currencyApi.fetchLeaderboards(this.guildService.currentGuildId, 50, 0).pipe(tap(g => {
      this.cache[this.guildService.currentGuildId] = g;
    }));
  }
}
