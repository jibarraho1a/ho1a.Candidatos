import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { Combo } from '../../../../shared/models/combo';
import { Candidatos } from '../../../../shared/class/Candidatos';
import { Store } from '../../../../shared/class/Store';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { EventApp } from '../../../../shared/models/event-app';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';
import { Alert } from '../../../../shared/models/alert';
import { AlertType } from '../../../../shared/enums/alert-type.enum';

@Component({
  selector: 'ho1a-step-search-candidate-custom',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-search-candidate-custom.component.html',
  styleUrls: ['./step-search-candidate-custom.component.scss']
})
export class StepSearchCandidateCustomComponent implements OnInit, OnChanges {

  @Input()
  candidate: any;

  @Input()
  ternas: Combo[];

  @Input()
  ternaIdDisabled = false;

  @Input()
  statusDisabled = false;

  @Input()
  ternasCandidatos: any[];

  @Output()
  complementoCV = new EventEmitter<any>();

  candidatos: Candidatos;

  form: FormGroup;
  estatus = [
    {value: '-2', text: 'Descartado' },
    {value: '-3', text: 'En duda' },
    {value: '-4', text: 'No Interesado' },
  ];

  constructor(
    private fb: FormBuilder,
    private store: Store
  ) {
    this.candidatos = new Candidatos();
  }

  ngOnInit() {
    this.form.get('status').valueChanges.subscribe( value => this.setStatus(value));
    this.form.get('ternaId').valueChanges.subscribe( value => {
      this.candidatos.setCandidatos(this.ternasCandidatos);
      const ternaId = this.candidate && this.candidate.data && this.candidate.data.ternaId ? this.candidate.data.ternaId : 0;
      const puedeAgregarceATerna = value && this.candidatos.puedoAgregarCandidatoATerna(value);
      const esUnaTernaDistintaALaActual = value !== ternaId;
      const esTernaSinDefinir = value === 0;
      const puedeSetearTerna = (esTernaSinDefinir && esUnaTernaDistintaALaActual) || (esUnaTernaDistintaALaActual && puedeAgregarceATerna);
      if (!puedeSetearTerna) {
        this.onShowMessage('Ya no se puede agregar mas candidatos a la Terna #' + value);
      }
      if (puedeSetearTerna) {
        this.setTerna(value);
      } else if (esUnaTernaDistintaALaActual) {
        this.form.get('ternaId').setValue(ternaId);
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes && changes.candidate !== undefined) {
      const value = this.candidate && this.candidate.data && this.candidate.data.ternaId ? this.candidate.data.ternaId : 0;
      const valueStatus = this.candidate.data && this.candidate.data.statusCandidato !== null ?
        this.candidate.data.statusCandidato.toString() : null;

      this.form = this.fb.group({
        ternaId: [{value: value, disabled: this.ternaIdDisabled}, Validators.required],
        status: [{value: valueStatus, disabled: this.statusDisabled}]
      });
    }
  }

  setTerna(value: any) {
    this.candidate.data['ternaId'] = value;
    this.store.set(StateOption.eventsRequisition,
        <EventApp>{
          typeEvent: TypeEvent.AssigningTernaToCandidateRequisition,
          data: this.candidate.data
        });
  }

  setStatus(value: any) {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.SelectingSuitableCandidateRequisition,
        data: {
          candidatoId: this.candidate.data.candidatoId,
          status: value
        }
      });
  }

  onShowMessage(message: string) {
    this.store.set(StateOption.alert,
      <Alert>{
        type: AlertType.Warning,
        description: message,
        title: '¡Alerta!'
      });
  }

  oncomplementoCV() {
    this.complementoCV.emit({ event: 'complementCV' });
  }

}
