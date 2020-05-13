import { NgModule } from '@angular/core';
import { ContainerModule } from './core/container/container.module';
import { ContainerComponent } from './core/container/container.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxsModule } from '@ngxs/store';
import { environment } from 'src/environments/environment';
import { LoaderState } from './store/state/loader.state';
import { NgxsLoggerPluginModule } from '@ngxs/logger-plugin';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';
import { RouterModule } from '@angular/router';
import { appRoutes } from './app-routing.module';
import { NotificacionState } from './store/state/notificaciones.state';
import { MaterialModule } from './material.module';
import { JwtModule } from '@auth0/angular-jwt';
import { Store } from './shared/class/Store';


export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  imports: [
    ContainerModule,
    RouterModule.forRoot(appRoutes),
    NgxsModule.forRoot([
      LoaderState,
      NotificacionState,
    ], { developmentMode: !environment.production }),
    NgxsLoggerPluginModule.forRoot(),
    NgxsReduxDevtoolsPluginModule.forRoot(),
    MaterialModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['https://localhost:4200/'],
        blacklistedRoutes: [],
      }
    }),
  ],
  providers: [Store],
  bootstrap: [ContainerComponent]
})
export class AppModule { }
