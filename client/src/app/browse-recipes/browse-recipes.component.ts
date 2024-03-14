import { Component, OnInit } from '@angular/core';
import { RecipesService } from '../_services/recipes.service';

@Component({
  selector: 'app-browse-recipes',
  templateUrl: './browse-recipes.component.html',
  styleUrls: ['./browse-recipes.component.css']
})
export class BrowseRecipesComponent implements OnInit {

  recipes: any;

  constructor(private recipeService: RecipesService)
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




}
