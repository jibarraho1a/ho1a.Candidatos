import {FormBuilder, FormGroup} from '@angular/forms';
import {EventEmitter, Output} from '@angular/core';

export abstract class ModalElement {

  @Output()
  accept = new EventEmitter<any>();

  @Output()
  cancel = new EventEmitter();

  form: FormGroup;
  fb: FormBuilder;

  constructor() {
    this.fb = new FormBuilder();
  }

  close() {
    this.onCancel();
  }

  onAccept() {
    this.accept.emit(this.form.value);
  }

  abstract onAcceptCustom();

  onCancel() {
    this.cancel.emit();
  }

}
