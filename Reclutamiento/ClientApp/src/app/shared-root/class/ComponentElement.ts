import {Input, SimpleChanges} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup} from '@angular/forms';
import {Combo, ComponentConfiguration, FormControlDefinition} from '../models';
import {AttributeConfiguration} from './AttributeConfiguration';
import {ValidationType} from '../enums';
import Pattern = ValidationType.pattern;

export abstract class ComponentElement {

  @Input()
  data: any;

  @Input()
  databackground: any;

  @Input()
  configuration: ComponentConfiguration;

  form: FormGroup;
  private fb: FormBuilder;
  listCatalog = [];
  catalogs: any;
  formControls: FormControlDefinition[];

  private isChangedData: boolean;
  private isFirstChangedData: boolean;
  private isChangedDataBackGround: boolean;
  private isFirstChangedDataBackGround: boolean;
  private isChangedConfiguration: boolean;
  private isFirstChangedConfiguration: boolean;

  private attributeConfiguration: AttributeConfiguration;

  constructor() {
    this.fb = new FormBuilder();
  }

  changes(changes?: SimpleChanges) {
    this.isChangedData = changes && changes.data !== undefined;
    this.isFirstChangedData = this.isChangedData && changes.data.isFirstChange();
    this.isChangedDataBackGround = changes && changes.databackground !== undefined;
    this.isFirstChangedDataBackGround = this.isChangedDataBackGround && changes.databackground.isFirstChange();
    this.isChangedConfiguration = changes && changes.configuration !== undefined;
    this.isFirstChangedConfiguration = this.isChangedConfiguration && changes.configuration.isFirstChange();
    this.attributeConfiguration = new AttributeConfiguration(this.configuration);
    this.configFormControl();
    this.configForm();
    if (!this.isFirstChangedDataBackGround) {
      this.setCatalogs();
    }
    this.changesCustom();
  }

  abstract configFormControl();

  abstract changesCustom();

  abstract setCatalogsCustom();

  configForm() {
    if (this.isFirstChangedData) {
      this.initFormGroup();
    }
    for (const control of this.formControls) {
      if (this.isFirstChangedData) {
        this.form.addControl(control.name, this.getFormControlConfigured(control));
      } else {
        const value = this.getValueControl(control);
        const currentConfigurationControl = this.getConfigurationByControl(control.name);
        if (this.isChangedData) {
          this.form.get(control.name).setValue(value, {onlySelf: true, emitEvent: false});
        }
        if (this.isChangedConfiguration) {
          this.form.get(control.name).clearValidators();
          this.form.get(control.name).setValidators(currentConfigurationControl.validators);
          if (currentConfigurationControl.configuration.edicion) {
            this.form.get(control.name).enable();
          } else {
            this.form.get(control.name).disable();
          }
        }
      }
    }
    if (this.isFirstChangedData || this.isFirstChangedConfiguration) {
      this.configValueChange();
    }
  }

