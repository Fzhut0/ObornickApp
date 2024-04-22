import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { Route, Router } from '@angular/router';
import { RecipesService } from 'src/app/_services/recipes.service';
import { Ingredient } from 'src/app/_models/ingredient';
import { Recipe } from 'src/app/_models/recipe';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { RecipeDescriptionStep } from 'src/app/_models/recipedescriptionstep';

@Component({
  selector: 'app-edit-recipe',
  templateUrl: './edit-recipe.component.html',
  styleUrls: ['./edit-recipe.component.css']
})
export class EditRecipeComponent implements OnInit {
  baseUrl = environment.apiUrl;
  @Output() cancelRecipe = new EventEmitter();

  recipeForm: FormGroup = new FormGroup({})

  recipeName: string = '';
  recipeId: number = 0;
  ingredients: Ingredient[] = [];
  descriptionSteps: RecipeDescriptionStep[] = [];

  selectedRecipe!: Recipe;

  constructor(private recipeService: RecipesService, private fb: FormBuilder, public bsModalRef: BsModalRef) {

  }

  ngOnInit(): void {
    this.initializeForm();
    this.loadIngredientsIntoForm();
    this.loadStepsIntoForm();
    console.log(this.descriptionSteps)
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

  loadStepsIntoForm()
  {
      if (this.descriptionSteps.length > 0)
      {
        for (let i = 0; i < this.descriptionSteps.length; i++)
        {
          this.addExistingDesciptionStep(this.descriptionSteps[i])
          }
        }
  }

  addExistingDesciptionStep(desc: RecipeDescriptionStep)
  {
    this.stepsArray.push(this.fb.group({
      description: [desc.description, Validators.required],
    }));
  }

  initializeForm()
  {
    this.recipeForm = this.fb.group({
      originalName: this.recipeName,
      name: [this.recipeName, Validators.required],
      ingredients: this.fb.array([]),
      recipeId: this.recipeId,
      recipeDescriptionSteps: this.fb.array([])
    });
  }

    get ingredientsArray() {
    return this.recipeForm.get('ingredients') as FormArray;
  }

  get stepsArray(): FormArray {
    return this.recipeForm.get('recipeDescriptionSteps') as FormArray;
  }

  addExistingStep(description: string): void {
    this.stepsArray.push(
      this.fb.group({
        description: [description, Validators.required]
      })
    );
  }

  addStepInForm(): void {
    this.stepsArray.push(this.fb.group({ description: ['', Validators.required] }));
  }

  removeStep(index: number): void {
    this.stepsArray.removeAt(index);
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

    this.recipeService.editRecipe(values).subscribe({
      next: () => {
        if (newName != this.recipeName)
        {
          this.recipeName = newName;
          }
        this.cancel();
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
