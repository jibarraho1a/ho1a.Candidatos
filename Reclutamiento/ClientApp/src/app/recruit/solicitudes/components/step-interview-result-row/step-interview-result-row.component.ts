import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

interface ResultInterview {
  nombre: string;
  paterno: string;
  materno: string;
  average: number;
  isSuitable: boolean;
  interviews: [
    {
      name: string,
      qualification: number,
      recommendIt: boolean,
    }
    ];
}

@Component({
  selector: 'ho1a-step-interview-result-row',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-interview-result-row.component.html',
  styleUrls: ['./step-interview-result-row.component.scss']
})
export class StepInterviewResultRowComponent implements OnInit {

  @Input()
  candidate: ResultInterview;

  @Output()
  clickpopup = new EventEmitter<any>();

  toggle = false;

  constructor() { }

  ngOnInit() {
    this.toggle = this.candidate.isSuitable;
  }

  onToggled() {
    this.toggle = !this.toggle;
  }

  onClickpopup(value: any) {
    this.clickpopup.emit(value);
  }

}
