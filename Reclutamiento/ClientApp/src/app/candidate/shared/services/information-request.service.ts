import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/filter';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/withLatestFrom';

import {StateOption, Store } from '../../../store';
import {environment } from '../../../../environments/environment';
import {AuthService} from '../../../auth/shared/services/auth/auth.service';
import {ServiceElement} from '../../../shared-root/class';
import {Candidato} from '../models/candidate';
import {Configuration} from '../../../../../src/app/shared-root/models';
import { EnvironmentService } from '../../../core/service/environment-specific.service';
import { EnvSpecific } from '../../../core/model/env-specific';
import { MessageType } from '../../../shared-root/enums';

const VISIBILITY = '/visibilidad';
const REQUESTPLACE = '/requisicion';
const CANDIDATES = '/Candidato';
const CANDIDATO = '/Candidato';
const CATALOGS = '/catalogos';
const PERSONALREFERENCE = '/ReferenciasPersonales';
const WORKREFERENCE = '/ReferenciasLaborales';
const BENEFIT = '/Prestaciones';
const INCOME = '/Ingresos';
const FILES = '/Documentos';
const FILESCANDIDATE = '/CandidatoDocumento';
const SETLOADFULLINFORMATION = '/SetCargaInformacionCompleta';
const CONFIGURATIONSITE = '/Configuracion';

@Injectable({
  providedIn: 'root'
})
export class InformationRequestService extends ServiceElement {

  private config: EnvSpecific = new EnvSpecific();
  private API: string;

  events$ = this.store.select<any>(StateOption.eventsRequisition).distinctUntilChanged();

  httpOptionsFile = {
    headers: new HttpHeaders({
      'Content-Type':  'multipart/form-data',
      'Accept':  'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('auth_token')
    })
  };

  constructor(
    private store: Store,
    private http: HttpClient,
    private auth: AuthService,
    private appConfig: EnvironmentService
  ) {
    super();
    this.config = this.appConfig.getConfig();
    this.API = this.config.apiRequisicionRH;
  }

  getConfigurations(pantalla: number): Observable<any> {
    return this.http.get<any>(`${this.API}${VISIBILITY}/${pantalla}`, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  create(): Observable<any> {
    return undefined;
  }

  delete(id: number): Observable<any> {
    return undefined;
  }

  get(): Observable<any> {
    return this.http.get<any>(`${this.API}${CANDIDATES}${CANDIDATO}`, this.httpOptions)
      .pipe(
        map(this.mapGet),
        catchError(this.handleError)
      );
  }

  mapGet(next: any) {
    const candidate = new Candidato();
    candidate.initFromObject(next);
    return candidate.parceToForm();
  }

  gets(): Observable<any[]> {
    return undefined;
  }

  save(item?: any): Observable<any> {
    const url = `${this.API}${CANDIDATES}${CANDIDATO}`;
    const candidate = new Candidato();
    candidate.initFromForm(item);
    return this.http.put(url, candidate.parceToObject(), this.httpOptions)
      .pipe(
        map(this.mapGet),
        catchError(this.handleError)
      );
  }

  setAlertStore() {
  }

  update(requisition: any): Observable<any> {
    return undefined;
  }

  get httpOptions() {
    return {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('auth_token')
      })
    };
  }

