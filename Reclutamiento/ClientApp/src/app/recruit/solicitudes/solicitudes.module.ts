import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MenuSolicitudComponent } from './containers/menu-solicitud/menu-solicitud.component';
import { NuevaSolicitudComponent } from './containers/nueva-solicitud/nueva-solicitud.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { MaterialModule } from '../../material.module';

import { FilterPipeModule} from 'ngx-filter-pipe';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { LoadingComponent } from './components/loading/loading.component';
import { FormRequestPlaceComponent } from './components/form-request-place/form-request-place.component';
import { StepRequestComponent } from './components/step-request/step-request.component';
import { StepAssignmentComponent } from './components/step-assignment/step-assignment.component';
import { StepSearchComponent } from './components/step-search/step-search.component';
import { StepTernaComponent } from './components/step-terna/step-terna.component';
import { StepInterviewComponent } from './components/step-interview/step-interview.component';
import { StepInterviewResultComponent } from './components/step-interview-result/step-interview-result.component';
import { StepEconomicOfferComponent } from './components/step-economic-offer/step-economic-offer.component';
import { StepAltaComponent } from './components/step-alta/step-alta.component';
import { StepExpedientComponent } from './components/step-expedient/step-expedient.component';
import { AccordionTitleCandidateComponent } from './components/accordion-title-candidate/accordion-title-candidate.component';
import { StepSearchCandidateCustomComponent } from './components/step-search-candidate-custom/step-search-candidate-custom.component';
import { FormGeneralDataComponent } from './components/form-general-data/form-general-data.component';
import { ModalComplementoCvComponent } from './components/modal-complemento-cv/modal-complemento-cv.component';
import { ModalCvComponent } from './components/modal-cv/modal-cv.component';
import { ModalListCandidateComponent } from './components/modal-list-candidate/modal-list-candidate.component';
import { ModalNewCandidateComponent } from './components/modal-new-candidate/modal-new-candidate.component';
import { ModalDatetimeComponent } from './components/modal-datetime/modal-datetime.component';
import { StepTernaListComponent } from './components/step-terna-list/step-terna-list.component';
import { ModalTemplateCompetenciesComponent } from './components/modal-template-competencies/modal-template-competencies.component';
import { StepInterviewCandidateCustomComponent } from './components/step-interview-candidate-custom/step-interview-candidate-custom.component';
import { ModalInterviewFormatComponent } from './components/modal-interview-format/modal-interview-format.component';
import { StepInterviewResultRowComponent } from './components/step-interview-result-row/step-interview-result-row.component';
import { StepInterviewButtomComponent } from './components/step-interview-buttom/step-interview-buttom.component';
import { ModalListCandidateRowComponent } from './components/modal-list-candidate-row/modal-list-candidate-row.component';
import { SolicitudesComponent } from './containers/solicitudes/solicitudes.component';

export const routes: Routes = [
  { path: '', component: MenuSolicitudComponent },
  { path: 'new', component: NuevaSolicitudComponent },
  { path: 'search', component: SolicitudesComponent },

];

@NgModule({
  declarations: [
    MenuSolicitudComponent,
    NuevaSolicitudComponent,
    LoadingComponent,
    FormRequestPlaceComponent,
    StepRequestComponent,
    StepAssignmentComponent,
    StepSearchComponent,
    StepTernaComponent,
    StepInterviewComponent,
    StepInterviewResultComponent,
    StepEconomicOfferComponent,
    StepAltaComponent,
    StepExpedientComponent,
    StepTernaComponent,
    StepTernaListComponent,
    StepInterviewCandidateCustomComponent,
    StepInterviewResultComponent,
    StepInterviewResultRowComponent,
    StepInterviewButtomComponent,
    AccordionTitleCandidateComponent,
    StepSearchCandidateCustomComponent,
    FormGeneralDataComponent,
    FormRequestPlaceComponent,
    ModalComplementoCvComponent,
    ModalCvComponent,
    ModalInterviewFormatComponent,
    ModalListCandidateComponent,
    ModalListCandidateRowComponent,
    ModalNewCandidateComponent,
    ModalDatetimeComponent,
    ModalTemplateCompetenciesComponent,
    SolicitudesComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    SharedModule,
    CurrencyMaskModule,
    FilterPipeModule,
    MaterialModule
  ]
})
export class SolicitudesModule { }
