import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnInit, Output} from '@angular/core';
import { ComponentElement } from '../../../../shared/class/ComponentElement';
import { StatusProcess } from '../../../../shared/models/status-process';


@Component({
  selector: 'ho1a-step-interview-result',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-interview-result.component.html',
  styleUrls: ['./step-interview-result.component.scss']
})
export class StepInterviewResultComponent extends ComponentElement implements OnInit, OnChanges {

  @Input()
  currentStep: StatusProcess;

  toggleModalInterviewFormat = false;
  candidateSelected: any;

  constructor() {
    super();
  }

  ngOnInit() {
    this.init();
  }

  ngOnChanges() {
    this.changes();
  }

  visible() {
    return this.configuration && this.configuration.visible;
  }

  readonly() {
    return false;
  }

  initCustom() {
  }

  configFormControl() {
    this.formControls = [];
  }

  configCatalogs() {
    this.listCatalog = [];
  }

  configValueChangeCustom() {

  }

  changesCustom() {

  }

  setCatalogsCustom() {
  }

  onClickPopup(event: any, candidate: any) {
    this.onToggleModalInterviewFormat();
    this.candidateSelected = {...candidate, event: event};
  }

  onToggleModalInterviewFormat() {
    this.toggleModalInterviewFormat = !this.toggleModalInterviewFormat;
  }

  onCancelModalInterviewFormat() {
    this.onToggleModalInterviewFormat();
  }

  onAcceptModalInterviewFormat(event: any) {
    this.onToggleModalInterviewFormat();
  }

}
