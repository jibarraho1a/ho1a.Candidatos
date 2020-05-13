import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { User } from 'src/app/shared/models/user';

@Component({
  selector: 'ho1a-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {

  @Input()
  user: User;

  @Output()
  logout = new EventEmitter<any>();

  @Output()
  toggled = new EventEmitter<any>();

  @Output()
  toggledsm = new EventEmitter<any>();

  logoutUser() {
    this.logout.emit();
  }

  toggle() {
    this.toggled.emit();
  }

  togglesm() {
    this.toggledsm.emit();
  }

}
