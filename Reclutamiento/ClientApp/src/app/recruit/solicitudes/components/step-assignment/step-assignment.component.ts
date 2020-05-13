import {ChangeDetectionStrategy, Component, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import { ComponentElement } from '../../../../shared/class/ComponentElement';
import { StatusProcess } from '../../../../shared/models/status-process';
import { CustomFormat } from '../../../../shared/class/CustomFormat';
import { Store } from '../../../../shared/class/Store';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { EventApp } from '../../../../shared/models/event-app';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';
import { DataType } from '../../../../shared/enums/data-type.enum';

@Component({
  selector: 'ho1a-step-assignment',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-assignment.component.html',
  styleUrls: ['./step-assignment.component.scss']
})
export class StepAssignmentComponent extends ComponentElement implements OnInit, OnChanges {

  @Input()
  currentStep: StatusProcess;

  toggleReassignment = false;

  format: CustomFormat;

  constructor(
    private store: Store
  ) {
    super();
    this.format = new CustomFormat();
  }

  ngOnInit() {
    this.init();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.changes(changes);
  }

  visible() {
    return  this.configuration && this.configuration.visible;
  }

  readonly() {
    return false;
  }

  initCustom() {
  }

  configFormControl() {
    this.formControls = [
      { name: 'reasignado', custom: false, isCombo: true },
    ];
  }

  configCatalogs() {
    this.listCatalog = [
    ];
  }

  configValueChangeCustom() {
  }

  changesCustom() {

  }

  setCatalogsCustom() {
  }

  onCancelReassignment() {
    this.form.get('reasignado').setValue(null);
    this.onToggledReassignment();
  }

  onToggledReassignment() {
    this.toggleReassignment = !this.toggleReassignment;
  }

  onSaveAssignment() {
    this.store.set(
      StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.AssignedRequisition,
        data: null
      });
  }

  onSaveReAssignment() {
    if (this.validForm) {
      this.store.set(
        StateOption.eventsRequisition,
        <EventApp>{
          typeEvent: TypeEvent.AssignedRequisition,
          data: this.form.get('reasignado').value
        });
    }
  }

  get canModify() {
    return this.data && this.data.inicioReclutamiento === null;
  }

  get inicioReclutamiento() {
    return this.format.format(DataType.DateFull, this.data.inicioReclutamiento);
  }

}
