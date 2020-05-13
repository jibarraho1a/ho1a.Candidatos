import {ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'ho1a-template-landing',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './template-landing.component.html',
  styleUrls: ['./template-landing.component.scss']
})
export class TemplateLandingComponent implements OnInit {

  @Output()
  logout = new EventEmitter();

  @Input()
  informacionComplementaria: {
    privacidadDescripcion: string,
    privacidadUrl: string,
    sucursalesDescripcion: string,
    sucursalesUrl: string
  };

  constructor() { }

  ngOnInit() {
  }

  onLogout() {
    this.logout.emit();
  }

}
