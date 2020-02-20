import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { authHost } from 'src/appsettings';

export interface ITokenResponse {
  access_token: string;
  expires_in: number;
  token_type: string;
  refresh_token: string;
  scope: string;
}

@Injectable({
  providedIn: 'root'
})
export class SessionWebApi {

  constructor(private readonly http: HttpClient) { }

  public authorize() {
    const redirectUri = encodeURI(`${window.location.origin}/login`);
    window.location.href = `${authHost}/connect/authorize?client_id=js&redirect_uri=${redirectUri}&response_type=code&scope=api%20offline_access`;
  }

  public login(code: string) {
    const params = new HttpParams()
      .append('code', code)
      .append('client_id', 'js')
      .append('grant_type', 'authorization_code')
      .append('redirect_uri', `${window.location.origin}/login`);

    return this.http.post<ITokenResponse>(`${authHost}/connect/token`,
      params.toString(), {
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
    });
  }

  public logout() {
    //return this.http.post('/api/account/logout', null);
  }

  public refreshToken() {
    return this.http.post('/api/account/login', null, { responseType: 'text' });
  }
}
