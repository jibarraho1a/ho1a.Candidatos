import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { SharedRootModule } from '../../shared-root/shared-root.module';

import { LoginComponent } from './containers/login/login.component';
import { LinkedinComponent } from './containers/linkedin/linkedin.component';
import { FacebookComponent } from './containers/facebook/facebook.component';


export const ROUTES: Routes = [
  { path: '', component: LoginComponent },
  { path: 'linkedin', component: LinkedinComponent },
  { path: 'facebook', component: FacebookComponent }
];

@NgModule({
  imports: [
    ReactiveFormsModule,
    RouterModule.forChild(ROUTES),
    SharedModule,
    SharedRootModule
  ],
  declarations: [
    LoginComponent,
    LinkedinComponent,
    FacebookComponent
  ]
})
export class LoginModule { }
