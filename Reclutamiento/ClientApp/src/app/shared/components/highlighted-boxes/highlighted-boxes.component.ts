import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'ho1a-highlighted-boxes',
  templateUrl: './highlighted-boxes.component.html',
  styleUrls: ['./highlighted-boxes.component.scss']
})
export class HighlightedBoxesComponent implements OnInit {

  @Input()
  data: [
    {
      name: string,
      value: string,
      tooltip: string
    }
  ];

  @Input()
  selectedbox: string;

  @Output()
  clickBox = new EventEmitter<{name: string; value: string}>();

  constructor() { }

  ngOnInit() {
  }

  onClick(event: any) {
    this.selectedbox = event.name;
    this.clickBox.emit(event);
  }

}
