import { Component, Input, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Category } from 'src/app/_models/category';
import { Link } from 'src/app/_models/link';
import { CategoryService } from 'src/app/_services/category.service';
import { LinksService } from 'src/app/_services/links.service';
import { ChangeLinkCategoryComponent } from 'src/app/modals/change-link-category/change-link-category.component';
import { DeleteCategoryComponent } from 'src/app/modals/delete-category/delete-category.component';
import { DeleteLinkComponent } from 'src/app/modals/delete-link/delete-link.component';

@Component({
  selector: 'app-recursive-category',
  templateUrl: './recursive-category.component.html',
  styleUrls: ['./recursive-category.component.css']
})
export class RecursiveCategoryComponent {
  @Input() categories: Category[] = [];
  bsCategoryModalRef: BsModalRef<DeleteCategoryComponent> = new BsModalRef<DeleteCategoryComponent>();
  bsLinkModalRef: BsModalRef<DeleteLinkComponent> = new BsModalRef<DeleteLinkComponent>();
  bsChangeLinkCategoryModalRef: BsModalRef<ChangeLinkCategoryComponent> = new BsModalRef<ChangeLinkCategoryComponent>();

  constructor(private modalService: BsModalService, private categoryService: CategoryService) {}

  openRemoveCategoryPopup(category: Category)
  {
    const config =
    {
      initialState: {
        category: category
      }
    }
    this.bsCategoryModalRef = this.modalService.show(DeleteCategoryComponent, config)
    
  }

  openRemoveLinkPopup(link: Link) {
      const config =
    {
      initialState: {
        link: link
      }
    }
    this.bsLinkModalRef = this.modalService.show(DeleteLinkComponent, config)
  }

  openChangeLinkCategoryPopup(link: Link) {
    const config =
    {
      initialState: {
        link: link
      }
    }
    this.bsChangeLinkCategoryModalRef = this.modalService.show(ChangeLinkCategoryComponent, config);
    this.bsChangeLinkCategoryModalRef.onHide?.subscribe({
      next: () => {
        this.categoryService.fetchCategories();
      }
    })
  }
}
