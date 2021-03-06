import { Injectable } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Router, CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})

export class AuthGuard implements CanActivate  {

  constructor(private authSvc: AuthService, private router: Router,
              private alertify: AlertifyService) { }

  canActivate(next: ActivatedRouteSnapshot): boolean {
    const roles = next.firstChild.data['roles'] as Array<string>;
    if (roles) {
      const match = this.authSvc.roleMatch(roles);
      if (match) {
        return true;
      } else {
        this.router.navigate(['/']);
        this.alertify.error('You are authorised to view this page');
      }
    }
    if (this.authSvc.loggedIn) {
      return true;
    }

    this.alertify.error('You are authorised to view this page');
    this.router.navigate(['/home']);
    return false;
  }
}
