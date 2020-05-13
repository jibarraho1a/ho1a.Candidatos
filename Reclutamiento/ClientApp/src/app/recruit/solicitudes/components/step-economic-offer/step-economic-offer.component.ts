import {ChangeDetectionStrategy, Component, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import { ComponentElement } from '../../../../shared/class/ComponentElement';
import { StatusProcess } from '../../../../shared/models/status-process';
import { Store } from '../../../../shared/class/Store';
import { TypeButtonAction } from '../../../../shared/enums/type-button-action.enum';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { EventApp } from '../../../../shared/models/event-app';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';


@Component({
  selector: 'ho1a-step-economic-offer',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-economic-offer.component.html',
  styleUrls: ['./step-economic-offer.component.scss']
})
export class StepEconomicOfferComponent extends ComponentElement implements OnInit, OnChanges {

  @Input()
  currentStep: StatusProcess;

  listFilter: any = { active: true };
  optionsReferenciasLaborales = <any>{};
  optionsReferenciasPersonales = <any>{};
  optionsUltimoTrabajoIngresos = <any>{};
  optionsUltimoTrabajoPrestaciones = <any>{};

  optionButtomAceptarPropuesta = <any>{};

  constructor(private store: Store) {
    super();
  }

  ngOnInit() {
    this.init();
    this.configureGridOption();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.changes(changes);
  }

  visible() {
    return this.configuration && this.configuration.visible;
  }

  readonly() {
    return this.databackground && this.databackground.propuesta && this.databackground.propuesta.propuestaAceptada;
  }

  initCustom() {
  }

  configFormControl() {
    this.formControls = [
      { name: 'id', custom: false, isCombo: false },
      { name: 'active', custom: false, isCombo: false },
      { name: 'nombre', custom: false, isCombo: false },
      { name: 'paterno', custom: false, isCombo: false },
      { name: 'materno', custom: false, isCombo: false },
      { name: 'email', custom: false, isCombo: false },

      { name: 'detalleId', custom: false, isCombo: false },
      { name: 'detalleActive', custom: false, isCombo: false },
      { name: 'detalleCandidatoId', custom: false, isCombo: false },
      { name: 'detalleFechaNacimiento', custom: false, isCombo: false },
      { name: 'detalleLugarNacimiento', custom: false, isCombo: false },
      { name: 'detalleTelefonoCasa', custom: false, isCombo: false },
      { name: 'detalleTelefonoCelular', custom: false, isCombo: false },
      { name: 'detalleRecomendadoPor', custom: false, isCombo: false },
      { name: 'detalleReferenciaVacanteId', custom: false, isCombo: false },
      { name: 'detalleOtraReferenciaVacante', custom: false, isCombo: false },
      { name: 'detalleUltimoSalarioId', custom: false, isCombo: false },
      { name: 'detalleCertificacion', custom: false, isCombo: false },
      { name: 'detalleEstadoCivilId', custom: false, isCombo: true },
      { name: 'detallePretencionEconomica', custom: false, isCombo: false },
      { name: 'detallePuestoSolicitadoId', custom: false, isCombo: false },
      { name: 'detalleLocalidadSucursalId', custom: false, isCombo: false },
      { name: 'detalleLinkedIn', custom: false, isCombo: false },
      { name: 'detalleCurp', custom: false, isCombo: false },
      { name: 'detalleNss', custom: false, isCombo: false },
      { name: 'detalleRfc', custom: false, isCombo: false },

      { name: 'detalleDireccionId', custom: false, isCombo: false },
      { name: 'detalleDireccionCandidatoId', custom: false, isCombo: false },
      { name: 'detalleDireccionCalle', custom: false, isCombo: false },
      { name: 'detalleDireccionCodigoPostal', custom: false, isCombo: false },
      { name: 'detalleDireccionColonia', custom: false, isCombo: false },
      { name: 'detalleDireccionEstado', custom: false, isCombo: false },
      { name: 'detalleDireccionMunicipio', custom: false, isCombo: false },

      { name: 'detalleUltimoTrabajoId', custom: false, isCombo: false },
      { name: 'detalleUltimoTrabajoCandidatoId', custom: false, isCombo: false },
      { name: 'detalleUltimoTrabajoEmpresa', custom: false, isCombo: false },
      { name: 'detalleUltimoTrabajoGastosMedicosMayores', custom: false, isCombo: false },
      { name: 'detalleUltimoTrabajoPuesto', custom: false, isCombo: false },
      { name: 'detalleUltimoTrabajoSeguroVida', custom: false, isCombo: false },
      { name: 'detalleUltimoTrabajoSueldoFijoMensual', custom: false, isCombo: false },

      { name: 'salario', custom: false, isCombo: false },
      { name: 'bonos', custom: false, isCombo: false },
      { name: 'beneficios', custom: false, isCombo: false },
    ];
  }

  configCatalogs() {
    this.listCatalog = [
      'documentosOfertaEconomica',
      'estadosCivil',
      'ultimosSalarios',
      'localidadesSucursa',
      'puestoSolicitado',
      'referenciasVacante'
    ];
  }

  configValueChangeCustom() {
  }

  changesCustom() {
    this.optionButtomAceptarPropuesta = {
      buttonActionConfirmation: {
        title: '',
        type: TypeButtonAction.MonetizationOn,
        text: this.getActionByControl('AceptarOfertaRequisicion').descripcion,
        textAccept: 'Aceptar',
        textCancel: 'Rechazar',
        positionLeft: false,
        visible: true,
        disabled: false
      },
      inputBox: {
        title: 'Comentarios',
        forCancel: { enabled: true, required: true },
        forConfirm: { enabled: true, required: false }
      }
    };
  }

  setCatalogsCustom() {
  }

  configureGridOption() {
    this.optionsReferenciasPersonales = {
      columnsDefs: [
        { field: 'nombre', title: 'Nombre', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'paterno', title: 'Apellido paterno', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'materno', title: 'Apellido materno', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'email', title: 'Email', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'solicitarReferencia', title: '¿Solicitar referencia?', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'telefono', title: 'Teléfono', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'tiempoConocerse', title: 'Tiempo conocerse', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'parentesco', title: 'Parentesco', isVisible: true, isFilter: false, canOrderBy: false },
      ],
      columnsForRow: 3,
      allowDelete: false,
      allowEdith: false,
    };
    this.optionsReferenciasLaborales = {
      columnsDefs: [
        { field: 'nombre', title: 'Nombre', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'paterno', title: 'Apellido paterno', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'materno', title: 'Apellido materno', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'email', title: 'Email', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'solicitarReferencia', title: '¿Solicitar referencia?', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'cargo', title: 'cargo', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'tiempoConocerse', title: 'Tiempo conocerse', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'parentesco', title: 'Parentesco', isVisible: true, isFilter: false, canOrderBy: false },
      ],
      columnsForRow: 3,
      allowDelete: false,
      allowEdith: false,
    };
    this.optionsUltimoTrabajoPrestaciones = {
      columnsDefs: [
        { field: 'descripcion', title: 'Descripción', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'valor', title: 'Monto', isVisible: true, isFilter: false, canOrderBy: false },
      ],
      columnsForRow: 2,
      allowDelete: false,
      allowEdith: false,
    };
    this.optionsUltimoTrabajoIngresos = {
      columnsDefs: [
        { field: 'descripcion', title: 'Descripción', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'monto', title: 'Monto', isVisible: true, isFilter: false, canOrderBy: false },
      ],
      columnsForRow: 2,
      allowDelete: false,
      allowEdith: false,
    };
  }

  onSendoffer() {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.SendingOfferRequisitionCandidate,
        data: null
      });
  }

  onActionResultAcceptOffer(event: any) {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.AcceptingOfferRequisitionCandidate,
        data: {
          aceptaPropuesta: event.buttomSelected,
          comentario: event.text
        }
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

  onSendSalaryOffer() {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.SendingSalaryOfferSuitableCandidateForTheRequisition,
        data: {
          salario: this.salary,
          bonos: this.bonos ? this.bonos : 0,
          beneficios: this.beneficios ? this.beneficios : '',
        }
      });
  }

  get salary() {
    return this.form.get('salario').value;
  }

  get bonos() {
    return this.form.get('bonos').value;
  }

  get beneficios() {
    return this.form.get('beneficios').value;
  }

  get buttonSave() {
    return TypeButtonAction.Save;
  }

  get canSendSalaryOffer() {
    const canSend = this.bonos >= 0 && this.salary > 0
      && this.getActionByControl('GuardarPropuestaSalarialCandidatoIdoneoRequisicion').visible;
    return canSend;
  }

}
