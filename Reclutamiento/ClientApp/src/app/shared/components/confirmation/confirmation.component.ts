import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'ho1a-confirmation',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.scss']
})
export class ConfirmationComponent implements OnInit {

  @Input()
  title = '¿Esta seguro?';

  @Input()
  textAccept = 'SI';

  @Input()
  textCancel = 'NO';

  @Input()
  positionLeft: boolean;

  @Input()
  confirmationtitle: string;

  @Input()
  btnaccepttext: string;

  @Input()
  btncanceltext: string;

  @Input()
  visible: boolean;

  @Output()
  accept = new EventEmitter();

  @Output()
  cancel = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  onAccept() {
    this.accept.emit();
  }

  onCancel() {
    this.cancel.emit();
  }

}
