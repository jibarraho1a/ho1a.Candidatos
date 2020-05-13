import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private jwtHelper: JwtHelperService) { }

  setToken(token: string) {
    localStorage.setItem('token', JSON.stringify(token));
  }

  getToken(): string {
    return this.jwtHelper.tokenGetter();
  }

  isTokenValid(): boolean {
    const token = this.getToken();
    return !this.jwtHelper.isTokenExpired(token);
  }
}
