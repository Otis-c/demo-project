import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RequisitionsService {
  baseUrl = environment.baseUrl;

constructor(private http: HttpClient) { }

  getUsers() {
    return this.http.get(this.baseUrl + '/requisitions/getRequisitions');
  }

  getReq(id) {
    return this.http.get(this.baseUrl + '/requisitions/getRequisition/' + id );
  }

  getQuotes(id) {
    return this.http.get(this.baseUrl + '/requisitions/getQuotations/' + id );
  }

  saveRequisition(model: any) {
    return this.http.post(this.baseUrl + '/requisitions/CreateRequisition' , model);
  }

  public downloadFile(filename: string) {
    return this.http.get( this.baseUrl + '/requisitions/downloadQuote/' + filename, {
      responseType: 'arraybuffer'
    });
  }

  public approveQuote(id, reqid) {
    return this.http.get( this.baseUrl + '/requisitions/approveQuote/' + id + '/' + reqid);
  }

  public authoriseReq(id) {
    return this.http.get( this.baseUrl + '/requisitions/authoriseReq/' + id);
  }

}
