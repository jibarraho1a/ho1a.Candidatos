import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/observable';
import { catchError, map } from 'rxjs/operators';

import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/filter';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/withLatestFrom';
import 'rxjs/add/operator/distinctUntilChanged';

import { environment } from '../../../environments/environment';
import { ServiceElement } from '../class/ServiceElement';
import { StateOption } from '../enums/state-option.enum';
import { AuthService } from '../../auth/services/auth.service';
import { Configuration } from '../models/configuration';
import { Store } from '../class/Store';
import { CardAccordion } from '../models/card-accordion';
import { DataType } from '../enums/data-type.enum';
import { Candidato } from '../models/candidato';

const API = environment.apiReclutamiento;
const REQUESTPLACE = '/requisicion';
const AUTORIZATION = '/Autorizacion';
const CONFIGURATION = '/visibilidad';
const REASSIGNMENTUSERS = '/UsuariosReasignacion';
const REASSIGNMENT = '/reasignar';
const CANDIDATES = '/Candidato';
const ADDCANDIDATES = '/addCandidatos';
const GETTERNA = '/getterna';
const INTERVIEW = '/entrevista';
const ADDINTERVIEWER = '/AddEntrevistador';
const ADDTERNA = '/addTerna';
const IDENTITY = '/Identity';
const IDEALCANDIDATE = '/CandidatoIdoneo';
const CATALOG = '/catalogo';
const LEADERSHIP = '/liderazgo';
const KNOWLEDGE = '/conocimiento';
const COMPETENCIES = '/competencia';

const COMMENT = '/comentario';
const PROPOSAL = '/Propuesta';
const SENDPROPOSAL = '/EnviarPropuesta';
const PROPOSEDANSWER = '/ContestacionPropuesta';
const SETINCOMEDATE = '/EstablecerFechaIngreso';
const NOTIFYALTA = '/NotificarAlta';
const CONFIRMALTA = '/ConfirmarAlta';
const FILES = '/Documentos';
const FILESCANDIDATE = '/Expediente';
const FILESECONOMICPROPOSAL = '/RequisicionPropuesta';
const CATALOGS = '/catalogos';
const TEMPLATEINTERVIEW  = '/PlantillaEntrevista';
const GETBYREQUISITION = '/GetByRequisicion';
const NOTIFYRH = '/NotificarRH';
const NOTIFYCANDIDATE = '/NotificarCandidato';
const SETSALARIUMPROPOSED = '/EstablecerSalarioPropuesto';
const JOBINFORMATION = '/InformacionTrabajo';
const SETTYPESEARCH = '/SetTipoBusqueda';
const SETFILEUSER = '/EstablecerUsuarioExpediente';
const VERIFYMAIL = '/VerificarCorreo';
const UPDATECV = '/UpdateCV';


@Injectable({
  providedIn: 'root'
})
export class SolicitudService extends ServiceElement  {

  events$ = this.store.select<any>(StateOption.eventsRequisition).distinctUntilChanged();

  constructor(
    private store: Store,
    private http: HttpClient,
    private auth: AuthService
  ) {
    super();
  }

