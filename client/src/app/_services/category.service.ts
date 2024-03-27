import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Category } from '../_models/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  baseUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  getCategories()
  {
    return this.httpClient.get<Category[]>(this.baseUrl + 'CheckLaterLinksCategories/getcategories');
  }

  addCategory(name: string)
  {
    const category: Category = { customName: name, links: [] };
    return this.httpClient.post(this.baseUrl + 'CheckLaterLinksCategories/addcategory', category, {responseType: 'text'})
  }

}
