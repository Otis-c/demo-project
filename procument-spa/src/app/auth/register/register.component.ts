import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { RegisterModel } from 'src/app/_models/register-model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: RegisterModel;

  constructor(private authSvc: AuthService, private router: Router,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.model = { email: '', password: '', confirmPassword: '', firstName: '',
                   lastName: '', companyName: '', phoneNo: '', role: ''};
  }

  register() {
    this.authSvc.register(this.model).subscribe(() => {
      this.alertify.success('registration successful, check you email to activate your account before logging in');
      this.router.navigate(['/home']);
    }, error => {
      this.alertify.error(error);
    });
  }
}
