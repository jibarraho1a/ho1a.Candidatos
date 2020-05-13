import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Select } from '@ngxs/store';
import { OverlayState } from 'src/app/store/state/overlay.state';

@Component({
  selector: 'ho1a-overlay',
  templateUrl: './overlay.component.html',
  styleUrls: ['./overlay.component.scss']
})
export class OverlayComponent implements OnInit {

  @Select(OverlayState.getOverlayDisplay) show$: Observable<boolean>
  constructor() { }

  ngOnInit() {
  }

}
