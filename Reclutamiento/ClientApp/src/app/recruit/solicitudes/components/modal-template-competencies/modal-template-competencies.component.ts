import {ChangeDetectionStrategy, Component, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import { Validators } from '@angular/forms';
import { ModalForm } from '../../../../shared/class/ModalForm';
import { Store } from '../../../../shared/class/Store';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { EventApp } from '../../../../shared/models/event-app';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';

@Component({
  selector: 'ho1a-modal-template-competencies',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './modal-template-competencies.component.html',
  styleUrls: ['./modal-template-competencies.component.scss']
})
export class ModalTemplateCompetenciesComponent extends ModalForm implements OnInit, OnChanges {

  @Input()
  requisicionId: number = null;

  @Input()
  readonly = false;

  butttom = [];

  title = 'Plantilla para entrevistas';

  constructor(
    private store: Store
  ) {
    super();
  }

  ngOnInit() {
    this.form = this.fb.group({
      nombre: [null, [Validators.pattern(/^(?!\s*$)[A-Za-z\sáéíóú]+$/)]]
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!this.readonly) {
      this.butttom = [
        { name: 'Guardar', value: 'Guardar', submit: true, class: 'save' }
      ];
    }
  }

  clickButtomCustom() {
    if (this.data.length > 0) {
      this.store.set(StateOption.eventsRequisition,
        <EventApp>{
          typeEvent: TypeEvent.SavingCompetencyInterviewCandidateRequisition,
          data: this.data
        });
    }
  }

  cancelCustom() {
  }

  canDelete(item: any) {
    return item.requisicionDetalleId !== null && !this.readonly;
  }

  onDelete(event: any, i: number) {
    this.data.splice(i, 1);
  }

  onAdd() {
    const nombre = this.form.get('nombre').value;
    if (nombre !== null) {
      this.data.push({
        descripcion: nombre,
        nombre: nombre,
        requisicionDetalleId: this.requisicionId,
        id: 0,
        active: true
      });
      this.form.reset();
    }
  }
}
