import {ChangeDetectionStrategy, Component, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {Validators} from '@angular/forms';
import { ModalForm } from '../../../../shared/class/ModalForm';
import { Store } from '../../../../shared/class/Store';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { EventApp } from '../../../../shared/models/event-app';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';

@Component({
  selector: 'ho1a-modal-interview-format',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './modal-interview-format.component.html',
  styleUrls: ['./modal-interview-format.component.scss']
})
export class ModalInterviewFormatComponent extends ModalForm implements OnInit, OnChanges {

  butttom = [
    {name: 'Guardar', value: 'Guardar', submit: true, class: 'save'}
  ];

  total = 0;

  maxRating = 5;

  constructor(
    private store: Store
  ) {
    super();
  }

  ngOnInit() {
    this.form = this.fb.group({
      fortalezas: [
        {value: this.data.event.data.fortalezas, disabled: this.readonly},
        [Validators.required, Validators.pattern(/^(?!\s*$)/)]
      ],
      debilidades: [
        {value: this.data.event.data.debilidades, disabled: this.readonly},
        [Validators.required, Validators.pattern(/^(?!\s*$)/)]
      ],
      comentarios: [
        {value: this.data.event.data.comentarios, disabled: this.readonly},
        [Validators.required, Validators.pattern(/^(?!\s*$)/)]
      ],
      recomendable: [this.data.event.data.recomendable, Validators.required],
      competenciasEvaluadas: [null, Validators.required],
    });
    this.setCompetenciasEvaluadas();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.changes(changes);
    this.setTotal();
  }

  clickButtomCustom() {
    if (this.validateEvaluatedCompetences()) {
      // TODO por definir estructura final de la respuesta
      this.data.event.data.fortalezas = this.form.get('fortalezas').value;
      this.data.event.data.debilidades = this.form.get('debilidades').value;
      this.data.event.data.comentarios = this.form.get('comentarios').value;
      this.data.event.data.recomendable = this.form.get('recomendable').value;
      this.store.set(StateOption.eventsRequisition, <EventApp>{
        typeEvent: TypeEvent.SavingInterviewCandidateRequisition,
        data: this.data.event.data
      });
    }
  }

  cancelCustom() {
  }

  validateEvaluatedCompetences() {
    const competenciasEvaluated = this.data.event.data.competencias.filter( f => f.resultado > 0);
    const competencias = this.data.event.data.competencias;
    return competencias && competenciasEvaluated && competencias.length > 0 && competencias.length === competenciasEvaluated.length;
  }

  onRatingClicked(value: number, item: any, index: number) {
    this.data.event.data.competencias[index].resultado = value;
    this.setCompetenciasEvaluadas();
    this.setTotal();
  }

  setCompetenciasEvaluadas() {
    const competenciasEvaluadas = this.validateEvaluatedCompetences() ? true : null;
    this.form.get('competenciasEvaluadas').setValue(competenciasEvaluadas);
  }

  setTotal() {
    const value = this.data && this.data.event && this.data.event.data && this.data.event.data.competencias;
    this.total = 0;
    if (value && value.length > 0) {
      for (const item of value) {
        this.total += item.resultado;
      }
      this.total = parseFloat((this.total / (value.length * this.maxRating) * this.maxRating).toFixed(2));
    }
    this.data.event.data['promedio'] = this.total;
  }

  onSetRecomended(value: boolean) {
    this.form.get('recomendable').setValue(value);
  }

  get recomended() {
    return this.form.get('recomendable').value;
  }

}
