import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Recipe } from '../_models/recipe';
import { Ingredient } from '../_models/ingredient';

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

  getRecipeIngredients(model: string)
  {
    const params = new HttpParams({
      fromObject: {
        name: model
      }
    })

    return this.httpClient.get<Ingredient>(this.baseUrl + 'Ingredients/getrecipeingredients', {params: params})
  }
}
