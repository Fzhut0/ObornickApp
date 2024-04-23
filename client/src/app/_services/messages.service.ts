import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
    baseUrl = environment.apiUrl;

    constructor(private httpClient: HttpClient) { }

    sendMessage(message: string): Observable<any> {
      const headers = new HttpHeaders().set('Content-Type', 'application/json');
      const body = JSON.stringify({ message: message }); 

      return this.httpClient.post(this.baseUrl + 'FacebookMessage/sendmessage', body, { headers: headers, responseType: 'text'} );
    }
  }
    


