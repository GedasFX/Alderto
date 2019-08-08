import { Component, OnInit } from '@angular/core';
import { IGuild } from '../models/guild';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: '.app-server-select',
  templateUrl: './server-select.component.html',
  styleUrls: ['./server-select.component.scss']
})
export class ServerSelectComponent implements OnInit {
  public currentServerIcon = "/assets/img/unknown.svg";
  public currentServerName = "Please select a server";

  public serverList: IGuild[] = [];

  constructor(private http: HttpClient) { }

  public ngOnInit() {
    this.http.get<IGuild[]>('/api/user/mutual-guilds').subscribe((guilds: IGuild[]) => {
      this.serverList = guilds;
    });
  }

}