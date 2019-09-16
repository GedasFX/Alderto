import { Component, OnInit } from '@angular/core';
import { AldertoWebNewsApi } from "src/app/services";
import { IMessage } from "src/app/models";
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  templateUrl: './news.component.html'
})
export class NewsComponent implements OnInit {
  public latestNews : Observable<IMessage[]>;

  constructor(private readonly news: AldertoWebNewsApi) { }

  public ngOnInit() {
    this.latestNews = this.news.fetchNews();
  }
}
