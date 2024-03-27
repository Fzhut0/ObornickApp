import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Category } from '../_models/category';
import { Link } from '../_models/link';

@Injectable({
  providedIn: 'root'
})
export class LinksService {

  baseUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  addLink(model: any)
  {
    return this.httpClient.post(this.baseUrl + 'CheckLaterLink/addlink', model, {responseType: 'text'});
  }

  deleteLink(model: string)
  {
    const params = new HttpParams({
      fromObject: {
        name: model
      }
    })
    return this.httpClient.delete(this.baseUrl + 'CheckLaterLink/deletelink', {params: params})
  }

  markLinkAsWatched(link: Link)
  {
    return this.httpClient.put(this.baseUrl + 'CheckLaterLink/setlinkviewed', link);
  }
 
}
