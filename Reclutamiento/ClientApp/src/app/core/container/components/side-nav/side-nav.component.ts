import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'ho1a-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss']
})
export class SideNavComponent {

  @Input()
  user: any;

  @Input()
  menu: any;

  @Input()
  specialPermission: boolean;

  @Output()
  logout = new EventEmitter<any>();

  @Output()
  toggledsm = new EventEmitter<any>();

  @Output()
  clickMenu = new EventEmitter<any>()

  constructor() {}

  canViewSection(item: any) {
    return item.specialPermission === true && this.specialPermission === true ||
           item.specialPermission === false;
  }

  logoutUser() {
    this.logout.emit();
  }
  
  togglesm() {
    this.toggledsm.emit();
  }

  onClickMenu(menu: any) {
    this.clickMenu.emit(menu);
  }

}
