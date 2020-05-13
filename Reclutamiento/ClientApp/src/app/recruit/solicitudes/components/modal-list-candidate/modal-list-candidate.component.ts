import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { ModalElement } from '../../../../shared/class/ModalElement';


@Component({
  selector: 'ho1a-modal-list-candidate',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './modal-list-candidate.component.html',
  styleUrls: ['./modal-list-candidate.component.scss']
})
export class ModalListCandidateComponent extends ModalElement implements OnInit {

  @Input()
  data: any[];

  dataFiltered: any[];

  candidates = [];

  constructor() {
    super();
  }

  puestos: any[];
  estatus: any[];

  ngOnInit() {
    this.puestos = [];
    this.estatus = [];
    this.data.forEach( (item, index) => {
      if (index === 0) {
        this.puestos.push( <any>{text: item.puesto, value: item.puesto});
        this.estatus.push( <any>{text: item.status, value: item.status});
        return;
      }
      if (this.puestos.findIndex( (f: any) => f.value === item.puesto) < 0) {
        this.puestos.push(<any>{text: item.puesto, value: item.puesto});
      }
      if (this.estatus.findIndex( (f: any) => f.value === item.status) < 0) {
        this.estatus.push(<any>{text: item.status, value: item.status});
      }
    });
    this.form = this.fb.group({
      filtro: [null],
      estatu: [null],
      puesto: [null]
      }
    );
    this.dataFiltered = Object.assign([], this.data);
    this.form.get('filtro').valueChanges.subscribe(value => {
      if (value && value.toString().trim().length === 0) {
        return;
      }
      const filtro = value.toString().toUpperCase();
      const clone = [...[], ...this.data];
      this.dataFiltered = clone.filter( candidate =>
        this.ifFoundCandidate(candidate, filtro) &&
        this.ifFound(this.puestosSelected || [], candidate.puesto) &&
        this.ifFound(this.estatusSelected || [], candidate.status)
      );
    });
    this.form.get('puesto').valueChanges.subscribe( (values: string[]) => {
      const clone = [...[], ...this.data];
      this.dataFiltered = clone.filter( candidate =>
        this.ifFoundCandidate(candidate, (this.filtroSelected || '').toUpperCase()) &&
        this.ifFound(values, candidate.puesto) &&
        this.ifFound(this.estatusSelected || [], candidate.status)
      );
    });
    this.form.get('estatu').valueChanges.subscribe((values: string[]) => {
      const clone = [...[], ...this.data];
      this.dataFiltered = clone.filter( candidate =>
        this.ifFoundCandidate(candidate, (this.filtroSelected || '').toUpperCase()) &&
        this.ifFound(values, candidate.status) &&
        this.ifFound(this.puestosSelected || [], candidate.puesto)
      );
    });
  }

  onAcceptCustom() {
    this.accept.emit(this.dataFiltered.filter( f => f.checked));
  }

  canAdd() {
    const candidates = this.dataFiltered.filter( f => f.checked);
    const can = candidates && (candidates.length > 0 && candidates.length < 4);
    return can ? true : false;
  }

  onKeydown(event) {
    if (event.key === 'Enter') {
      return false;
    }
  }

  ifFound(values: string[], value: string) {
    return values.length === 0 || values.find( f => f === value) !== undefined;
  }

  ifFoundCandidate(candidate: any, value: string) {
    const completeName = ((candidate.nombre || '') + ' ' + (candidate.materno || '') + ' ' + (candidate.paterno || '')).toUpperCase();
    return completeName.indexOf(value) > -1;
  }

  get estatusSelected() {
    return this.form.get('estatu').value;
  }

  get puestosSelected() {
    return this.form.get('puesto').value;
  }

  get filtroSelected() {
    return this.form.get('filtro').value;
  }

}
