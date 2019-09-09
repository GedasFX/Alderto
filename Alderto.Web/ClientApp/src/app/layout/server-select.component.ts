import { Component, OnInit } from '@angular/core';
import { GuildService } from 'src/app/services';

@Component({
  selector: '.app-server-select',
  templateUrl: './server-select.component.html',
  styleUrls: ['./server-select.component.scss']
})
export class ServerSelectComponent implements OnInit {
  public currentServerIcon = "/assets/img/unknown.svg";
  public currentServerName = "Please select a server";

  constructor(
    public readonly guilds: GuildService) { }

  public ngOnInit() {
    this.guilds.currentGuild$.subscribe(g => {
      if (g !== undefined)
        this.currentServerName = g.name;
    });
  }
}
