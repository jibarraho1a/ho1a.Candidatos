import { Observable } from 'rxjs/Observable';
import { OrderPipe } from 'ngx-order-pipe';
import {GridOption} from '../models';

export abstract class ListElement {

  orderPipe: OrderPipe;
  items: any[];
  itemsFiltered: any[];
  configurations: any[];
  loading = false;
  errorMensage: string;
  idSelected: number;
  completeList = false;
  completeConfiguration = false;
  observer: any;
  itemChange: any;
  totalResults = 0;
  showFilter = false;
  textSearch = '';
  order = '';
  reverse = false;
  options: GridOption;

  constructor(
  ) {
    this.orderPipe = new OrderPipe();
    this.configureGridOption();
  }

  abstract configureGridOption();

  init() {
    this.initAction();
    this.getDatasInit().subscribe(
      next => console.log(''),
      error => this.handlerError(error),
      () => this.completeAction()
    );
  }

  initAction() {
    this.loading = true;
  }

  getDatasInit() {
    const request = new Observable((observer) => {
      this.observer = observer;
      this.getListService();
    });
    return request;
  }

  abstract getListService();

  handlerError(error: string) {
    this.errorMensage = error;
    this.alertService();
    this.completeAction();
  }

  abstract alertService();

  completeAction() {
    this.loading = false;
  }

  abstract get ruta();

  toggleFilter(toggle: boolean) {
    this.showFilter = toggle;
  }

  textFilter(textSearch: string) {
    this.textSearch = textSearch;
    this.itemsFiltered = this.getItemsFiltered();
    this.totalResults = this.itemsFiltered.length;
  }

  getItemsFiltered() {
    const columnsTextSearch = this.getColumnsTextSearch();
    return this.items.filter(
      f => columnsTextSearch.findIndex( fi =>
        f[fi.field] && f[fi.field].toString().toUpperCase().indexOf(this.textSearch.toUpperCase()) !== -1
      ) >= 0
    );
  }

  getColumnsTextSearch() {
    return this.options.columnsDefs.filter( f => f.isVisible);
  }

  onShowFilter() {
    this.toggleFilter(true);
  }

  orderByChange(event: any) {
    this.order = event.column;
    this.reverse = event.reverse;
    this.itemsFiltered = this.orderPipe.transform(this.itemsFiltered, this.order, this.reverse, true);
  }

  columnsFilterForm(event: any) {
    if (event.controls && event.keys) {
      this.itemsFiltered = this.items.filter(  item => {
        const notMatchingField = event.keys.every( key =>
          event.controls[key].value.findIndex( value => value === item[key]) >= 0
        );
        return notMatchingField;
      });
      this.totalResults = this.itemsFiltered.length;
    }
  }

  next(next: any[]) {
    this.items = next;
    this.itemsFiltered = next;
    if (next) {
      this.totalResults = next.length;
    }
  }

  abstract btnAction(event: any);

}
