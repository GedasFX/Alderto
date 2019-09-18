import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IGuild, IGuildChannel, IGuildRole } from 'src/app/models';
import { DiscordWebApi, AldertoWebUserApi, AldertoWebGuildApi } from './web';
import { NavigationService } from './navigation.service';

@Injectable({
  providedIn: 'root'
})
export class GuildService {
  public readonly userGuilds$ = new BehaviorSubject(undefined as IGuild[]);
  public readonly mutualGuilds$ = new BehaviorSubject(undefined as IGuild[]);

  public readonly currentGuild$ = new BehaviorSubject(undefined as IGuild);

  constructor(
    private readonly discord: DiscordWebApi,
    private readonly userApi: AldertoWebUserApi,
    private readonly guildApi: AldertoWebGuildApi,
    private readonly nav: NavigationService
  ) {
    this.discord.fetchGuilds().subscribe(g => {
      g.forEach(e => e.isAdmin = this.isAdmin(e));
      this.userGuilds$.next(g);

      this.userApi.fetchMutualGuilds(g).subscribe(mg => {
        mg.forEach(e => e.isAdmin = this.isAdmin(e));
        this.mutualGuilds$.next(mg);

        this.nav.currentGuildId$.subscribe(id => {
          this.currentGuild$.next(this.mutualGuilds$.getValue().find(g => g.id === id));
        });
      });
    });
  }

  public getChannels(guild: IGuild): Promise<IGuildChannel[]> {
    if (guild.channels != null)
      return new Promise<IGuildChannel[]>(r => r(guild.channels));
    else
      return this.updateChannels(guild);
  }

  public getRoles(guild: IGuild): Promise<IGuildRole[]> {
    if (guild.roles != null)
      return new Promise<IGuildRole[]>(r => r(guild.roles));
    else
      return this.updateRoles(guild);
  }

  public updateChannels(guild: IGuild): Promise<IGuildChannel[]> {
    return new Promise<IGuildChannel[]>(resolve => {
      this.guildApi.fetchChannels(guild.id).subscribe(channels => {
        guild.channels = channels;
        resolve(channels);
      });
    });
  }

  public updateRoles(guild: IGuild): Promise<IGuildRole[]> {
    return new Promise<IGuildRole[]>(resolve => {
      this.guildApi.fetchRoles(guild.id).subscribe(roles => {
        guild.roles = roles;
        resolve(roles);
      });
    });
  }

  public getCurrentGuildId(): string {
    return this.currentGuild$.value.id;
  }

  public isAdmin(guild: IGuild): boolean {
    return ((guild.permissions & 1 << 2) !== 0);
  }
}
