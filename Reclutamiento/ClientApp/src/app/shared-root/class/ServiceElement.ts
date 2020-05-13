import { MessageType } from '../enums';
import { throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { Alert } from '../models';
import {Observable} from 'rxjs/Observable';

export abstract class ServiceElement {

  alert: Alert;

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

  alertError(mensaje: string) {
    const alert = <Alert>{
      description: mensaje,
      title: '¡Detalles al procesar la información!',
      type: MessageType.danger,
    };
    this.alert = alert;
    this.setAlertStore();
  }

  abstract gets(): Observable<any[]>;

  abstract get(id: number): Observable<any>;

  abstract save(item?: any): Observable<any>;

  abstract create(): Observable<any>;

  abstract update(requisition: any): Observable<any>;

  abstract delete(id: number): Observable<any>;

  abstract setAlertStore();

}
