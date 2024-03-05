import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'Hello!';
  users: any;

  constructor( private http: HttpClient) {}

  ngOnInit(): void {
  
    this.http.get('https://localhost:5001/api/User').subscribe({
      next: response => this.users = response
    })
  }


}
