import {ChangeDetectionStrategy, Component, Input, OnInit} from '@angular/core';
import { CustomFormat } from '../../../../shared/class/CustomFormat';
import { DataType } from '../../../../shared/enums/data-type.enum';


@Component({
  selector: 'ho1a-step-terna-list',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-terna-list.component.html',
  styleUrls: ['./step-terna-list.component.scss']
})
export class StepTernaListComponent implements OnInit {

  @Input()
  data: any;

  toggle = false;

  format: CustomFormat;

  constructor(
  ) {
    this.format = new CustomFormat();
  }

  ngOnInit() {
  }

  onToggle() {
    this.toggle = !this.toggle;
  }

  formatDate(value: any) {
    return this.format.format(DataType.DateFullTimeMedium, value);
  }

}
