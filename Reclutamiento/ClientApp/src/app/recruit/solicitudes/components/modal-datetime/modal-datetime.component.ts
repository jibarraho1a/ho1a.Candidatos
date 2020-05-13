import {ChangeDetectionStrategy, Component, OnChanges, OnInit, SimpleChanges, Input} from '@angular/core';
import {Validators} from '@angular/forms';
import { ModalForm } from '../../../../shared/class/ModalForm';
import { CustomFormat } from '../../../../shared/class/CustomFormat';
import { DataType } from '../../../../shared/enums/data-type.enum';

@Component({
  selector: 'ho1a-modal-datetime',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './modal-datetime.component.html',
  styleUrls: ['./modal-datetime.component.scss']
})
export class ModalDatetimeComponent extends ModalForm implements OnInit, OnChanges {

  @Input()
  fechaMinima: Date = new Date(1900, 1);

  butttom = [
    {name: 'Guardar', value: 'Guardar', submit: true, class: 'save'}
  ];

  format: CustomFormat;

  constructor() {
    super();
    this.format = new CustomFormat();
  }

  ngOnInit() {
    this.form = this.fb.group({
      fecha: [new Date(), Validators.required],
      hora: [12, [Validators.min(0), Validators.max(24), Validators.required]],
      minuto: [0, [Validators.min(0), Validators.max(60), Validators.required]],
      datetime: [null],
      datetimestring: [null],
    });
    this.setDates();
    this.form.get('fecha').valueChanges.subscribe( value => {
      this.setDates();
    });
    this.form.get('hora').valueChanges.subscribe( value => {
      this.setDates();
    });
    this.form.get('minuto').valueChanges.subscribe( value => {
      this.setDates();
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.changes(changes);
  }

  clickButtomCustom() {
  }

  cancelCustom() {
  }

  setDates() {
    const fecha = this.format.format(DataType.DateDash, this.form.get('fecha').value.toString() );
    const horas = this.form.get('hora').value.toString().padStart(2, '0');
    const minutos = this.form.get('minuto').value.toString().padStart(2, '0');
    const segundos = ''.toString().padStart(2, '0');
    const milisegundos = '.0000000';
    const datetimestring = fecha + 'T' + horas + ':' + minutos + ':' + segundos + milisegundos;
    this.form.get('datetimestring').setValue(datetimestring);
    this.form.get('datetime').setValue(new Date(datetimestring));
  }

}
