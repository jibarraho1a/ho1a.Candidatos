import { Component, OnInit, OnDestroy } from '@angular/core';
import { EdithElement } from '../../../../shared/class/EdithElement';
import { SolicitudService } from '../../../../shared/services/solicitud.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '../../../../shared/class/Store';
import { CustomFormat } from '../../../../shared/class/CustomFormat';
import { LoadingAction } from '../../../../shared/enums/loading-action.enum';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { DataType } from '../../../../shared/enums/data-type.enum';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';
import { EventApp } from '../../../../shared/models/event-app';

@Component({
  selector: 'app-nueva-solicitud',
  templateUrl: './nueva-solicitud.component.html',
  styleUrls: ['./nueva-solicitud.component.scss']
})
export class NuevaSolicitudComponent extends EdithElement implements OnInit, OnDestroy {

  candidateId = null;
  util: CustomFormat;

  constructor(
    private service: SolicitudService,
    private router: Router,
    private route: ActivatedRoute,
    private store: Store
  ) {
    super();
    this.util = new CustomFormat();
  }

  ngOnInit() {
    const key = this.route.snapshot.params.id;
    this.init(key);
    this.subscriptions = [
      this.service.events$.subscribe( (next: EventApp) => this.configureSubscriptions(next) ),
    ];
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  configureSubscriptions(next: EventApp) {
    if (!next || !this.exist) {
      return;
    }
    switch (next.typeEvent) {
      case TypeEvent.SavingRequisition:
        this.save(next.data);
        break;
      case TypeEvent.RemovingRequisition:
        this.delete();
        break;
      case TypeEvent.SavingRequisitionSendingToValidationRequisition:
        this.saveRequisitionSendToValidationRequisition(next.data);
        break;
      case TypeEvent.SendingToValidationRequisition:
        this.sendToValidationRequisition(next.data);
        break;
      case TypeEvent.AssignedRequisition:
        this.assignRequisition(next.data);
        break;
      case TypeEvent.AddingCandidate:
        this.addCandidate(next.data);
        break;
      case TypeEvent.AddingCandidateToTheRequisition:
        this.addCandidatesToTheRequisition(next.data);
        break;
      case TypeEvent.AssigningTernaToCandidateRequisition:
        this.assignTernaToCandidateRequisition(next.data);
        break;
      case TypeEvent.SavingInterviewCandidateRequisition:
        this.saveInterviewCandidateRequisition(next.data);
        break;
      case TypeEvent.AddingGuestCandidateInterviewRequisition:
        this.addGuestCandidateInterviewRequisition(next.data);
        break;
      case TypeEvent.LookingForACollaboratorByUsername:
        this.getCollaboratorByUsername(next.data);
        break;
      case TypeEvent.SelectingSuitableCandidateRequisition:
        this.saveSelectSuitableCandidateRequisition(next.data);
        break;

      case TypeEvent.SavingCommentaryInterviewCandidateRequisition:
        this.saveCommentaryInterviewCandidateRequisition(next.data);
        break;
      case TypeEvent.SavingCompetencyKnowledgeInterviewCandidateRequisition:
        this.saveCompetencyKnowledgeInterviewCandidateRequisition(next.data);
        break;
      case TypeEvent.SavingCompetencyLeadershipInterviewCandidateRequisition:
        this.saveCompetencyLeadershipInterviewCandidateRequisition(next.data);
        break;
      case TypeEvent.SavingFormatInterviewCandidateRequisition:
        this.saveFormatInterviewCandidateRequisition(next.data);
        break;

      case TypeEvent.SavingDocumentRequisition:
        this.saveDocumentRequisition(next.data);
        break;
      case TypeEvent.DeletingDocumentRequisition:
        this.deleteDocumentRequisition(next.data);
        break;
      case TypeEvent.SavingDocumentEconomicOfferRequisition:
        this.saveDocumentEconomicOfferRequisition(next.data);
        break;
      case TypeEvent.DeletingDocumentEconomicOfferRequisition:
        this.deleteDocumentEconomicOfferRequisition(next.data);
        break;
      case TypeEvent.SendingOfferRequisitionCandidate:
        this.sendOfferRequisitionCandidate();
        break;
      case TypeEvent.AcceptingOfferRequisitionCandidate:
        this.acceptOfferRequisitionCandidate(next.data);
        break;
      case TypeEvent.SavingDateEntrySuitableCandidateRequisition:
        this.saveDateEntrySuitableCandidateRequisition(next.data);
        break;
      case TypeEvent.SavingNotificationAltaSuitableCandidateRequisition:
        this.saveNotificationAltaSuitableCandidateRequisition();
        break;
      case TypeEvent.SavingConfirmationAltaSuitableCandidateRequisition:
        this.saveConfirmationAltaSuitableCandidateRequisition();
        break;
      case TypeEvent.SendingDocumentationOfCandidateFile:
        this.sendDocumentationOfCandidateFile();
        break;
      case TypeEvent.RespondingToTheDocumentationOfCandidateFile:
        this.respondToTheDocumentationOfCandidateFile(next.data);
        break;
      case TypeEvent.SavingCompetencyInterviewCandidateRequisition:
        this.saveCompetencyInterviewCandidateRequisition(next.data);
        break;

      case TypeEvent.SendingSalaryOfferSuitableCandidateForTheRequisition:
        this.sendSalaryOfferSuitableCandidateForTheRequisition(next.data);
        break;

      case TypeEvent.GettingAlias:
        this.getAlias(next.data);
        break;
      case TypeEvent.GettingPlaces:
        this.getPlaces(next.data);
        break;
      case TypeEvent.GettingJobInformationAdam:
        this.getJobInformationAdam(next.data);
        break;

      case TypeEvent.SavingTypeSearchForTheRequisition:
        this.saveTypeSearchForTheRequisition(next.data);
        break;

      case TypeEvent.AssociatingDomainUserWithNewCollaborator:
        this.associateDomainUserWithNewCollaborator(next.data);
        break;

      case TypeEvent.VerifyingCandidateMail:
        this.verifyCandidateMail(next.data);
        break;

      case TypeEvent.SavingComplementCV:
        this.saveComplementCV(next.data);
        break;
      case TypeEvent.SavingDocumentCVCandidate:
        this.saveDocumentCVCandidate(next.data);
        break;

    }
  }

  alertService() {
    this.service.alertError(this.errorMensage);
  }

  getConfigurarionService() {
    return this.service.getConfigurations(this.id).subscribe(
      (next: any) => this.configuration = next,
      (error: any) => this.observer.error(error),
      () => {
        this.completeConfiguration = true;
        if (this.completeItem) {
          this.observer.complete();
        }
      }
    );
  }

  getItemService() {
    return this.service.get(this.id).subscribe(
      next => this.item = next,
      error => this.observer.error(error),
      () => {
        this.completeItem = true;
        if (this.completeConfiguration) {
          this.observer.complete();
        }
      }
    );
  }

  saveService() {
    this.service.save(this.item).subscribe(
      next => { },
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.getConfigurations();
      }
    );
  }

