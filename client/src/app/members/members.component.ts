import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { RecipesService } from '../_services/recipes.service';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.css']
})
export class MembersComponent implements OnInit {
  baseUrl = environment.apiUrl;
  @Output() cancelRecipe = new EventEmitter();

  recipeForm: FormGroup = new FormGroup({})

  constructor(private recipeService: RecipesService, private fb: FormBuilder, private router: Router) {

  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm()
  {
    this.recipeForm = this.fb.group({
      name: ['', Validators.required],
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

  removeIngredient(index: number) {
    this.ingredientsArray.removeAt(index);
  }

  addRecipe()
  {
    const values = {...this.recipeForm.value};
    console.log(values);

      this.recipeService.addRecipe(values).subscribe({
      error: error => {
        console.log(error)
        }
      })
    this.resetForm();
  }

    resetForm() {
    this.recipeForm.reset();
    this.ingredientsArray.clear();
  }

  cancel() {
    this.recipeForm.reset();
    if (this.ingredientsArray.length > 0)
    {
      this.ingredientsArray.clear;
      for (let i = this.ingredientsArray.length - 1; i >= 0; i--)
      {
        this.ingredientsArray.removeAt(i);
        }
      }
    this.cancelRecipe.emit(false);
  }

}
