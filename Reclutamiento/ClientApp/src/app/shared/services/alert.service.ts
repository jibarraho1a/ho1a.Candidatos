import { AlertType } from './../enums/alert-type.enum';
import { Injectable } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { Observable, Subject, throwError } from 'rxjs';
import { Alert } from '../models/alert';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class AlertService {
    private subject = new Subject<Alert>();
    private keepAfterRouteChange = false;

    constructor(private router: Router) {
        // clear alert messages on route change unless 'keepAfterRouteChange' flag is true
        router.events.subscribe(event => {
            if (event instanceof NavigationStart) {
                if (this.keepAfterRouteChange) {
                    // only keep for a single route change
                    this.keepAfterRouteChange = false;
                } else {
                    // clear alert messages
                    this.clear();
                }
            }
        });
    }

    getAlert(): Observable<any> {
        return this.subject.asObservable();
    }

    success(title: string, description: string, keepAfterRouteChange = false) {
        this.alert(AlertType.Success, title, description, keepAfterRouteChange);
    }

    error(title: string, description: string, keepAfterRouteChange = false) {
        this.alert(AlertType.Error, title, description, keepAfterRouteChange);
    }

    info(title: string, description: string, keepAfterRouteChange = false) {
        this.alert(AlertType.Info, title, description, keepAfterRouteChange);
    }

    warn(title: string, description: string, keepAfterRouteChange = false) {
        this.alert(AlertType.Warning, title, description, keepAfterRouteChange);
    }

    alert(type: AlertType, title: string, description: string, keepAfterRouteChange = false) {
        this.keepAfterRouteChange = keepAfterRouteChange;
        this.subject.next(<Alert>{ type: type, title: title, description: description });
    }

    clear() {
        // clear alerts
        this.subject.next();
    }

    handleError(err: HttpErrorResponse) {
      // in a real world app, we may send the server to some remote logging infrastructure
      // instead of just logging it to the console
      let errorMessage: string;
      if (!(err.error instanceof Object)) {
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
