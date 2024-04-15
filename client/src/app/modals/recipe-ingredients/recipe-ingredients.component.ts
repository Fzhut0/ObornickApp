import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Ingredient } from 'src/app/_models/ingredient';
import { Recipe } from 'src/app/_models/recipe';
import { AccountService } from 'src/app/_services/account.service';
import { MessagesService } from 'src/app/_services/messages.service';
import { RecipesService } from 'src/app/_services/recipes.service';
import { EditRecipeComponent } from '../edit-recipe/edit-recipe.component';
import { User } from 'src/app/_models/user';

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
  user: User | undefined;
  recipeId: number = 0;

  userHasRecipe: boolean = false;

  constructor(private recipeService: RecipesService, public bsModalRef: BsModalRef, private messagesService: MessagesService,
    private modalService: BsModalService, public accountService: AccountService) {
   
  }

  ngOnInit(): void {
    this.listRecipeIngredients(this.recipeId);
    this.checkHasUserRecipe(this.recipeName);
  }

  listRecipeIngredients(recipeId: number) {
    this.recipeService.getRecipeIngredients(recipeId).subscribe({
      next: response => this.ingredients = response
    })
  }

  sendIngredientsAsList(username: string) {
    var message = '';

    if (this.ingredients.length > 0 && this.recipeName.length > 0) {
      message = `Do zrobienia ${this.recipeName} potrzebujesz:\\n`
    }
    for (let i = 0; i < this.ingredients.length; i++) {
      message = message.concat(`${this.ingredients[i].ingredientName} ${this.ingredients[i].quantity}\\n`);
    }
    
    message = encodeURIComponent(message);  
    
    this.messagesService.sendMessage(message, username).subscribe();
  }

  openEditRecipeModal(recipe: Recipe) {
    const config =
    {
      initialState: {
        
        recipeName: recipe.name,
        selectedRecipe: recipe,
        ingredients: this.ingredients,
        recipeId: recipe.recipeId
      }
    }
    const modalRef = this.modalService.show(EditRecipeComponent, config);
    modalRef.onHide?.subscribe({
      next: () => {
        this.listRecipeIngredients(modalRef.content!.recipeId)
        this.changeRecipeName(modalRef.content!.recipeName)
      }
    })
  }

  changeRecipeName(name: string)
  {
    this.recipeName = name;
  }

  deleteRecipe(name: string)
  {
    this.recipeService.deleteRecipe(name).subscribe({
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

  checkHasUserRecipe(name: string)
  {
    this.recipeService.checkUserRecipe(name).subscribe({
      next: response => {
        if (response == true)
        {
          this.userHasRecipe = true;
        }
        else {
          this.userHasRecipe = false;
        }
      }
    })
  }
}

