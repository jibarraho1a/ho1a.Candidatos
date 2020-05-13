import { Injectable } from '@angular/core';
import {Router, CanActivate} from '@angular/router';

import 'rxjs/add/operator/map';

import { Store } from '../../../store';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private store: Store
  ) {}

  canActivate() {
    const user = JSON.parse(localStorage.getItem('user'));
    const userIsAuthenticated = !(!user || (user && !user.authenticated));
    if (!userIsAuthenticated) {
      this.router.navigate(['/auth/login']);
    }
    this.store.set('user', user);
    return userIsAuthenticated;
  }

}
