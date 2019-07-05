import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { RequisitionsService } from 'src/app/_services/requisitions.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-requisition',
  templateUrl: './create-requisition.component.html',
  styleUrls: ['./create-requisition.component.css']
})
export class CreateRequisitionComponent implements OnInit {
  model: any = {};
  reqItem: any = {};
  reqItems: any = [];

  constructor(private reqSvc: RequisitionsService, private alertify: AlertifyService,
              private router: Router) { }

  ngOnInit() {
    this.model = { description: '', summary: '' };
  }
  saveRequisition() {
    console.log(this.model);
    this.reqSvc.saveRequisition(this.model).subscribe(() => {
      this.alertify.success('Requisition Saved successfully');
      this.router.navigate(['requisitions']);
    }, error => {
      console.log(error);
      this.alertify.error(error);
    });
  }

  }
