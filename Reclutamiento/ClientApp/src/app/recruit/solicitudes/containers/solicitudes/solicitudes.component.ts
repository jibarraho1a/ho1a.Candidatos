import { Component, OnInit } from '@angular/core';
import { ListElement } from 'src/app/shared/class';
import { Router } from '@angular/router';

@Component({
  selector: 'ho1a-solicitudes',
  templateUrl: './solicitudes.component.html',
  styleUrls: ['./solicitudes.component.scss']
})
export class SolicitudesComponent extends ListElement implements OnInit {

  tittleFilter = 'Filtrado de Reporte Detallado';
  status = [];
  selectedbox = 'Solicitud nueva';

  constructor(private router: Router) {
    super();
  }

  ngOnInit() {
    // this.init();
    this.configureGridOption();
    this.configureHighlightedBoxes();
  }

  configureGridOption() {
    this.options = {
      columnsDefs: [
        { field: 'id', title: 'Id', isVisible: false, isFilter: false, canOrderBy: true, isExportable: false },
        { field: 'idSolicitud', title: 'Id', isVisible: true, isFilter: true, canOrderBy: false, isExportable: true },
        { field: 'diasSolicitud', title: 'Días solicitud', isVisible: true, isFilter: true, canOrderBy: true, isExportable: true, applyStyles: true },
        { field: 'solicitante', title: 'Solicitante', isVisible: true, isFilter: true, canOrderBy: true, isExportable: true },
        { field: 'puestoSolicitado', title: 'Puesto Solicitado', isVisible: true, isFilter: true, canOrderBy: true, isExportable: true },
        { field: 'motivoIngreso', title: 'Motivo Ingreso', isVisible: true, isFilter: true, canOrderBy: true, isExportable: true },
        { field: 'tabulador', title: 'Tabulador', isVisible: true, isFilter: true, canOrderBy: true, isExportable: true },
        { field: 'tipoPlaza', title: 'Tipo de Plaza', isVisible: true, isFilter: true, canOrderBy: true, isExportable: true },
        { field: 'statusMatriz', title: 'Estado Matriz', isVisible: false, isFilter: false, canOrderBy: false, isExportable: true },
        { field: 'statusRequisicion', title: 'Estado Solicitud', isVisible: true, isFilter: true, canOrderBy: true, isExportable: true },
        { field: 'reclutador', title: 'Reclutador', isVisible: true, isFilter: true, canOrderBy: true, isExportable: true },
        { field: 'localidad', title: 'Localidad', isVisible: true, isFilter: true, canOrderBy: true, isExportable: true },
        { field: 'area', title: 'Área', isVisible: true, isFilter: true, canOrderBy: true, isExportable: true },
        { field: 'direccion', title: 'Dirección', isVisible: true, isFilter: true, canOrderBy: true, isExportable: true }

      ],
      columnsForRow: 3,
      allowDelete: true,
      allowEdith: true,
    };
  }

  getListService() {
    // this.service.gets().subscribe(
    //   next => this.next(next),
    //   error => this.observer.error(error),
    //   () => {
    //     this.observer.complete();
    // this.configureHighlightedBoxes();
    //   }
    // );
  }

  alertService() {
    // this.service.alertError(this.errorMensage);
  }

  get ruta() {
    return '/mirh/requestplace/';
  }

  reload() {
    this.router.navigate([`mirh/requestplace`]);
  }

  btnAction(event: any) {

  }

  onEdith(event: any) {
    this.router.navigate([`${this.ruta}${event.id}`]);
  }

  onDelete(event: any) {

    this.initAction();
    // this.service.delete(event.id).subscribe(
    //   next => {
    //     this.status = [];
    //     this.items = [];
    //     this.itemsFiltered = [];
    //     this.totalResults = 0;
    //     this.init();
    //     this.configureHighlightedBoxes();
    //   },
    //   error => this.handlerError(error),
    //   () => this.completeAction()
    // );
  }

  onClickStatus(event: any) {
    this.filterByStatus(event);
  }

