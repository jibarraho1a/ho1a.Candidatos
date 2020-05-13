import {ChangeDetectionStrategy, Component, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import { ComponentElement } from '../../../../shared/class/ComponentElement';
import { StatusProcess } from '../../../../shared/models/status-process';
import { Ternas } from '../../../../shared/class/Ternas';
import { Store } from '../../../../shared/class/Store';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { EventApp } from '../../../../shared/models/event-app';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';


@Component({
  selector: 'ho1a-step-search',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-search.component.html',
  styleUrls: ['./step-search.component.scss']
})
export class StepSearchComponent extends ComponentElement implements OnInit, OnChanges {

  @Input()
  currentStep: StatusProcess;

  toggleModalListCandidate = false;
  toggleModalNewCandidate = false;
  toggleModalDateTime = false;
  toggleModalComplementoCV = false;
  toggleModalCV = false;
  candidateSelected: any;

  today: Date = new Date();

  ternasWithCandidates: Ternas;

  constructor(
    private store: Store
  ) {
    super();
    this.ternasWithCandidates = new Ternas();
  }

  ngOnInit() {
    this.databackground.catalogs['ternas'] = this.ternasWithCandidates.getCatalogTernas();
    this.init();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.changes(changes);
  }

  visible() {
    return this.configuration && this.configuration.visible;
  }

  readonly() {
    const candidatoIdoneo =  this.databackground && this.databackground.propuesta && this.databackground.propuesta.candidatoIdoneo;
    const existeCandidatoIdoneo = candidatoIdoneo !== null && candidatoIdoneo !== undefined;
    return existeCandidatoIdoneo;
  }

  initCustom() {
  }

  configFormControl() {
    this.formControls = [
      { name: 'tipoBusqueda', custom: true, isCombo: false }
    ];
  }

  configCatalogs() {
    this.listCatalog = [
      'ternas',
      'tiposBusqueda',
      'ternasCandidatos'
    ];
  }

  configValueChangeCustom(controlName: string, value: any) {
    switch (controlName) {
      case 'tipoBusqueda':
        this.configValueChangeTipoBusqueda(value);
        break;
    }
  }

  private configValueChangeTipoBusqueda(value: any) {
    if (this.data.tipoBusqueda === value) {
      return;
    }
    if (value) {
      this.store.set(StateOption.eventsRequisition,
        <EventApp>{
          typeEvent: TypeEvent.SavingTypeSearchForTheRequisition,
          data: value
        });
    }
  }

  changesCustom() {
  }

  setCatalogsCustom() {
  }

  onClickPopup(event: any, candidate: any) {
    const canSeeModalDatetime = this.getConfigurationByControl('FechaEntrevista').configuration.edicion
      && !this.readonly() && candidate.data.ternaId > 0;
    if (!canSeeModalDatetime) {
      return;
    }
    if (event.event.indexOf('FechaEntrevista', 0) >= 0) {
      this.onToggleModalDateTime();
    } else if (event.event.indexOf('complementCV', 0) >= 0) {
      this.onClickButtomActionModalComplementoCV(null);
    } else if (event.event.indexOf('CV', 0) >= 0) {
      this.onToggleModalCV();
    }
    this.candidateSelected = {...candidate.data, event: event};
  }

  onToggleModalListCandidate() {
    this.toggleModalListCandidate = !this.toggleModalListCandidate;
  }

  onToggleModalNewCandidate() {
    this.toggleModalNewCandidate = !this.toggleModalNewCandidate;
  }

  onToggleModalDateTime() {
    this.toggleModalDateTime = !this.toggleModalDateTime;
  }

  onToggleModalComplementoCV() {
    this.toggleModalComplementoCV = !this.toggleModalComplementoCV;
  }

  onToggleModalCV() {
    this.toggleModalCV = !this.toggleModalCV;
  }

  onCancelModalListCandidate() {
    this.onToggleModalListCandidate();
  }

  onAcceptModalListCandidate(event: any[]) {
    this.onToggleModalListCandidate();
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.AddingCandidateToTheRequisition,
        data: event
      });
  }

  onCancelModalNewCandidate() {
    this.onToggleModalNewCandidate();
  }

  onClickButtomActionModalNewCandidate(event: any) {
    this.onToggleModalNewCandidate();
  }

  onCancelModalDateTime() {
    this.onToggleModalDateTime();
  }

  onClickButtomActionModalDateTime(event: any) {
    this.candidateSelected.event.data['fechaInicioEntrevista'] = event.form.datetimestring;
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.SavingInterviewCandidateRequisition,
        data: this.candidateSelected.event.data
      });
    this.onToggleModalDateTime();
  }

  onCancelModalComplementoCV() {
    this.onToggleModalComplementoCV();
  }

  onClickButtomActionModalComplementoCV(event: any) {
    this.onToggleModalComplementoCV();
  }

  onCancelModalCV() {
    this.onToggleModalCV();
  }

  onClickButtomActionModalCV(event: any) {
    this.onToggleModalCV();
  }

}
