import { HttpClient, HttpParams } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Category } from '../_models/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  baseUrl = environment.apiUrl;

  @Output() categorySelectedEvent = new EventEmitter<Category>();

  constructor(private httpClient: HttpClient) { }

  getCategories()
  {
    // const params = new HttpParams({
    //   fromObject: {
    //     username: username
    //   }
    // })
    return this.httpClient.get<Category[]>(this.baseUrl + 'CheckLaterLinksCategories/getcategories');
  }

  getSubcategories(parentCategoryName: string)
  {
    const params = new HttpParams({
      fromObject: {
        parentCategoryName: parentCategoryName
      }
    })

    return this.httpClient.get<Category[]>(this.baseUrl + 'CheckLaterLinksCategories/getsubcategories', {params: params});
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

  deleteCategory(name: string) {
    const params = new HttpParams({
      fromObject: {
        name: name
      }
    })
    return this.httpClient.delete(this.baseUrl + 'CheckLaterLinksCategories/deletecategory', { params: params })
  }

  addSubcategory(subcategoryName: string, parentCategory: string, username: string) {
    const requestBody = {
      username: username,
      customName: subcategoryName,
      parentCategoryName: parentCategory
    };
    return this.httpClient.post(`${this.baseUrl}CheckLaterLinksCategories/addsubcategory`, requestBody, { responseType: 'text' });
  }

  categorySelected(category: Category)
  {
    this.categorySelectedEvent.emit(category);
  }

}

