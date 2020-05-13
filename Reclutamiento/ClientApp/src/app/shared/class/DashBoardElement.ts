import {Observable} from 'rxjs/Observable';

export abstract class DashboardElement {

  loading = false;
  errorMensage: string;
  idSelected: number;
  observer: any;
  itemChange: any;

  init() {
    this.initAction();
    this.getDatasInit().subscribe(
      next => { },
      error => this.handlerError(error),
      () => this.completeAction()
    );
  }

  initAction() {
    this.loading = true;
  }

  getDatasInit() {
    const request = new Observable((observer) => {
      this.observer = observer;
      this.getDashboard();
    });
    return request;
  }

  abstract getDashboard();

  handlerError(error: string) {
    this.errorMensage = error;
    this.alertService();
    this.completeAction();
  }

  abstract alertService();

  completeAction() {
    this.loading = false;
  }

}
