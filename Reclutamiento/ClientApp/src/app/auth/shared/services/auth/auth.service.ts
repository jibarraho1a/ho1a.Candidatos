import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Store } from '../../../../store';


import {BehaviorSubject, throwError} from 'rxjs';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/filter';
import 'rxjs/add/observable/of';

import { environment } from '../../../../../environments/environment';
import { LinkedIn } from '../../models';
import { tap } from 'rxjs/operators';
import { catchError } from 'rxjs/internal/operators';
import { EnvironmentService } from '../../../../core/service/environment-specific.service';
import { EnvSpecific } from '../../../../core/model/env-specific';

const CANDIDATE = '/Candidato';
const AUTH = '/auth';
const LOGIN = '/login';
const LOGINFACEBOOK = '/loginFacebook';
const EXTERNALAUTH = '/externalauth';
const LINKEDIN = '/linkedIn';
const FACEBOOK = '/facebook';
const REGISTER = '/register';
const RESET = '/ResetPassword';
const FORGOT = '/ForgotPassword';

@Injectable()
export class AuthService {

  private loggedIn = false;
  private httpOptions = { headers: new HttpHeaders({ 'Content-Type':  'application/json' }) };
  private config: EnvSpecific = new EnvSpecific();
  private API: string;

  constructor(
    private store: Store,
    private http: HttpClient,
    private appConfig: EnvironmentService,
  ) {
    this.config = this.appConfig.getConfig();
    this.API = this.config.apiRequisicionRH;
  }

  get user() {
    return this.store.select<any>('user');
  }

  loginEmailPassword(email: string, password: string) {
    const url = `${this.API}${CANDIDATE}${AUTH}${LOGIN}`;
    const body = { userName : email, password : password };
    return this.http.post(url, body, this.httpOptions )
      .pipe(
        tap( (res: any) => {
          localStorage.setItem('auth_token', res.auth_token);
        }),
        catchError(this.handleError)
      );
  }

  loginFacebook(email: string, firstName: string, lastName: string, profilePic: string) {
    const url = `${this.API}${CANDIDATE}${AUTH}${LOGINFACEBOOK}`;
    const body = { email: email, firstName: firstName, lastName: lastName, profilePic: profilePic };
    return this.http.post(url, body, this.httpOptions)
      .pipe(
        tap((res: any) => {
          localStorage.setItem('auth_token', res.auth_token);
        }),
        catchError(this.handleError)
      );
  }

  resetPassword(body: any) {
    const url = `${this.API}${CANDIDATE}${AUTH}${RESET}`;
    body.code = this.replaceAllOneCharAtATime(body.code, ' ', '+');
    return this.http.post(url, body, this.httpOptions )
      .pipe(
        catchError(this.handleError)
      );
  }

  // Todo Eliminar o moverlo a otro lado
  private replaceAllOneCharAtATime(inSource, inToReplace, inReplaceWith) {
    var output = "";
    var firstReplaceCompareCharacter = inToReplace.charAt(0);
    var sourceLength = inSource.length;
    var replaceLengthMinusOne = inToReplace.length - 1;
    for (var i = 0; i < sourceLength; i++) {
      var currentCharacter = inSource.charAt(i);
      var compareIndex = i;
      var replaceIndex = 0;
      var sourceCompareCharacter = currentCharacter;
      var replaceCompareCharacter = firstReplaceCompareCharacter;
      while (true) {
        if (sourceCompareCharacter != replaceCompareCharacter) {
          output += currentCharacter;
          break;
        }
        if (replaceIndex >= replaceLengthMinusOne) {
          i += replaceLengthMinusOne;
          output += inReplaceWith;
          //was a match
          break;
        }
        compareIndex++; replaceIndex++;
        if (i >= sourceLength) {
          // not a match
          break;
        }
        sourceCompareCharacter = inSource.charAt(compareIndex)
        replaceCompareCharacter = inToReplace.charAt(replaceIndex);
      }
      replaceCompareCharacter += currentCharacter;
    }
    return output;
  }


  forgotPassword(body: any) {
    const url = `${this.API}${CANDIDATE}${AUTH}${FORGOT}`;
    return this.http.post(url, body, this.httpOptions )
      .pipe(
        catchError(this.handleError)
      );
  }

  logoutUser() {
    localStorage.setItem('user', null);
    this.store.set('user', null);
  }

  linkedInLogin(accessToken: string) {
    const body = JSON.stringify({ accessToken });
    const url = `${this.API}${EXTERNALAUTH}${LINKEDIN}`;
    return this.http.post<LinkedIn>(url, body, this.httpOptions)
      .pipe(
        tap(res => res),
        tap( res => {
          localStorage.setItem('auth_token', res.auth_token);
          this.loggedIn = true;
          return true;
        } ),
        catchError(this.handleError)
      );
  }

  facebookLogin(email: string, firstName: string, lastName: string, profilePic: string) {
    const body = { email: email, firstName: firstName, lastName: lastName, profilePic: profilePic };
    const url = `${this.API}${EXTERNALAUTH}${FACEBOOK}`;
    return this.http.post<LinkedIn>(url, body, this.httpOptions )
      .pipe(
        tap(res => res),
        tap( res => {
          localStorage.setItem('auth_token', res.auth_token);
          this.loggedIn = true;
          return true;
        } ),
        catchError(this.handleError)
      );
  }

  register(userCandidate: any) {
    const url = `${this.API}${CANDIDATE}${AUTH}${REGISTER}`;
    return this.http.post(url, userCandidate, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  getUrlLogin() {
    return 'https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=' +
      this.config.clientId +
      '&redirect_uri=' +
      this.config.urlApplication +
      '/auth/login/linkedin&scope=r_liteprofile,r_emailaddress,w_member_social';
  }

  handleError(err: HttpErrorResponse) {
    // in a real world app, we may send the server to some remote logging infrastructure
    // instead of just logging it to the console
    let errorMessage: any;
    if (err.error instanceof Object) {
      errorMessage = err.error;
    } else if (!(err.error instanceof Object)) {
      errorMessage = `Ocurrió un error: ${err.error}`;
    } else if (err.error instanceof Error) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `Ocurrió un error: ${err.error.message}`;
    } else if (err && err.message) {
      errorMessage = `Backend devuelve el siguiente message: ${err.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Backend devuelve "code" ${err.status}, "body" fue: ${err.error}`;
    }
    return throwError(errorMessage);
  }

}
