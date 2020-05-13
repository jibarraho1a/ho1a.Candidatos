import {ComponentConfiguration, Action} from '../models';
import {Validators} from '@angular/forms';

export class AttributeConfiguration {

  private configuration: ComponentConfiguration;
  private nameCurrentControl: string;

  constructor(configuration: ComponentConfiguration) {
    this.configuration = configuration;
  }

  getConfigurationByControlName(name: string) {
    this.nameCurrentControl = name;
    const configuration = this.getConfigurationByCurrentControl();
    return {
      configuration,
      validators: this.getValidatorsByCurrentControl(configuration)
    };
  }

  private getConfigurationByCurrentControl() {
    const currentControlConfiguration = <ComponentConfiguration>{ edicion: false, validaciones: [] };
    if (this.configuration && this.configuration.componentes && this.configuration.componentes.length) {
      const foundConfiguration = this.configuration.componentes.find( f => f.nombre === this.nameCurrentControl);
      return foundConfiguration ? foundConfiguration : currentControlConfiguration;
    }
    return currentControlConfiguration;
  }

  private getValidatorsByCurrentControl(configuration: ComponentConfiguration) {
    const validators: any[] = [];
    if (configuration.validaciones.length > 0) {
      for (const validation of configuration.validaciones) {
        if (validation.nombre === 'Required' && validation.valor === 'true') {
          validators.push(Validators.required);
        } else if (validation.nombre === 'Min') {
          validators.push(Validators.min(validation.valor));
        } else if (validation.nombre === 'Max') {
          validators.push(Validators.max(validation.valor));
        } else if (validation.nombre === 'MaxLength') {
          validators.push(Validators.maxLength(validation.valor));
        } else if (validation.nombre === 'MinLength') {
          validators.push(Validators.minLength(validation.valor));
        } else if (validation.nombre === 'Pattern') {
          validators.push(Validators.pattern(validation.valor));
        }
      }
    }
    return validators;
  }

  getActionByControlName(name: string) {
    const currentControlAction = <Action>{ activo: false, descripcion: name, id: null, nombre: name, visible: false};
    if (this.configuration && this.configuration.acciones && this.configuration.acciones.length) {
      const foundConfiguration = this.configuration.acciones.find( f => f.nombre === name);
      return foundConfiguration ? foundConfiguration : currentControlAction;
    }
    return currentControlAction;
  }

}
