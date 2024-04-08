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

  getCategories(username: string)
  {
    const params = new HttpParams({
      fromObject: {
        username: username
      }
    })
    return this.httpClient.get<Category[]>(this.baseUrl + 'CheckLaterLinksCategories/getcategories', {params: params});
  }

  addCategory(categoryName: string, username: string)
  {
    //const category: Category = { customName: name, links: []};
    const requestBody = {
      username: username,
      customName: categoryName,
      links: []
    }
    return this.httpClient.post(this.baseUrl + 'CheckLaterLinksCategories/addcategory', requestBody, {responseType: 'text'})
  }

  deleteCategory(name: string, username: string)
  {
    const params = new HttpParams({
      fromObject: {
        name: name,
        username: username
      }
    })
    return this.httpClient.delete(this.baseUrl + 'CheckLaterLinksCategories/deletecategory', {params: params})
  }

}
