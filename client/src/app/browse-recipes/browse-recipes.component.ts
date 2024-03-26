import { Component, OnInit } from '@angular/core';
import { RecipesService } from '../_services/recipes.service';
import { RecipeIngredientsComponent } from '../modals/recipe-ingredients/recipe-ingredients.component';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Recipe } from '../_models/recipe';

@Component({
  selector: 'app-browse-recipes',
  templateUrl: './browse-recipes.component.html',
  styleUrls: ['./browse-recipes.component.css']
})
export class BrowseRecipesComponent implements OnInit {
  bsModalRef: BsModalRef<RecipeIngredientsComponent> = new BsModalRef<RecipeIngredientsComponent>();
  recipes: any;

  constructor(private recipeService: RecipesService, private modalService: BsModalService)
  {

  }

  ngOnInit(): void {
    this.listAddedRecipes();
  }

  listAddedRecipes()
  {
    this.recipeService.getAllRecipes().subscribe({
      next: response => this.recipes = response
    });
  }

  openRolesModal(recipe: Recipe)
  {
    const config =
    {
      initialState: {
        
        recipeName: recipe.name,
        selectedRecipe: recipe
        }
      }
    this.bsModalRef = this.modalService.show(RecipeIngredientsComponent, config);
    this.bsModalRef.onHide?.subscribe({
      next: () => {
        this.listAddedRecipes()
      }
    })
  }

}
