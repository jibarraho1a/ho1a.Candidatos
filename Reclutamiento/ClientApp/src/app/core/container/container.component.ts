import { Component } from '@angular/core';
import { Menu } from '../../shared/models/menu';
import { User } from '../../shared/models/user';
import { TitleBar } from '../../shared/models/title-bar';
import { UserService } from '../../shared/services/user.service';
import { AuthService } from '../../auth/services/auth.service';

@Component({
  selector: 'ho1a-container',
  templateUrl: './container.component.html',
  styleUrls: ['./container.component.scss']
})
export class ContainerComponent {
  user$: User;
  menus: Menu[];
  toggled = false;
  toggledSm = false;

  titleBar: TitleBar = {
    name: 'Solicitud de Plazas',
    breadcrumbs: [
      'Inicio',
      'Solicitud de Plazas'
    ]
  };

  constructor(private authService: AuthService,
              private userService: UserService) {
    this.user$ = this.userService.getUserMock();
    this.menus = this.userService.getMenuMock(true, true, true, true);
  }

  onToggled() {
    console.log(this.toggled);
    this.toggled = !this.toggled;
  }

  onToggledSm() {
    console.log(this.toggled);
    this.toggledSm = !this.toggledSm;
  }

  onLogout() {
    this.authService.logoutUser();
  }

}