  reload(id: number) {
    this.router.navigate([`mirh/solicitud/${id}`]);
  }

  loadInformationBackground() {
    this.getCatalogs();
    this.getCollaborators();
    this.getSuitableCandidateRequisition();
    this.getTemplateInterviewRequisition();
  }

  getConfigurations() {
    this.service.getConfigurations(this.id).subscribe(
      next => this.configuration = next,
      error => {},
      () => {},
    );
  }

  getCatalogs() {
    this.service.getCatalogs().subscribe(
      next => {
        const catalogos = {
          ...{},
          ...this.itemBackground.catalogs,
          estadosCivil: next.estadosCivil,
          ultimosSalarios: next.ultimosSalarios,
          localidadesSucursa: next.localidadesSucursa,
          puestoSolicitado: next.puestoSolicitado,
          alias: next.alias,
          referenciasVacante: next.referenciasVacante,
          tiposBusqueda: [
            { value: null, text: 'Seleccione un tipo de busqueda' },
            { value: 1, text: 'Interno' },
            { value: 2, text: 'Externo' }
          ]
        };
        const data = {...{}, ...this.itemBackground};
        data['catalogs'] = catalogos;
        this.itemBackground = data;
      },
      error => {},
      () => {
        this.getTernasRequisition();
      },
    );
  }

  getPlaces(data: any) {
    if (data.withLoading) {
      this.typeLoadingAction = LoadingAction.change;
      this.initAction();
      this.service.getPlaces(data.value).subscribe(
        next => this.getPlacesNext(next),
        error => this.handlerError(error),
        () => this.completeAction()
      );
    } else {
      this.service.getPlaces(data.value).subscribe(
        next => this.getPlacesNext(next),
        error => {},
        () => {},
      );
    }
  }

  private getPlacesNext(next: any) {
    const catalogos = {...{}, ...this.itemBackground.catalogs, puestosadam: next};
    const data = {...{}, ...this.itemBackground};
    data['catalogs'] = catalogos;
    this.itemBackground = data;
  }

