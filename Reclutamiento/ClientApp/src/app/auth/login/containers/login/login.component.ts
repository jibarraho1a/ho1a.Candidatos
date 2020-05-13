import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../../../shared/services/auth/auth.service';
import {FormBuilder, Validators} from '@angular/forms';
import { AuthService as ServiceSocial, FacebookLoginProvider, SocialUser, GoogleLoginProvider, LinkedInLoginProvider } from 'angularx-social-login';



@Component({
  selector: 'ho1a-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  error: string;
  urlLogin = '';
  user: SocialUser;
  fbUrl = '';
  disabledButtom = false;
  textButtom = 'Entrar';

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private social: ServiceSocial
  ) { }

  signInWithFB(): void {
    this.social.signIn(FacebookLoginProvider.PROVIDER_ID).then((r) => {
      console.log(r);
      this.authService.facebookLogin(r.email, r.firstName, r.lastName, r.photoUrl).subscribe(
        data => {
          this.router.navigate(['/ho1a/candidate']);
        },
        error => {
          this.error = 'Usuario/Contraseña incorrecta';
          this.textButtom = 'Entrar';
          this.disabledButtom = false;
        }
      );
    });
    console.log(localStorage);
  }
  signInWithGoogle(): void {
    this.social.signIn(GoogleLoginProvider.PROVIDER_ID).then((r) => {
      this.authService.facebookLogin(r.email, r.firstName, r.lastName, r.photoUrl).subscribe(
        data => {
          this.router.navigate(['/ho1a/candidate']);
        },
        error => {
          this.error = 'Usuario/Contraseña incorrecta';
          this.textButtom = 'Entrar';
          this.disabledButtom = false;
        }
      );
    });
  }

  signInWithLinkedin(): void {
    this.authService.getUrlLogin();
  }

  ngOnInit() {
    this.urlLogin = this.authService.getUrlLogin();
  }

  disabled() {
    return this.disabledButtom ? 'disabled' : null;
  }

  onKeydown(event: any) {
    if (event.keyCode === 13) {
      this.loginUser();
    }
  }

  loginUser() {
    if (this.disabledButtom) {
      return;
    }
    this.error = '';
    this.textButtom = 'Cargando...';
    this.disabledButtom = true;
    this.authService.loginEmailPassword(this.form.get('email').value, this.form.get('password').value)
      .subscribe(
        data => {
          this.router.navigate(['/ho1a/candidate']);
        },
      error => {
          this.error = 'Usuario/Contraseña incorrecta';
          this.textButtom = 'Entrar';
          this.disabledButtom = false;
        }
      );
  }

}
