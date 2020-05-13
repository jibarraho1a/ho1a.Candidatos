import {ChangeDetectionStrategy, Component, Input, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';

@Component({
  selector: 'ho1a-modal-list-candidate-row',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './modal-list-candidate-row.component.html',
  styleUrls: ['./modal-list-candidate-row.component.scss']
})
export class ModalListCandidateRowComponent implements OnInit {

  @Input()
  data: any;

  form: FormGroup;

  constructor(
    private fb: FormBuilder
  ) {
  }

  ngOnInit() {
    this.form = this.fb.group({
        checked: [this.data.checked]
      }
    );
    this.form.get('checked').valueChanges.subscribe(value => {
      this.data.checked = value;
    });
  }

}
