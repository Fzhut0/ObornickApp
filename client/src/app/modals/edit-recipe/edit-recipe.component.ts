import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { Route, Router } from '@angular/router';
import { RecipesService } from 'src/app/_services/recipes.service';
import { Ingredient } from 'src/app/_models/ingredient';
import { Recipe } from 'src/app/_models/recipe';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-edit-recipe',
  templateUrl: './edit-recipe.component.html',
  styleUrls: ['./edit-recipe.component.css']
})
export class EditRecipeComponent implements OnInit {
  baseUrl = environment.apiUrl;
  @Output() cancelRecipe = new EventEmitter();

  recipeForm: FormGroup = new FormGroup({})

  recipeName: any;
  ingredients: Ingredient[] = [];

  selectedRecipe!: Recipe;

  constructor(private recipeService: RecipesService, private fb: FormBuilder, public bsModalRef: BsModalRef) {

  }

  ngOnInit(): void {
    this.initializeForm();
    this.loadIngredientsIntoForm();
  }

  loadIngredientsIntoForm()
  {
    if (this.ingredients.length > 0)
    {
      for (let i = 0; i < this.ingredients.length; i++) {
        this.addExistingIngredient(this.ingredients[i])
        }
      }
  }

  initializeForm()
  {
    this.recipeForm = this.fb.group({
      originalName: this.recipeName,
      name: [this.recipeName, Validators.required],
      ingredients: this.fb.array([])
    });
  }

    get ingredientsArray() {
    return this.recipeForm.get('ingredients') as FormArray;
  }

  addIngredientInForm()
  {
    this.ingredientsArray.push(this.fb.group({
      ingredientName: ['', Validators.required],
      quantity: ['', Validators.required]
    }));
  }

  addExistingIngredient(ing: Ingredient)
  {
     this.ingredientsArray.push(this.fb.group({
      ingredientName: [ing.ingredientName, Validators.required],
      quantity: [ing.quantity, Validators.required]
    }));
  }

  removeIngredient(index: number) {
    this.ingredientsArray.removeAt(index);
  }

  editRecipe()
  {
    const values = { ...this.recipeForm.value };
    const newName = values.name;
    console.log(values);

    this.recipeService.editRecipe(values).subscribe({
      next: () => {
        if (newName != this.recipeName)
        {
          this.recipeName = newName;
          }
        this.cancel();
      },
      error: error => {
        console.log(error)
        }
      })
  }

    resetForm() {
    this.recipeForm.reset();
    this.ingredientsArray.clear();
  }

  cancel() {
    this.bsModalRef.hide(); 
  }
}
