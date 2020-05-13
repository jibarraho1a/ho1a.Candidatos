import {ChangeDetectionStrategy, Component, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {Validators} from '@angular/forms';
import { ModalForm } from '../../../../shared/class/ModalForm';
import { Store } from '../../../../shared/class/Store';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { EventApp } from '../../../../shared/models/event-app';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';


@Component({
  selector: 'ho1a-modal-cv',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './modal-cv.component.html',
  styleUrls: ['./modal-cv.component.scss']
})
export class ModalCvComponent extends ModalForm implements OnInit, OnChanges {

  documents: any[] = [];

  constructor(
    private store: Store
  ) {
    super();
  }

  ngOnInit() {
    this.form = this.fb.group({
      detalleUltimoTrabajoEmpresa: [
        {value: null, disabled: this.readonly},
        [Validators.required, Validators.pattern(/^(?!\s*$)/)]
      ],
      detalleUltimoTrabajoPuesto: [
        {value: null, disabled: this.readonly},
        [Validators.required, Validators.pattern(/^(?!\s*$)/)]
      ],
      detalleUltimoSalarioId: [
        {value: null, disabled: this.readonly},
        Validators.required
      ],
      detallePretencionEconomica: [null, Validators.required],
      detalleCertificacion: [false, Validators.required],
      anosExperiencia: [null, Validators.required],
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.data && this.data.detalle && this.data.detalle.cv) {
      this.documents.push(this.data.detalle.cv);
    }
    this.changes(changes);
  }

  clickButtomCustom() {
    /*
      this.data.event.data.fortalezas = this.form.get('fortalezas').value;
      this.data.event.data.debilidades = this.form.get('debilidades').value;
      this.data.event.data.comentarios = this.form.get('comentarios').value;
      this.data.event.data.recomendable = this.form.get('recomendable').value;
      this.store.set(StateOption.eventsRequisition, <EventApp>{
        typeEvent: EnumTypeEvent.SavingInterviewCandidateRequisition,
        data: this.data.event.data
      });*/
  }

  cancelCustom() {
  }

  onDelete(event: any) {
    console.log('onDelete');
  }

  onUpload(event: any) {
    this.store.set(StateOption.eventsRequisition, <EventApp>{
      typeEvent: TypeEvent.SavingDocumentCVCandidate,
      data: event
    });
    this.onCancel();
  }

}
