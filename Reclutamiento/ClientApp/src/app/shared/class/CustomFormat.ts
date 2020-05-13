import * as moment from 'moment';
import { DataType } from '../enums/data-type.enum';

export class CustomFormat {

  format(columnsDefType: DataType, value: any) {
    let result = '';
    if (value === undefined || value === null) {
      return result;
    }
    switch (columnsDefType) {
      case DataType.Number:
        result = Number(value).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
        break;
      case DataType.Currency:
        result = '$ ' + this.format(DataType.Number, value);
        break;
      case DataType.Date:
        moment.locale('es');
        result = moment(value).format('YYYY/MM/DD');
        break;
      case DataType.DateDash:
        moment.locale('es');
        result = moment(value).format('YYYY-MM-DD');
        break;
      case DataType.DateFull:
        moment.locale('es');
        result = moment(value).format('dddd, DD MMMM YYYY');
        result = result.charAt(0).toUpperCase() + result.slice(1);
        break;
      case DataType.DateFullTimeMedium:
        moment.locale('es');
        result = moment(value).format('dddd, DD MMMM YYYY, kk:mm');
        result = result.charAt(0).toUpperCase() + result.slice(1);
        break;
      case DataType.DateFullTimeFull:
        moment.locale('es');
        result = moment(value).format('dddd, DD MMMM YYYY, kk:mm:ss');
        result = result.charAt(0).toUpperCase() + result.slice(1);
        break;
      default:
        result = value;
    }
    return result;
  }

  startIsBeforeEnd(start: any, end: any): boolean {
    return moment(start).isBefore(end);
  }

  startIsAfterEnd(start: any, end: any): boolean {
    return moment(start).isAfter(end);
  }

  startIsSameOrBeforeEnd(start: any, end: any): boolean {
    return moment(start).isSameOrBefore(end);
  }

  startIsSameOrAfterEnd(start: any, end: any): boolean {
    return moment(start).isSameOrAfter(end);
  }

  isSame(start: any, end: any): boolean {
    return moment(start).isSame(end);
  }

}
