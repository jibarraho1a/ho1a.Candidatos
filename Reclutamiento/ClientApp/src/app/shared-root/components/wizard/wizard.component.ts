import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { StateValidation } from '../../enums';
import { StatusProcess } from '../../models';
import index from '@angular/cli/lib/cli';

@Component({
  selector: 'ho1a-wizard',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './wizard.component.html',
  styleUrls: ['./wizard.component.scss']
})
export class WizardComponent implements OnInit {

  @Input()
  processes: StatusProcess[];

  @Output()
  selected = new EventEmitter<StatusProcess>();

  firstStep = 0;
  index = this.firstStep;

  constructor() { }

  ngOnInit() {
    this.setProcesses();
  }

  setProcesses(index?: number, item?: StatusProcess) {
    if (item !== undefined && item.visible === false) {
      return;
    }
    this.index = index !== undefined ? index : this.index;
    this.processes.forEach( (value: any, i: number) => {
      if (i === this.index) {
        value.stateValidation = StateValidation.current;
      } else {
        value.stateValidation = StateValidation.pending;
      }
    });
    this.selected.emit(this.currentStep());
  }

  currentStep() {
    const proces = {...this.processes[this.index]};
    proces.currentIndex = this.index;
    return proces;
  }

  next() {
    if (this.index === this.lastStep()) {
      this.index = 0;
    } else {
      this.index++;
    }
    if (this.processes[this.index].visible === false) {
      this.next();
    }
    this.setProcesses();
  }

  back() {
    if (this.index === this.firstStep) {
      this.index = this.lastStep();
    } else {
      this.index--;
    }
    if (this.processes[this.index].visible === false) {
      this.back();
    }
    this.setProcesses();
  }

  lastStep() {
    return this.processes.length - 1;
  }

}
