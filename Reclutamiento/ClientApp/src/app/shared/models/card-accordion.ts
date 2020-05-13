import { DataType } from '../enums/data-type.enum';

export interface CardAccordion {
  tittle?: string;
  toggle?: boolean;
  sections?: Section[];
}

interface Section {
  tittle?: string;
  toggle?: boolean;
  rows?: Row[];
}

interface Row {
  label: string;
  value: any;
  type: DataType;
}
