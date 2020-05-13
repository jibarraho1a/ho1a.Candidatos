import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './components/login/login.component';

@NgModule({
  declarations: [
      LoginComponent
  ],
  imports: [
      CommonModule,
      FormsModule,
      ReactiveFormsModule,
  ],
  exports: [
      LoginComponent
  ]
})
export class AuthModule {
}
