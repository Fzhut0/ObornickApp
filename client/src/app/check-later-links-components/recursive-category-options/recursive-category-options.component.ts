import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Category } from 'src/app/_models/category';
import { CategoryService } from 'src/app/_services/category.service';

@Component({
  selector: 'app-recursive-category-options',
  templateUrl: './recursive-category-options.component.html',
  styleUrls: ['./recursive-category-options.component.css']
})
export class RecursiveCategoryOptionsComponent {

  @Input() categories: Category[] = [];
  

  selectedCategory: Category | null = null;
  selectedSubcategory: Category | null = null;

  oneAtATime = true;

  shouldBeDisabled = false;

  constructor(private categoryService: CategoryService) {}

  selectCategory(category: Category)
  {
    this.categoryService.categorySelected(category);
    console.log(category)
  }

}
