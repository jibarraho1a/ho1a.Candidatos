import {ChangeDetectionStrategy, Component, Input, OnChanges, OnInit} from '@angular/core';
import { ComponentElement } from '../../../../shared/class/ComponentElement';
import { StatusProcess } from '../../../../shared/models/status-process';
import { Store } from '../../../../shared/class/Store';
import { TypeButtonAction } from '../../../../shared/enums/type-button-action.enum';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { EventApp } from '../../../../shared/models/event-app';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';


@Component({
  selector: 'ho1a-step-expedient',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-expedient.component.html',
  styleUrls: ['./step-expedient.component.scss']
})
export class StepExpedientComponent extends ComponentElement implements OnInit, OnChanges{

  @Input()
  currentStep: StatusProcess;

  @Input()
  visualizarEnvioDocumentacion = false;

  optionButtomAceptarPropuesta = <any>{};

  constructor(private store: Store) {
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

  configCatalogs() {
    this.listCatalog = [
    ];
  }

  configFormControl() {
    this.formControls = [];
  }

  configValueChangeCustom() {
  }

  changesCustom() {
    this.optionButtomAceptarPropuesta = {
      buttonActionConfirmation: {
        title: '¿Enviar observaciones?',
        type: TypeButtonAction.NotificationImportant,
        text: this.getActionByControl('ResponderDocumentacionRequisicion').descripcion,
        textAccept: 'SI',
        textCancel: 'NO',
        positionLeft: false,
        visible: true,
        disabled: false
      },
      inputBox: {
        title: 'Observaciones',
        forCancel: { enabled: false, required: false },
        forConfirm: { enabled: true, required: true }
      }
    };
  }

  setCatalogsCustom() {
  }

  onUpload(document: any) {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.SavingDocumentRequisition,
        data: document
      });
  }

  onDelete(document: any) {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.DeletingDocumentRequisition,
        data: document
      });
  }

  onSend() {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.SendingDocumentationOfCandidateFile,
        data: null
      });
  }

  get isVisibleSend() {
    let required = [];
    let loaded = [];
    let total = 0;

    if (this.databackground && this.databackground.expediente) {
      total = this.databackground.expediente.length;
      required = this.databackground.expediente.filter( f => f.required);
      loaded = this.databackground.expediente.filter( f => f.required && f.loaded);
    }

    return total > 0 && loaded.length >= required.length;
  }

  onActionResultAcceptOffer(event: any) {
    this.store.set(StateOption.eventsRequisition,
        <EventApp>{
          typeEvent: TypeEvent.RespondingToTheDocumentationOfCandidateFile,
          data: {
            comentario: event.text
          }
      });
  }

}
