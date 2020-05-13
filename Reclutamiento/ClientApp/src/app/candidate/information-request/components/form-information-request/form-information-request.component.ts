import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, DoCheck, ElementRef, Renderer} from '@angular/core';
import {ComponentElement} from '../../../../shared-root/class';
import {Router} from '@angular/router';
import {EnumTypeEvent, EventApp, StateOption, Store} from '../../../../store';
import {Benefit, Income, PersonalReference, WorkReference} from '../../../shared/models/candidate';
import {Validators} from '@angular/forms';

@Component({
  selector: 'ho1a-form-information-request',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './form-information-request.component.html',
  styleUrls: ['./form-information-request.component.scss']
})
export class FormInformationRequestComponent extends ComponentElement implements OnInit, OnChanges, DoCheck {

  @Input()
  completenameuser: string;

  @Output()
  logout = new EventEmitter();

  tittle = 'Solicitud de datos a candidatos';

  referenciaLaboral: WorkReference;
  referenciaPersonal: PersonalReference;
  ultimoTrabajoIngreso: Income;
  ultimoTrabajoPrestacion: Benefit;

  optionsReferenciasLaborales = <any>{};
  optionsReferenciasPersonales = <any>{};
  optionsUltimoTrabajoIngresos = <any>{};
  optionsUltimoTrabajoPrestaciones = <any>{};

  toggleModalNewUltimoTrabajoPrestaciones = false;
  toggleModalNewUltimoTrabajoIngresos = false;
  toggleModalNewReferenciasPersonales = false;
  toggleModalNewReferenciasLaborales = false;

  toggleUltimoTrabajoPrestaciones = false;
  toggleUltimoTrabajoIngresos = false;
  toggleReferenciasPersonales = false;
  toggleReferenciasLaborales = false;

  listFilter: any = { active: true };

  detalleOtraReferenciaVacanteActive = false;
  detalleRecomendadoPorActive = false;

  constructor(
    public el: ElementRef,
    public renderer: Renderer,
    private router: Router,
    private store: Store
  ) {
    super();
  }

  visible() {
    return true;
  }

  readonly() {
    return false;
  }

  ngDoCheck() {
    //let valLength = this.el.nativeElement.value.length;
    //console.log("valLength " + valLength);

    //if (valLength > 0) {
    //  this.renderer.setElementClass(this.el.nativeElement.parentElement, 'focused', true);
    //}
    //else {
    //  this.renderer.setElementClass(this.el.nativeElement.parentElement, 'focused', false);
    //}
  }

  ngOnChanges(changes: SimpleChanges) {
    this.changes(changes);
    this.setCatalogs();
  }

  ngOnInit() {
    this.init();
    this.configureGridOption();
  }

  initCustom() {
    this.initDetalleReferenciaVacanteId();
    this.initDetalleRecomendadoPor();
  }

  initDetalleReferenciaVacanteId() {
    const value = this.form.get('detalleReferenciaVacanteId').value;
    this.detalleOtraReferenciaVacanteActive = value === '7' ? true : false;
    this.detalleRecomendadoPorActive = value === '6' ? true : false;
  }

  initDetalleRecomendadoPor() {
    const value = this.form.get('detalleRecomendadoPor').value;
    this.detalleRecomendadoPorActive = value === '6' ? true : false;
  }

