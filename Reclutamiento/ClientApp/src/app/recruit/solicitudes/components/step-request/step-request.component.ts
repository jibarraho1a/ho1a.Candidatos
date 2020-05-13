import { ComponentElement } from '../../../../shared/class/ComponentElement';
import { StatusProcess } from '../../../../shared/models/status-process';
import { CustomFormat } from '../../../../shared/class/CustomFormat';
import { Store } from '../../../../shared/class/Store';
import { DataType } from '../../../../shared/enums/data-type.enum';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { AlertType } from '../../../../shared/enums/alert-type.enum';
import { Alert } from '../../../../shared/models/alert';
import { EventApp } from '../../../../shared/models/event-app';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';
import { TypeButtonAction } from '../../../../shared/enums/type-button-action.enum';

import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';



@Component({
  selector: 'ho1a-step-request',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-request.component.html',
  styleUrls: ['./step-request.component.scss']
})
export class StepRequestComponent extends ComponentElement implements OnInit, OnChanges {

  @Input()
  currentStep: StatusProcess;

  today: Date = new Date();

  startfechaInicio: Date = new Date();

  util: CustomFormat;

  otraLocalidadVisible = false;
  puestoSolicitadoVisible = false;
  aliasVisible = false;
  infoAdamVisible = false;

  constructor(
    private store: Store,
  ) {
    super();
    this.util = new CustomFormat();
  }

  ngOnInit() {
    this.init();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.changes(changes);
  }

  visible() {
    return this.configuration && this.configuration.visible;
  }

  readonly() {
    return false;
  }

  initCustom() {
    this.initMotivoIngreso();
    this.initPuestoSolicitado();
    this.initAlias();
    this.initLocalidad();
    this.initTipoPlaza();
  }

  private initMotivoIngreso() {
    const motivoIngreso = this.form.get('motivoIngreso');
    const cargarcatalogoPuestosAdam = motivoIngreso.value > 0;
    if (cargarcatalogoPuestosAdam) {
      this.onGetPlaces(motivoIngreso.value);
    }
  }

  private initPuestoSolicitado() {
    this.puestoSolicitadoVisible = this.form.get('motivoIngreso').value ? true : false;
    const puestoSolicitado = this.form.get('puestoSolicitado');
    const motivoIngreso = this.form.get('motivoIngreso');
    const cargarcatalogoAliasAdam = puestoSolicitado.value > 0 && motivoIngreso.value > 0;
    if (cargarcatalogoAliasAdam) {
      this.onGetAlias(motivoIngreso.value, puestoSolicitado.value);
    }
  }

  private initAlias() {
    this.aliasVisible = this.form.get('puestoSolicitado').value ? true : false;
    const aliasId = this.form.get('aliasId');
    const puestoSolicitado = this.form.get('puestoSolicitado');
    this.infoAdamVisible = (aliasId.value === '-1' || aliasId.value > 0) && puestoSolicitado.value > 0;
    const cargarcatalogoPuestosAdam = aliasId.value && puestoSolicitado.value
      && this.getConfigurationByControl('motivoIngreso').configuration.edicion
      && this.getConfigurationByControl('puestoSolicitado').configuration.edicion
      && this.getConfigurationByControl('aliasId').configuration.edicion;
    if (cargarcatalogoPuestosAdam) {
      this.onGetJobInformationAdam(puestoSolicitado.value, aliasId.value);
    } else {
      this.form.get('tabuladorSalarioMonto').setValue(this.getTabuladorTransformed());
    }
  }

  private getTabuladorTransformed() {
    const tabuladorSalario = this.data['tabuladorSalario'];
    const minimo = tabuladorSalario && tabuladorSalario.minimo;
    const maximo = tabuladorSalario && tabuladorSalario.maximo;
    const tabuladorTransformed = this.util.format(DataType.Currency, minimo) + ' - ' + this.util.format(DataType.Currency, maximo);
    return tabuladorTransformed;
  }

  private initLocalidad() {
    const localidad = this.form.get('localidad');
    if (localidad.value === '5') {
      this.otraLocalidadVisible = true;
    }
    if (this.otraLocalidadVisible && !this.form.get('otraLocalidad').value) {
      this.resetLocalidad(this.otraLocalidadVisible);
    } else if (this.otraLocalidadVisible !== true) {
      this.resetLocalidad(false);
    }
  }

