import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import * as moment from 'moment';

@Component({
  selector: 'ho1a-list-item',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './list-item.component.html',
  styleUrls: ['./list-item.component.scss']
})
export class ListItemComponent implements OnInit {

  @Input()
  options: any = {
    columnsDefs: [
      { field: '', title: '', type: 'date', style: '' }
    ],
    columnsDefsButton: [
      { key: 'view', icon: 'remove_red_eye', tooltip: '...'}
    ],
    columnsForRow: 3,
    allowDelete: true,
    allowEdith: false,
  };

  @Input()
  row: any;

  @Output()
  edith = new EventEmitter<any>();

  @Output()
  delete = new EventEmitter<any>();

  @Output()
  btnAction = new EventEmitter<any>();

  toggled = false;
  columnsDefsVisibles = [];

  constructor() { }

  ngOnInit() {
    this.columnsDefsVisibles = this.options && this.options.columnsDefs && this.options.columnsDefs.filter(f => f.isVisible);
    this.options.allowDelete = this.row.canDelete;
  }

  get classColumnsForRow() {
    if (this.options && this.options.columnsForRow) {
      switch (this.options.columnsForRow) {
        case 1:
          return 'col-xs-12 col-sm-12 col-md-12 col-lg-12';
        case 2:
          return 'col-xs-12 col-sm-6 col-md-6 col-lg-6';
        case 3:
          return 'col-xs-12 col-sm-6 col-md-6 col-lg-4';
        case 4:
          return 'col-xs-12 col-sm-6 col-md-6 col-lg-3';
        case 6:
          return 'col-xs-12 col-sm-6 col-md-6 col-lg-2';
      }
    }
    return 'col-xs-12 col-sm-6 col-md-6 col-lg-4';
  }

  get allowDelete() {
    return this.options && this.options.allowDelete;
  }

  get allowEdith() {
    return this.options && this.options.allowEdith;
  }

  onEdith() {
    this.edith.emit(this.row);
  }

  onDelete() {
    this.onToggled();
    this.delete.emit(this.row);
  }

  onToggled() {
    this.toggled = !this.toggled;
  }

  onBtnAction(columns: any, row: any, value: any) {
    this.btnAction.emit({columns: columns, row: row, key: value} );
  }

  format(columnsDefType: string, value: any) {
    let result: string;

    switch (columnsDefType) {
      case 'currency':
        result = '$ ' + value.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
        break;
      case 'date':
        moment.locale('es');
        result = moment(value).format('YYYY/MM/DD');
        break;
      default:
        result = value;
    }
    return result;
  }

  formatCustomBoolean(option: any) {
    return this.row[option.field] == true ? option.styleTrue : option.styleFalse;
  }

  isCustomBoolean(type: any) {
    return type === 'customBoolean';
  }

}
