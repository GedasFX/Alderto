import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IGuild, IGuildChannel, IGuildRole } from 'src/app/models';
import { AldertoWebGuildApi } from './web';
import { NavigationService } from './navigation.service';
import { UserService } from './user.service';
import { map, tap, switchMap } from 'rxjs/operators';

export class Guild {
  public readonly id: string;
  public readonly name: string;

  public readonly icon: string;

  public readonly userPermissions: number;
  public get userIsAdmin(): boolean {
    return ((this.userPermissions & 1 << 2) !== 0);
  }

  private loadedChannels: IGuildChannel[];
  public get channels(): Promise<IGuildChannel[]> {
    return new Promise<IGuildChannel[]>(async r => {
      if (this.loadedChannels != null) {
        r(this.loadedChannels);
        return;
      }

      this.loadedChannels = await this.guildApi.fetchChannels(this.id).toPromise();
      r(this.loadedChannels);
    });
  }

  private loadedRoles: IGuildRole[];
  public get roles(): Promise<IGuildRole[]> {
    return new Promise<IGuildRole[]>(async r => {
      if (this.loadedRoles != null) {
        r(this.loadedRoles);
        return;
      }

      this.loadedRoles = await this.guildApi.fetchRoles(this.id).toPromise();
      r(this.loadedRoles);
    });
  }

  private loadedUserRoles: IGuildRole[];
  public get userRoles(): Promise<IGuildRole[]> {
    return new Promise<IGuildRole[]>(async r => {
      if (this.loadedUserRoles != null) {
        r(this.loadedUserRoles);
        return;
      }

      const roleIds = await this.guildApi.fetchUserRoles(this.id).toPromise();
      const roles = await this.roles;
      this.loadedUserRoles = [];
      roleIds.forEach(rid => {
        this.loadedUserRoles.push(roles.find(i => i.id === rid));
      });
      r(this.loadedUserRoles);
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
  public readonly mutualGuilds$: Observable<Guild[]>;
  public readonly currentGuild$: Observable<Guild>;

  public mutualGuilds: Guild[];
  public currentGuildId: string;
  public currentGuild: Guild;

  constructor(
    userService: UserService,
    guildApi: AldertoWebGuildApi,
    nav: NavigationService
  ) {
    this.mutualGuilds$ = userService.userGuilds$.pipe(
      map(u => u ? u.map(g => new Guild(g, guildApi)) : undefined)
    );

    this.currentGuild$ = nav.currentGuildId$.pipe(
      tap(id => this.currentGuildId = id),
      switchMap(id => this.mutualGuilds$.pipe(map(g => g
        ? g.find(l => l.id === id)
        : null)))
    );

    nav.currentGuildId$.subscribe(id => this.currentGuildId = id);
    this.mutualGuilds$.subscribe(g => this.mutualGuilds = g);
    this.currentGuild$.subscribe(g => this.currentGuild = g);
  }
}
