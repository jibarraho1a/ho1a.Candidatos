import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnInit, Output} from '@angular/core';

@Component({
  selector: 'ho1a-card-accordion',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './card-accordion.component.html',
  styleUrls: ['./card-accordion.component.scss']
})
export class CardAccordionComponent implements OnInit, OnChanges {

  toggle = false;

  @Input()
  data: any;

  @Input()
  index: number;

  @Output()
  clickpopup = new EventEmitter<string>();

  constructor() { }

  ngOnInit() {
  }

  ngOnChanges() {
    this.toggle = this.data.toggle;
  }

  onToggle() {
    this.toggle = !this.toggle;
  }

  onClickpopup(value: string) {
    this.clickpopup.emit(value);
  }

}
