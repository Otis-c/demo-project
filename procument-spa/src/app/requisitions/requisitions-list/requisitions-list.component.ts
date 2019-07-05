import { Component, OnInit } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-requisitions-list',
  templateUrl: './requisitions-list.component.html',
  styleUrls: ['./requisitions-list.component.css']
})
export class RequisitionsListComponent implements OnInit {
  model: any = {};
  reqItem: any = {};
  reqItems: any = [];
  values: [];
  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
    const token = localStorage.getItem('token');
    this.http.get('http://localhost:5000/api/Requisitions/GetRequisitions')
    .subscribe((response: any) => {
      this.values = response;
    }, err => {
      console.log(err);
    });
  }

  submitQuote(id: any) {
    console.log(id);
    this.router.navigate(['submitQuote', id]);
  }

}
