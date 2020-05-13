import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';

enum EnumFilterType {
  PorCoincidencia = 1,
  PorSeleccion = 2
}

@Component({
  selector: 'ho1a-filter',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.css']
})
export class FilterComponent implements OnInit, OnChanges {

  @Output()
  toggleFilter = new EventEmitter<boolean>();

  @Output()
  textFilter = new EventEmitter<string>();

  @Output()
  orderByChange = new EventEmitter<{reverse: boolean, column: string}>();

  @Output()
  columnsFilterForm = new EventEmitter<{controls: any, keys: string[]}>();

  @Input()
  toggle: boolean;

  @Input()
  tittle: boolean;

  @Input()
  activateDateFilter: boolean;

  @Input()
  columnsDefs: any[];

  @Input()
  items: any[];

  @Input()
  totalResults = 0;

  columnsApplyOrderBy = [];
  columnsApplyFilter = [];
  keysControl = [];

  itemsWithdistinct: {};

  form = this.fb.group({
    description: [null],
    textSearch: [''],
    typeFilter: ['1'],
    columnOrderBy: [null],
    categories: this.fb.group([''])
  });

  constructor(
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.columnsApplyOrderBy = this.columnsDefs.filter(f => f.canOrderBy);
    this.columnsDefs.forEach(f => {
      if (f.isFilter) {
        this.columnsApplyFilter.push(f);
        this.keysControl.push(f.field);
      }
    });
    this.typeFilter.valueChanges
      .subscribe(value => {
        this.keysControl.forEach( key => this.categories.get(key).setValue(null) );
        this.applyFilter();
        this.textSearch.setValue('');
        this.textFilter.emit(this.textSearch.value);
      });
  }

  ngOnChanges(changes: any) {
    if (this.items) {
      this.itemsWithdistinct = this.distinctColumns();
      this.categories.removeControl('0');
      this.keysControl.forEach( key => {
        this.categories.addControl(key, new FormControl(null));
      });
    }
  }

  distinctColumns() {
    let list = {};
    for (const column of this.columnsDefs) {
      if (column.isFilter) {
        let col = [];
        for (const item of this.items) {
          if (col.indexOf(item[column.field]) === -1) {
            col.push(item[column.field]);
          }
        }
        list[column.field] = col;
      }
    }
    return list;
  }

  onToggleFilter() {
    this.toggle = !this.toggle;
    this.toggleFilter.emit(this.toggle);
  }

  onTextFilter(event: any) {
    this.textFilter.emit(this.textSearch.value);
  }

  OnOrderByChangeUp() {
    if (this.columnOrderByContaintValue) {
      this.OnOrderByChange(true);
    }
  }

  OnOrderByChangeDown() {
    if (this.columnOrderByContaintValue) {
      this.OnOrderByChange(false);
    }
  }

  OnOrderByChange(reverse: boolean) {
    this.orderByChange.emit( { reverse: reverse, column: this.columnOrderBy.value } );
  }

  get columnOrderByContaintValue() {
    return this.columnOrderBy.value != null;
  }

  get textSearch() {
    return this.form.get('textSearch');
  }

  get typeFilter() {
    return this.form.get('typeFilter');
  }

  get columnOrderBy() {
    return this.form.get('columnOrderBy');
  }

  get matchTypeFilterSelected() {
    return this.typeFilter.value === '1';
  }

  get selectionTypeFilterSelected() {
    return this.typeFilter.value === '2';
  }

  get categories() {
    return this.form.get('categories') as FormGroup;
  }

  get categoriesControl() {
    return this.categories.controls;
  }

  applyFilter() {
    let controlsApplyFilter = {};
    this.keysControl.forEach( f => {
      if (this.categories.controls[f].value && this.categories.controls[f].value.length) {
        controlsApplyFilter[f] = this.categories.controls[f];
      }
    });
    this.columnsFilterForm.emit({controls: controlsApplyFilter, keys: Object.keys(controlsApplyFilter)});
  }

}
