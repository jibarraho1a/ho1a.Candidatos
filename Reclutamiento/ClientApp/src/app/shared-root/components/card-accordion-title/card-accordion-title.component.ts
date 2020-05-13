import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'ho1a-card-accordion-title',
  templateUrl: './card-accordion-title.component.html',
  styleUrls: ['./card-accordion-title.component.scss']
})
export class CardAccordionTitleComponent implements OnInit {

  @Input()
  toggle = false;

  @Input()
  title = '';

  constructor() { }

  ngOnInit() {
  }

}
