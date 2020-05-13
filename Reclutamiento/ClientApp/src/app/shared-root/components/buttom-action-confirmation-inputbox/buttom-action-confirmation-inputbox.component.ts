import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

interface OptionButtomActionConfirmationInputboxComponent {
  buttonActionConfirmation: {
    title: string;
    type: any;
    text: string;
    textAccept: string;
    textCancel: string;
    positionLeft: boolean;
    visible: boolean;
    disabled: boolean;
  };
  inputBox: {
    title: string;
    forCancel: { enabled: boolean; required: boolean };
    forConfirm: { enabled: boolean; required: boolean };
  };
}

@Component({
  selector: 'ho1a-buttom-action-confirmation-inputbox',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './buttom-action-confirmation-inputbox.component.html',
  styleUrls: ['./buttom-action-confirmation-inputbox.component.scss']
})
export class ButtomActionConfirmationInputboxComponent implements OnInit {

  @Input()
  option: OptionButtomActionConfirmationInputboxComponent;

  @Output()
  actionResult = new EventEmitter<{buttomSelected: boolean, text: string}>();

  toggledModalInputCancel = false;
  toggledModalInputConfirm = false;

  buttomSelected: boolean;

  constructor() { }

  ngOnInit() {
  }

  onToggledModalInputCancel() {
    this.toggledModalInputCancel = !this.toggledModalInputCancel;
  }

  onToggledModalInputConfirm() {
    this.toggledModalInputConfirm = !this.toggledModalInputConfirm;
  }

  onButtonActionCancel() {
    this.buttomSelected = false;
    if (this.option.inputBox.forCancel.enabled) {
      this.onToggledModalInputCancel();
    } else {
      this.actionResult.emit({ buttomSelected: this.buttomSelected, text: null });
    }
  }

  onButtonActionConfirm() {
    this.buttomSelected = true;
    if (this.option.inputBox.forConfirm.enabled) {
      this.onToggledModalInputConfirm();
    } else {
      this.actionResult.emit({ buttomSelected: this.buttomSelected, text: null });
    }
  }

  onAccept(text: string) {
    this.actionResult.emit({ buttomSelected: this.buttomSelected, text});
    if (this.buttomSelected) {
      this.onToggledModalInputConfirm();
    } else {
      this.onToggledModalInputCancel();
    }
  }

}
