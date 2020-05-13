import { Component, OnInit } from '@angular/core';
import {AuthService} from '../../../shared/services/auth/auth.service';
import {FormBuilder, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'ho1a-reset',
  templateUrl: './reset.component.html',
  styleUrls: ['./reset.component.scss']
})
export class ResetComponent implements OnInit {

  form = this.fb.group({
    code: [null, Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
  });

  error = '';
  disabledButtom = false;
  textButtom = 'Resetear';

  constructor(
    private service: AuthService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    this.form.get('code').setValue(this.route.snapshot.queryParams.code);
  }

  disabled() {
    return this.disabledButtom ? 'disabled' : null;
  }

  onKeydown(event: any) {
    if (event.keyCode === 13) {
      this.onReset();
    }
  }

  onReset() {
    if (this.disabledButtom) {
      return;
    }
    if (!this.form.get('code').valid) {
      this.error = 'Código de seguridad no especificado';
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
    if (this.form.get('password').value !== this.form.get('confirmPassword').value) {
      this.error = 'Contraseñas no coinciden';
      return;
    }
    this.error = '';
    this.textButtom = 'Cargando...';
    this.disabledButtom = true;
    this.service.resetPassword(this.form.value).subscribe(
      next => this.router.navigate(['/auth/login']),
      error => {
        this.textButtom = 'Resetear';
        this.disabledButtom = false;
      });
  }

}
