import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Category } from '../_models/category';

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

  getCategories()
  {
    return this.httpClient.get<Category[]>(this.baseUrl + 'CheckLaterLinksCategories/getcategories');
  }

  
}
