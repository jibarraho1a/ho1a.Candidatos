import { Component, OnInit } from '@angular/core';
import {AuthService} from '../../../shared/services/auth/auth.service';
import {FormBuilder, Validators} from '@angular/forms';
import {Router} from '@angular/router';

@Component({
  selector: 'ho1a-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    nombre: ['', Validators.required],
    paterno: ['', Validators.required],
    materno: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(6)]],
    rePassword: ['', [Validators.required, Validators.minLength(6)]],
  });

  error = '';
  disabledButtom = false;
  textButtom = 'Crear cuenta';

  constructor(
    private service: AuthService,
    private fb: FormBuilder,
    private router: Router,
  ) { }

  ngOnInit() {
  }

  disabled() {
    return this.disabledButtom ? 'disabled' : null;
  }

  onKeydown(event: any) {
    if (event.keyCode === 13) {
      this.onRegister();
    }
  }

  onRegister() {
    if (this.disabledButtom) {
      return;
    }
    if (!this.form.get('email').valid) {
      this.error = 'Correo ingresado incorrectamente';
      return;
    }
    if (!this.form.valid) {
      this.error = 'Datos ingresados incorrectamente';
      return;
    }
    if (this.form.get('password').value !== this.form.get('rePassword').value) {
      this.error = 'Contraseñas no coinciden';
      return;
    }
    this.error = '';
    this.textButtom = 'Cargando...';
    this.disabledButtom = true;
    this.service.register(this.form.value).subscribe(
      next => this.loginUser(this.form.get('email').value, this.form.get('password').value),
      error => {
        if (error.DuplicateUserName) {
          this.error = 'Cuenta de correo ya ha sido usada';
        }
        this.textButtom = 'Crear cuenta';
        this.disabledButtom = false;
      });
  }

  loginUser(email: string, password: string) {
    this.service.loginEmailPassword(email, password)
      .subscribe(
        data => {
          this.router.navigate(['/ho1a/candidate']);
        },
        error => {
          console.log(error);
        }
      );
  }

}
