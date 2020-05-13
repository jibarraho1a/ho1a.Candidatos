import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

@Component({
  selector: 'ho1a-collaborator-search-engine',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './collaborator-search-engine.component.html',
  styleUrls: ['./collaborator-search-engine.component.scss']
})
export class CollaboratorSearchEngineComponent implements OnInit, OnChanges {

  @Input()
  title: string;

  @Input()
  result: string;

  @Output()
  search = new EventEmitter<any>();

  @Output()
  accept = new EventEmitter<any>();

  form: FormGroup;

  buttomsPopover = [
    {name: 'Agregar', value: true, class: 'btn-success'},
    {name: 'Cancelar', value: false, class: 'btn-primary'}
  ];

  constructor(
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.configureForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
  }

  configureForm() {
    this.form = this.fb.group({
      textSearch: [ {value: '', disabled: false}, Validators.required ],
      result: [ {value: '', disabled: true}, null ]
    });
  }

  onClickButtom(event: any) {
    this.accept.emit(this.form.get('textSearch').value);
  }

  onSearchCollaboratorByUserName() {
    if (this.form.valid) {
      this.search.emit(this.form.get('textSearch').value);
    }
  }

}
