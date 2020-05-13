import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import 'rxjs/add/operator/pluck';
import 'rxjs/add/operator/distinctUntilChanged';

import { Alert, User } from './shared-root/models';

export enum EnumTypeEvent {
  RemovingRequisition = 'removingRequisition',
  SavingRequisition = 'savingRequisition',
  SavingBenefitLastJob = 'savingBenefitLastJob',
  DeletingBenefitLastJob = 'deletingBenefitLastJob',
  SavingIncomeLastJob = 'savingIncomeLastJob',
  DeletingIncomeLastJob = 'deletingIncomeLastJob',
  SavingPersonalReference = 'savingPersonalReference',
  DeletingPersonalReference = 'deletingPersonalReference',
  SavingWorkReference = 'savingWorkReference',
  DeletingWorkReference = 'deletingWorkReference',
  UploadingFile = 'uploadingFile',
  DeletingFile = 'deletingFile',
  NotifyingLoadFullInformation = 'notifyingLoadFullInformation'
}

export interface EventApp {
  typeEvent: EnumTypeEvent;
  data: any;
}

export enum StateOption {
  alert = 'alert',
  user = 'user',
  alertRequisition = 'alertRequisition',
  chiefs = 'chiefs',
  filters = 'filters',

  eventsRequisition = 'eventsRequisition',
  userSearch = 'userSearch'
}

export interface State {
  alert: Alert;
  user: User;
  requisitions: any[];
  configurations: any[];
  loadingRequisitions: boolean;
  loadingConfigurations: boolean;
  alertRequisition: Alert;
  chiefs: any[];
  filters: any;

  eventsRequisition: EventApp;
  userSearch: User;

  [key: string]: any;
}

const state: State = {
  alert: undefined,
  user: undefined,
  requisitions: undefined,
  configurations: undefined,
  loadingRequisitions: undefined,
  loadingConfigurations: undefined,
  alertRequisition: undefined,
  chiefs: undefined,
  filters: undefined,

  eventsRequisition: undefined,
  userSearch: undefined
};

export class Store {

  private subject = new BehaviorSubject<State>(state);
  private store = this.subject.asObservable().distinctUntilChanged();

  get value() {
    return this.subject.value;
  }

  select<T>(name: string): Observable<T> {
    return this.store.pluck(name);
  }

  set(name: string, state: any) {
    this.subject.next(<State>{...this.value, [name]: state} );
  }

}
