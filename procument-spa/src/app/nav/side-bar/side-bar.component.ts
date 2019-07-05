import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.css']
})
export class SideBarComponent implements OnInit {

  constructor(public authSvc: AuthService) { }

  ngOnInit() {
  }

  loggedIn() {
    return this.authSvc.loggedIn();
  }

  logOut() {
    this.authSvc.logOut();
  }
}
