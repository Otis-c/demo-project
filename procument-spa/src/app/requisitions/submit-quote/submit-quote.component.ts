import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RequisitionsService } from 'src/app/_services/requisitions.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { Quote } from 'src/app/_models/quote';
import { Observable } from 'rxjs';
import { saveAs } from 'file-saver';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-submit-quote',
  templateUrl: './submit-quote.component.html',
  styleUrls: ['./submit-quote.component.css']
})
export class SubmitQuoteComponent implements OnInit {
  req: any = {};
  model: any = {};
  progress: number;
  message: string;
  quotes: Quote[];

  @Output() public onUploadFinished = new EventEmitter();

  constructor(private route: ActivatedRoute,
              private reqSvc: RequisitionsService,
              private alertify: AlertifyService,
              private authSvc: AuthService,
              private http: HttpClient) { }

  ngOnInit() {
    const reqId = this.route.snapshot.paramMap.get('id');
    this.reqSvc.getReq(reqId).subscribe((req: any) => {
      this.req = req;
      this.getQuotations(this.req.id);
    }, error => {
      this.alertify.error(error);
    });
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }

    const fileToUpload =  files[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    formData.append('reqId', this.req.id);
    formData.append('cost', this.model.cost);
    formData.append('summary', this.model.summary);
    formData.append('description', this.model.description);

    this.http.post('http://localhost:5000/api/requisitions/SubmitQuote', formData, { reportProgress: true, observe: 'events' })
    .subscribe(event => {
      if (event.type === HttpEventType.UploadProgress) {
        this.progress = Math.round(100 * event.loaded / event.total);
      } else if (event.type === HttpEventType.Response) {
        this.alertify.success('Quotation uploaded successfully');
        this.onUploadFinished.emit(event.body);
      }
    });
  }

  getQuotations(id) {
    if (this.authSvc.roleMatch(['Approver', 'Authoriser'])) {
      this.reqSvc.getQuotes(id).subscribe((quotes: Quote[]) => {
        this.quotes = quotes;
        console.log(quotes);
      }, error => {
        this.alertify.error(error);
      });
    }
  }

  downloadFile(filename) {
    this.reqSvc.downloadFile(filename)
      .subscribe((data: any) => {
        saveAs(new Blob([data], {type: 'txt'}), filename);
      });
  }

  approveQuote(id, reqId) {
    this.reqSvc.approveQuote(id, reqId)
      .subscribe((data: number) => {
        if (data) {
          this.alertify.success('Quotation Approved successfully');
          this.ngOnInit();
        }
      });
  }

  authoriseRequisition(id) {
    this.reqSvc.authoriseReq(id)
      .subscribe((data: number) => {
        if (data) {
          this.alertify.success('Requisition Authorised successfully');
          this.ngOnInit();
        }
      });
  }



}