  private initFormGroup() {
    this.form = this.fb.group({id: [null]});
    this.form.removeControl('id');
  }

  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;

  }

  private getFormControlConfigured(control: FormControlDefinition): AbstractControl {
    const value = this.getValueControl(control);
    const currentControl = this.attributeConfiguration.getConfigurationByControlName(control.name);
    const disabled = !currentControl.configuration.edicion;
    return this.fb.control(
      { value: value,  disabled: disabled }, currentControl.validators
    );
  }

  private getValueControl(control: FormControlDefinition) {
    let valor = null;
    if (control.isCombo && this.data[control.name]) {
      valor = this.data[control.name].value;
    } else if (!control.isCombo) {
      valor = this.data[control.name];
    }
    return valor;
  }

  init() {
    this.configCatalogs();
    this.setCatalogs();
    this.initCustom();
  }

  abstract initCustom();

  abstract configCatalogs();

  public abstract readonly (): boolean;

  public abstract visible (): boolean;

  setCatalogs() {
    this.catalogs = {};
    for (const catalog of this.listCatalog) {
      if (this.data[catalog]) {
        this.catalogs[catalog] = this.data[catalog];
      } else if (this.databackground && this.databackground.catalogs && this.databackground.catalogs[catalog]) {
        this.catalogs[catalog] = this.databackground.catalogs[catalog];
      }
    }
    this.setCatalogsCustom();
  }

  configValueChange() {
    this.configValueChangeAutomatic();
  }

  configValueChangeAutomatic() {
    for (const control of this.formControls) {
      if (control.isCombo) {
        this.form.get(control.name).valueChanges.subscribe(value => {
          if (control.custom) {
            this.configValueChangeCustom(control.name, value);
          }
          this.data[control.name] = this.setCombo(value);
        });
      } else {
        this.form.get(control.name).valueChanges.subscribe(value => {
          if (control.custom) {
            this.configValueChangeCustom(control.name, value);
          }
          this.data[control.name] = value;
        });
      }
    }
  }

  abstract configValueChangeCustom(controlName: string, value: any);

  setCombo(value: any) {
    return <Combo>{
      value: value,
      selected: false,
      disabled: false,
      group: null,
      text: ''
    };
  }

  getHintByControl(name: string) {
    let result = '';
    const currentControl = this.attributeConfiguration.getConfigurationByControlName(name);

    const required =
      currentControl.configuration.validaciones.find(f => f.nombre === ValidationType.required && f.valor === 'true');
    if (required) {
      result = 'Requerido';
    }

    const min = currentControl.configuration.validaciones.find(f => f.nombre === ValidationType.min);
    if (min) {
      result += ((result.length > 0) ? ', ' : '') + 'Valor minimo debe ser ' + min.valor;
    }

    const max = currentControl.configuration.validaciones.find(f => f.nombre === ValidationType.max);
    if (max) {
      result += ((result.length > 0) ? ', ' : '') + 'Valor máximo debe ser ' + max.valor;
    }

    const minLength = currentControl.configuration.validaciones.find(f => f.nombre === ValidationType.minLength);
    if (minLength) {
      result += ((result.length > 0) ? ', ' : '') + 'Longitud minima debe ser ' + minLength.valor;
    }

    const maxLength = currentControl.configuration.validaciones.find(f => f.nombre === ValidationType.maxLength);
    if (maxLength) {
      const control = this.form.get(name);
      const currentLength = control.value ? control.value.length : 0;
      result += ((result.length > 0) ? ', ' : '') + 'Longitud máxima ' + currentLength + ' / ' + maxLength.valor;
    }

    const pattern = currentControl.configuration.validaciones.find(f => f.nombre === ValidationType.pattern);
    if (pattern) {

      result += ((result.length > 0) ? ', ' : '');

      if (pattern.label) {
        result += pattern.label;
      } else {
        result += 'Patrón esperado debe ser ' + pattern.valor;
      }
    }

    return result;
  }

  getErrorByControl(name: string) {
    let result = '';
    const errors = this.form.get(name).errors;

    if (errors) {
      if (errors.required) {
        result = 'Requerido';
      }
      if (errors.min) {
        result = result + ' Valor minimo: ' + errors.min.min;
      }
      if (errors.max) {
        result = result + ' Valor máximo: ' + errors.max.max;
      }
      if (errors.minlength) {
        result = result + ' Longitud minima: ' + errors.minlength.requiredLength;
      }
      if (errors.maxlength) {
        result = result + ' Longitud máxima: ' + errors.maxlength.requiredLength;
      }
      if (errors.pattern) {

        const currentControl = this.attributeConfiguration.getConfigurationByControlName(name);
        console.log(currentControl.configuration.validaciones);

        const pattern = currentControl.configuration.validaciones.find(f => f.nombre == "Pattern");

        if (pattern) {
          if (pattern.label) {
            result += pattern.label;
          } else {
            result += result + ' Patrón esperado: ' + errors.pattern.requiredPattern;
          }
          
        }
      }
    }

    return result;
  }

  get validForm() {
    return this.form.valid;
  }

  get exist() {
    return this.data && this.data.id > 0;
  }

  getActionByControl(name: string) {
    return this.attributeConfiguration.getActionByControlName(name);
  }

  getConfigurationByControl(name: string) {
    return this.attributeConfiguration.getConfigurationByControlName(name);
  }

  showAsRequired(controlName: string) {
    return this.form.get(controlName) && this.form.get(controlName).errors !== null;
  }

}
