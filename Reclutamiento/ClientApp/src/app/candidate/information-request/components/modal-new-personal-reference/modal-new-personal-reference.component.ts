import {ChangeDetectionStrategy, Component, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {ModalForm} from '../../../../shared-root/class/ModalForm';
import {Validators} from '@angular/forms';
import {EnumTypeEvent, EventApp, StateOption, Store} from '../../../../store';

@Component({
  selector: 'ho1a-modal-new-personal-reference',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './modal-new-personal-reference.component.html',
  styleUrls: ['./modal-new-personal-reference.component.scss']
})
export class ModalNewPersonalReferenceComponent extends ModalForm implements OnInit, OnChanges {

  butttom = [
    {name: 'Guardar', value: 'Guardar', submit: true, class: 'save'}
  ];

  constructor(
    private store: Store
  ) {
    super();
  }

  ngOnInit() {

  }

  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.form = this.fb.group({
      email: [this.data.email, [Validators.required, Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.([a-zA-Z]{2,4})+$/)]],
      materno: [this.data.materno, [Validators.required, Validators.pattern(/^(?!\s*$)[A-Za-z\sáéíóú]+$/)]],
      nombre: [this.data.nombre, [Validators.required, Validators.pattern(/^(?!\s*$)[A-Za-z\sáéíóú]+$/)]],
      parentesco: [this.data.parentesco, [Validators.required, Validators.pattern(/^(?!\s*$)[A-Za-z\sáéíóú]+$/)]],
      paterno: [this.data.paterno, [Validators.required, Validators.pattern(/^(?!\s*$)[A-Za-z\sáéíóú]+$/)]],
      solicitarReferencia: [this.data.solicitarReferencia, Validators.required],
      telefono: [this.data.telefono, [Validators.required, Validators.pattern(/^([0-9]){10}$/)]],
      tiempoConocerse: [this.data.tiempoConocerse, [Validators.required, Validators.pattern(/^([0-9]){2}$/)]],
      id: [this.data.id],
      candidatoDetalleId: [this.data.candidatoDetalleId],
      active: [this.data.active],
    });
  }

  clickButtomCustom() {
    this.store.set(StateOption.eventsRequisition, <EventApp>{
      typeEvent: EnumTypeEvent.SavingPersonalReference,
      data: this.form.value
    });
  }

  cancelCustom() {
  }

}
