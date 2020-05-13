import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { TypeButtonAction } from '../../enums';

@Component({
  selector: 'ho1a-button-action-confirmation',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './button-action-confirmation.component.html',
  styleUrls: ['./button-action-confirmation.component.scss']
})
export class ButtonActionConfirmationComponent implements OnInit {

  @Output()
  confirm = new EventEmitter();

  @Output()
  cancel = new EventEmitter();

  @Output()
  toggled = new EventEmitter<{typebutton: TypeButtonAction, toggle: boolean}>();

  @Input()
  visible = true;

  @Input()
  disabled = true;

  @Input()
  typebutton = TypeButtonAction.Help;

  @Input()
  textButton = 'undefined';

  @Input()
  positionLeft = true;

  @Input()
  title = '¿Esta seguro?';

  @Input()
  textAccept = 'SI';

  @Input()
  textCancel = 'NO';

  toggle = false;

  constructor() { }

  ngOnInit() {
  }

  onClick() {
    if (!this.disabled) {
      this.onToggled();
    }
  }

  onToggled() {
    this.toggle = !this.toggle;
  }

  onConfirm() {
    this.confirm.emit();
    this.onToggled();
  }

  onCancel() {
    this.cancel.emit();
    this.onToggled();
  }

  isVisible() {
    return this.visible;
  }

  isDisabled() {
    return this.disabled  ? 'disabled' : null;
  }

}
