import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Category } from 'src/app/_models/category';
import { Link } from 'src/app/_models/link';
import { CategoryService } from 'src/app/_services/category.service';
import { LinksService } from 'src/app/_services/links.service';

@Component({
  selector: 'app-change-link-category',
  templateUrl: './change-link-category.component.html',
  styleUrls: ['./change-link-category.component.css']
})
export class ChangeLinkCategoryComponent implements OnInit {
  selectedCategory: Category | null = null;
  category: Category | null = null;
  categories: Category[] = [];
  link: Link | null = null;

  constructor(public bsModalRef: BsModalRef, private categoryService: CategoryService, private linksService: LinksService) {}

  ngOnInit(): void {
    this.getCategories();
    this.categoryService.categorySelectedEvent.subscribe((data: Category) => {
      this.selectedCategory = data;
    })
  }

  getCategories() {
    this.categoryService.getCategories().subscribe({
      next: response => {
        this.categories = response
        this.categories.forEach(category => {
          this.getSubcategories(category)
        })
      }   
    })
  }

  getSubcategories(category: Category)
  {
    this.categoryService.getSubcategories(category.categoryId).subscribe({
      next: (response: Category[]) => {
        category.subcategories = response;
        category.subcategories.forEach(subcategory => {
          this.getSubcategories(subcategory)
        });
        
      }
    })
  }

  updateLinkCategory(link: Link, newCategoryId: number)
  {
    this.linksService.updateLinkCategory(link, newCategoryId).subscribe({
      next: () => {
        this.bsModalRef.hide()
      }
    })
  }
}
