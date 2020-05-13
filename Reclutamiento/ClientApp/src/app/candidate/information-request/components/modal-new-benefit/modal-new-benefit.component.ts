import {ChangeDetectionStrategy, Component, OnInit} from '@angular/core';
import {ModalForm} from '../../../../shared-root/class/ModalForm';
import {Validators} from '@angular/forms';
import {EnumTypeEvent, EventApp, StateOption, Store} from '../../../../store';

@Component({
  selector: 'ho1a-modal-new-benefit',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './modal-new-benefit.component.html',
  styleUrls: ['./modal-new-benefit.component.scss']
})
export class ModalNewBenefitComponent extends ModalForm implements OnInit {

  butttom = [
    { name: 'Guardar', value: 'Guardar', submit: true, class: 'save'}
  ];

  constructor(
    private store: Store
  ) {
    super();
  }

  ngOnInit() {
    this.form = this.fb.group({
      descripcion: [this.data.descripcion, Validators.required],
      valor: [this.data.valor, [Validators.required, Validators.min(0)]],
      nombre: [this.data.nombre],
      id: [this.data.id],
      active: [this.data.active],
      ultimoTrabajoId: [this.data.ultimoTrabajoId],
      tipoPrestacionId: [this.data.tipoPrestacionId],
    });
  }

  clickButtomCustom() {
    this.form.get('nombre').setValue(this.form.get('descripcion').value);
    this.store.set(StateOption.eventsRequisition, <EventApp>{
      typeEvent: EnumTypeEvent.SavingBenefitLastJob,
      data: this.form.value
    });
  }

  cancelCustom() {
  }

}
