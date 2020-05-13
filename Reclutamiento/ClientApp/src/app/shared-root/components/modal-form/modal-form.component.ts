import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {FormGroup} from '@angular/forms';

@Component({
  selector: 'ho1a-modal-form',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './modal-form.component.html',
  styleUrls: ['./modal-form.component.scss']
})
export class ModalFormComponent implements OnInit, OnChanges {

  @Input()
  tittle = '';

  @Input()
  width = '800px';

  @Input()
  buttoms: {name: string; value: string; submit: boolean; class: string; visible?: boolean; }[];

  @Input()
  form: FormGroup;

  @Output()
  cancel = new EventEmitter();

  @Output()
  clickButtom = new EventEmitter<{buttom: any, form: any}>();

  constructor() { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges): void {
  }

  close() {
    this.onCancel();
  }

  onCancel() {
    this.cancel.emit();
  }

  onClick(buttom: any) {
    if (buttom.submit && this.form.valid) {
      this.clickButtom.emit({buttom: buttom, form: this.form.value});
    } else if (!buttom.submit) {
      this.clickButtom.emit({buttom: buttom, form: null});
    }
  }

}
