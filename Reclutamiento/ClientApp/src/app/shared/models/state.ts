import { Alert } from './alert';
import { User } from './user';
import { EventApp } from './event-app';

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
  emailSearch: boolean;

  [key: string]: any;
}
