import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IUser, IGuild } from 'src/app/models';

export interface ITokenResponse {
  access_token: string;
  expires_in: number;
  token_type: string;
  refresh_token: string;
  scope: string;
}

export interface IUserInfoResponse {
  user: IUser;
  user_guilds: IGuild[];
}

@Injectable({
  providedIn: 'root'
})
export class SessionWebApi {

  constructor(private readonly http: HttpClient) { }

  public authorize() {
    const redirectUri = encodeURI(`${window.location.origin}/login`);
    window.location.href = `${(window as any).config.AUTH_HOST}/connect/authorize?client_id=js&redirect_uri=${redirectUri}&response_type=code&scope=openid%20api%20offline_access`;
  }

  public login(code: string) {
    const params = new HttpParams()
      .append('code', code)
      .append('client_id', 'js')
      .append('grant_type', 'authorization_code')
      .append('redirect_uri', `${window.location.origin}/login`);

    return this.http.post<ITokenResponse>(`${(window as any).config.AUTH_HOST}/connect/token`,
      params.toString(), {
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
    });
  }

  public logout() {
    return this.http.post('/api/account/logout', null);
  }

  public refresh(refreshToken: string) {
    const params = new HttpParams()
      .append('refresh_token', refreshToken)
      .append('client_id', 'js')
      .append('grant_type', 'refresh_token');

    return this.http.post<ITokenResponse>(`${(window as any).config.AUTH_HOST}/connect/token`,
      params.toString(), {
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
      });
  }

  public userInfo() {
    return this.http.get<IUserInfoResponse>('/api/connect/userinfo');
  }
}
