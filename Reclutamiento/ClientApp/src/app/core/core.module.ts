import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';

// third-party modules
import { MaterialModule } from '../material.module';

// features modules
import { AuthModule } from '../auth/auth.module';
import { CandidateModule } from '../candidate/candidate.module';

// containers
import { ShellComponent } from './containers/shell/shell.component';

// components
import { AlertComponent } from './components/alert/alert.component';

// routes
export const ROUTES: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'auth' },
/*{ path: '**', component: PageNotFoundComponent }*/
];

@NgModule({
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(ROUTES),
    MaterialModule,
    AuthModule,
    CandidateModule,
  ],
  declarations: [
    ShellComponent,
    AlertComponent
  ],
  exports: [
    ShellComponent
  ]
})
export class CoreModule { }
