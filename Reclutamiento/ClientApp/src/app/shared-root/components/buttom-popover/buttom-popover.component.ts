import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'ho1a-buttom-popover',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './buttom-popover.component.html',
  styleUrls: ['./buttom-popover.component.scss']
})
export class ButtomPopoverComponent implements OnInit {

  @Input()
  name: string;

  @Input()
  buttoms: {name: string; value: string; class: string}[];

  @Output()
  clickButtom = new EventEmitter<any>();

  @Output()
  toggleClick = new EventEmitter<boolean>();

  toggle = false;

  constructor() { }

  ngOnInit() {
  }

  onClickButtom(value: any) {
    if (value.value !== false) {
      this.clickButtom.emit(value);
    }
    this.onToggled();
  }

  onToggled() {
    this.toggle = !this.toggle;
    this.toggleClick.emit(this.toggle);
  }

}
