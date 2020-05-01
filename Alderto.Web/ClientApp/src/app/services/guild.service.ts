import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { IGuild, IGuildChannel, IGuildRole, GuildConfiguration } from 'src/app/models';
import { AldertoWebGuildApi } from './web';
import { NavigationService } from './navigation.service';
import { UserService } from './user.service';
import { map, switchMap } from 'rxjs/operators';

export class Guild {
  public readonly id: string;
  public readonly name: string;

  public readonly icon: string;

  public readonly userPermissions: number;
  public get userIsAdmin() {
    return ((this.userPermissions & 1 << 2) !== 0);
  }

  private loadedPreferences: GuildConfiguration;
  public get preferences(): Promise<GuildConfiguration> {
    return new Promise<GuildConfiguration>(r => {
      if (this.loadedPreferences) {
        r(this.loadedPreferences);
        return;
      }

      this.guildApi.fetchPreferences(this.id).subscribe(p => {
        this.loadedPreferences = p;
        r(this.loadedPreferences);
      });
    });
  }

  private loadedChannels: IGuildChannel[];
  public get channels(): Promise<IGuildChannel[]> {
    return new Promise<IGuildChannel[]>(r => {
      if (this.loadedChannels) {
        r(this.loadedChannels);
        return;
      }

      this.guildApi.fetchChannels(this.id).subscribe(g => {
        this.loadedChannels = g;
        r(this.loadedChannels);
      });
    });
  }

  private loadedRoles: IGuildRole[];
  public get roles(): Promise<IGuildRole[]> {
    return new Promise<IGuildRole[]>(r => {
      if (this.loadedRoles) {
        r(this.loadedRoles);
        return;
      }

      this.guildApi.fetchRoles(this.id).subscribe(roles => {
        this.loadedRoles = roles;
        r(this.loadedRoles);
      });
    });
  }

  private loadedUserRoles: IGuildRole[];
  public get userRoles(): Promise<IGuildRole[]> {
    return new Promise<IGuildRole[]>(r => {
      if (this.loadedUserRoles) {
        r(this.loadedUserRoles);
        return;
      }

      this.guildApi.fetchUserRoles(this.id).subscribe(async roleIds => {
        const roles = await this.roles;
        this.loadedUserRoles = [];
        roleIds.forEach(rid => {
          this.loadedUserRoles.push(roles.find(i => i.id === rid));
        });
        r(this.loadedUserRoles);
      });
    });
  }

  constructor(guild: IGuild,
    private readonly guildApi: AldertoWebGuildApi
  ) {
    this.id = guild.id;
    this.name = guild.name;
    this.icon = guild.icon;
    this.userPermissions = guild.permissions;
  }
}

@Injectable({
  providedIn: 'root'
})
export class GuildService {
  public readonly mutualGuilds$ = new BehaviorSubject<Guild[]>(undefined);
  public readonly currentGuild$: Observable<Guild>;

  public mutualGuilds: Guild[];
  public currentGuildId: string;
  public currentGuild: Guild;

  constructor(
    userService: UserService,
    guildApi: AldertoWebGuildApi,
    nav: NavigationService
  ) {
    userService.userGuilds$.subscribe(userGuilds => {
      if (!userGuilds)
        return;

      const guilds = userGuilds.map(g => new Guild(g, guildApi));
      this.mutualGuilds$.next(guilds);
    });

    this.currentGuild$ = nav.currentGuildId$.pipe(
      switchMap(id => this.mutualGuilds$.pipe(map(g => g
        ? g.find(l => l.id === id)
        : null)))
    );

    nav.currentGuildId$.subscribe(id => this.currentGuildId = id);
    this.mutualGuilds$.subscribe(g => this.mutualGuilds = g);
    this.currentGuild$.subscribe(g => this.currentGuild = g);
  }
}
