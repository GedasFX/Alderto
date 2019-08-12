import { Component, OnInit } from '@angular/core';
import { IGuild } from '../models';
import { DiscordService, WebUserService, NavigationService } from '../services';

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
    private readonly webUser: WebUserService,
    private readonly discord: DiscordService,
    private readonly nav: NavigationService) { }

  public ngOnInit() {
    this.discord.fetchGuilds().subscribe((guilds: IGuild[]) => {
      this.userGuilds = guilds;

      this.webUser.getMutualGuilds(guilds).subscribe((mutualGuilds: IGuild[]) => {
        this.mutualGuilds = mutualGuilds;

        this.nav.currentGuildId$.subscribe(guildId => {
          var guild = mutualGuilds.find(g => g.id === guildId);
          if (guild !== undefined)
            this.currentServerName = mutualGuilds.find(g => g.id === guildId).name;
        });
      });
    });
  }
}
