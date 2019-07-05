import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/user';
import { RoleUpdateModel } from '../_models/role-update-model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.baseUrl;

constructor(private http: HttpClient) { }
model: RoleUpdateModel

  getUsers() {
    return this.http.get(this.baseUrl + '/admin/getUsers');
  }

  editUserRole(user: User, role: string) {

    this.model = {username: user.userName, role: role};
    console.log(this.model);
    return this.http.post(this.baseUrl + '/admin/editUserRole',  this.model);
  }

  getLogs() {
    return this.http.get(this.baseUrl + '/admin/getLogs');
  }
}
