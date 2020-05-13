import { Component, OnInit } from '@angular/core';
import { AuthService, FacebookLoginProvider, SocialUser } from 'angularx-social-login';


@Component({
  selector: 'ho1a-facebook',
  templateUrl: './facebook.component.html',
  styleUrls: ['./facebook.component.scss']
})
export class FacebookComponent implements OnInit {

  title = 'angular-fblogin';
  user: SocialUser;
  loggedIn: boolean;

  constructor(private authService: AuthService) { }

  signInWithFB(): void {
    this.authService.signIn(FacebookLoginProvider.PROVIDER_ID).then((r) => {
      console.log(r);
    });
    console.log(localStorage);
  }

  signOut(): void {
    this.authService.signOut();
  }

  ngOnInit() {
    if (localStorage.fbToken) {
      this.loggedIn = true;
    }
    this.authService.authState.subscribe((user) => {
      this.user = user;
      this.loggedIn = (user != null);
      console.log(this.user);
    });
  }
}