  getCatalogs() {
    return this.http.get<any>(`${this.API}${CATALOGS}`, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  saveBenefitLastJob(candidate: any, benefit: any): Observable<any> {
    if (benefit && benefit.id) {
      return this.updateBenefitLastJob(benefit);
    } else {
      return this.createBenefitLastJob(candidate, benefit);
    }
  }

  updateBenefitLastJob(benefit: any): Observable<any> {
    const url = `${this.API}${CANDIDATES}${BENEFIT}/${benefit.id}`;
    return this.http.put(url, benefit, this.httpOptions )
      .pipe(
        map(this.mapGet),
        catchError(this.handleError)
      );
  }

  createBenefitLastJob(candidate: any, benefit: any): Observable<any> {
    const url = `${this.API}${CANDIDATES}${BENEFIT}`;
    return this.http.post(url, benefit, this.httpOptions )
      .pipe(
        map(this.mapGet),
        catchError(this.handleError)
      );
  }

  deleteBenefitLastJob(benefit: any): Observable<any> {
    const url = `${this.API}${CANDIDATES}${BENEFIT}/${benefit.id}`;
    return this.http.delete(url, this.httpOptions )
      .pipe(
        map(this.mapGet),
        catchError(this.handleError)
      );
  }


  saveIncomeLastJob(candidate: any, income: any): Observable<any> {
    if (income && income.id) {
      return this.updateIncomeLastJob(income);
    } else {
      return this.createIncomeLastJob(candidate, income);
    }
  }

  updateIncomeLastJob(income: any): Observable<any> {
    const url = `${this.API}${CANDIDATES}${INCOME}/${income.id}`;
    return this.http.put(url, income, this.httpOptions )
      .pipe(
        map(this.mapGet),
        catchError(this.handleError)
      );
  }

  createIncomeLastJob(candidate: any, income: any): Observable<any> {
    const url = `${this.API}${CANDIDATES}${INCOME}`;
    return this.http.post(url, income, this.httpOptions )
      .pipe(
        map(this.mapGet),
        catchError(this.handleError)
      );
  }

  deleteIncomeLastJob(income: any): Observable<any> {
    const url = `${this.API}${CANDIDATES}${INCOME}/${income.id}`;
    return this.http.delete(url, this.httpOptions )
      .pipe(
        map(this.mapGet),
        catchError(this.handleError)
      );
  }


  saveWorkReference(candidate: any, workReference: any): Observable<any> {
    if (workReference && workReference.id) {
      return this.updateWorkReference(workReference);
    } else {
      return this.createWorkReference(candidate, workReference);
    }
  }

  updateWorkReference(workReference: any): Observable<any> {
    const url = `${this.API}${CANDIDATES}${WORKREFERENCE}/${workReference.id}`;
    return this.http.put(url, workReference, this.httpOptions )
      .pipe(
        map(this.mapGet),
        catchError(this.handleError)
      );
  }

  createWorkReference(candidate: any, workReference: any): Observable<any> {
    const url = `${this.API}${CANDIDATES}${WORKREFERENCE}`;
    return this.http.post(url, workReference, this.httpOptions )
      .pipe(
        map(this.mapGet),
        catchError(this.handleError)
      );
  }

  deleteWorkReference(workReference: any): Observable<any> {
    const url = `${this.API}${CANDIDATES}${WORKREFERENCE}/${workReference.id}`;
    return this.http.delete(url, this.httpOptions )
      .pipe(
        map(this.mapGet),
        catchError(this.handleError)
      );
  }


  savePersonalReference(candidate: any, personalReference: any): Observable<any> {
    if (personalReference && personalReference.id) {
      return this.updatePersonalReference(personalReference);
    } else {
      return this.createPersonalReference(candidate, personalReference);
    }
  }

  updatePersonalReference(personalReference: any): Observable<any> {
    const url = `${this.API}${CANDIDATES}${PERSONALREFERENCE}/${personalReference.id}`;
    return this.http.put(url, personalReference, this.httpOptions )
      .pipe(
        map(this.mapGet),
        catchError(this.handleError)
      );
  }

  createPersonalReference(candidate: any, personalReference: any): Observable<any> {
    const url = `${this.API}${CANDIDATES}${PERSONALREFERENCE}`;
    return this.http.post(url, personalReference, this.httpOptions )
      .pipe(
        map(this.mapGet),
        catchError(this.handleError)
      );
  }

  deletePersonalReference(personalReference: any): Observable<any> {
    const url = `${this.API}${CANDIDATES}${PERSONALREFERENCE}/${personalReference.id}`;
    return this.http.delete(url, this.httpOptions )
      .pipe(
        map(this.mapGet),
        catchError(this.handleError)
      );
  }


  uploadFile(requisition: any, document: any) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');
    // headers.append('Authorization', 'Bearer ' + localStorage.getItem('auth_token'));
    const formData: FormData = new FormData();
    formData.append('file', document.file, document.file.name);
    return this.http.post(document.path, formData, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  deleteFile(requisition: any, document: any) {
    return this.http.delete(document.path, this.httpOptions )
      .pipe(
        catchError(this.handleError)
      );
  }

  getFilesByCandidate(candidate: any): Observable<any> {
    return this.http.get<any>(`${this.API}${FILES}${FILESCANDIDATE}/${candidate.id}`, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  notifyLoadFullInformation(estatusCarga: number) {
    const url = `${this.API}${CANDIDATES}${CANDIDATES}${SETLOADFULLINFORMATION}/${estatusCarga}`;
    return this.http.put(url, null, this.httpOptions )
      .pipe(
        catchError(this.handleError)
      );
  }

  getInformacionComplementariaSitio(): Observable<any> {
    return this.http.get<any>(`${this.API}${CONFIGURATIONSITE}`, this.httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  sendAlert(mensaje: string) {
    if (mensaje.length <= 0) {
      return;
    }
    const alert = <any>{
      description: mensaje,
      title: 'Mensaje',
      type: MessageType.success,
    };
    this.store.set(StateOption.alert, alert);
  }

}
