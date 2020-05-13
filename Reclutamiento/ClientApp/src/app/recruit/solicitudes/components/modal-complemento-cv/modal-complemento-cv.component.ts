import {ChangeDetectionStrategy, Component, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {Validators} from '@angular/forms';
import { ModalForm } from '../../../../shared/class/ModalForm';
import { Store } from '../../../../shared/class/Store';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { EventApp } from '../../../../shared/models/event-app';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';


@Component({
  selector: 'ho1a-modal-complemento-cv',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './modal-complemento-cv.component.html',
  styleUrls: ['./modal-complemento-cv.component.scss']
})
export class ModalComplementoCvComponent extends ModalForm implements OnInit, OnChanges {

  @Input()
  data: any;

  butttom = [
    {name: 'Guardar', value: 'Guardar', submit: true, class: 'save'}
  ];

  ultimosSalarios = [
    {disabled: false, group: null, selected: true,  text: 'SELECCIONE RANGO DE SALARIO', value: null},
    {disabled: false, group: null, selected: false, text: 'Menos de  $5,000.00', value: '1'},
    {disabled: false, group: null, selected: false, text: '$5,000.00 - $10,000.00',  value: '2'},
    {disabled: false, group: null, selected: false, text: '$10,000.00 - $15,000.00', value: '3'},
    {disabled: false, group: null, selected: false, text: '$15,000.00 - $20,000.00', value: '4'},
    {disabled: false, group: null, selected: false, text: '$20,000.00 - $25,000.00', value: '5'},
    {disabled: false, group: null, selected: false, text: '$25,000.00 - $30,000.00', value: '6'},
    {disabled: false, group: null, selected: false, text: '$30,000.00 - $35,000.00', value: '7'},
    {disabled: false, group: null, selected: false, text: '$35,000.00 - $40,000.00', value: '8'},
    {disabled: false, group: null, selected: false, text: '$40,000.00 - $45,000.00', value: '9'},
    {disabled: false, group: null, selected: false, text: '$45,000.00 - $50,000.00', value: '10'},
    {disabled: false, group: null, selected: false, text: '$50,000.00 - $55,000.00', value: '11'},
    {disabled: false, group: null, selected: false, text: '$55,000.00 - $60,000.00', value: '12'},
    {disabled: false, group: null, selected: false, text: 'Más de $60,000.00', value: '13'}
  ];

  constructor(
    private store: Store
  ) {
    super();
  }

  ngOnInit() {
    const empresa = this.data.detalle && this.data.detalle.ultimoTrabajo && this.data.detalle.ultimoTrabajo.empresa || null;
    const puesto = this.data.detalle && this.data.detalle.ultimoTrabajo && this.data.detalle.ultimoTrabajo.puesto || null;
    const ultimoSalarioId = this.data.detalle && this.data.detalle.ultimoSalarioId || null;
    const pretencionEconomica = this.data.detalle && this.data.detalle.pretencionEconomica || null;
    const certificacion = this.data.detalle && this.data.detalle.certificacion ? true : false;
    const experiencia = this.data.detalle && this.data.detalle.experiencia || null;

    this.form = this.fb.group({
      detalleUltimoTrabajoEmpresa: [
        {value: empresa, disabled: this.readonly},
        [Validators.required, Validators.pattern(/^(?!\s*$)/)]
      ],
      detalleUltimoTrabajoPuesto: [
        {value: puesto, disabled: this.readonly},
        [Validators.required, Validators.pattern(/^(?!\s*$)/)]
      ],
      detalleUltimoSalarioId: [
        {value: ultimoSalarioId, disabled: this.readonly},
        Validators.required
      ],
      detallePretencionEconomica: [pretencionEconomica, Validators.required],
      detalleCertificacion: [certificacion, Validators.required],
      anosExperiencia: [experiencia, Validators.required],
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.changes(changes);
  }

  clickButtomCustom() {
    const data = {
      Empresa: this.form.get('detalleUltimoTrabajoEmpresa').value,
      Puesto: this.form.get('detalleUltimoTrabajoPuesto').value,
      PretencionEconomica: this.form.get('detallePretencionEconomica').value,
      Certificacion: this.form.get('detalleCertificacion').value,
      UltimoSalario: { value: this.form.get('detalleUltimoSalarioId').value },
      experiencia: this.form.get('anosExperiencia').value,
      candidatoId: this.data.candidatoId
    };
    this.store.set(StateOption.eventsRequisition, <EventApp>{
      typeEvent: TypeEvent.SavingComplementCV,
      data: data
    });
  }

  cancelCustom() {
  }

}
