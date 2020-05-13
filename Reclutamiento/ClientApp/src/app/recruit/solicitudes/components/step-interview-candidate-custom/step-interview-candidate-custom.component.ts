import { ChangeDetectionStrategy, Component, Input, OnChanges, OnInit, SimpleChanges } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { Store } from '../../../../shared/class/Store';
import { StateOption } from '../../../../shared/enums/state-option.enum';
import { TypeEvent } from '../../../../shared/enums/type-event.enum';
import { EventApp } from '../../../../shared/models/event-app';


@Component({
  selector: "ho1a-step-interview-candidate-custom",
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: "./step-interview-candidate-custom.component.html",
  styleUrls: ["./step-interview-candidate-custom.component.scss"]
})
export class StepInterviewCandidateCustomComponent implements OnInit, OnChanges {

  @Input()
  candidate: any;

  @Input()
  disabled = false;

  @Input()
  visibleButtomInterview = true;

  @Input()
  visibleCheckSuitableCandidate = true;

  toggleCheck = false;
  estatus = [
    { value: '-2', text: 'Descartado' },
    { value: '-3', text: 'En duda' },
    { value: '-4', text: 'No Interesado' },
  ];

  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private store: Store
  ) {
  }

  ngOnInit() {
    this.form.get('status').valueChanges.subscribe(value => {
      if (value === '-1') {
        this.onToggleCheck();
      } else if (value === '1') {
        this.sendEvent(value);
      } else if (value !== '0') {
        this.sendEvent(value);
        this.onToggleCheck();
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    const value = this.candidate.data && this.candidate.data.statusCandidato !== null
      ? this.candidate.data.statusCandidato.toString()
      : null;
    this.form = this.fb.group({
      status: [{ value: value, disabled: false }]
    });
  }

  onToggleCheck() {
    this.toggleCheck = !this.toggleCheck;
  }

  sendEvent(value: string) {
    this.store.set(StateOption.eventsRequisition,
      {
        typeEvent: TypeEvent.SelectingSuitableCandidateRequisition,
        data: {
          candidatoId: this.candidate.data.candidatoId,
          status: value
        }
      } as EventApp);
  }

  cancelNoSuitable() {
    this.resetStatus();
    this.onToggleCheck();
  }

  saveNoSuitable() {
    const value = this.form.get('status').value;
    this.sendEvent(value);
    this.resetStatus();
  }

  resetStatus() {
    this.form.get('status').setValue('0');
  }

}
