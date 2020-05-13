import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { SideNavComponent } from './components/side-nav/side-nav.component';
import { NotAuthorizedComponent } from './components/not-authorized/not-authorized.component';
import { TitleBarComponent } from './components/title-bar/title-bar.component';
import { ContainerComponent } from './container.component';
import { CoreModule } from '../core.module';
import { RecruitModule } from '../../recruit/recruit.module';
import { NotificationButtonComponent } from './components/notification-button/notification-button.component';
import { MaterialModule } from 'src/app/material.module';
import { PopupModule } from '@progress/kendo-angular-popup';
import { LoaderComponent } from './components/loader/loader.component';

@NgModule({
  declarations: [
    HeaderComponent,
    PageNotFoundComponent,
    SideNavComponent,
    NotAuthorizedComponent,
    TitleBarComponent,
    ContainerComponent,
    NotificationButtonComponent,
    LoaderComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    RouterModule,
    MaterialModule,
    CoreModule,
    RecruitModule,
    PopupModule,
    BrowserAnimationsModule,
  ],
  exports: [
    ContainerComponent
  ],

})
export class ContainerModule { }
