import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'ho1a-filter-categories',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './filter-categories.component.html',
  styleUrls: ['./filter-categories.component.css']
})
export class FilterCategoriesComponent implements OnInit {

  @Input()
  columnDef: any;

  @Input()
  items: any[];

  constructor() { }

  ngOnInit() {
  }

}
