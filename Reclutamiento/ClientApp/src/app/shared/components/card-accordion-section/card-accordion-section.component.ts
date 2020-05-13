import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnInit, Output} from '@angular/core';
import { CustomFormat } from '../../class/CustomFormat';
import { DataType } from '../../enums/data-type.enum';


@Component({
  selector: 'ho1a-card-accordion-section',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './card-accordion-section.component.html',
  styleUrls: ['./card-accordion-section.component.scss']
})
export class CardAccordionSectionComponent implements OnInit, OnChanges {

  @Input()
  data: any;

  @Output()
  clickpopup = new EventEmitter<string>();

  toggle = false;

  format: CustomFormat;

  constructor() {
    this.format = new CustomFormat();
  }

  ngOnInit() {
  }

  ngOnChanges() {
    this.toggle = this.data.toggle;
  }

  onToggle() {
    this.toggle = !this.toggle;
  }

  onPopUp(value: string) {
    this.clickpopup.emit(value);
  }

  get typePopUp() {
    return DataType.PopUp;
  }

  get typePopUpDateTime() {
    return DataType.PopUpDateTime;
  }

  getHtmlRow(row: any) {
    let result = '';
    switch (row.type) {
      case DataType.PopUp:
        result += '<a class=\"text-gray material-icons cursor-pointer\" click=\"onPopUp(' + row.value + ')\">launch</a>';
        break;
      case DataType.Link:
        result += '<a class=\"text-gray material-icons cursor-pointer\" target="_blank" href=\"' + row.value + '\">file_download</a>';
        break;
      case DataType.String:
        result += row.value;
        break;
      case DataType.Number:
      case DataType.Currency:
      case DataType.Date:
      case DataType.DateFullTimeMedium:
      case DataType.DateFull:
      case DataType.DateFullTimeFull:
        result += this.format.format(row.type, row.value);
        break;
    }
    return result;
  }

  isRowWithEvent(dataType: DataType) {
    return dataType === DataType.PopUp ? true : false;
  }

}
