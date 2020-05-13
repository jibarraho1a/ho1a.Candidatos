import { State } from '../models/state';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs/behaviorSubject';

import 'rxjs/add/operator/pluck';
import 'rxjs/add/operator/distinctUntilChanged';

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
  userSearch: undefined,
  emailSearch: undefined
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
