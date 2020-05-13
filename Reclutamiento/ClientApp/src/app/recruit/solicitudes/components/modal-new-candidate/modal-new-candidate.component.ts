import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  OnChanges,
  OnDestroy,
  OnInit,
  Renderer2,
  SimpleChanges,
  ViewChild
} from '@angular/core';
import {Validators} from '@angular/forms';
import {Observable, Subscription} from 'rxjs';
import { ModalForm } from '../../../../shared/class/ModalForm';
import { Store } from '../../../../shared/class/Store';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { EventApp } from '../../../../shared/models/event-app';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';

@Component({
  selector: 'ho1a-modal-new-candidate',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './modal-new-candidate.component.html',
  styleUrls: ['./modal-new-candidate.component.scss']
})
export class ModalNewCandidateComponent extends ModalForm implements OnInit, OnChanges, OnDestroy {

  @ViewChild('email', {static: false}) emailElement: ElementRef;

  butttom = [];

  existingMail$ = new Observable<any>();
  existMail = undefined;
  readOnlyMail = false;

  subscriptions: Subscription[] = [];

  constructor(
    private store: Store,
    private renderer: Renderer2
  ) {
    super();
  }

  ngOnInit() {
    this.form = this.fb.group({
      nombre: [null, [Validators.required]],
      paterno: [null, [Validators.required]],
      materno: [null, [Validators.required]],
      email: [null, [Validators.required, Validators.email] ],
    });
    this.form.get('email').valueChanges.subscribe(value => {
      this.existMail = undefined;
    });
    this.existingMail$ = this.store.select<boolean>(StateOption.emailSearch);
    this.subscriptions = [
      this.existingMail$.subscribe( next => this.configureSubscriptions(next) )
    ];
  }

  configureSubscriptions(next: any) {
    this.existMail = next;
    this.readOnlyMail = !(this.existMail === true || this.existMail === undefined) || this.existMail === false;
    if (this.existMail === false) {
      this.butttom = [
        {name: 'Guardar', value: 'Guardar', submit: true, class: 'save'}
      ];
    }
    if (this.emailElement) {
      this.renderer.selectRootElement(this.emailElement.nativeElement).click();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.changes(changes);
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  clickButtomCustom() {
    this.store.set(StateOption.eventsRequisition,
      <EventApp>{
        typeEvent: TypeEvent.AddingCandidate,
        data: this.form.value
      });
    this.resetEmailSearch();
  }

  cancelCustom() {
    this.resetEmailSearch();
  }

  resetEmailSearch() {
    this.store.set(StateOption.emailSearch, undefined);
  }

  onSearchEmail() {
    const email = this.form.get('email');
    if (!this.showAsRequired('email') && email && email.value && email.value.toString().length > 0) {
      this.store.set(StateOption.eventsRequisition,
        <EventApp>{
          typeEvent: TypeEvent.VerifyingCandidateMail,
          data: email.value
        });
    }
  }

  onKeydown(event: any) {
    if (event.keyCode === 13 && !this.showAsRequired('email') && (this.existMail || this.existMail === undefined) ) {
      this.onSearchEmail();
    }
  }

}
