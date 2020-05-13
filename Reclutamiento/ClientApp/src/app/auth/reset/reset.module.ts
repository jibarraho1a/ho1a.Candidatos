import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
/*import { SharedRootModule } from '../../shared-root/shared-root.module';*/

import { ResetComponent } from './containers/reset/reset.component';

export const ROUTES: Routes = [
  { path: '', component: ResetComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(ROUTES),
    SharedModule,
/*    SharedRootModule*/
    ReactiveFormsModule
  ],
  declarations: [
    ResetComponent
  ]
})
export class ResetModule { }
