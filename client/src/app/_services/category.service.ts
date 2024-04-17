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
  @Output() fetchCategoriesEvent = new EventEmitter();

  constructor(private httpClient: HttpClient) { }

  getCategories()
  {
    return this.httpClient.get<Category[]>(this.baseUrl + 'CheckLaterLinksCategories/getcategories');
  }

  getSubcategories(parentCategoryId: number)
  {
    const params = new HttpParams({
      fromObject: {
        parentCategoryId: parentCategoryId
      }
    })

    return this.httpClient.get<Category[]>(this.baseUrl + 'CheckLaterLinksCategories/getsubcategories', {params: params});
  }

  addCategory(categoryName: string)
  {
    const requestBody = {
      customName: categoryName,
      links: []
    }
    return this.httpClient.post(this.baseUrl + 'CheckLaterLinksCategories/addcategory', requestBody, {responseType: 'text'})
  }

  deleteCategory(categoryId: number) {
    const params = new HttpParams({
      fromObject: {
        categoryId: categoryId
      }
    })
    return this.httpClient.delete(this.baseUrl + 'CheckLaterLinksCategories/deletecategory', { params: params })
  }

  addSubcategory(subcategoryName: string, id: number) {
    const requestBody = {
      customName: subcategoryName,
      categoryId: id 
    };
    return this.httpClient.post(`${this.baseUrl}CheckLaterLinksCategories/addsubcategory`, requestBody, { responseType: 'text'});
  }

  categorySelected(category: Category)
  {
    this.categorySelectedEvent.emit(category);
  }

  fetchCategories()
  {
    this.fetchCategoriesEvent.emit();
  }

}