  getAlias(data: any) {
    this.service.getAlias(data.value).subscribe(
      next => this.getAliasNext(next),
      error => {},
      () => {},
    );
  }

  private getAliasNext(next: any) {
    if (next && next.length === 1) {
      next.push(<any>{
        disabled: false,
        group: null,
        selected: true,
        text: 'N/A',
        value: '-1'
      });
    }
    const catalogos = { ...{}, ...this.itemBackground.catalogs, aliasadam: next };
    const data = { ...{}, ...this.itemBackground };
    data['catalogs'] = catalogos;
    this.itemBackground = data;
  }

  getJobInformationAdam(data: any) {
    if (data.withLoading) {
      this.typeLoadingAction = LoadingAction.change;
      this.initAction();
      this.service.getJobInformationAdam(data.value).subscribe(
        next => this.getJobInformationAdamNext(next),
        error => this.handlerError(error),
        () => this.completeAction()
      );
    } else {
      this.service.getJobInformationAdam(data.value).subscribe(
        next => this.getJobInformationAdamNext(next),
        error => {},
        () => {},
      );
    }
  }

  private getJobInformationAdamNext(next: any) {
    const data = {...{}, ...this.item};
    data['numeroVacantes'] = next.numeroVacantes;
    data['mercado'] = next.mercado;
    data['departamento'] = next.departamento;
    data['descripcionTrabajo'] = next.descripcion;
    data['responsabilidades'] = next.responsabilidades;
    const minimo = next.tabulador && next.tabulador.minimo;
    const maximo = next.tabulador && next.tabulador.maximo;
    const tabulador = this.util.format(DataType.Currency, minimo) + ' - ' + this.util.format(DataType.Currency, maximo);
    data['tabuladorSalarioMonto'] = tabulador;
    data['tabuladorSalarioId'] = next.tabulador && next.tabulador.id;
    data['nivelOrganizacional'] = next.nivelOrganizacional;
    this.item = data;
  }

  getCollaborators() {
    this.service.getCollaborators().subscribe(
      next => this.itemBackground['colaboradores'] = next,
      error => {},
      () => {},
    );
  }

  getCandidatesRepository() {
    this.service.getCandidatesRepository().subscribe(
      next => {
        const candidatos = this.itemBackground['componenteBusqueda'];
        const clone = next.filter( item => {
          return (candidatos && candidatos.length === 0) ||
            (candidatos && candidatos.find(f => f.data.candidatoId === item.id) === undefined);
        });
        this.itemBackground['candidatosRepositorio'] = clone;
      },
      error => {},
      () => {},
    );
  }

  getTernasRequisition() {
    this.service.getTernasRequisition(this.item).subscribe(
      (next: any) => {
        const data = {...{}, ...this.itemBackground};
        // data['catalogs']['ternas'] = next.ternas;
        data['catalogs']['ternasCandidatos'] = next.ternasCandidatos;
        data['componenteEntrevistas'] = next.componenteEntrevistas;
        data['componenteBusqueda'] = next.componenteBusqueda;
        data['componenteTernas'] = next.componenteTernas;
        data['componenteResultadoEntrevista'] = next.componenteResultadoEntrevista;
        data['acciones'] = {
          terna: {
            evaluacionesIniciadas: next.evaluacionesIniciadas
          }
        };
        this.itemBackground = data;
      },
      error => {},
      () => {
        this.getCandidatesRepository();
      },
    );
  }

  getCollaboratorByUsername(userName: string) {
    this.service.getCollaboratorByUserName(userName).subscribe(
      next => this.store.set(StateOption.userSearch, next),
      error => {},
      () => {},
    );
  }

  getFilesByCandidate() {
    this.service.getFilesByCandidate(this.item, this.candidateId).subscribe(
      next => {
        const data = {...{}, ...this.itemBackground};
        data['expediente'] = next;
        this.itemBackground = data;
      },
      error => {},
      () => {},
    );
  }

  getDocumentationEconomicProposal() {
    this.service.getDocumentationEconomicProposal(this.item, this.candidateId).subscribe(
      next => {
        const data = {...{}, ...this.itemBackground};
        data['propuesta']['documentosPropuesta'] = next;
        this.itemBackground = data;
      },
      error => {},
      () => {},
    );
  }

