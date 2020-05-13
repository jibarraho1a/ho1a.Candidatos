import {FormBuilder, FormGroup} from '@angular/forms';
import {EventEmitter, Input, Output, SimpleChanges} from '@angular/core';

export abstract class ModalForm {

  @Input()
  data: any;

  @Input()
  readonly: boolean;

  @Output()
  cancel = new EventEmitter();

  @Output()
  clickButtomAction = new EventEmitter<{buttom: any, form: any}>();

  form: FormGroup;
  fb: FormBuilder;

  buttomSelected: any;

  butttom = [];

  constructor() {
    this.fb = new FormBuilder();
  }

  changes(changes: SimpleChanges) {
    if (this.readonly) {
      this.butttom = [];
    }
  }

  onCancel() {
    this.cancel.emit();
    this.cancelCustom();
  }

  abstract cancelCustom();

  onClickButtom(event: any) {
    this.buttomSelected = event.buttom;
    this.clickButtomAction.emit(event);
    this.clickButtomCustom();
  }

  abstract clickButtomCustom();

  isSubmit() {
    return this.buttomSelected && this.buttomSelected.submit === true;
  }

  getHintByControl() {

  }

  getErrorByControl() {

  }

  showAsRequired(controlName: string) {
    return this.form.get(controlName) && this.form.get(controlName).errors !== null;
  }

}
