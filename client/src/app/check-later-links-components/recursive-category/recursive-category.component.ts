import { Component, Input } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Category } from 'src/app/_models/category';
import { Link } from 'src/app/_models/link';
import { CategoryService } from 'src/app/_services/category.service';
import { LinksService } from 'src/app/_services/links.service';
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

  constructor(private categoryService: CategoryService, private linksService: LinksService, private modalService: BsModalService) {}

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
  
}
