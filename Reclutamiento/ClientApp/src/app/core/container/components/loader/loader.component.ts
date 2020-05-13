import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Select } from '@ngxs/store';
import { LoaderState } from 'src/app/store/state/loader.state';

@Component({
  selector: 'ho1a-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss']
})
export class LoaderComponent implements OnInit {

  @Select(LoaderState.getLoaderDisplay) show$: Observable<boolean>;
  constructor() { }

  ngOnInit() {
  }

}
