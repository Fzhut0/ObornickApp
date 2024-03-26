import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Ingredient } from 'src/app/_models/ingredient';
import { Recipe } from 'src/app/_models/recipe';
import { AccountService } from 'src/app/_services/account.service';
import { MessagesService } from 'src/app/_services/messages.service';
import { RecipesService } from 'src/app/_services/recipes.service';
import { EditRecipeComponent } from '../edit-recipe/edit-recipe.component';

@Component({
  selector: 'app-recipe-ingredients',
  templateUrl: './recipe-ingredients.component.html',
  styleUrls: ['./recipe-ingredients.component.css']
})
export class RecipeIngredientsComponent implements OnInit {
  recipeName: any;
  ingredients: Ingredient[] = [];
  message: string = '';
  selectedRecipe!: Recipe;

  constructor(private recipeService: RecipesService, public bsModalRef: BsModalRef, private messagesService: MessagesService, private modalService: BsModalService) {
   
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

    if (this.ingredients.length > 0 && this.recipeName.length > 0) {
      message = `Do zrobienia ${this.recipeName} potrzebujesz:\\n`
    }
    for (let i = 0; i < this.ingredients.length; i++) {
      message = message.concat(`${this.ingredients[i].ingredientName} ${this.ingredients[i].quantity}\\n`);
    }
    
    message = encodeURIComponent(message);

    console.log(message);
    
    this.messagesService.sendMessage(message).subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    });
  }

  openEditRecipeModal(recipe: Recipe) {
    const config =
    {
      initialState: {
        
        recipeName: recipe.name,
        selectedRecipe: recipe,
        ingredients: this.ingredients
      }
    }
    const modalRef = this.modalService.show(EditRecipeComponent, config);
    modalRef.onHide?.subscribe({
      next: () => {
        this.listRecipeIngredients(modalRef.content?.recipeName)
      }
    })
  }

  deleteRecipe(recipe: Recipe)
  {
    this.recipeService.deleteRecipe(recipe.name).subscribe({
      next: () => {
        this.bsModalRef.hide()
        this.bsModalRef.onHide?.subscribe({
          next: () => {
            window.location.reload();
          }
        })
      }
    });
  }
}

