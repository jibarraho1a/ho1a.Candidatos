import {Component, OnDestroy, OnInit} from '@angular/core';
import {InformationRequestService} from '../../../shared/services/information-request.service';
import {ActivatedRoute, Router} from '@angular/router';
import {EdithElement} from '../../../../shared-root/class/EdithElement';
import {LoadingAction} from '../../../../shared-root/enums';
import {Observable, Subscription} from 'rxjs';
import {EnumTypeEvent, EventApp, StateOption, Store} from '../../../../store';
import {catchError} from 'rxjs/operators';

@Component({
  selector: 'ho1a-information-request',
  templateUrl: './information-request.component.html',
  styleUrls: ['./information-request.component.scss']
})
export class InformationRequestComponent extends EdithElement implements OnInit, OnDestroy {

  subscriptions: Subscription[] = [];
  textAlert: string = '';

  constructor(
    private service: InformationRequestService,
    private router: Router,
    private route: ActivatedRoute,
  ) {
    super();
  }

  ngOnInit() {
    this.init(null);
    this.subscriptions = [
      this.service.events$.subscribe( (next: EventApp) => this.configureSubscriptions(next) ),
    ];
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  configureSubscriptions(next: EventApp) {
    if (!next || !this.exist) {
      return;
    }
    switch (next.typeEvent) {
      case EnumTypeEvent.SavingRequisition:
        this.textAlert = 'Información guardada correctamente';
        this.save(next.data);
        break;
      case EnumTypeEvent.SavingBenefitLastJob:
        this.saveBenefitLastJob(next.data);
        break;
      case EnumTypeEvent.SavingIncomeLastJob:
        this.saveIncomeLastJob(next.data);
        break;
      case EnumTypeEvent.SavingWorkReference:
        this.saveWorkReference(next.data);
        break;
      case EnumTypeEvent.SavingPersonalReference:
        this.savePersonalReference(next.data);
        break;
      case EnumTypeEvent.DeletingBenefitLastJob:
        this.deleteBenefitLastJob(next.data);
        break;
      case EnumTypeEvent.DeletingIncomeLastJob:
        this.deleteIncomeLastJob(next.data);
        break;
      case EnumTypeEvent.DeletingWorkReference:
        this.deleteWorkReference(next.data);
        break;
      case EnumTypeEvent.DeletingPersonalReference:
        this.deletePersonalReference(next.data);
        break;
      case EnumTypeEvent.UploadingFile:
        this.uploadFile(next.data);
        break;
      case EnumTypeEvent.DeletingFile:
        this.deleteFile(next.data);
        break;
      case EnumTypeEvent.NotifyingLoadFullInformation:
        this.notifyLoadFullInformation(next.data);
        break;
    }
  }

  alertService() {
    this.service.alertError(this.errorMensage);
  }

  completeActionCustom() {
    this.service.sendAlert(this.textAlert);
    this.textAlert = '';
  }

  getConfigurarionService() {
    this.completeConfiguration = true;
  }

  getItemService() {
    return this.service.get().subscribe(
      next => this.item = next,
      error => this.observer.error(error),
      () => {
        this.completeItem = true;
        if (this.completeConfiguration) {
          this.observer.complete();
        }
        if (this.item && this.item.detalleStatusCapturaInformacion >= 3) {
          this.getConfigurarion(5);
        } else {
          this.getConfigurarion(4);
        }
      }
    );
  }

  saveService() {
    return this.service.save(this.item).subscribe(
      next => this.item = next,
      error => this.observer.error(error),
      () => this.completeAction()
    );
  }

  getConfigurarion(pantalla: number) {
    this.service.getConfigurations(pantalla).subscribe(
      next => this.configuration = next,
      error => console.log(error),
    );
  }


  loadInformationBackground() {
    this.getCatalogs();
    this.getFilesByCandidate();
    this.getInformacionComplementariaSitio();
  }

  getCatalogs() {
    return this.service.getCatalogs().subscribe(
      next => {
        const data = {...{}, ...this.itemBackground};
        data['catalogs'] = next;
        this.itemBackground = data;
      },
      error => console.log(error),
    );
  }

  saveBenefitLastJob(benefit: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveBenefitLastJob(this.item, benefit).subscribe(
      next => this.item = next,
      error => this.observer.error(error),
      () => this.completeAction()
    );
  }

  saveIncomeLastJob(income: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveIncomeLastJob(this.item, income).subscribe(
      next => this.item = next,
      error => this.observer.error(error),
      () => this.completeAction()
    );
  }

  saveWorkReference(workReference: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.saveWorkReference(this.item, workReference).subscribe(
      next => this.item = next,
      error => this.observer.error(error),
      () => this.completeAction()
    );
  }

  savePersonalReference(personalReference: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.savePersonalReference(this.item, personalReference).subscribe(
      next => this.item = next,
      error => this.observer.error(error),
      () => this.completeAction()
    );
  }

  deleteBenefitLastJob(benefit: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.deleteBenefitLastJob(benefit).subscribe(
      next => this.item = next,
      error => this.observer.error(error),
      () => this.completeAction()
    );
  }

  deleteIncomeLastJob(income: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.deleteIncomeLastJob(income).subscribe(
      next => this.item = next,
      error => this.observer.error(error),
      () => this.completeAction()
    );
  }

  deleteWorkReference(workReference: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.deleteWorkReference(workReference).subscribe(
      next => this.item = next,
      error => this.observer.error(error),
      () => this.completeAction()
    );
  }

  deletePersonalReference(personalReference: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.deletePersonalReference(personalReference).subscribe(
      next => this.item = next,
      error => this.observer.error(error),
      () => this.completeAction()
    );
  }

  uploadFile(document: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.uploadFile(this.item, document).subscribe(
      next => this.getFilesByCandidate(),
      error => this.observer.error(error),
      () => this.completeAction()
    );
  }

  deleteFile(document: any) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.deleteFile(this.item, document).subscribe(
      next => this.getFilesByCandidate(),
      error => this.observer.error(error),
      () => this.completeAction()
    );
  }

  getFilesByCandidate() {
    return this.service.getFilesByCandidate(this.item).subscribe(
      next => {
        const data = {...{}, ...this.itemBackground};
        data['documents'] = next;
        this.itemBackground = data;
      },
      error => console.log(error),
    );
  }

  notifyLoadFullInformation(estatusCarga: number) {
    this.typeLoadingAction = LoadingAction.change;
    this.initAction();
    this.service.notifyLoadFullInformation(estatusCarga).subscribe(
      next => {
        // console.log(next);
      },
      error => this.observer.error(error),
      () => this.completeAction()
    );
  }

  getInformacionComplementariaSitio() {
    this.service.getInformacionComplementariaSitio().subscribe(
      next => {
        const data = {...{}, ...this.itemBackground};
        data['informacionComplementaria'] = next;
        this.itemBackground = data;
      },
      error => console.log(error),
    );
  }

}
