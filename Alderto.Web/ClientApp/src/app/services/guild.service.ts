import { Injectable } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';
import { IGuild, IGuildChannel } from 'src/app/models';
import { DiscordWebApi, AldertoWebUserApi, AldertoWebGuildApi } from './web';
import { NavigationService } from './navigation.service';

@Injectable({
  providedIn: 'root'
})
export class GuildService {
  public readonly userGuilds$ = new BehaviorSubject(undefined as IGuild[]);
  public readonly mutualGuilds$ = new BehaviorSubject(undefined as IGuild[]);

  public readonly currentGuild$ = new BehaviorSubject(undefined as IGuild);

  private idSub: Subscription;

  constructor(
    private readonly discord: DiscordWebApi,
    private readonly userApi: AldertoWebUserApi,
    private readonly guildApi: AldertoWebGuildApi,
    private readonly nav: NavigationService
  ) {
    this.updateGuilds();
  }

  public updateGuilds(): void {
    if (this.idSub !== undefined)
      this.idSub.unsubscribe();

    this.discord.fetchGuilds().subscribe(g => {
      this.userGuilds$.next(g);
      this.userApi.fetchMutualGuilds(g).subscribe(mg => {
        this.mutualGuilds$.next(mg);

        this.idSub = this.nav.currentGuildId$.subscribe(id => {
          this.currentGuild$.next(this.mutualGuilds$.getValue().find(g => g.id === id));
        });
      });
    });
  }

  public updateChannels(guildId: string): Promise<IGuildChannel[]> {
    return new Promise<IGuildChannel[]>((resolve) => {
      const guild = this.mutualGuilds$.getValue().find(g => g.id === guildId);
      this.guildApi.fetchChannels(guildId).subscribe(c => {
        guild.channels = c;
        resolve(c);
      });
    });
  }
}
