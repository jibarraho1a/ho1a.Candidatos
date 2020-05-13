export class Candidato {

  private candidate: Candidate;
  private candidateForm: any;

  constructor() {
  }

  initFromForm(item: CandidateForm) {
    this.candidateForm = item;

    this.candidate = <Candidate>{};
    this.candidate.id = item.id;
    this.candidate.active = item.active;
    this.candidate.nombre = item.nombre;
    this.candidate.paterno = item.paterno;
    this.candidate.materno = item.materno;
    this.candidate.email = item.email;
    this.candidate.userAd = item.userAd;

    this.candidate.detalle = <Detail>{
      id: item.detalleId !== null ? item.detalleId : 0,
      active: item.detalleActive === null ? true : item.detalleActive,
      candidatoId: item.detalleCandidatoId !== null ? item.detalleCandidatoId : item.id,
      fechaNacimiento: item.detalleFechaNacimiento,
      lugarNacimiento: item.detalleLugarNacimiento,
      telefonoCasa: item.detalleTelefonoCasa,
      telefonoCelular: item.detalleTelefonoCelular,
      recomendadoPor: item.detalleRecomendadoPor,
      referenciaVacanteId: item.detalleReferenciaVacanteId,
      // referenciaVacante: item.detalleReferenciaVacante, //
      otraReferenciaVacante: item.detalleOtraReferenciaVacante,
      ultimoSalarioId: item.detalleUltimoSalarioId,
      // ultimoSalario: item.detalleUltimoSalario,
      certificacion: item.detalleCertificacion === null ? false : item.detalleCertificacion,
      estadoCivilId: item.detalleEstadoCivilId,
      // estadoCivil: item.detalleEstadoCivil,
      pretencionEconomica: item.detallePretencionEconomica,
      puestoSolicitadoId: item.detallePuestoSolicitadoId,
      // puestoSolicitado: item.detallePuestoSolicitado,
      localidadSucursalId: item.detalleLocalidadSucursalId,
      // localidadSucursal: item.detalleLocalidadSucursal,
      linkedIn:  item.detalleLinkedIn,
      curp: item.detalleCurp,
      nss: item.detalleNss,
      rfc: item.detalleRfc,
      statusCapturaInformacion: item.detalleStatusCapturaInformacion,
      statusSeleccion: item.detalleStatusSeleccion,
      direccion: <Address>{
        id: item.detalleDireccionId !== null ? item.detalleDireccionId : 0,
        active : item.detalleDireccionActive,
        candidatoId: item.detalleDireccionCandidatoId !== null ? item.detalleDireccionCandidatoId : item.id,
        calle: item.detalleDireccionCalle,
        codigoPostal: item.detalleDireccionCodigoPostal,
        colonia: item.detalleDireccionColonia,
        estado: item.detalleDireccionEstado,
        municipio: item.detalleDireccionMunicipio,
      },

      ultimoTrabajo: <Job>{
        id: item.detalleUltimoTrabajoId !== null ? item.detalleUltimoTrabajoId : 0,
        active : item.detalleUltimoTrabajoActive,
        candidatoId: item.detalleUltimoTrabajoCandidatoId,
        ingresos: item.detalleUltimoTrabajoIngresos,
        prestaciones: item.detalleUltimoTrabajoPrestaciones,
        empresa: item.detalleUltimoTrabajoEmpresa,
        gastosMedicosMayores: item.detalleUltimoTrabajoGastosMedicosMayores,
        puesto: item.detalleUltimoTrabajoPuesto,
        seguroVida: item.detalleUltimoTrabajoSeguroVida,
        sueldoFijoMensual: item.detalleUltimoTrabajoSueldoFijoMensual,
      },

      referenciasPersonales: item.detalleReferenciasPersonales,
      referenciasLaborales: item.detalleReferenciasLaborales
    };
  }

  initFromObject(candidate: Candidate) {
    this.candidate = candidate;

    this.candidateForm = <CandidateForm>{
      id: this.candidate.id,
      active: this.candidate.active,
      nombre: this.candidate.nombre,
      paterno: this.candidate.paterno,
      materno: this.candidate.materno,
      email: this.candidate.email,
      userAd: this.candidate.userAd,

      detalleId: this.candidate.detalle !== null && this.candidate.detalle.id ? this.candidate.detalle.id : null,
      detalleActive: this.candidate.detalle ? this.candidate.detalle.active : null,
      detalleCandidatoId: this.candidate.detalle ? this.candidate.detalle.candidatoId : null,
      detalleFechaNacimiento: this.candidate.detalle ? this.candidate.detalle.fechaNacimiento : null,
      detalleLugarNacimiento: this.candidate.detalle ? this.candidate.detalle.lugarNacimiento : null,
      detalleTelefonoCasa: this.candidate.detalle ? this.candidate.detalle.telefonoCasa : null,
      detalleTelefonoCelular: this.candidate.detalle ? this.candidate.detalle.telefonoCelular : null,
      detalleRecomendadoPor: this.candidate.detalle ? this.candidate.detalle.recomendadoPor : null,
      detalleReferenciaVacanteId: this.candidate.detalle ? this.candidate.detalle.referenciaVacanteId : null,
      detalleOtraReferenciaVacante: this.candidate.detalle ? this.candidate.detalle.otraReferenciaVacante : null,
      detalleUltimoSalarioId: this.candidate.detalle ? this.candidate.detalle.ultimoSalarioId : null,
      detalleCertificacion: this.candidate.detalle ? this.candidate.detalle.certificacion : null,
      detalleEstadoCivilId: this.candidate.detalle ? this.candidate.detalle.estadoCivilId : null,
      detallePretencionEconomica: this.candidate.detalle ? this.candidate.detalle.pretencionEconomica : null,
      detallePuestoSolicitadoId: this.candidate.detalle ? this.candidate.detalle.puestoSolicitadoId : null,
      detalleLocalidadSucursalId: this.candidate.detalle ? this.candidate.detalle.localidadSucursalId : null,
      detalleLinkedIn: this.candidate.detalle ? this.candidate.detalle.linkedIn : null,
      detalleCurp: this.candidate.detalle ? this.candidate.detalle.curp : null,
      detalleRfc: this.candidate.detalle ? this.candidate.detalle.rfc : null,
      detalleNss: this.candidate.detalle ? this.candidate.detalle.nss : null,
      detalleStatusCapturaInformacion: this.candidate.detalle ? this.candidate.detalle.statusCapturaInformacion : null,
      detalleStatusSeleccion: this.candidate.detalle ? this.candidate.detalle.statusSeleccion : null,

      detalleDireccionId: this.candidate.detalle !== null && this.candidate.detalle.direccion !== null && this.candidate.detalle.direccion.id ? this.candidate.detalle.direccion.id : null,
      detalleDireccionActive : this.candidate.detalle !== null && candidate.detalle.direccion !== null && this.candidate.detalle.direccion.active ? this.candidate.detalle.direccion.active : false,
      detalleDireccionCandidatoId: this.candidate.detalle !== null && this.candidate.detalle.direccion !== null ? this.candidate.detalle.direccion.candidatoId : null,
      detalleDireccionCalle: this.candidate.detalle && this.candidate.detalle.direccion ? this.candidate.detalle.direccion.calle : null,
      detalleDireccionCodigoPostal: this.candidate.detalle && this.candidate.detalle.direccion ? this.candidate.detalle.direccion.codigoPostal : null,
      detalleDireccionColonia: this.candidate.detalle && this.candidate.detalle.direccion ? this.candidate.detalle.direccion.colonia : null,
      detalleDireccionEstado: this.candidate.detalle && this.candidate.detalle.direccion ? this.candidate.detalle.direccion.estado : null,
      detalleDireccionMunicipio: this.candidate.detalle && this.candidate.detalle.direccion ? this.candidate.detalle.direccion.municipio : null,

      detalleUltimoTrabajoId: this.candidate.detalle !== null && this.candidate.detalle.ultimoTrabajo !== null && this.candidate.detalle.ultimoTrabajo.id ? this.candidate.detalle.ultimoTrabajo.id : null,
      detalleUltimoTrabajoActive: this.candidate.detalle !== null && this.candidate.detalle.ultimoTrabajo !== null && this.candidate.detalle.ultimoTrabajo.active ? this.candidate.detalle.ultimoTrabajo.active : false,
      detalleUltimoTrabajoCandidatoId: this.candidate.detalle !== null && this.candidate.detalle.ultimoTrabajo !== null ? this.candidate.detalle.ultimoTrabajo.candidatoId : null,
      detalleUltimoTrabajoIngresos: this.candidate.detalle && this.candidate.detalle.ultimoTrabajo ? this.candidate.detalle.ultimoTrabajo.ingresos : null,
      detalleUltimoTrabajoPrestaciones: this.candidate.detalle && this.candidate.detalle.ultimoTrabajo ? this.candidate.detalle.ultimoTrabajo.prestaciones : null,
      detalleUltimoTrabajoEmpresa: this.candidate.detalle && this.candidate.detalle.ultimoTrabajo ? this.candidate.detalle.ultimoTrabajo.empresa : null,
      detalleUltimoTrabajoGastosMedicosMayores: this.candidate.detalle && this.candidate.detalle.ultimoTrabajo ? this.candidate.detalle.ultimoTrabajo.gastosMedicosMayores : null,
      detalleUltimoTrabajoPuesto: this.candidate.detalle && this.candidate.detalle.ultimoTrabajo ? this.candidate.detalle.ultimoTrabajo.puesto : null,
      detalleUltimoTrabajoSeguroVida: this.candidate.detalle && this.candidate.detalle.ultimoTrabajo ? this.candidate.detalle.ultimoTrabajo.seguroVida : null,
      detalleUltimoTrabajoSueldoFijoMensual: this.candidate.detalle && this.candidate.detalle.ultimoTrabajo ? this.candidate.detalle.ultimoTrabajo.sueldoFijoMensual : null,

      detalleReferenciasPersonales: this.candidate.detalle ?  this.candidate.detalle.referenciasPersonales : null,
      detalleReferenciasLaborales: this.candidate.detalle ? this.candidate.detalle.referenciasLaborales : null,
    };
  }

  parceToForm() {
    return this.candidateForm;
  }

  parceToObject() {
    return this.candidate;
  }

}