  filterByStatus(event: any) {
    this.itemsFiltered = this.items.filter(f => f.statusMatriz === event.name);
    this.totalResults = this.itemsFiltered.length;
  }

  configureHighlightedBoxes() {
    const list = [];
    for (const column of this.items) {
      if (column.statusMatriz && column.statusMatriz.length > 0) {
        if (list.findIndex(f => f.name === column.statusMatriz) === -1) {
          list.push({ name: column.statusMatriz, tooltip: column.tooltipStatusMatriz });
        }
      }
    }

    const catStatus = [
      {
        name: 'BUSQUEDA DE CANDIDATOS',
        tooltip: 'Tus vacantes en proceso de buscar al colaborador indicado'
      },
      {
        name: 'ENTREVISTA POR RH',
        tooltip: 'Tus vacantes en proceso de entrevista con el reclutador asignado'
      },
      {
        name: 'ENTREVISTA POR SOLICITANTE',
        tooltip: 'Entrevista que TU tienes pendiente de realizar.'
      },
      {
        name: 'ENTREVISTA PENDIENTE',
        tooltip: 'Entrevista que TU tienes pendiente de realizar'
      },
      {
        name: 'ABIERTA',
        tooltip: 'Son TUS solicitudes guardadas que no haz enviado autorizar'
      },
      {
        name: 'PENDIENTE DE AUTORIZAR',
        tooltip:
          'Son solicitudes asignadas a TI para autorización, también TUS solicitudes que aún no cuentan con todas las autorizaciones'
      },
      {
        name: 'PENDIENTE DE AUTORIZACIÓN',
        tooltip:
          'Son solicitudes asignadas a TI para autorización, también TUS solicitudes que aún no cuentan con todas las autorizaciones'
      },
      {
        name: 'AUTORIZADA',
        tooltip:
          'Son solitudes que te asignaron a TI y ya  autorizaste, también TUS solicitudes que ya cuentan con todas las autorizaciones'
      },
      {
        name: 'PENDIENTE POR REASIGNAR',
        tooltip: 'Son solicitudes que se han autorizado y necesitan asignarse a un reclutador'
      },
      {
        name: 'CERRADA',
        tooltip: 'Son TUS solicitudes que terminaron el proceso con la contratación del colaborador'
      },
      {
        name: 'CANCELADA',
        tooltip: 'Son solicitudes que no se aprobaron por algún autorizador por lo que ya no seguirá el proceso'
      },
      {
        name: 'RECHAZADA',
        tooltip:
          'Son las solicitudes que TU generaste y algún autorizador te rechazo, solo es necesario realizar los cambios y enviarla a validación de nuevo'
      },
      {
        name: 'ENTREVISTA PENDIENTE',
        tooltip: 'Entrevista que TU tienes pendientes de realizar'
      },
      {
        name: 'DECISIÓN',
        tooltip: 'Tus vacantes en proceso de seleccionar al mejor'
      },
      {
        name: 'OFERTA',
        tooltip: 'Tus vacantes en proceso de envío de propuesta económica'
      },
      {
        name: 'ALTA',
        tooltip: 'Tus vacantes en proceso de alta'
      },
      {
        name: 'CONTRATADO',
        tooltip: 'Tus vacantes con colaborador pendiente de ingresar'
      },
      {
        name: 'EXPEDIENTE INCOMPLETO',
        tooltip: 'Tus colaboradores con documentos pendientes de entrega'
      }
    ];

    for (const item of list) {
      const listFilter = this.items.filter(f => f.statusMatriz === item.name);
      const status = catStatus.find(f => f.name.toUpperCase() === item.name.toUpperCase());

      if (status) {
        this.status.push({ name: item.name, value: (listFilter ? listFilter.length : 0), tooltip: status.tooltip });
      }
    }

    this.filterByStatus({ name: this.selectedbox });
  }

  columnsFilterFormCustom() {
    this.selectedbox = new Date().toTimeString();
  }

}
