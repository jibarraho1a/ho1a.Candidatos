export class Configuration {
  acciones: Action[];
  componentes: ComponentConfiguration[];
  descripcion: string;
  id: number;
  vista: string;
}

export class Action {
  activo: boolean;
  descripcion: string;
  id: number;
  nombre: string;
  visible: boolean;
  readonly?: boolean;
}

export class ComponentConfiguration {
  acciones: Action[];
  componentes: ComponentConfiguration[];
  descripcion: string;
  id: number;
  index: number;
  nombre: string;
  edicion: boolean;
  visible: boolean;
  stateValidation: number;
  currentStep: boolean;
  validaciones: Validation[];
}

class Validation {
  id: number;
  nombre: string;
  tipoDato: string;
  valor: any;
}
