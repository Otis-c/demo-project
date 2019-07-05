import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/_services/admin.service';
import { LogModel } from 'src/app/_models/log-model';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-view-logs',
  templateUrl: './view-logs.component.html',
  styleUrls: ['./view-logs.component.css']
})
export class ViewLogsComponent implements OnInit {
  logs: LogModel[];

  constructor(private adminSvc: AdminService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.adminSvc.getLogs().subscribe((logs: LogModel[]) => {
      this.logs = logs;
      console.log(logs);
    }, error => {
      this.alertify.error(error);
    });
  }
}
