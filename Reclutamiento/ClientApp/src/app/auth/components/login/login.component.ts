import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '@ngxs/store';
import { Loader } from 'src/app/store/actions/loader.actions';
import { AuthService } from '../../services/auth.service';


@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {

  constructor(
    private store: Store,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    this.loginUser('jtiradol');
  }

  showInfo() {
    this.store.dispatch(new Loader.Toggle);
  }

  loginUser(user: string) {
    this.authService.loginUser(user)
      .subscribe(
        data => {
          if (!data) {
            this.router.navigate(['/notauthorized']);
            return;
          }
          this.router.navigate(['/home']);
        },
        error => {
          this.router.navigate(['/notauthorized']);
        }
      );
  }
}
