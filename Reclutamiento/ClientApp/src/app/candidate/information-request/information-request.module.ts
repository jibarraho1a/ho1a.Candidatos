import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule} from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';


// third-party modules
import {MaterialModule} from '../../material.module';
import {CurrencyMaskModule} from 'ng2-currency-mask';
import {FilterPipeModule} from 'ngx-filter-pipe';

// shared modules
import { SharedModule } from '../shared/shared.module';
import { SharedRootModule } from '../../shared-root/shared-root.module';

// components
import { FormInformationRequestComponent } from './components/form-information-request/form-information-request.component';

// containers
import { InformationRequestComponent } from './containers/information-request/information-request.component';
import { ModalNewIncomeComponent } from './components/modal-new-income/modal-new-income.component';
import { ModalNewBenefitComponent } from './components/modal-new-benefit/modal-new-benefit.component';
import { ModalNewWorkReferenceComponent } from './components/modal-new-work-reference/modal-new-work-reference.component';
import { ModalNewPersonalReferenceComponent } from './components/modal-new-personal-reference/modal-new-personal-reference.component';

export const ROUTES: Routes = [
  { path: '', component: InformationRequestComponent },
];

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule.forChild(ROUTES),
    SharedModule,
    SharedRootModule,
    MaterialModule,
    CurrencyMaskModule,
    FilterPipeModule
  ],
  declarations: [
    InformationRequestComponent,
    FormInformationRequestComponent,
    ModalNewIncomeComponent,
    ModalNewBenefitComponent,
    ModalNewWorkReferenceComponent,
    ModalNewPersonalReferenceComponent
  ]
})
export class InformationRequestModule { }
