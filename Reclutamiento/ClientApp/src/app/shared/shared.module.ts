import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { ModalInputComponent } from './components/modal-input/modal-input.component';
import { UploadFilesComponent } from './components/upload-files/upload-files.component';
import { ListItemComponent } from './components/list-item/list-item.component';
import { FilterComponent } from './components/filter/filter.component';
import { ExportComponent } from './components/export/export.component';
import { FilterCategoriesComponent } from './components/filter-categories/filter-categories.component';
import { StatusComponent } from './components/status/status.component';
import { AuthorizationMatrixComponent } from './components/authorization-matrix/authorization-matrix.component';
import { WizardComponent } from './components/wizard/wizard.component';
import { ButtonActionConfirmationComponent } from './components/buttom-action-confirmation/button-action-confirmation.component';
import { HighlightedBoxesComponent } from './components/highlighted-boxes/highlighted-boxes.component';
import { CardAccordionComponent } from './components/card-accordion/card-accordion.component';
import { CardAccordionSectionComponent } from './components/card-accordion-section/card-accordion-section.component';
import { ButtomPopoverComponent } from './components/buttom-popover/buttom-popover.component';
import { ModalFormComponent } from './components/modal-form/modal-form.component';
import { ButtomActionConfirmationInputboxComponent } from './components/buttom-action-confirmation-inputbox/buttom-action-confirmation-inputbox.component';
import { RatingComponent } from './components/rating/rating.component';
import { TemplateLandingComponent } from './components/template-landing/template-landing.component';
import { CollaboratorSearchEngineComponent } from './components/collaborator-search-engine/collaborator-search-engine.component';
import { CardAccordionTitleComponent } from './components/card-accordion-title/card-accordion-title.component';
import { CardAccordionSectionTitleComponent } from './components/card-accordion-section-title/card-accordion-section-title.component';
import { MaterialModule } from '../material.module';


@NgModule({
  declarations: [
    ConfirmationComponent,
    ModalInputComponent,
    UploadFilesComponent,
    ListItemComponent,
    FilterComponent,
    ExportComponent,
    FilterCategoriesComponent,
    StatusComponent,
    AuthorizationMatrixComponent,
    WizardComponent,
    ButtonActionConfirmationComponent,
    HighlightedBoxesComponent,
    CardAccordionComponent,
    CardAccordionSectionComponent,
    ButtomPopoverComponent,
    ModalFormComponent,
    ButtomActionConfirmationInputboxComponent,
    RatingComponent,
    TemplateLandingComponent,
    CollaboratorSearchEngineComponent,
    CardAccordionTitleComponent,
    CardAccordionSectionTitleComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MaterialModule
  ],
  exports: [
    ConfirmationComponent,
    ModalInputComponent,
    UploadFilesComponent,
    ListItemComponent,
    FilterComponent,
    ExportComponent,
    FilterCategoriesComponent,
    StatusComponent,
    AuthorizationMatrixComponent,
    WizardComponent,
    ButtonActionConfirmationComponent,
    HighlightedBoxesComponent,
    CardAccordionComponent,
    CardAccordionSectionComponent,
    ButtomPopoverComponent,
    ModalFormComponent,
    ButtomActionConfirmationInputboxComponent,
    RatingComponent,
    TemplateLandingComponent,
    CollaboratorSearchEngineComponent,
    CardAccordionTitleComponent,
    CardAccordionSectionTitleComponent
  ],
})
export class SharedModule { }
