import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './container/home.component';
import { SharedModule } from '../../shared/shared.module';
import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { Routes, RouterModule } from '@angular/router';

export const routes: Routes = [
  { path: 'mirh/home', component: HomeComponent },
];

@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ButtonsModule,
    SharedModule
  ],
  exports: [
    HomeComponent
  ]
})
export class HomeModule { }
