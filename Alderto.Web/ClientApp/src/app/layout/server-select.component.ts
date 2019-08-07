import { Component, OnInit } from '@angular/core';
import { IGuild } from '../models/guild';

@Component({
  selector: '.app-server-select',
  templateUrl: './server-select.component.html',
  styleUrls: ['./server-select.component.scss']
})
export class ServerSelectComponent implements OnInit {
  public currentServerIcon: string = "/assets/img/unknown.svg";
  public currentServerName: string = "Please select a server";

  public serverList: IGuild[] = [];

  constructor() { }

  ngOnInit() {
  }

}
