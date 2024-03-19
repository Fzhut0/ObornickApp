import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Ingredient } from 'src/app/_models/ingredient';
import { Recipe } from 'src/app/_models/recipe';
import { MessagesService } from 'src/app/_services/messages.service';
import { RecipesService } from 'src/app/_services/recipes.service';

@Component({
  selector: 'app-recipe-ingredients',
  templateUrl: './recipe-ingredients.component.html',
  styleUrls: ['./recipe-ingredients.component.css']
})
export class RecipeIngredientsComponent implements OnInit {
  recipeName: any;
  ingredients: Ingredient[] = [];
  message: string = '';

  constructor(private recipeService: RecipesService, public bsModalRef: BsModalRef, private messagesService: MessagesService) {
   
  }

  ngOnInit(): void {
    this.listRecipeIngredients(this.recipeName);
  }

  listRecipeIngredients(name: string) {
    this.recipeService.getRecipeIngredients(name).subscribe({
      next: response => this.ingredients = response
    })
  }

  sendIngredientsAsList() {
    var message = '';

    if (this.ingredients.length > 0 && this.recipeName.length > 0)
    {
      message = `Do zrobienia ${this.recipeName} potrzebujesz:\\n`   
    }
      for (let i = 0; i < this.ingredients.length; i++)
      {
        message = message.concat(`${this.ingredients[i].ingredientName} ${this.ingredients[i].quantity}\\n`);
      }
    
    message = encodeURIComponent(message);

    console.log(message);
    

    this.messagesService.sendMessage(message).subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    });
  }

}

