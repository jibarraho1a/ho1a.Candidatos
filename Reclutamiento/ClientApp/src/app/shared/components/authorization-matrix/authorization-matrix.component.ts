import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { StatusProcess } from '../../models/status-process';


@Component({
  selector: 'ho1a-authorization-matrix',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './authorization-matrix.component.html',
  styleUrls: ['./authorization-matrix.component.scss']
})
export class AuthorizationMatrixComponent implements OnInit {

  @Input()
  enableButtonAction: boolean;

  @Input()
  titleModalInputCommentsConfirmation: string;

  @Input()
  matrix: any[];

  @Output()
  confirmationValidation = new EventEmitter<StatusProcess>();

  toogledModalInputComments = false;

  approvalSelected: StatusProcess;

  constructor() { }

  ngOnInit() {
  }

  changeStatus(data: StatusProcess) {
    this.approvalSelected = data;
    if (this.enableButtonAction && data.canDeny === true && data.canApprove === true) {
      this.toogledModalInputComments = !this.toogledModalInputComments;
    } else if (this.enableButtonAction && data.canDeny === false) {
      this.confirmationValidation.emit(this.approvalSelected);
    }
  }

  cancelCommentsConfirmation(event: any) {
    this.toogledModalInputComments = !this.toogledModalInputComments;
    this.approvalSelected = null;
  }

  acceptCommentsConfirmation(event: any) {
    this.approvalSelected.description = event;
    this.toogledModalInputComments = !this.toogledModalInputComments;
    this.confirmationValidation.emit(this.approvalSelected);
  }

}
