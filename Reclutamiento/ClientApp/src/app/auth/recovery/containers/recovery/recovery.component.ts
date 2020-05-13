import { Component, OnInit } from '@angular/core';
import {FormBuilder, Validators} from '@angular/forms';
import {AuthService} from '../../../shared/services/auth/auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'ho1a-recovery',
  templateUrl: './recovery.component.html',
  styleUrls: ['./recovery.component.scss']
})
export class RecoveryComponent implements OnInit {

  error = '';
  disabledButtom = false;
  textButtom = 'Recuperar';

  form = this.fb.group({
    email: [null, [Validators.required, Validators.email]],
  });

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
      this.onRecovery();
    }
  }

  onRecovery() {
    if (this.disabledButtom) {
      return;
    }
    if (!this.form.valid) {
      this.error = 'Datos ingresados incorrectamente';
      return;
    }
    this.error = '';
    this.textButtom = 'Cargando...';
    this.disabledButtom = true;
    this.service.forgotPassword(this.form.value).subscribe(
      next => this.router.navigate(['/auth/login']),
      error => {
        this.error = 'Detalles al asignar la nueva contreseña';
        this.textButtom = 'Recuperar';
        this.disabledButtom = false;
      });
  }

}
