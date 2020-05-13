import { Component, OnInit } from '@angular/core';
import {AuthService} from '../../../shared/services/auth/auth.service';
import {Router} from '@angular/router';

function getParameterByName(name, url?) {
  if (!url) url = window.location.href;
  name = name.replace(/[\[\]]/g, "\\$&");
  var regex = new RegExp("[?&#]" + name + "(=([^&#]*)|&|#|$)"),
    results = regex.exec(url);
  if (!results) return null;
  if (!results[2]) return '';
  return decodeURIComponent(results[2].replace(/\+/g, " "));
}

@Component({
  selector: 'ho1a-linkedin',
  templateUrl: './linkedin.component.html',
  styleUrls: ['./linkedin.component.scss']
})
export class LinkedinComponent implements OnInit {

  constructor(
    private service: AuthService,
    private router: Router,
  ) { }

  ngOnInit() {
    const accessToken = getParameterByName('code');

    if (!accessToken) {
      this.router.navigate(['/auth/login']);
    }

    this.service.linkedInLogin(accessToken).subscribe(next => {
      this.router.navigate(['/ho1a/candidate']);
      },
      error => {
        this.router.navigate(['/auth/login']);
      });
  }

}
