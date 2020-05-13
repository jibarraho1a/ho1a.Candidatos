
import { Routes } from '@angular/router';

import { NotAuthorizedComponent } from './core/container/components/not-authorized/not-authorized.component';
import { PageNotFoundComponent } from './core/container/components/page-not-found/page-not-found.component';


export const appRoutes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'mirh/home' },
  { path: 'notauthorized', component: NotAuthorizedComponent },
  { path: '**', component: PageNotFoundComponent }
];
