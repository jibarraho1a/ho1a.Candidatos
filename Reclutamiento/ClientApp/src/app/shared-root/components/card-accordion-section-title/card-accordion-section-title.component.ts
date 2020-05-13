import {ChangeDetectionStrategy, Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'ho1a-card-accordion-section-title',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './card-accordion-section-title.component.html',
  styleUrls: ['./card-accordion-section-title.component.scss']
})
export class CardAccordionSectionTitleComponent implements OnInit {

  @Input()
  title = '';

  @Input()
  toggle = false;

  constructor() { }

  ngOnInit() {
  }

}
