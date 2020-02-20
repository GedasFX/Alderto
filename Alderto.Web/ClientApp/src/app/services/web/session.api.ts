import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class SessionWebApi {

    constructor(private readonly http: HttpClient) { }

    public login() {
        window.location.href = '/api/account/login';
    }

    public logout() {
        return this.http.post('/api/account/logout', null);
    }

    public refreshToken() {
        return this.http.post('/api/account/login', null, { responseType: 'text' });
    }
}
