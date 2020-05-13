import { NgModule, APP_INITIALIZER } from '@angular/core';

import { Store } from '../app/store';

// core module
import { CoreModule } from './core/core.module';

// component
import { ShellComponent } from './core/containers/shell/shell.component';
import { EnvironmentService } from './core/service/environment-specific.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SocialLoginModule, AuthServiceConfig } from 'angularx-social-login';
import { GoogleLoginProvider, FacebookLoginProvider, LinkedInLoginProvider } from "angularx-social-login";



const config = new AuthServiceConfig([
  {
    id: FacebookLoginProvider.PROVIDER_ID,
    provider: new FacebookLoginProvider('879364479141277')
  },
  {
    id: GoogleLoginProvider.PROVIDER_ID,
    provider: new GoogleLoginProvider("363525629096-mrb3l1eqovuj8oo30juok0fejlijrm3o.apps.googleusercontent.com")
  }, 
  {
    id: LinkedInLoginProvider.PROVIDER_ID,
    provider: new LinkedInLoginProvider("78rkld9apdut56")
  }
]);

export function provideConfig() {
  return config;
}

@NgModule({
  imports: [
    CoreModule,
    BrowserAnimationsModule,
    SocialLoginModule
  ],
  providers: [
    {
      provide: AuthServiceConfig,
      useFactory: provideConfig
    },
    Store,
    EnvironmentService,
    {
      provide: APP_INITIALIZER,
      useFactory: (svc: EnvironmentService) => () => svc.loadAppConfig(),
      multi: true,
      deps: [EnvironmentService]
    },
  ],
  bootstrap: [
    ShellComponent
  ]
})
export class AppModule { }