  private initTipoPlaza() {
    if (!this.tipoPlazaVisible) {
      this.resetFechas();
    }
  }

  get tipoPlazaVisible() {
    const tipoPlaza = this.form.get('tipoPlaza');
    return tipoPlaza.value === '2' || tipoPlaza.value === '3';
  }

  configFormControl() {
    this.formControls = [
      { name: 'empresa', custom: false, isCombo: true },
      { name: 'motivoIngreso', custom: true, isCombo: true },
      { name: 'puestoSolicitado', custom: true, isCombo: true },
      { name: 'tipoPlaza', custom: true, isCombo: true },
      { name: 'fechaInicio', custom: true, isCombo: false },
      { name: 'fechaFin', custom: true, isCombo: false },
      { name: 'departamento', custom: false, isCombo: false },
      { name: 'localidad', custom: true, isCombo: true },
      { name: 'otraLocalidad', custom: false, isCombo: false },
      { name: 'numeroVacantes', custom: false, isCombo: false },
      { name: 'descripcionTrabajo', custom: false, isCombo: false },
      { name: 'responsabilidades', custom: false, isCombo: false },
      { name: 'tabuladorSalarioMonto', custom: false, isCombo: false },
      { name: 'observaciones', custom: false, isCombo: false },
      { name: 'aliasId', custom: true, isCombo: false },
      { name: 'tabuladorSalarioId', custom: false, isCombo: false },
      { name: 'nivelOrganizacional', custom: false, isCombo: false },
    ];
  }

  configCatalogs() {
    this.listCatalog = [
      'empresas',
      'localidades',
      'motivosIngresos',
      'tiposPlazas',
      'puestosadam',
      'aliasadam'
    ];
  }

  configValueChangeCustom(controlName: string, value: any) {
    switch (controlName) {
      case 'motivoIngreso':
        this.configValueChangeMotivoIngreso(value);
        break;
      case 'puestoSolicitado':
        this.configValueChangePuestoSolicitado(value);
        break;
      case 'aliasId':
        this.configValueChangeAlias(value);
        break;
      case 'localidad':
        this.configValueChangeLocalidad(value);
        break;
      case 'tipoPlaza':
        this.configValueChangeTipoPlaza(value);
        break;
      case 'fechaInicio':
        this.configValueChangeFechaInicio(value);
        break;
      case 'fechaFin':
        this.configValueChangeFechaFin(value);
        break;
    }
  }

  private configValueChangeMotivoIngreso(value: any) {
    const sameValue = this.data['motivoIngreso'] && value && this.data['motivoIngreso'].value === value;
    if (sameValue) {
      return;
    }
    this.resetInfoAdam();
    this.puestoSolicitadoVisible = value ? true : false;
    this.aliasVisible = false;
    if (value === '1' || value === '3') {
      this.onShowMessage('Debera ser autorizado por director de RH.');
    }
    this.onGetPlaces(value);
  }

  private resetInfoAdam() {
    this.resetOnlyInfoAdamString();
    this.resetPuestos();
    this.resetAlias();
  }

  private resetPuestos() {
    this.form.get('puestoSolicitado').setValue('');
    this.catalogs.puestosadam = [];
  }

  private resetAlias() {
    this.form.get('aliasId').setValue('');
    this.catalogs.aliasadam = [];
  }

  private resetOnlyInfoAdamString() {
    this.form.get('numeroVacantes').setValue(0);
    this.form.get('tabuladorSalarioMonto').setValue('');
    this.form.get('departamento').setValue('');
    this.form.get('descripcionTrabajo').setValue('');
    this.form.get('responsabilidades').setValue('');
    this.form.get('nivelOrganizacional').setValue('');
  }

  private configValueChangePuestoSolicitado(value: any) {
    const sameValue = this.data['puestoSolicitado'] && this.data['puestoSolicitado'].value === value;
    if (sameValue) {
      return;
    }
    this.aliasVisible = value ? true : false;
    this.resetOnlyInfoAdamString();
    this.resetAlias();
    const motivoIngreso = this.form.get('motivoIngreso').value;
    if (motivoIngreso > 0 && value > 0) {
      this.onGetAlias(motivoIngreso, value);
    }
  }

