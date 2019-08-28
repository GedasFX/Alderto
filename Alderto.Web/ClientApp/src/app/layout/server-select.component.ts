import { Component, OnInit } from '@angular/core';
import { IGuild } from '../models';
import { GuildService, NavigationService } from 'src/app/services';

@Component({
  selector: '.app-server-select',
  templateUrl: './server-select.component.html',
  styleUrls: ['./server-select.component.scss']
})
export class ServerSelectComponent implements OnInit {
  public currentServerIcon = "/assets/img/unknown.svg";
  public currentServerName = "Please select a server";

  constructor(
    private readonly guilds: GuildService,
    private readonly nav: NavigationService) { }

  public ngOnInit() {
    this.guilds.updateGuilds();


    //this.nav.currentGuildId$.subscribe(guildId => {
    //  var guild = mutualGuilds.find(g => g.id === guildId);
    //  if (guild !== undefined)
    //    this.currentServerName = mutualGuilds.find(g => g.id === guildId).name;
    //});
  }
}