  getConfigurations(id: number): Observable<Configuration> {
    if (!id) {
      return Observable.of(<Configuration>{});
    }
    return this.http.get<Configuration>(`${API}${CONFIGURATION}/3/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  gets(): Observable<any[]> {
    return this.http.get<any[]>(`${API}${REQUESTPLACE}`)
      .pipe(
        map( (next: any[]) => {
          const valores = next.map(next => {
            const dias = parseInt(next.diasSolicitud, 0);
            if (dias <= 30) {
              next.styles = {
                'background-color': 'green',
                'color': 'white',
                'padding-left': '5px',
                'letter-spacing': '5px'
              };
            } else if (dias > 30 && dias <= 45) {
              next.styles = {
                'background-color': '#ffbb34',
                'color': 'white',
                'padding-left': '5px',
                'letter-spacing': '5px'
              };
            } else {
              next.styles = {
                'background-color': 'red',
                'color': 'white',
                'padding-left': '5px',
                'letter-spacing': '5px'
              };
            }
            return next;
          });
          return valores;
        }),
        catchError(this.handleError)
      );
  }

  get(id: number): Observable<any> {
    if (!id) {
      return Observable.of(<any>{});
    }
    return this.http.get<any>(`${API}${REQUESTPLACE}/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  save(item?: any): Observable<any> {
    if (item && item.id) {
      return this.update(item);
    } else {
      return this.create();
    }
  }

  create(): Observable<any> {
    return this.http.get<any>(`${API}${REQUESTPLACE}/0`)
      .pipe(
        catchError(this.handleError)
      );
  }

  update(requisition: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${REQUESTPLACE}/${requisition.id}`;
    return this.http.put<any>(url, requisition, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  delete(id: number): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${REQUESTPLACE}/${id}`;

    return this.http.delete<any>(url, { headers: headers})
      .pipe(
        catchError(this.handleError)
      );
  }

  setAlertStore() {
    this.store.set(StateOption.alert, this.alert);
  }

  saveValidation(requisition: any, validation?: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${AUTORIZATION}/${requisition.id}`;
    return this.http.put<any>(url, validation, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  saveAssignment(requisition: any, userName: string | null): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    let url = `${API}${REASSIGNMENT}/${requisition.id}`;
    if (userName !== null) {
      url += `/${userName}`;
    }
    return this.http.put<any>(url, null, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  addCandidate(requisition: any, candidate: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${CANDIDATES}/${requisition.id}`;
    return this.http.post(url, candidate, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  addCandidatesRequisition(requisition: any, candidates: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${REQUESTPLACE}${ADDCANDIDATES}/${requisition.id}`;
    return this.http.put(url, candidates, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  addTernaToCandidateRequisition(requisition: any, candidateRequisition: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${CANDIDATES}${ADDTERNA}/${requisition.id}/${candidateRequisition.id}/${candidateRequisition.ternaId}`;
    return this.http.put(url, null, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  saveInterviewCandidateRequisition(requisition: any, interview: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${INTERVIEW}/${requisition.id}/${interview.id}`;
    return this.http.put(url, interview, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  addGuestCandidateInterviewRequisition(requisition: any, interview: any) {
    const headers = new HttpHeaders({'Content-Type': 'application/json'});
    const url = `${API}${INTERVIEW}${ADDINTERVIEWER}/${requisition.id}/${interview.id}`;
    return this.http.put(url, interview.value, {headers: headers})
      .pipe(
        catchError(this.handleError)
      );
  }

  saveSelectSuitableCandidateRequisition(requisition: any, data: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${REQUESTPLACE}${IDEALCANDIDATE}/${requisition.id}/${data.candidatoId}/${data.status}`;
    return this.http.put(url, null, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  saveLeadershipCompetenciesInterviewCandidateRequisition(requisition: any, interview: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${LEADERSHIP}/${requisition.id}/${interview.idEntrevista}`;
    return this.http.put(url, interview.data, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  saveKnowledgeCompetenciesInterviewCandidateRequisition(requisition: any, interview: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${KNOWLEDGE}/${requisition.id}/${interview.idEntrevista}`;
    return this.http.put(url, interview.data, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  saveCommentaryInterviewCandidateRequisition(requisition: any, interview: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${COMMENT}/${requisition.id}/${interview.idEntrevista}`;
    return this.http.put(url, interview.data, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  saveFormatInterviewCandidateRequisition(requisition: any, interview: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    // TODO url
    const url = `${API}${COMPETENCIES}/${requisition.id}/${interview.idEntrevista}`;
    return this.http.put(url, interview.data, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }


  saveDocumentRequisition(requisition: any, document: any) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');
    const formData: FormData = new FormData();
    formData.append('file', document.file, document.file.name);
    return this.http.post(document.path, formData, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  deleteDocumentRequisition(requisition: any, document: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.delete(document.path, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  sendOfferRequisitionCandidate(requisition: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${PROPOSAL}${SENDPROPOSAL}/${requisition.id}`;
    return this.http.put(url, null, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  acceptOfferRequisitionCandidate(requisition: any, respuesta: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${PROPOSAL}${PROPOSEDANSWER}/${requisition.id}`;
    return this.http.put(url, respuesta, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  saveDateEntrySuitableCandidateRequisition(requisition: any, dateEntry: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${PROPOSAL}${SETINCOMEDATE}/${requisition.id}/${dateEntry}`;
    return this.http.put(url, null, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  saveNotificationAltaSuitableCandidateRequisition(requisition: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${PROPOSAL}${NOTIFYALTA}/${requisition.id}`;
    return this.http.put(url, null, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  saveConfirmationAltaSuitableCandidateRequisition(requisition: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${PROPOSAL}${CONFIRMALTA}/${requisition.id}`;
    return this.http.put(url, null, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  sendDocumentationOfCandidateFile(requisition: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${FILESCANDIDATE}${NOTIFYRH}/${requisition.id}`;
    return this.http.put(url, null, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  respondToTheDocumentationOfCandidateFile(requisition: any, comentary: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${FILESCANDIDATE}${NOTIFYCANDIDATE}/${requisition.id}`;
    return this.http.put(url, comentary, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  saveCompetencyInterviewCandidateRequisition(requisition: any, competencies: any[]) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${TEMPLATEINTERVIEW}/${requisition.id}`;
    return this.http.post(url, competencies, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  saveTypeSearchForTheRequisition(requisition: any, idTipoBusqueda: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${REQUESTPLACE}${SETTYPESEARCH}/${requisition.id}/${idTipoBusqueda}`;
    return this.http.put(url, null, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  sendSalaryOfferSuitableCandidateForTheRequisition(requisition: any, data: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${PROPOSAL}${SETSALARIUMPROPOSED}/${requisition.id}`;
    return this.http.put(url, data, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  associateDomainUserWithNewCollaborator(requisition: any, userName: string) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${PROPOSAL}${SETFILEUSER}/${requisition.id}/${userName}`;
    return this.http.put(url, null, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  getCollaborators(): Observable<any[]> {
    return this.http.get<any[]>(`${API}${REASSIGNMENTUSERS}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getCandidatesRepository(): Observable<any[]> {
    return this.http.get<any[]>(`${API}${CANDIDATES}${CATALOG}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getTernasRequisition(requisition: any): Observable<any[]> {
    return this.http.get<any[]>(`${API}${REQUESTPLACE}${GETTERNA}/${requisition.id}`)
      .pipe(
        map( next => this.mapCandidateTernaInterview(next, this) ),
        catchError(this.handleError)
      );
  }

  mapCandidateTernaInterview(next: any[], service: any) {
    const nextCopy = {...{}, ...next};
    const result: any = {};

    result['ternas'] = service.mapTernaCatalog(nextCopy);
    result['componenteTernas'] = service.mapTernaComponent(nextCopy);
    result['ternasCandidatos'] = service.convertToTernasCandidatosplanos([...[], ...result['componenteTernas']]);
    const candidates = service.convertToCandidatesList(result['componenteTernas']);
    result['componenteBusqueda'] = service.mapCandidateComponent([...[], ...candidates]);
    const candidatesWithTerna = candidates.filter( f => f.ternaId > 0 );
    result['componenteEntrevistas'] = service.mapInterviewComponent([...[], ...candidatesWithTerna]);
    const componenteResultadoEntrevista = service.mapFormatInterviewComponent([...[], ...candidatesWithTerna], service);
    result['componenteResultadoEntrevista'] = componenteResultadoEntrevista;
    const evaluacionesIniciadas = service.mapEvaluacionesIniciadas([...[], ...componenteResultadoEntrevista]);
    result['evaluacionesIniciadas'] = evaluacionesIniciadas;

    return result;
  }

  mapTernaCatalog(next: any) {
    const ternas = [];

    for (const i of Object.keys(next)) {
      const terna = {
        text: next[i].terna,
        value: next[i].ternaId
      };
      ternas.push(terna);
    }

    return ternas;
  }

  mapTernaComponent(next: any) {
    const ternas = [];

    for (const i of Object.keys(next)) {
      const terna = {
        terna: next[i].terna,
        ternaId: next[i].ternaId,
        id: next[i].id,
        active: next[i].active,
        candidatos: []
      };
      for (const candidato of next[i].candidatos) {
        candidato.ternaId = next[i].ternaId;
        terna.candidatos.push(candidato);
      }
      if (terna.candidatos && terna.candidatos.length > 0) {
        ternas.push(terna);
      }
    }

    return ternas;
  }

  convertToCandidatesList(next: any[]) {
    const candidatos = [];

    for (const terna of next) {
      for (const candidato of terna.candidatos) {
        candidato.ternaId = terna.ternaId;
        candidatos.push(candidato);
      }
    }

    return candidatos;
  }

  convertToTernasCandidatosplanos(next: any[]) {
    const contactos = [];
    next.forEach(terna => {
      if (terna.candidatos && terna.candidatos.length) {
        terna.candidatos.forEach(candidato => {
          contactos.push({
            ternaId: terna.ternaId,
            candidatoId: candidato.candidatoId,
            candidato: (candidato.nombre || '') + ' ' +  (candidato.materno || '') + ' ' + (candidato.paterno || '')
          });
        });
      }
    });
    return contactos;
  }

  mapCandidateComponent(next: any[]) {
    const clone: CardAccordion[] = [];
    next.forEach( candidate => {
      const parent: CardAccordion = { sections: [] };
      parent.tittle = (candidate.nombre || '') + ' ' + (candidate.paterno || '') + ' ' + (candidate.materno || '');
      parent.toggle = true;
      const section = {
        tittle: 'Entrevistas',
        toggle: true,
        rows: [],
      };
      candidate.entrevistas.forEach( (entrevista, index) => {
        if (index < 2) {
          section.rows.push(
            {
              label: entrevista.entrevistador,
              value: entrevista.fechaInicioEntrevista === null || entrevista.fechaInicioEntrevista === undefined ?
                    { event: 'FechaEntrevista', data: entrevista } : entrevista.fechaInicioEntrevista,
              type: entrevista.fechaInicioEntrevista === null || entrevista.fechaInicioEntrevista === undefined ?
                    DataType.PopUpDateTime : DataType.DateFullTimeMedium
            }
          );
        }
      });
      parent.sections.push(section);
      const sectionEmpleoAnterior = {
        tittle: 'Empleo anterior',
        toggle: false,
        rows: [
          {
            label: 'Empresa',
            value: candidate.detalle && candidate.detalle.ultimoTrabajo && candidate.detalle.ultimoTrabajo.empresa || '',
            type: DataType.String
          },
          {
            label: 'Puesto',
            value: candidate.detalle && candidate.detalle.ultimoTrabajo && candidate.detalle.ultimoTrabajo.puesto || '',
            type: DataType.String
          },
          {
            label: 'Salario',
            value: candidate.detalle && candidate.detalle.ultimoSalarioDescripcion || '',
            type: DataType.String
          },
          {
            label: 'Pretensión económica',
            value: candidate.detalle && candidate.detalle.pretencionEconomica,
            type: DataType.Currency
          },
        ],
      };
      parent.sections.push(sectionEmpleoAnterior);
      let cuentaConCertificacion = '';
      if (candidate.detalle && candidate.detalle.ultimoTrabajo && candidate.detalle.certificacion === true) {
        cuentaConCertificacion = 'SI';
      } else if (candidate.detalle && candidate.detalle.ultimoTrabajo && candidate.detalle.certificacion === false) {
        cuentaConCertificacion = 'NO';
      }
      const cvCargado = candidate.detalle && candidate.detalle.cv && candidate.detalle.cv.loaded;
      const sectionExperiencia = {
        tittle: 'Experiencia',
        toggle: false,
        rows: [
          {
            label: '¿Cuenta con certificación?',
            value: cuentaConCertificacion,
            type: DataType.String
          },
          {
            label: 'Años de experiencia',
            value: (candidate.detalle && candidate.detalle.experiencia) || '',
            type: DataType.String
          },
          {
            label: 'CV',
            value: cvCargado ? candidate.detalle.cv.path : { event: 'CV', data: candidate },
            type: cvCargado ? DataType.Link : DataType.PopUp
          }
        ],
      };
      parent.sections.push(sectionExperiencia);
      parent['data'] = candidate;
      clone.push(parent);
    });
    return clone;
  }

  mapInterviewComponent(next: any[]) {
    const clone: CardAccordion[] = [];
    next.forEach( f => {
      const parent: CardAccordion = { sections: [] };
      parent.tittle = (f.nombre || '') + ' ' + (f.paterno || '') + ' ' + (f.materno || '') ;
      f.entrevistas.forEach( entrevista => {
        const puestoSolicitado = f.detalle && f.detalle.puestoSolicitado;
        const section = {
          tittle: entrevista.tipoEntrevista,
          rows: [
            {
              label: 'Fecha entrevista:',
              value: entrevista.fechaInicioEntrevista === null || entrevista.fechaInicioEntrevista === undefined ?
                { event: 'fechaInicioEntrevista', data: entrevista } : entrevista.fechaInicioEntrevista,
              type: entrevista.fechaInicioEntrevista === null || entrevista.fechaInicioEntrevista === undefined ?
                DataType.PopUpDateTime : DataType.DateFullTimeMedium
            },
            {
              label: 'Entrevistador:',
              value: entrevista.entrevistador,
              type: DataType.String
            },
            {
              label: 'Formato de entrevista:',
              value: {
                event: 'formatoentrevista',
                tittle: entrevista.tipoEntrevista + ' Para el puesto: ' + puestoSolicitado,
                idEntrevista: entrevista.id,
                entrevistador: entrevista.entrevistador,
                data: entrevista
              },
              type: DataType.PopUp
            },
            {
              label: 'Fecha cierre entrevista:',
              value: entrevista.fechaTerminoEntrevista,
              type: DataType.DateFullTimeMedium
            },
          ],
        };
        section['data'] = entrevista;
        parent.sections.push(section);
      });
      parent['data'] = f;
      const totalEntrevistas = f.entrevistas.length;
      const totalRecomenbles = f.entrevistas.filter( f => f.recomendable);
      parent['data']['puedeSeleccionarCandidatoIdoneo'] = totalRecomenbles && totalRecomenbles.length === totalEntrevistas;
      clone.push(parent);
    });
    return clone;
  }

  mapFormatInterviewComponent(next: any[], service: any) {
    const clone: any[] = [];
    next.forEach( (item , index) => {
      const candidate: any = <any>{ interviews: [] };
      candidate.nombre = item.nombre;
      candidate.paterno = item.paterno;
      candidate.materno = item.materno;
      candidate.isSuitable = item.statusCandidato === 1;
      item.entrevistas.forEach( (entrevista, index) => {
        const puestoSolicitado = item.detalle && item.detalle.puestoSolicitado;
        const interview = {
          name: entrevista.tipoEntrevista,
          qualification: service.getQualificationCompetencyInterview(entrevista.competencias),
          recommendIt: entrevista.recomendable,
          data: {
            label: 'Formato de entrevista:',
            value: {
              event: 'formatoentrevista',
              tittle: entrevista.tipoEntrevista + ' Para el puesto: ' + puestoSolicitado,
              entrevistador: entrevista.entrevistador,
              idEntrevista: entrevista.id,
              data: entrevista
            },
            type: DataType.PopUp
          },
        };
        candidate.interviews.push(interview);
      });
      candidate.average = service.getAverageCompetencyInterview(candidate.interviews);
      candidate['detalle'] = item.detalle;
      clone.push(candidate);
    });
    return clone;
  }

  mapEvaluacionesIniciadas(next: any[], service: any) {
    const evaluacionIniciada = false;
    for (const candidato of next) {
      if (candidato.interviews) {
        const seHaIniciadoSuEvaluacion = candidato.interviews.find( f => f.recommendIt === true || f.recommendIt === false) !== undefined;
        if (seHaIniciadoSuEvaluacion) {
          return true;
        }
      }
    }
    return evaluacionIniciada;
  }

  getQualificationCompetencyInterview(competencies: any[]) {
    const maxRating = 5;
    const totalCompetencias = competencies.length;
    let totalResultado = 0;
    for (const item of competencies) {
      totalResultado += item.resultado;
    }
    return parseFloat((totalResultado / (totalCompetencias * maxRating) * maxRating).toFixed(2));
  }

  getAverageCompetencyInterview(interviews: any[]) {
    const totalInterviews = interviews.length;
    let totalResultado = 0;
    for (const item of interviews) {
      totalResultado += item.qualification;
    }
    return parseFloat((totalResultado / totalInterviews).toString()).toFixed(2);
  }

  getCollaboratorByUserName(userName: string): Observable<any> {
    return this.http.get<any>(`${API}${IDENTITY}/${userName}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getSuitableCandidateRequisition(requisition: any): Observable<any> {
    return this.http.get<any>(`${API}${REQUESTPLACE}${IDEALCANDIDATE}/${requisition.id}`)
      .pipe(
        map( next => {
          const candidate = new Candidato();
          if (next) {
            candidate.initFromObject(next);
          }
          return candidate.parceToForm();
        }),
        catchError(this.handleError)
      );
  }

  getFilesByCandidate(requisition: any, candidateId: number): Observable<any> {
    return this.http.get<any>(`${API}${FILES}${FILESCANDIDATE}/${requisition.id}/${candidateId}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getDocumentationEconomicProposal(requisition: any, candidateId: number): Observable<any> {
    return this.http.get<any>(`${API}${FILES}${FILESECONOMICPROPOSAL}/${requisition.id}/${candidateId}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getCatalogs() {
    return this.http.get<any>(`${API}${CATALOGS}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getPlaces(data: any) {
    const url = `${API}${JOBINFORMATION}${CATALOG}/${data.motivoIngresoId}`;
    return this.http.get<any>(url)
      .pipe(
        catchError(this.handleError)
      );
  }

  getAlias(data: any) {
    const url = `${API}${JOBINFORMATION}${CATALOG}/${data.motivoIngresoId}/${data.puestoId}`;
    return this.http.get<any>(url)
      .pipe(
        catchError(this.handleError)
      );
  }

  getJobInformationAdam(data: any) {
    let url = '';
    if (data.aliasId === -1) {
      url = `${API}${JOBINFORMATION}/${data.puestoId}`;
    } else {
      url = `${API}${JOBINFORMATION}/${data.puestoId}/${data.aliasId}`;
    }
    return this.http.get<any>(url)
      .pipe(
        catchError(this.handleError)
      );
  }

  getTemplateInterviewRequisition(requisition: any) {
    return this.http.get<any>(`${API}${TEMPLATEINTERVIEW}${GETBYREQUISITION}/${requisition.id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getMyFiles(): Observable<any> {
    return this.http.get<any>(`${API}${FILES}${FILESCANDIDATE}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  sendMyDocumentation() {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${FILESCANDIDATE}${NOTIFYRH}`;
    return this.http.put(url, null, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  saveMyDocument(document: any) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');
    const formData: FormData = new FormData();
    formData.append('file', document.file, document.file.name);
    return this.http.post(document.path, formData, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  deleteMyDocument(document: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.delete(document.path, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

  verifyCandidateMail(email: string): Observable<any> {
    return this.http.get<any>(`${API}${CANDIDATES}${VERIFYMAIL}/${email}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  saveComplementCV(complementCV: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${API}${CANDIDATES}${UPDATECV}/${complementCV.candidatoId}`;
    return this.http.put(url, complementCV, { headers: headers} )
      .pipe(
        catchError(this.handleError)
      );
  }

}
