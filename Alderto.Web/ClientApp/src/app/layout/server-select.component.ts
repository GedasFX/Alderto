import { Component, OnInit } from '@angular/core';
import { IGuild } from '../models/guild';
import { HttpClient } from '@angular/common/http';
import { DiscordApiService, NavigationProviderService } from '../services';

@Component({
  selector: '.app-server-select',
  templateUrl: './server-select.component.html',
  styleUrls: ['./server-select.component.scss']
})
export class ServerSelectComponent implements OnInit {
  public currentServerIcon = "/assets/img/unknown.svg";
  public currentServerName = "Please select a server";

  public userGuilds: IGuild[];
  public mutualGuilds: IGuild[];

  constructor(
    private readonly http: HttpClient,
    private readonly discord: DiscordApiService,
    private readonly nav: NavigationProviderService) { }

  public ngOnInit() {
    this.discord.fetchGuilds().subscribe((guilds: IGuild[]) => {
      this.userGuilds = guilds;

      this.http.post<IGuild[]>('/api/user/guilds', guilds).subscribe((mutualGuilds: IGuild[]) => {
        this.mutualGuilds = mutualGuilds;
      });

      this.nav.currentGuild$.subscribe((guild: IGuild) => { this.currentServerName = guild.name });
    });
  }

}
