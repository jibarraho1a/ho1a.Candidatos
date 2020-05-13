import { LoadingAction } from '../enums';
import { Subscription, Observable } from 'rxjs';
import { Configuration } from '../models/configuration';

export abstract class EdithElement {

  item: any;
  itemBackground: any = {};
  configuration: Configuration;

  subscriptions: Subscription[] = [];

  id: number;
  itemChange: any;

  loadingStart = false;
  loadingChange = false;

  loadingInTheBackground = false;

  typeLoadingAction = LoadingAction.start;

  completeItem = false;
  completeConfiguration = false;

  errorMensage: string;

  observer: any;

  init(id: number) {
    this.id = id;
    this.typeLoadingAction = LoadingAction.start;
    this.initAction();
    this.getDatasInit().subscribe(
      next => {},
      error => this.handlerError(error),
      () => this.completeAction()
    );
  }

  initAction() {
    if (this.typeLoadingAction === LoadingAction.start) {
      this.loadingStart = true;
    } else if (this.typeLoadingAction === LoadingAction.change) {
      this.loadingChange = true;
    }
  }

  getDatasInit() {
    const request = new Observable((observer) => {
      this.completeItem = false;
      this.completeConfiguration = false;
      this.observer = observer;
      this.getConfigurarionService();
      this.getItemService();
    });
    return request;
  }

  abstract getConfigurarionService();

  abstract getItemService();

  handlerError(error: string) {
    this.errorMensage = error;
    this.alertService();
    this.completeAction();
  }

  abstract alertService();

  completeAction() {
    if (this.typeLoadingAction === LoadingAction.start) {
      this.loadingStart = false;
      if (this.exist) {
        this.loadInformationBackground();
      }
    } else if (this.typeLoadingAction === LoadingAction.change) {
      this.loadingChange = false;
    }
    this.typeLoadingAction = null;
  }

  abstract loadInformationBackground();

  save(item: any) {
    this.itemChange = item;
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.saveService();
  }

  get exist() {
    return this.item && this.item.id > 0;
  }

  abstract saveService();

}