export interface CandidateForm {
  id: number;
  active: boolean;
  nombre: string;
  paterno: string;
  materno: string;
  email: string;
  userAd: string;

  detalleId: number;
  detalleActive: boolean;
  detalleCandidatoId: number;
  detalleFechaNacimiento: Date;
  detalleLugarNacimiento: string;
  detalleTelefonoCasa: number;
  detalleTelefonoCelular: number;
  detalleRecomendadoPor: string; //
  detalleReferenciaVacanteId: string;
  detalleOtraReferenciaVacante: string;
  detalleUltimoSalarioId: string;
  detalleCertificacion: boolean;
  detalleEstadoCivilId: string;
  detallePretencionEconomica: number;
  detallePuestoSolicitadoId: string;
  detalleLocalidadSucursalId: string;
  detalleLinkedIn: string;
  detalleCurp: string;
  detalleNss: string;
  detalleRfc: string;
  detalleStatusCapturaInformacion: number;
  detalleStatusSeleccion: number;

  detalleDireccionId: number;
  detalleDireccionActive: boolean;
  detalleDireccionCandidatoId: number;
  detalleDireccionCalle: string;
  detalleDireccionCodigoPostal: number;
  detalleDireccionColonia: string;
  detalleDireccionEstado: string;
  detalleDireccionMunicipio: string;

