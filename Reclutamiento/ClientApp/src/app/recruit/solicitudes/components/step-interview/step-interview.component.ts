import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnInit, Output} from '@angular/core';
import { ComponentElement } from '../../../../shared/class/ComponentElement';
import { StatusProcess } from '../../../../shared/models/status-process';
import { Store } from '../../../../shared/class/Store';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { EventApp } from '../../../../shared/models/event-app';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';


@Component({
  selector: 'ho1a-step-interview',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-interview.component.html',
  styleUrls: ['./step-interview.component.scss']
})
export class StepInterviewComponent extends ComponentElement implements OnInit, OnChanges {

  @Input()
  currentStep: StatusProcess;

  toggleModalInterviewFormat = false;
  toggleModalDateTime = false;
  candidateSelected: any;

  today: Date = new Date();

  readOnlyFormatIntervier = true;

  constructor(private store: Store) {
    super();
  }

  ngOnInit() {
    this.init();
  }

  ngOnChanges() {
    this.changes();
  }

  visible() {
    return this.configuration && this.configuration.visible;
  }

  readonly() {
    const candidatoIdoneo = this.databackground && this.databackground.propuesta && this.databackground.propuesta.candidatoIdoneo;
    const existeCandidatoIdoneo = candidatoIdoneo !== null && candidatoIdoneo !== undefined;
    return existeCandidatoIdoneo;
  }

  initCustom() {
  }

  configFormControl() {
    this.formControls = [];
  }

  configCatalogs() {
    this.listCatalog = [];
  }

  configValueChangeCustom() {

  }

  changesCustom() {

  }

  setCatalogsCustom() {
  }

  onClickPopup(event: any, candidate: any) {
    if (event.event.indexOf('formatoentrevista', 0) >= 0) {
      this.readOnlyFormatIntervier = !this.userLoginIsSameInterviewer(event.data.entrevistadorUserName) ||
      this.readonly() || event.data.fechaTerminoEntrevista !== null;
      this.onToggleModalInterviewFormat();
    } else if (event.event.indexOf('fechaInicioEntrevista', 0) >= 0
      && this.getConfigurationByControl(event.event).configuration.edicion
      && !this.readonly()
    ) {
      this.onToggleModalDateTime();
    } else if (event.event.indexOf('fechaTerminoEntrevista', 0) >= 0
      && (this.getConfigurationByControl(event.event).configuration.edicion
        || this.userLoginIsSameInterviewer(event.data.entrevistadorUserName))
      && !this.readonly()
    ) {
      this.onToggleModalDateTime();
    }

    this.candidateSelected = {...candidate.data, event: event};
  }

  userLoginIsSameInterviewer(userInterviewer: string) {
    return userInterviewer.toUpperCase() === JSON.parse(localStorage.getItem('user')).userName.toUpperCase();
  }

  onToggleModalInterviewFormat() {
    this.toggleModalInterviewFormat = !this.toggleModalInterviewFormat;
  }

  onCancelModalInterviewFormat() {
    this.onToggleModalInterviewFormat();
  }

  onClickButtomActionModalInterviewFormat(event: any) {
    this.onToggleModalInterviewFormat();
  }

  onToggleModalDateTime() {
    this.toggleModalDateTime = !this.toggleModalDateTime;
  }

  onCancelModalDateTime() {
    this.onToggleModalDateTime();
  }

  onClickButtomActionModalDateTime(event: any) {
    this.candidateSelected.event.data[this.candidateSelected.event.event] = event.form.datetimestring;
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.SavingInterviewCandidateRequisition,
        data: this.candidateSelected.event.data
      });
    this.onToggleModalDateTime();
  }

}
