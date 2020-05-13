import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'ho1a-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss']
})
export class AlertComponent implements OnChanges {

  @Input()
  data: any;

  constructor() { }

  ngOnChanges(changes: SimpleChanges): void {
    const isChangedData = changes && changes.data !== undefined;
    if (isChangedData && changes.data !== null) {
      setTimeout(() => {
        this.data = null;
      }, 5000);
    }
  }

  onClose() {
    this.data = null;
  }

}
