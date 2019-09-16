import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IMessage } from 'src/app/models';

@Injectable({
  providedIn: 'root'
})
export class AldertoWebNewsApi {
  constructor(private readonly http: HttpClient) { }

  public fetchNews(count = 10) {
    return this.http.get<IMessage[]>(`/api/news?count=${count}`);
  }
}
