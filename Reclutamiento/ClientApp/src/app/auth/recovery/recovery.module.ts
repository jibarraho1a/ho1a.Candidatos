import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
/*import { SharedRootModule } from '../../shared-root/shared-root.module';*/

import { RecoveryComponent } from './containers/recovery/recovery.component';

export const ROUTES: Routes = [
  { path: '', component: RecoveryComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(ROUTES),
    SharedModule,
/*    SharedRootModule*/
    ReactiveFormsModule
  ],
  declarations: [
    RecoveryComponent
  ]
})
export class RecoveryModule { }
