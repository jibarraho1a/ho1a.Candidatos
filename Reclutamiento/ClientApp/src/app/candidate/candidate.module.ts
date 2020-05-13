import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// shared modules
import { SharedModule } from './shared/shared.module';

// guards
import { AuthGuard } from '../auth/shared/guards/auth.guard';

export const ROUTES: Routes = [
  { path: 'ho1a/candidate', loadChildren: './information-request/information-request.module#InformationRequestModule' },
];

@NgModule({
  imports: [
    RouterModule.forChild(ROUTES),
    SharedModule.forRoot()
  ]
})
export class CandidateModule { }