  changesCustom() {
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
      allowDelete: true,
      allowEdith: true,
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
      ],
      columnsForRow: 3,
      allowDelete: true,
      allowEdith: true,
    };
    this.optionsUltimoTrabajoPrestaciones = {
      columnsDefs: [
        { field: 'nombre', title: 'Nombre', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'valor', title: 'Valor', isVisible: true, isFilter: false, canOrderBy: false },
      ],
      columnsForRow: 2,
      allowDelete: true,
      allowEdith: true,
    };
    this.optionsUltimoTrabajoIngresos = {
      columnsDefs: [
        { field: 'nombre', title: 'Nombre', isVisible: true, isFilter: false, canOrderBy: false },
        { field: 'monto', title: 'Monto', isVisible: true, isFilter: false, canOrderBy: false },
      ],
      columnsForRow: 2,
      allowDelete: true,
      allowEdith: true,
    };
  }

  configCatalogs() {
    this.listCatalog = [
      'estadosCivil',
      'ultimosSalarios',
      'referenciasVacante',
      'localidadesSucursa',
      'puestoSolicitado'
    ];
  }

  setCatalogsCustom() {
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
      { name: 'detalleRecomendadoPor', custom: true, isCombo: false },
      { name: 'detalleReferenciaVacanteId', custom: true, isCombo: false },
      { name: 'detalleOtraReferenciaVacante', custom: false, isCombo: false },
      { name: 'detalleUltimoSalarioId', custom: false, isCombo: false },
      { name: 'detalleCertificacion', custom: false, isCombo: false },
      { name: 'detalleEstadoCivilId', custom: false, isCombo: false },
      { name: 'detallePretencionEconomica', custom: false, isCombo: false },
      { name: 'detallePuestoSolicitadoId', custom: false, isCombo: false },
      { name: 'detalleLocalidadSucursalId', custom: false, isCombo: false },
      { name: 'detalleLinkedIn', custom: false, isCombo: false },
      { name: 'detalleCurp', custom: false, isCombo: false },
      { name: 'detalleNss', custom: false, isCombo: false },
      { name: 'detalleRfc', custom: false, isCombo: false },
      { name: 'detalleStatusCapturaInformacion', custom: false, isCombo: false },
      { name: 'detalleStatusSeleccion', custom: false, isCombo: false },

      { name: 'detalleReferenciasPersonales', custom: false, isCombo: false },
      { name: 'detalleReferenciasLaborales', custom: false, isCombo: false },

      { name: 'detalleDireccionId', custom: false, isCombo: false },
      { name: 'detalleDireccionCandidatoId', custom: false, isCombo: false },
      { name: 'detalleDireccionCalle', custom: false, isCombo: false },
      { name: 'detalleDireccionCodigoPostal', custom: false, isCombo: false },
      { name: 'detalleDireccionColonia', custom: false, isCombo: false },
      { name: 'detalleDireccionEstado', custom: false, isCombo: false },
      { name: 'detalleDireccionMunicipio', custom: false, isCombo: false },

      { name: 'detalleUltimoTrabajoId', custom: false, isCombo: false },
      { name: 'detalleUltimoTrabajoIngresos', custom: false, isCombo: false },
      { name: 'detalleUltimoTrabajoPrestaciones', custom: false, isCombo: false },
      { name: 'detalleUltimoTrabajoCandidatoId', custom: false, isCombo: false },
      { name: 'detalleUltimoTrabajoEmpresa', custom: false, isCombo: false },
      { name: 'detalleUltimoTrabajoGastosMedicosMayores', custom: false, isCombo: false },
      { name: 'detalleUltimoTrabajoPuesto', custom: false, isCombo: false },
      { name: 'detalleUltimoTrabajoSeguroVida', custom: false, isCombo: false },
      { name: 'detalleUltimoTrabajoSueldoFijoMensual', custom: false, isCombo: false },

    ];
  }

  configValueChangeCustom(controlName: string, value: any) {
    switch (controlName) {
      case 'detalleReferenciaVacanteId':
          this.detalleOtraReferenciaVacanteActive = value === '7' ? true : false;
          this.detalleRecomendadoPorActive = value === '6' ? true : false;
        break;
      case 'detalleRecomendadoPor':
        if (value === '6') {
          this.detalleRecomendadoPorActive = true;
        }
        break;
    }
  }

  onLogout() {
    localStorage.removeItem('auth_token');
    this.router.navigate([`auth/login`]);
  }

  onSave() {
    this.save();
  }

  save() {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: EnumTypeEvent.SavingRequisition,
        data: this.data
      });
  }

  onNotify() {
    if (this.canNotify) {
      this.data.detalleStatusCapturaInformacion += 1;
      this.store.set(StateOption.eventsRequisition,
        <EventApp>{
          typeEvent: EnumTypeEvent.NotifyingLoadFullInformation,
          data: this.data.detalleStatusCapturaInformacion
        });
    }
  }

  get canNotify() {
    return this.mandatoryDocumentsLoaded() && this.form.valid;
  }

  disabled() {
    return !this.canNotify ? 'disabled' : null;
  }

  mandatoryDocumentsLoaded() {
    let required = [];
    let loaded = [];
    let total = 0;

    if (this.databackground && this.databackground.documents) {
      total = this.databackground.documents.length;
      required = this.databackground.documents.filter( f => f.required);
      loaded = this.databackground.documents.filter( f => f.required && f.loaded);
    }

    return total > 0 && loaded.length >= required.length;
  }

  get canSendNotify() {
    return this.data.detalleStatusCapturaInformacion === 1 || this.data.detalleStatusCapturaInformacion === 3;
  }

  onToggleModalNewReferenciasPersonales() {
    this.toggleModalNewReferenciasPersonales = !this.toggleModalNewReferenciasPersonales;
  }

  onCancelModalNewReferenciasPersonales() {
    this.onToggleModalNewReferenciasPersonales();
  }

  onAcceptModalNewReferenciasPersonales(event: any) {
    this.onToggleModalNewReferenciasPersonales();
  }

  onNewReferenciasPersonales() {
    this.referenciaPersonal = <PersonalReference>{
      solicitarReferencia: false,
      id: 0,
      active: true,
      candidatoDetalleId: this.data.detalleId
    };
    this.onToggleModalNewReferenciasPersonales();
  }

  onEdithReferenciasPersonales(event: PersonalReference) {
    this.referenciaPersonal = event;
    this.onToggleModalNewReferenciasPersonales();
  }

  onDeleteReferenciasPersonales(event: PersonalReference) {
    this.store.set(StateOption.eventsRequisition, <EventApp>{
      typeEvent: EnumTypeEvent.DeletingPersonalReference,
      data: event
    });
  }


  onToggleModalNewReferenciasLaborales() {
    this.toggleModalNewReferenciasLaborales = !this.toggleModalNewReferenciasLaborales;
  }

  onCancelModalNewReferenciasLaborales() {
    this.onToggleModalNewReferenciasLaborales();
  }

  onAcceptModalNewReferenciasLaborales(event: any) {
    this.onToggleModalNewReferenciasLaborales();
  }

  onNewReferenciasLaborales() {
    this.referenciaLaboral = <WorkReference>{
      solicitarReferencia: false,
      id: 0,
      active: true,
      candidatoDetalleId: this.data.detalleId
    };
    this.onToggleModalNewReferenciasLaborales();
  }

  onEdithReferenciasLaborales(event: WorkReference) {
    this.referenciaLaboral = event;
    this.onToggleModalNewReferenciasLaborales();
  }

  onDeleteReferenciasLaborales(event: WorkReference) {
    this.store.set(StateOption.eventsRequisition, <EventApp>{
      typeEvent: EnumTypeEvent.DeletingWorkReference,
      data: event
    });
  }


  onToggleModalNewUltimoTrabajoPrestaciones() {
    this.toggleModalNewUltimoTrabajoPrestaciones = !this.toggleModalNewUltimoTrabajoPrestaciones;
  }

  onCancelModalNewUltimoTrabajoPrestaciones() {
    this.onToggleModalNewUltimoTrabajoPrestaciones();
  }

  onAcceptModalNewUltimoTrabajoPrestaciones(event: any) {
    this.onToggleModalNewUltimoTrabajoPrestaciones();
  }

  onNewUltimoTrabajoPrestaciones() {
    this.ultimoTrabajoPrestacion = <Benefit>{
      id: 0,
      active: true,
      tipoPrestacionId: 4,
      ultimoTrabajoId: this.data.detalleUltimoTrabajoId
    };
    this.onToggleModalNewUltimoTrabajoPrestaciones();
  }

  onEdithUltimoTrabajoPrestaciones(event: Benefit) {
    this.ultimoTrabajoPrestacion = event;
    this.onToggleModalNewUltimoTrabajoPrestaciones();
  }

  onDeleteUltimoTrabajoPrestaciones(event: Benefit) {
    if (event.tipoPrestacionId > 3) {
      this.store.set(StateOption.eventsRequisition, <EventApp>{
        typeEvent: EnumTypeEvent.DeletingBenefitLastJob,
        data: event
      });
    }
  }


  onToggleModalNewUltimoTrabajoIngresos() {
    this.toggleModalNewUltimoTrabajoIngresos = !this.toggleModalNewUltimoTrabajoIngresos;
  }

  onCancelModalNewUltimoTrabajoIngresos() {
    this.onToggleModalNewUltimoTrabajoIngresos();
  }

  onAcceptModalNewUltimoTrabajoIngresos(event: any) {
    this.onToggleModalNewUltimoTrabajoIngresos();
  }

  onNewUltimoTrabajoIngresos() {
    this.ultimoTrabajoIngreso = <Income>{
      id: 0,
      active: true,
      tipoIngresoId: 9,
      ultimoTrabajoId: this.data.detalleUltimoTrabajoId
    };
    this.onToggleModalNewUltimoTrabajoIngresos();
  }

  onEdithUltimoTrabajoIngresos(event: Income) {
    this.ultimoTrabajoIngreso = event;
    this.onToggleModalNewUltimoTrabajoIngresos();
  }

  onDeleteUltimoTrabajoIngresos(event: Income) {
    if (event.tipoIngresoId > 8) {
      this.store.set(StateOption.eventsRequisition, <EventApp>{
        typeEvent: EnumTypeEvent.DeletingIncomeLastJob,
        data: event
      });
    }
  }

  onUpload(document: any) {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: EnumTypeEvent.UploadingFile,
        data: document
      });
  }

  onDelete(document: any) {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: EnumTypeEvent.DeletingFile,
        data: document
      });
  }

  onToggleReferenciasLaborales() {
    this.toggleReferenciasLaborales = !this.toggleReferenciasLaborales;
  }

  onToggleReferenciasPersonales() {
    this.toggleReferenciasPersonales = !this.toggleReferenciasPersonales;
  }

  onToggleUltimoTrabajoPrestaciones() {
    this.toggleUltimoTrabajoPrestaciones = !this.toggleUltimoTrabajoPrestaciones;
  }

  onToggleUltimoTrabajoIngresos() {
    this.toggleUltimoTrabajoIngresos = !this.toggleUltimoTrabajoIngresos;
  }

  get hasReferenciasLaborales() {
    return this.data.detalleReferenciasLaborales !== null && this.data.detalleReferenciasLaborales.length;
  }

  get hasReferenciasPersonales() {
    return this.data.detalleReferenciasPersonales !== null && this.data.detalleReferenciasPersonales.length;
  }

  get hasUltimoTrabajoPrestaciones() {
    return this.data.detalleUltimoTrabajoPrestaciones !== null && this.data.detalleUltimoTrabajoPrestaciones.length;
  }

  get hasUltimoTrabajoIngresos() {
    return this.data.detalleUltimoTrabajoIngresos !== null && this.data.detalleUltimoTrabajoIngresos.length;
  }

  get otrosDatosVisibles() {
    return this.getConfigurationByControl('detalleReferenciaVacanteId').configuration.visible ||
      this.getConfigurationByControl('detalleOtraReferenciaVacante').configuration.visible ||
      this.getConfigurationByControl('detalleRecomendadoPor').configuration.visible;
  }

  get datosGeneralesUltimoTrabajoVisibles() {
    return this.getConfigurationByControl('detalleUltimoTrabajoEmpresa').configuration.visible ||
      this.getConfigurationByControl('detalleUltimoTrabajoPuesto').configuration.visible ||
      this.getConfigurationByControl('detalleUltimoTrabajoSueldoFijoMensual').configuration.visible ||
      this.getConfigurationByControl('detalleUltimoTrabajoGastosMedicosMayores').configuration.visible ||
      this.getConfigurationByControl('detalleUltimoTrabajoSeguroVida').configuration.visible;
  }

}
