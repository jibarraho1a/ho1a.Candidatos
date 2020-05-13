import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

// third-party modules


// components


// services
import {InformationRequestService} from './services/information-request.service';
import { FilterDetailCandidatePipe } from './pipes/filter-detail-candidate.pipe';

// pipes


@NgModule({
  imports: [
    CommonModule,
    RouterModule,
  ],
  declarations: [FilterDetailCandidatePipe]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers: [
        InformationRequestService,
      ]
    };
  }
}
