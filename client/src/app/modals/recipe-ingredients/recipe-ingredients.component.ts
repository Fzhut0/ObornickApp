import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Ingredient } from 'src/app/_models/ingredient';
import { Recipe } from 'src/app/_models/recipe';
import { RecipesService } from 'src/app/_services/recipes.service';

@Component({
  selector: 'app-recipe-ingredients',
  templateUrl: './recipe-ingredients.component.html',
  styleUrls: ['./recipe-ingredients.component.css']
})
export class RecipeIngredientsComponent implements OnInit {
  recipeName: any;
  ingredients: Ingredient[] = [];

  constructor(private recipeService: RecipesService, public bsModalRef: BsModalRef) {
   
  }

  ngOnInit(): void {
    this.listRecipeIngredients(this.recipeName);
  }

  listRecipeIngredients(name: string)
  {
    this.recipeService.getRecipeIngredients(name).subscribe({
      next: response => this.ingredients = response
    })
  }

  sendIngredientsAsList()
  {
    console.log('wysyłam wiadomość')
  }

}

