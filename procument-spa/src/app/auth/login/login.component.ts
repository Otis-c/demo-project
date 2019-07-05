import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from 'src/app/_services/auth.service';
import { LoginModel } from 'src/app/_models/login-model';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  model: LoginModel;
  constructor(private authSvc: AuthService, private router: Router, private alertify: AlertifyService) { }

  ngOnInit() {
    this.model = { username: '', password: '' };
  }

  login() {
    this.authSvc.login(this.model).subscribe(next => {

      if ( this.authSvc.roleMatch(['Admin'])) {
        this.router.navigate(['admin']);
      } else  {
        this.router.navigate(['requisitions']);
      }
    }, error => {
      this.alertify.error(error);
    });
  }
}
