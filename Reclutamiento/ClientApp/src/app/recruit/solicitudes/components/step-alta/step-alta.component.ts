import {ChangeDetectionStrategy, Component, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import { ComponentElement } from '../../../../shared/class/ComponentElement';
import { CustomFormat } from '../../../../shared/class/CustomFormat';
import { StatusProcess } from '../../../../shared/models/status-process';
import { Store } from '../../../../shared/class/Store';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { EventApp } from '../../../../shared/models/event-app';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';
import { DataType } from '../../../../shared/enums/data-type.enum';


@Component({
  selector: 'ho1a-step-alta',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-alta.component.html',
  styleUrls: ['./step-alta.component.scss']
})
export class StepAltaComponent extends ComponentElement implements OnInit, OnChanges {

  format: CustomFormat;

  @Input()
  currentStep: StatusProcess;

  today: Date = new Date();

  constructor(
    private store: Store
  ) {
    super();
    this.format = new CustomFormat();
  }

  ngOnInit() {
    this.init();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.changes(changes);
  }

  visible() {
    return this.data.propuestaAceptada && this.configuration && this.configuration.visible;
  }

  readonly() {
    return false;
  }

  initCustom() {
  }

  configFormControl() {
    this.formControls = [
      { name: 'fechaIngreso', custom: true, isCombo: false },
      { name: 'fechaConfirmacionAlta', custom: false, isCombo: false },
      { name: 'fechaNotificacionAlta', custom: false, isCombo: false },
      { name: 'userAd', custom: false, isCombo: false },
    ];
  }

  configCatalogs() {
    this.listCatalog = [
    ];
  }

  configValueChangeCustom(controlName: string, value: any) {
    switch (controlName) {
      case 'fechaIngreso':
        this.configValueChangeFechaIngreso(value);
        break;
    }
  }

  private configValueChangeFechaIngreso(value: any) {
    if (value && !this.format.isSame(value, this.data['fechaIngreso'])) {
      this.store.set(StateOption.eventsRequisition,
        <EventApp>{
          typeEvent: TypeEvent.SavingDateEntrySuitableCandidateRequisition,
          data: this.format.format(DataType.DateDash, value)
        });
    }
  }

  changesCustom() {

  }

  setCatalogsCustom() {
  }

  onNotificationAlta() {
    this.form.get('fechaNotificacionAlta').setValue(new Date());
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.SavingNotificationAltaSuitableCandidateRequisition,
        data: null
      });
  }

  onConfirmationAlta() {
    this.form.get('fechaConfirmacionAlta').setValue(new Date());
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.SavingConfirmationAltaSuitableCandidateRequisition,
        data: null
      });
  }

  onUpload(document: any) {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.SavingDocumentEconomicOfferRequisition,
        data: document
      });
  }

  onDelete(document: any) {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.DeletingDocumentEconomicOfferRequisition,
        data: document
      });
  }

  get fechaIngresoEstablecida() {
    return this.form.get('fechaIngreso').value !== null;
  }

  get altaConfirmada() {
    return this.form.get('fechaConfirmacionAlta').value !== null;
  }

  get altaNotificada() {
    return this.form.get('fechaNotificacionAlta').value !== null;
  }

  get usuarioDominioAsociado() {
    const value = this.data && this.data.candidatoIdoneo && this.data.candidatoIdoneo.userAd;
    return value !== null && value !== undefined;
  }

  get salary() {
    return this.format.format(DataType.Currency, this.data.salario);
  }

  get bonos() {
    return this.format.format(DataType.Currency, this.data.bonos);
  }

  get propuestaAceptada() {
    let result = '';
    if (this.data.propuestaAceptada) {
      result = 'SI';
    } else if (this.data.propuestaAceptada === false) {
      result = 'SI';
    }
    return result;
  }

  isVisibleByControl(controlName: string) {
    let visible = false;
    switch (controlName) {
      case 'NotificacionAltaRequisicion_btn':
        visible = this.fechaIngresoEstablecida && !this.altaNotificada
          && this.getActionByControl('NotificacionAltaRequisicion').visible;
        break;
      case 'NotificacionAltaRequisicion':
        visible = this.fechaIngresoEstablecida && this.altaNotificada;
        break;
      case 'AsignarUserNameAD':
        visible = this.fechaIngresoEstablecida && this.altaNotificada
          && !this.usuarioDominioAsociado && this.getActionByControl(controlName).visible;
        break;
      case 'ConfirmacionAltaRequisicion_btn':
        visible = this.fechaIngresoEstablecida && this.altaNotificada
          && this.usuarioDominioAsociado && !this.altaConfirmada
          && this.getActionByControl('ConfirmacionAltaRequisicion').visible;
        break;
      case 'ConfirmacionAltaRequisicion':
        visible = this.fechaIngresoEstablecida && this.altaNotificada && this.usuarioDominioAsociado && this.altaConfirmada;
        break;
    }
    return visible;
  }

}
