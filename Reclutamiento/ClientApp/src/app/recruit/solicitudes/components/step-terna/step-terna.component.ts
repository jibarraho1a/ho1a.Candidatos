import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { ComponentElement } from '../../../../shared/class/ComponentElement';
import { StatusProcess } from '../../../../shared/models/status-process';

@Component({
  selector: 'ho1a-step-terna',
  templateUrl: './step-terna.component.html',
  styleUrls: ['./step-terna.component.scss']
})
export class StepTernaComponent extends ComponentElement implements OnInit, OnChanges {

  @Input()
  currentStep: StatusProcess;

  toggleModalTemplateCompetencies = false;

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

  getAction(controlName: string) {
    const action = this.getActionByControl(controlName);
    switch (controlName) {
      case 'PlantillaEntrevistaRequisicion':
        const actions = this.getActions();
        if ((actions && actions.evaluacionesIniciadas) === true) {
          action.readonly = true;
        } else {
          action.readonly = false;
        }
        break;
    }
    return action;
  }

  getActions() {
    return this.databackground && this.databackground.acciones
    && this.databackground.acciones.terna;
  }

  onToggleModalTemplateCompetencies() {
    this.toggleModalTemplateCompetencies = !this.toggleModalTemplateCompetencies;
  }

  onCancelModalTemplateCompetencies() {
    this.onToggleModalTemplateCompetencies();
  }

  onAcceptModalTemplateCompetencies(event: any) {
    this.onToggleModalTemplateCompetencies();
  }

}