  getSuitableCandidateRequisition() {
    this.service.getSuitableCandidateRequisition(this.item).subscribe(
      next => {
        const data = {...{}, ...this.itemBackground};
        if (next) {
          data['propuesta'] = {...{}, ...this.item.propuesta};
          const salario = data['propuesta']['salario'];
          data['propuesta']['candidatoIdoneo'] = next;
          data['propuesta']['candidatoIdoneo']['salario'] = salario;
          data['propuesta']['candidatoIdoneo']['bonos'] = data['propuesta']['bonos'];
          data['propuesta']['candidatoIdoneo']['beneficios'] = data['propuesta']['beneficios'];

          const minimo = this.item.tabuladorSalario && this.item.tabuladorSalario.minimo;
          const maximo = this.item.tabuladorSalario && this.item.tabuladorSalario.maximo;
          const tabulador = this.util.format(DataType.Currency, minimo) + ' - ' + this.util.format(DataType.Currency, maximo);
          data['propuesta']['tabulador'] = tabulador;

          this.itemBackground = data;
          this.candidateId = next.id;
          this.getFilesByCandidate();
          this.getDocumentationEconomicProposal();
        } else if (data['propuesta']) {
          data['propuesta'] = null;
          data['expediente'] = null;
          this.itemBackground = data;
        }
      },
      error => {},
      () => {},
    );
  }

  getTemplateInterviewRequisition() {
    this.service.getTemplateInterviewRequisition(this.item).subscribe(
      (next: any) => {
        const data = {...{}, ...this.itemBackground};
        data['templateInterview'] = next;
        this.itemBackground = data;
      },
      error => {},
      () => {},
    );
  }

  onCreateRequisition() {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.save(this.item).subscribe(
      next => this.reload(next.id),
      error => this.handlerError(error),
      () => this.completeAction()
    );
  }

  delete() {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.delete(this.item.id).subscribe(
      next => {},
      error => this.handlerError(error),
      () => this.router.navigate([`mirh/solicitud`])
    );
  }

  saveRequisitionSendToValidationRequisition(event: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.save(this.item).subscribe(
      next => {},
      error => this.handlerError(error),
      () => {
        this.onValidateAuthorizationRequisition();
      }
    );
  }

  sendToValidationRequisition(event: any) {
    this.onValidateAuthorizationRequisition(event);
  }

