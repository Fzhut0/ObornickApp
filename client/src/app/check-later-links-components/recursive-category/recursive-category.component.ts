import { Component, Input } from '@angular/core';
import { Category } from 'src/app/_models/category';
import { CategoryService } from 'src/app/_services/category.service';
import { LinksService } from 'src/app/_services/links.service';

@Component({
  selector: 'app-recursive-category',
  templateUrl: './recursive-category.component.html',
  styleUrls: ['./recursive-category.component.css']
})
export class RecursiveCategoryComponent {
  @Input() categories: Category[] = [];

  constructor(private categoryService: CategoryService, private linksService: LinksService) {}

  openPopup(name: string)
  {
    this.categoryService.deleteCategory(name).subscribe({
      next: () => {
        window.location.reload()
      }
    })
  }

    removeLink(name: string) {
    this.linksService.deleteLink(name).subscribe({
      next: response => {
        console.log(response),
          window.location.reload()
      }    
    })
  }
  
}
