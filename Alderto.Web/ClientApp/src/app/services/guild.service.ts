import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IGuild } from 'src/app/models';
import { DiscordWebApi, AldertoWebUserApi, AldertoWebGuildApi } from './web';

@Injectable({
  providedIn: 'root'
})
export class GuildService {
  public readonly userGuilds$ = new BehaviorSubject(undefined as IGuild[]);
  public readonly mutualGuilds$ = new BehaviorSubject(undefined as IGuild[]);

  constructor(
    private readonly discord: DiscordWebApi,
    private readonly userApi: AldertoWebUserApi,
    private readonly guildApi: AldertoWebGuildApi
  ) { }

  public updateGuilds(): void {
    this.discord.fetchGuilds().subscribe(g => {
      this.userGuilds$.next(g);
      this.userApi.fetchMutualGuilds(g).subscribe(mg => {
        this.mutualGuilds$.next(mg);
      });
    });
  }

  public updateChannels(guildId: string): void {
    const guild = this.mutualGuilds$.getValue().find(g => g.id === guildId);
    this.guildApi.fetchChannels(guildId).subscribe(c => {
      guild.channels = c;
    });
  }
}
