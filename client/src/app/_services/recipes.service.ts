import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Recipe } from '../_models/recipe';

@Injectable({
  providedIn: 'root'
})
export class RecipesService {
  baseUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  addRecipe(model: any)
  {
    return this.httpClient.post<Recipe>(this.baseUrl + 'recipes/addrecipe', model);
  }

  getAllRecipes()
  {
    return this.httpClient.get<Recipe>(this.baseUrl + 'recipes/getrecipes');
  }
}
