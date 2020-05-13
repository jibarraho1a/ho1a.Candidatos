import { Component, ViewChild, TemplateRef } from '@angular/core';
import { Store } from '@ngxs/store';
import { Loader } from 'src/app/store/actions/loader.actions';
import { ToasterService } from 'src/app/shared/toaster/toaster.service';

@Component({
  selector: 'ho1a-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  constructor(private store: Store, private toastr: ToasterService) {
  }

  showDefault() {
    this.toastr.show('info', 'hello there');
  }

  showSuccess() {
    this.toastr.show('success', 'This was successful');
  }

  showInfo() {
    this.store.dispatch(new Loader.Toggle);
  }

  showError() {
    this.toastr.show('error', 'This is an error');
  }

  showWarning() {
    this.toastr.show('warning', 'Warning crazy error bien pinche largo que no tiene nada que ver con lo que esta pasando pero estaria chido ver que tanto puede aguantar');
  }
}
