import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
/*import { SharedRootModule } from '../../shared-root/shared-root.module';*/

import { RegisterComponent } from './containers/register/register.component';

export const ROUTES: Routes = [
  { path: '', component: RegisterComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(ROUTES),
    SharedModule,
/*    SharedRootModule*/
    ReactiveFormsModule
  ],
  declarations: [
    RegisterComponent
  ]
})
export class RegisterModule { }
