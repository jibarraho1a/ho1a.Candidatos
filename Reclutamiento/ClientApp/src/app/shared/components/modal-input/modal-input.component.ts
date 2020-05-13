import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnInit, Output} from '@angular/core';
import { FormArray, FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'ho1a-modal-input',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './modal-input.component.html',
  styleUrls: ['./modal-input.component.scss']
})
export class ModalInputComponent implements OnInit, OnChanges {

  @Input()
  title: string;

  @Input()
  required = false;

  @Output()
  accept = new EventEmitter<string>();

  @Output()
  cancel = new EventEmitter();

  form = this.fb.group({
    description: [null]
  });

  constructor(
    private fb: FormBuilder
  ) { }

  ngOnInit() {
  }

  ngOnChanges(): void {
    if (this.required) {
      const description = this.form.get('description');
      description.clearValidators();
      description.setValidators([Validators.required, Validators.pattern(/^(?!\s*$)/)]);
      description.updateValueAndValidity();
    }
  }

  close() {
    this.onCancel();
  }

  onAccept() {
    if (this.validForm) {
      this.accept.emit(this.form.get('description').value);
    }
  }

  onCancel() {
    this.cancel.emit();
  }

  get validForm() {
    return this.form.valid;
  }

}
