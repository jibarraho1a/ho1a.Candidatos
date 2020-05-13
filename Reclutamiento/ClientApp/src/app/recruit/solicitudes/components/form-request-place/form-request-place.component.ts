import { ChangeDetectionStrategy, Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Configuration } from '../../../../shared/models/configuration';
import { StatusProcess } from '../../../../shared/models/status-process';
import { StateValidation } from '../../../../shared/enums/state-validation.enum';

@Component({
  selector: 'ho1a-form-request-place',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './form-request-place.component.html',
  styleUrls: ['./form-request-place.component.scss']
})
export class FormRequestPlaceComponent implements OnInit, OnChanges {

  @Input()
  data: any;

  @Input()
  databackground: any;

  @Input()
  configuration: Configuration;

  steps: StatusProcess[] = [
    { name: 'SOLICITUD', stateValidation: StateValidation.current, description: 'Solicitud de personal', index: 0, visible: true },
    { name: 'ASIGNACIÓN', stateValidation: StateValidation.pending, description: 'Asignación', index: 1, visible: true },
    { name: 'BÚSQUEDA', stateValidation: StateValidation.pending, description: 'Búsqueda', index: 2, visible: true },
    { name: 'TERNA', stateValidation: StateValidation.pending, description: 'Ternas', index: 3, visible: true },
    { name: 'ENTREVISTA', stateValidation: StateValidation.pending, description: 'Entrevista', index: 4, visible: true },
    { name: 'RESULTADOS', stateValidation: StateValidation.pending, description: 'Resultados de las entrevistas', index: 5, visible: true },
    { name: 'OFERTA ECONÓMICA', stateValidation: StateValidation.pending, description: 'Oferta Económica', index: 6, visible: true },
    { name: 'ALTA', stateValidation: StateValidation.pending, description: 'Alta', index: 7, visible: true },
    { name: 'EXPEDIENTE', stateValidation: StateValidation.pending, description: 'Expediente', index: 8, visible: true },
  ];

  currentStep = undefined;
  currentStepCheck = false;
  firstStep = 0;

  constructor() {

  }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    const isChangedConfiguration = changes && changes.configuration !== undefined;
    if (!isChangedConfiguration || this.currentStepCheck) {
      return;
    }
    Object.keys(this.configuration.componentes).map( (value: any, index: number) => {
      if (this.configuration.componentes[index].currentStep) {
        this.firstStep = index;
      }
    });
    this.currentStepCheck = true;
  }

  onSelected(event: any) {
    this.currentStep = event;
  }

  isVisible(index: number) {
    return this.currentStep && this.currentStep.currentIndex === index;
  }

  isVisibleStepExpedient() {
    const userAd = this.databackground && this.databackground.propuesta && this.databackground.propuesta.candidatoIdoneo.userAd;
    const existeUserAd = userAd !== null && userAd !== undefined;
    const fechaConfirmacionAlta = this.databackground && this.databackground.propuesta
      && this.databackground.propuesta.fechaConfirmacionAlta;
    const existeFechaConfirmacionAlta = fechaConfirmacionAlta !== null && fechaConfirmacionAlta !== undefined;
    return existeUserAd && existeFechaConfirmacionAlta;
  }

}
