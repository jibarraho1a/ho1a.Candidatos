import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeModule } from './home/home.module';
import { SolicitudesModule } from './solicitudes/solicitudes.module';
import { MaterialModule } from '../material.module';

export const appRoutes: Routes = [
  { path: 'mirh/home', loadChildren: () => import('./home/home.module').then(m => m.HomeModule)},
  { path: 'mirh/solicitud', loadChildren: () => import('./solicitudes/solicitudes.module').then(m => m.SolicitudesModule) },
];

@NgModule({
  imports: [
    HomeModule,
    SolicitudesModule,
    RouterModule.forChild(appRoutes),
    MaterialModule
  ]
})
export class RecruitModule { }