  onValidateAuthorizationRequisition(validation?: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveValidation(this.item, validation).subscribe(
      next => {},
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.init(this.id);
      }
    );
  }

  assignRequisition(username: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveAssignment(this.item, username).subscribe(
      next => {
        const data = {...{}, ...this.item};
        data['asignado'] = next;
        data['inicioReclutamiento'] = new Date();
        this.item = data;
      },
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.getConfigurations();
      }
    );
  }

  addCandidate(candidate: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.addCandidate(this.item, candidate).subscribe(
      next => this.getTernasRequisition(),
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.getConfigurations();
      }
    );
  }

  addCandidatesToTheRequisition(candidates: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.addCandidatesRequisition(this.item, candidates).subscribe(
      next => this.getTernasRequisition(),
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.getConfigurations();
      }
    );
  }

  assignTernaToCandidateRequisition(candidateRequisition: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.addTernaToCandidateRequisition(this.item, candidateRequisition).subscribe(
      next => this.getTernasRequisition(),
      error => this.handlerError(error),
      () => {
        this.completeAction();
        if (candidateRequisition.ternaId > 0) {
          this.getConfigurations();
        }
      }
    );
  }

  saveInterviewCandidateRequisition(interview: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveInterviewCandidateRequisition(this.item, interview).subscribe(
      next => this.getTernasRequisition(),
      error => this.handlerError(error),
      () => this.completeAction()
    );
  }

  addGuestCandidateInterviewRequisition(interview: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.addGuestCandidateInterviewRequisition(this.item, interview).subscribe(
      next => this.getTernasRequisition(),
      error => this.handlerError(error),
      () => this.completeAction()
    );
  }

  saveSelectSuitableCandidateRequisition(data: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveSelectSuitableCandidateRequisition(this.item, data).subscribe(
      next => {
        this.getTernasRequisition();
        this.getSuitableCandidateRequisition();
      },
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.getConfigurations();
      }
    );
  }

  saveCommentaryInterviewCandidateRequisition(interview: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveCommentaryInterviewCandidateRequisition(this.item, interview).subscribe(
      next => this.getTernasRequisition(),
      error => this.handlerError(error),
      () => this.completeAction()
    );
  }

  saveCompetencyKnowledgeInterviewCandidateRequisition(interview: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveKnowledgeCompetenciesInterviewCandidateRequisition(this.item, interview).subscribe(
      next => this.getTernasRequisition(),
      error => this.handlerError(error),
      () => this.completeAction()
    );
  }

  saveCompetencyLeadershipInterviewCandidateRequisition(interview: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveLeadershipCompetenciesInterviewCandidateRequisition(this.item, interview).subscribe(
      next => this.getTernasRequisition(),
      error => this.handlerError(error),
      () => this.completeAction()
    );
  }

  saveFormatInterviewCandidateRequisition(interview: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveFormatInterviewCandidateRequisition(this.item, interview).subscribe(
      next => this.getTernasRequisition(),
      error => this.handlerError(error),
      () => this.completeAction()
    );
  }

  saveDocumentRequisition(document: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveDocumentRequisition(this.item, document).subscribe(
      next => this.getFilesByCandidate(),
      error => this.handlerError(error),
      () => this.completeAction()
    );
  }

  deleteDocumentRequisition(document: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.deleteDocumentRequisition(this.item, document).subscribe(
      next => this.getFilesByCandidate(),
      error => this.handlerError(error),
      () => this.completeAction()
    );
  }

  saveDocumentEconomicOfferRequisition(document: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveDocumentRequisition(this.item, document).subscribe(
      next => this.getDocumentationEconomicProposal(),
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.getConfigurations();
      }
    );
  }

  deleteDocumentEconomicOfferRequisition(document: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.deleteDocumentRequisition(this.item, document).subscribe(
      next => this.getDocumentationEconomicProposal(),
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.getConfigurations();
      }
    );
  }

  sendOfferRequisitionCandidate() {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.sendOfferRequisitionCandidate(this.item).subscribe(
      next => this.getConfigurations(),
      error => this.handlerError(error),
      () => this.completeAction()
    );
  }

  acceptOfferRequisitionCandidate(respuesta: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.acceptOfferRequisitionCandidate(this.item, respuesta).subscribe(
      next => {},
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.init(this.id);
      }
    );
  }

  saveDateEntrySuitableCandidateRequisition(dateEntry: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveDateEntrySuitableCandidateRequisition(this.item, dateEntry).subscribe(
      next => {},
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.getConfigurations();
      }
    );
  }

  saveNotificationAltaSuitableCandidateRequisition() {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveNotificationAltaSuitableCandidateRequisition(this.item).subscribe(
      next => {},
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.getConfigurations();
      }
    );
  }

  saveConfirmationAltaSuitableCandidateRequisition() {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveConfirmationAltaSuitableCandidateRequisition(this.item).subscribe(
      next => {},
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.getConfigurations();
      }
    );
  }

  sendDocumentationOfCandidateFile() {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.sendDocumentationOfCandidateFile(this.item).subscribe(
      next => {},
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.getConfigurations();
      }
    );
  }

  respondToTheDocumentationOfCandidateFile(comentario: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.respondToTheDocumentationOfCandidateFile(this.item, comentario).subscribe(
      next => {},
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.getConfigurations();
      }
    );
  }

  saveCompetencyInterviewCandidateRequisition(competencies: any[]) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveCompetencyInterviewCandidateRequisition(this.item, competencies).subscribe(
      next => {
        this.getTemplateInterviewRequisition();
        this.getTernasRequisition();
      },
      error => this.handlerError(error),
      () => this.completeAction()
    );
  }

  saveTypeSearchForTheRequisition(idTipoBusqueda: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveTypeSearchForTheRequisition(this.item, idTipoBusqueda).subscribe(
      next => {
      },
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.getConfigurations();
      }
    );
  }

  sendSalaryOfferSuitableCandidateForTheRequisition(data: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.sendSalaryOfferSuitableCandidateForTheRequisition(this.item, data).subscribe(
      next => {
      },
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.init(this.id);
      }
    );
  }

  associateDomainUserWithNewCollaborator(userName: string) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.associateDomainUserWithNewCollaborator(this.item, userName).subscribe(
      next => {
        const data = {...{}, ...this.itemBackground};
        data['propuesta']['candidatoIdoneo']['userAd'] = userName;
        this.itemBackground = data;
      },
      error => this.handlerError(error),
      () => {
        this.completeAction();
        this.getConfigurations();
      }
    );
  }

  verifyCandidateMail(email: string) {
    this.service.verifyCandidateMail(email).subscribe(
      next => {
        this.store.set(StateOption.emailSearch, next);
      },
      error => {},
      () => { },
    );
  }

  saveComplementCV(complementCV: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveComplementCV(complementCV).subscribe(
      next => {
        console.log(next);
        this.getTernasRequisition();
      },
      error => this.handlerError(error),
      () => {
        this.completeAction();
      }
    );
  }

  saveDocumentCVCandidate(cv: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveMyDocument(cv).subscribe(
      next => {
        console.log(next);
        this.getTernasRequisition();
      },
      error => this.handlerError(error),
      () => {
        this.completeAction();
      }
    );
  }

}
