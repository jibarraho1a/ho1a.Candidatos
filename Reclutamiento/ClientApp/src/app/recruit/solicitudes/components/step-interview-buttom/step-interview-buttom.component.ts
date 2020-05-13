import {
  ChangeDetectionStrategy,
  Component,
  ElementRef, EventEmitter,
  Input,
  OnDestroy,
  OnInit, Output, Renderer2,
  ViewChild
} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Observable, Subscription} from 'rxjs';
import { Store } from '../../../../shared/class/Store';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { EventApp } from '../../../../shared/models/event-app';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';


@Component({
  selector: 'ho1a-step-interview-buttom',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-interview-buttom.component.html',
  styleUrls: ['./step-interview-buttom.component.scss']
})
export class StepInterviewButtomComponent implements OnInit, OnDestroy {

  @ViewChild('userName', {static: false}) userNameElement: ElementRef;

  @Input()
  data: any;

  @Input()
  enableEdition = false;

  @Output()
  userNameSelected = new EventEmitter();

  form: FormGroup;

  buttomsPopover = [
    {name: 'Cancelar', value: false, class: 'btn-primary'}
  ];

  userSearch$ = new Observable<any>();

  result = '';

  subscriptions: Subscription[] = [];

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private renderer: Renderer2
  ) { }

  ngOnInit() {
    this.configureForm();
    this.userSearch$ = this.store.select<any>(StateOption.userSearch).distinctUntilChanged();
    this.subscriptions = [
      this.userSearch$.subscribe( next => this.configureSubscriptions(next) )
    ];
  }

  configureSubscriptions(next: any) {
    this.result = '';
    if ( (next && next.nivelCompetencia >= 4 && this.userName.value )
          ||
         (next && this.data.tipo === 'asociarUsuarioDominio' && this.userName.value) ) {
      this.nameComplete.setValue(next.nombre + ' ' + next.apellidos);
      this.buttomsPopover = [
        {name: 'Agregar', value: true, class: 'btn-success'},
        {name: 'Cancelar', value: false, class: 'btn-primary'}
      ];
    } else {
      if (next && next.nivelCompetencia < 4) {
        this.result = 'Su nivel debe ser igual o mayor a N4';
      }
      this.nameComplete.setValue(null);
      this.buttomsPopover = [
        {name: 'Cancelar', value: false, class: 'btn-primary'}
      ];
    }
    if (this.userNameElement) {
      this.renderer.selectRootElement(this.userNameElement.nativeElement).click();
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  configureForm() {
    this.form = this.fb.group({
      userName: [ {value: '', disabled: false}, Validators.required ],
      nameComplete: [ {value: '', disabled: false}, Validators.required ]
    });
  }

  onClickButtom(event: any) {
    if (this.form.valid) {
      if (this.data.tipo === 'nuevoEntrevistador') {
        this.store.set(StateOption.eventsRequisition,
          <EventApp>{
            typeEvent: TypeEvent.AddingGuestCandidateInterviewRequisition,
            data: {
              id: this.data.id,
              value: {
                entrevistadorUserName: this.userName.value
              }
            }
          });
      } else if (this.data.tipo === 'asociarUsuarioDominio') {
        this.store.set(StateOption.eventsRequisition,
          <EventApp>{
            typeEvent: TypeEvent.AssociatingDomainUserWithNewCollaborator,
            data: this.userName.value
          });
      }
      this.userNameSelected.emit(this.userName.value);
    }
  }

  onToggleClick(event: any) {
    this.form.reset();
    this.result = '';
    this.buttomsPopover = [
      {name: 'Cancelar', value: false, class: 'btn-primary'}
    ];
  }

  onSearchCollaboratorByUserName() {
    if (this.userName.value !== null && this.userName.value.toString().trim().length > 0) {
        this.store.set(StateOption.eventsRequisition,
          <EventApp>{
            typeEvent: TypeEvent.LookingForACollaboratorByUsername,
            data: this.userName.value
          });
    }
  }

  onKeydown(event: any) {
    if (event.keyCode === 13) {
      this.onSearchCollaboratorByUserName();
    }
  }

  get userName() {
    return this.form.get('userName');
  }

  get nameComplete() {
    return this.form.get('nameComplete');
  }

}