  detalleUltimoTrabajoId: number;
  detalleUltimoTrabajoActive: boolean;
  detalleUltimoTrabajoCandidatoId: number;
  detalleUltimoTrabajoIngresos: Income[];
  detalleUltimoTrabajoPrestaciones: Benefit[];
  detalleUltimoTrabajoEmpresa: string;
  detalleUltimoTrabajoGastosMedicosMayores: boolean;
  detalleUltimoTrabajoPuesto: string;
  detalleUltimoTrabajoSeguroVida: boolean;
  detalleUltimoTrabajoSueldoFijoMensual: number;

  detalleReferenciasPersonales: PersonalReference[];
  detalleReferenciasLaborales: WorkReference[];
}

export interface Candidate {
  id?: number | null;
  active: boolean | null;
  nombre: string;
  paterno: string;
  materno: string;
  email: string;
  userAd: string;
  candidatoId?: number | null;
  entrevistas?: any[] | null;
  status?: number | null;
  terna?: any | null;
  ternaId?: number | null;
  detalle?: Detail | null;
}

export interface Detail {
  id?: number | null;
  active: boolean | null;
  candidatoId?: number | null;
  fechaNacimiento: Date | null;
  lugarNacimiento: string | null;
  telefonoCasa: number | null;
  telefonoCelular: number | null;
  recomendadoPor: string | null;
  referenciaVacanteId: string | null;
  referenciaVacante: any | null;
  otraReferenciaVacante: string | null;
  ultimoSalarioId: string | null;
  ultimoSalario: any | null;
  certificacion: boolean | null;
  estadoCivilId: string | null;
  estadoCivil: any | null;
  pretencionEconomica: number | null;
  puestoSolicitadoId: string | null;
  puestoSolicitado: any | null;
  localidadSucursalId: string | null;
  localidadSucursal: any | null;
  linkedIn: string | null;
  curp: string;
  nss: string;
  rfc: string;
  statusCapturaInformacion: number;
  statusSeleccion: number;
  direccion?: Address | null;
  ultimoTrabajo?: Job | null;
  referenciasLaborales: WorkReference[] | null;
  referenciasPersonales: PersonalReference[] | null;
}

