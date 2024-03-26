import { Component, EventEmitter, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RecipesService } from 'src/app/_services/recipes.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-add-recipes',
  templateUrl: './add-recipes.component.html',
  styleUrls: ['./add-recipes.component.css']
})
export class AddRecipesComponent {
  baseUrl = environment.apiUrl;
  @Output() cancelRecipe = new EventEmitter();

  recipeForm: FormGroup = new FormGroup({})

  constructor(private recipeService: RecipesService, private fb: FormBuilder) {

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
