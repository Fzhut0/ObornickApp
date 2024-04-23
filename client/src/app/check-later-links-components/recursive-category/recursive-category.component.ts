import { AfterContentInit, AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { filter } from 'rxjs';
import { Category } from 'src/app/_models/category';
import { Link } from 'src/app/_models/link';
import { AccountService } from 'src/app/_services/account.service';
import { CategoryService } from 'src/app/_services/category.service';
import { LinksService } from 'src/app/_services/links.service';
import { MessagesService } from 'src/app/_services/messages.service';
import { ChangeLinkCategoryComponent } from 'src/app/modals/change-link-category/change-link-category.component';
import { ChangeLinkNameComponent } from 'src/app/modals/change-link-name/change-link-name.component';
import { DeleteCategoryComponent } from 'src/app/modals/delete-category/delete-category.component';
import { DeleteLinkComponent } from 'src/app/modals/delete-link/delete-link.component';

@Component({
  selector: 'app-recursive-category',
  templateUrl: './recursive-category.component.html',
  styleUrls: ['./recursive-category.component.css']
})
export class RecursiveCategoryComponent implements OnInit {
  @ViewChild(TabsetComponent) tabSet: TabsetComponent | undefined;
  @Input() categories: Category[] = [];
  bsCategoryModalRef: BsModalRef<DeleteCategoryComponent> = new BsModalRef<DeleteCategoryComponent>();
  bsLinkModalRef: BsModalRef<DeleteLinkComponent> = new BsModalRef<DeleteLinkComponent>();
  bsChangeLinkCategoryModalRef: BsModalRef<ChangeLinkCategoryComponent> = new BsModalRef<ChangeLinkCategoryComponent>();
  bsChangeLinkNameModalRef: BsModalRef<ChangeLinkNameComponent> = new BsModalRef<ChangeLinkNameComponent>();

  userHasMessagingId: boolean = false;

  constructor(private modalService: BsModalService, private categoryService: CategoryService, private accountService: AccountService, private messagesService: MessagesService)
  { }
  

  ngOnInit(): void {
    this.checkHasUserMessagingId();

  }

  selectTab(event: TabDirective)
  {
    if (event.id)
    {
      this.categoryService.lastSelectedCategoryId = event.id;
    }
  }

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
    this.bsLinkModalRef.onHide?.subscribe({
      next: () => {
        this.categoryService.fetchCategories();
              
      }
    })
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

  checkHasUserMessagingId()
  {
    this.accountService.hasUserMessagingId().subscribe({
      next: response => {
        if (response == true)
        {
          this.userHasMessagingId = true;
        }
        else {
          this.userHasMessagingId = false;
        }
      }
    })
  }

  sendMessage(link: Link, categoryName: string)
  {
    var message = '';

    message = `Link o nazwie: ${link.customName}\\n Kategoria: ${categoryName}\\n Link: ${link.savedUrl}`

    message = encodeURIComponent(message);

    this.messagesService.sendMessage(message).subscribe({})
  }

  editLinkName(link: Link)
  {
    const config =
    {
      initialState: {
        link: link
      }
    }
    this.bsChangeLinkNameModalRef = this.modalService.show(ChangeLinkNameComponent, config)
    this.bsChangeLinkNameModalRef.onHide?.subscribe({})
  }
}
