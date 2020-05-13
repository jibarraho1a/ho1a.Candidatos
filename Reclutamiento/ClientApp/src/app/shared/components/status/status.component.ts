import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { StatusProcess } from '../../models/status-process';
import { StateValidation } from '../../enums/state-validation.enum';

@Component({
  selector: 'ho1a-status',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './status.component.html',
  styleUrls: ['./status.component.scss']
})
export class StatusComponent implements OnInit {

  @Input()
  status: StatusProcess[];

  @Input()
  enableButtonAction: boolean;

  @Output()
  change = new EventEmitter();

  private toggledApprove = false;
  private toggledReject = false;
  private toggledCancel = false;

  constructor() { }

  ngOnInit() {
  }

  onchangeStatus(data: any, stateValidation: StateValidation) {
    this.toggledReject = false;
    this.toggledApprove = false;
    this.toggledCancel = false;
    const selected = Object.assign({}, <StatusProcess>{}, data);
    selected.stateValidation = stateValidation;
    this.change.emit(selected);
  }

  onToggledApprove() {
    this.toggledApprove = !this.toggledApprove;
  }

  onToggledReject() {
    this.toggledReject = !this.toggledReject;
  }

  onToggledCancel() {
    this.toggledCancel = !this.toggledCancel;
  }

}
