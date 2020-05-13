import {ChangeDetectionStrategy, Component, Input, OnInit} from '@angular/core';
import { TypeButtonAction } from '../../../../shared/enums/type-button-action.enum';
import { StatusCaptureCandidateInformationEnum } from '../../../../shared/enums/status-capture-candidate-information.enum';


@Component({
  selector: 'ho1a-accordion-title-candidate',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './accordion-title-candidate.component.html',
  styleUrls: ['./accordion-title-candidate.component.scss']
})
export class AccordionTitleCandidateComponent implements OnInit {

  @Input()
  candidate: any;

  constructor() { }

  ngOnInit() {
  }

  getStatus(statusValue: number) {
    const status = {
      text: '',
      icon: TypeButtonAction.RadioButtonUnchecked
    };
    switch (statusValue) {
      case -4:
        status.text = 'Candidato no interesado';
        status.icon = TypeButtonAction.NotInterested;
        break;
      case -3:
        status.text = 'Candidato en duda';
        status.icon = TypeButtonAction.NotInterested;
        break;
      case -2:
        status.text = 'Candidato descartado';
        status.icon = TypeButtonAction.NotInterested;
        break;
      case -1:
        status.text = 'Candidato no idoneo';
        status.icon = TypeButtonAction.NotInterested;
        break;
      case 0:
        status.text = 'Aún no calificado';
        status.icon = TypeButtonAction.RadioButtonUnchecked;
        break;
      case 1:
        status.text = 'Candidato idoneo';
        status.icon = TypeButtonAction.CheckCircle;
        break;
    }
    return status;
  }

  get hasNotification() {
    return this.candidate.data.statusCaptura === StatusCaptureCandidateInformationEnum.CapturaInicial ||
           this.candidate.data.statusCaptura === StatusCaptureCandidateInformationEnum.CapturaComplementaria;
  }

}