export interface Address {
  id?: number | null;
  active: boolean | null;
  candidatoId?: number | null;
  calle: string | null;
  codigoPostal: number | null;
  colonia: string | null;
  estado: string | null;
  municipio: string | null;
}

export interface Job {
  id?: number | null;
  active: boolean | null;
  candidatoId?: number | null;
  ingresos?: Income[] | null;
  prestaciones?: Benefit[] | null;
  empresa?: string;
  gastosMedicosMayores?: boolean;
  puesto?: string;
  seguroVida?: boolean;
  sueldoFijoMensual?: number;
}

export interface Income {
  id?: number | null;
  ultimoTrabajoId?: number | null;
  tipoIngresoId?: number | null;
  descripcion: string | null;
  monto: number | null;
  active?: boolean | null;
}

export interface Benefit {
  id?: number | null;
  ultimoTrabajoId?: number | null;
  tipoPrestacionId?: number | null;
  nombre: string | null;
  valor: number | null;
  descripcion: string | null;
  active?: boolean | null;
}

export interface WorkReference {
  id?: number | null;
  candidatoDetalleId?: number | null;
  email: string | null;
  materno: string | null;
  nombre: string | null;
  parentesco: string | null;
  paterno: string | null;
  solicitarReferencia: boolean | null;
  cargo: string | null;
  tiempoConocerse: string | null;
  active?: boolean | null;
}

export interface PersonalReference {
  id?: number | null;
  candidatoDetalleId?: number | null;
  email: string | null;
  materno: string | null;
  nombre: string | null;
  parentesco: string | null;
  paterno: string | null;
  solicitarReferencia: boolean | null;
  telefono: string | null;
  tiempoConocerse: string | null;
  active?: boolean | null;
}
