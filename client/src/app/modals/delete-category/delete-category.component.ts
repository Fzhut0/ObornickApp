import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Category } from 'src/app/_models/category';
import { CategoryService } from 'src/app/_services/category.service';

@Component({
  selector: 'app-delete-category',
  templateUrl: './delete-category.component.html',
  styleUrls: ['./delete-category.component.css']
})
export class DeleteCategoryComponent implements OnInit {
  category: Category | null = null;

  constructor(public bsModalRef: BsModalRef, private categoryService: CategoryService) {}

  ngOnInit(): void {
    
  }

  removeCategory()
  {
    if (!this.category)
    {
      return;
      }
    this.categoryService.deleteCategory(this.category.categoryId).subscribe({
      next: () => {
        this.bsModalRef.hide(),
        this.categoryService.fetchCategories()
      }
    })
  }
}
