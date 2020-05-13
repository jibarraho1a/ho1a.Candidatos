import { Component, OnInit } from '@angular/core';

import { Observable } from 'rxjs';

import { Store, StateOption } from '../../../../app/store';

import { Alert, Menu, TitleBar, User } from '../../../shared-root/models';

@Component({
  selector: 'ho1a-root',
  templateUrl: './shell.component.html',
  styleUrls: ['./shell.component.scss']
})
export class ShellComponent implements OnInit {

  alert$: Observable<Alert>;

  constructor(
    private store: Store
  ) {
  }

  ngOnInit() {
    this.alert$ = this.store.select(StateOption.alert);
  }

}
