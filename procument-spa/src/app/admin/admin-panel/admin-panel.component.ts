import { Component, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';

import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import { RolesModalComponent } from '../roles-modal/roles-modal.component';
import { AlertifyService } from 'src/app/_services/alertify.service';


@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
 users: User[];
 bsModalRef: BsModalRef;
  constructor(private adminSvc: AdminService, private modalService: BsModalService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.getUser();
  }
  getUser() {
    this.adminSvc.getUsers().subscribe((users: User[]) => {
      this.users = users;
      console.log(users);
    }, error => {
      this.alertify.error(error);
    });
  }

  editRolesModal(user: User) {
    const initialState = {
      user,
      roles: this.getRoles(user)
    };
    this.bsModalRef = this.modalService.show(RolesModalComponent, {initialState});
    this.bsModalRef.content.updateSelectedRoles.subscribe((values) => {
      const rolesToUpdate = {
      roleNames: [...values.filter(el => el.checked === true ).map(el => el.name)]
      };
      if ( rolesToUpdate) {
        console.log(rolesToUpdate.roleNames);
        let role = rolesToUpdate.roleNames[0];
        this.adminSvc.editUserRole(user, role).subscribe(() => {
          this.alertify.success('Updated user role');
          this.ngOnInit();
        }, error => {
          this.alertify.error(error);
        });
      }
    });

  }

  private getRoles(user) {
    const roles = [];
    const userRole = user.role;
    const availableRoles: any[] = [
      {name: 'Admin', value: 'Admin'},
      {name: 'User', value: 'User'},
      {name: 'Approver', value: 'Approver'},
      {name: 'Authoriser', value: 'Authoriser'},
      {name: 'Vendor', value: 'Vendor'},
    ];

    for (let i = 0; i < availableRoles.length; i++) {
      let isMatch = false;
      if (availableRoles[i].name === userRole) {
        isMatch = true;
        availableRoles[i].checked = true;
        roles.push(availableRoles[i]);
      }
      if (!isMatch) {
        availableRoles[i].checked = false;
        roles.push(availableRoles[i]);
      }
    }
    return roles;
  }
}
