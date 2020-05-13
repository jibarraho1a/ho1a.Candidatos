import {Component, Input, OnInit} from '@angular/core';
import { TitleBar } from 'src/app/shared/models/title-bar';

@Component({
  selector: 'ho1a-title-bar',
  templateUrl: './title-bar.component.html',
  styleUrls: ['./title-bar.component.scss']
})
export class TitleBarComponent implements OnInit {

  @Input()
  titleBar: TitleBar;

  constructor() { }

  ngOnInit() {
  }

}
