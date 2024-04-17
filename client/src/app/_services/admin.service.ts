import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  getUsers()
  {
    return this.httpClient.get<User[]>(this.baseUrl + 'user/getusers');
  }


  updateUserFacebookId(id: string, username: string)
  {
    const requestBody = {
      MessageServiceRecipientId: id,
      Username: username
    }
    console.log(requestBody)
    return this.httpClient.put(this.baseUrl + 'admin/addfacebookid', requestBody, {responseType: 'text'});
  }

  clearUserFacebookId(username: string)
  {
    const requestBody = {
      Username: username
    }
    return this.httpClient.put(this.baseUrl + 'admin/clearfacebookid', requestBody, {responseType: 'text'});
  }
}
