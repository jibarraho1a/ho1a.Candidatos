import {Component, EventEmitter, Input, OnChanges, OnInit, Output} from '@angular/core';
import {FormBuilder} from '@angular/forms';

@Component({
  selector: 'ho1a-form-general-data',
  templateUrl: './form-general-data.component.html',
  styleUrls: ['./form-general-data.component.scss']
})
export class FormGeneralDataComponent implements OnInit, OnChanges {

  @Output()
  create = new EventEmitter();

  @Input()
  item: any;

  exist = false;

  form = this.fb.group(
    {
      id: [null],
      fechaSolicitud: [null],
      fechaUltimoEstatus: [null]
      });

  constructor(
    private fb: FormBuilder
  ) { }

  ngOnInit() {
  }

  ngOnChanges() {
    if (this.item && this.item.id) {
      const value = this.item;
      this.exist = true;
      this.form.patchValue(value);
    }
  }

  onCreate() {
    if (this.form.valid) {
      this.create.emit();
    }
  }

  get id() {
    return this.form.get('id').value;
  }

  get fechaSolicitud() {
    return this.form.get('fechaSolicitud').value;
  }

  get fechaUltimoEstatus() {
    return this.form.get('fechaUltimoEstatus').value;
  }

}