  private configValueChangeAlias(value: any) {
    const sameValue = this.data['aliasId'] === value;
    if (sameValue) {
      return;
    }
    this.resetOnlyInfoAdamString();
    const puestoSolicitado = this.form.get('puestoSolicitado').value;
    this.infoAdamVisible = puestoSolicitado > 0 && (value > 0 || value === '-1');
    if (this.infoAdamVisible) {
      this.onGetJobInformationAdam(puestoSolicitado, value, true);
    } else {
      this.resetOnlyInfoAdamString();
    }
  }

  private configValueChangeLocalidad(value: any) {
    if (this.data['localidad'] && this.data['localidad'].value === value) {
      return;
    }
    this.otraLocalidadVisible = value === '5' ? true : false;
    this.resetLocalidad(this.otraLocalidadVisible);
  }

  private resetLocalidad(required = false) {
    const cadena = required ? '' : ' ';
    this.form.get('otraLocalidad').setValue(cadena);
  }

  private configValueChangeTipoPlaza(value: any) {
    if (this.data['tipoPlaza'] && this.data['tipoPlaza'].value === value) {
      return;
    }
    this.resetFechas(this.tipoPlazaVisible);
  }

  private resetFechas(forzarEntrada: boolean = false) {
    if (forzarEntrada) {
      this.form.get('fechaInicio').setValue(null);
      this.form.get('fechaFin').setValue(null);
    } else {
      this.form.get('fechaInicio').setValue(this.startfechaInicio);
      this.form.get('fechaFin').setValue(this.startfechaInicio);
    }
  }

  private configValueChangeFechaInicio(value: any) {
    if (this.form.get('fechaFin').value !== null &&
        value !== null &&
        !this.util.startIsSameOrBeforeEnd(value, this.form.get('fechaFin').value)) {
        this.form.get('fechaFin').setValue(null);
      }
      if (value != null) {
        this.startfechaInicio = this.form.get('fechaInicio').value;
    }
  }

  private configValueChangeFechaFin(value: any) {
    if (this.form.get('fechaInicio').value !== null &&
        value !== null &&
        !this.util.startIsSameOrBeforeEnd(this.form.get('fechaInicio').value, value)) {
        this.form.get('fechaInicio').setValue(null);
    }
  }

  onShowMessage(message: string) {
    this.store.set(StateOption.alert,
      <Alert>{
        type: AlertType.Warning,
        description: message,
        title: '¡Alerta!'
      });
  }

  onGetPlaces(motivoIngresoId: number, withLoading = false) {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.GettingPlaces,
        data: {
          withLoading: withLoading,
          value: {
            motivoIngresoId: motivoIngresoId,
          }
        }
      });
  }

  onGetAlias(motivoIngresoId: number, puestoId: number, withLoading = false) {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.GettingAlias,
        data: {
          withLoading: withLoading,
          value: {
            puestoId: puestoId,
            motivoIngresoId: motivoIngresoId
          }
        }
      });
  }

  onGetJobInformationAdam(puestoId: number, aliasId: string, withLoading = false) {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.GettingJobInformationAdam,
        data: {
          withLoading: withLoading,
          value: {
            puestoId: puestoId,
            aliasId: aliasId,
          }
        }
      });
  }

  changesCustom() {
  }

  setCatalogsCustom() {
    const motivoIngreso = this.form.get('motivoIngreso');
    const cargarcatalogoPuestosAdam = motivoIngreso.value
      && this.getConfigurationByControl('motivoIngreso').configuration.edicion
      && this.getConfigurationByControl('puestoSolicitado').configuration.edicion;
    if (!cargarcatalogoPuestosAdam) {
      this.catalogs['puestosadam'] = this.databackground.catalogs && this.databackground.catalogs.puestoSolicitado;
    }
  }

  onSave() {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.SavingRequisition,
        data: this.data
      });
  }

  onDelete() {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.RemovingRequisition,
        data: null
      });
  }

  onSaveValidation() {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.SavingRequisitionSendingToValidationRequisition,
        data: this.data
      });
  }

  get buttonSend() {
    return TypeButtonAction.Send;
  }

  get buttonSave() {
    return TypeButtonAction.Save;
  }

  get buttonDelete() {
    return TypeButtonAction.Delete;
  }

}
