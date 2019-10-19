import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IGuild, IGuildChannel, IGuildRole } from 'src/app/models';
import { DiscordWebApi, AldertoWebUserApi, AldertoWebGuildApi } from './web';
import { NavigationService } from './navigation.service';

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
        return new Promise<IGuildChannel[]>(r => {
            if (this.loadedChannels != null)
                r(this.loadedChannels);

            this.guildApi.fetchChannels(this.id).subscribe(channels => {
                this.loadedChannels = channels;
                r(channels);
            });
        });
    }

    private loadedRoles: IGuildRole[];
    public get roles(): Promise<IGuildRole[]> {
        return new Promise<IGuildRole[]>(r => {
            if (this.loadedRoles != null)
                r(this.loadedRoles);

            this.guildApi.fetchRoles(this.id).subscribe(roles => {
                this.loadedRoles = roles;
                r(roles);
            });
        });
    }

    private loadedUserRoles: IGuildRole[];
    public get userRoles(): Promise<IGuildRole[]> {
        return new Promise<IGuildRole[]>(r => {
            if (this.loadedUserRoles != null)
                r(this.loadedUserRoles);

            this.guildApi.fetchUserRoles(this.id).subscribe(roleIds => {
                this.roles.then(roles => {
                    this.loadedUserRoles = [];
                    roleIds.forEach(rid => {
                        this.loadedUserRoles.push(roles.find(i => i.id === rid));
                    });
                });
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
    public readonly userGuilds: Guild[] = [];
    public readonly mutualGuilds: Guild[] = [];

    public readonly currentGuild$ = new BehaviorSubject(undefined as Guild);

    constructor(
        discord: DiscordWebApi,
        userApi: AldertoWebUserApi,
        guildApi: AldertoWebGuildApi,
        nav: NavigationService
    ) {
        discord.fetchGuilds().subscribe(g => {
            g.forEach(e => this.userGuilds.push(new Guild(e, guildApi)));

            userApi.fetchMutualGuilds(g).subscribe(mg => {
                // Shallow copy of user guilds array, filtered by mutual guild id's
                mg.forEach(o => this.mutualGuilds.push(this.userGuilds.find(l => l.id === o.id)));

                nav.currentGuildId$.subscribe(id => {
                    this.currentGuild$.next(this.mutualGuilds.find(l => l.id === id));
                });
            });
        });
    }

    public get currentGuildId(): string {
        return this.currentGuild$.getValue().id;
    }

    public get currentGuild(): Guild {
        return this.currentGuild$.getValue();
    }
}
