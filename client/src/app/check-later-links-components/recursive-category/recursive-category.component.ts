import { AfterContentInit, AfterViewChecked, AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit, ViewChild } from '@angular/core';
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
export class RecursiveCategoryComponent implements OnInit, AfterViewChecked {
  @ViewChild(TabsetComponent) tabSet: TabsetComponent | undefined;
  @Input() categories: Category[] = [];
  bsCategoryModalRef: BsModalRef<DeleteCategoryComponent> = new BsModalRef<DeleteCategoryComponent>();
  bsLinkModalRef: BsModalRef<DeleteLinkComponent> = new BsModalRef<DeleteLinkComponent>();
  bsChangeLinkCategoryModalRef: BsModalRef<ChangeLinkCategoryComponent> = new BsModalRef<ChangeLinkCategoryComponent>();
  bsChangeLinkNameModalRef: BsModalRef<ChangeLinkNameComponent> = new BsModalRef<ChangeLinkNameComponent>();

  userHasMessagingId: boolean = false;


  constructor(private modalService: BsModalService, private categoryService: CategoryService,
    private accountService: AccountService, private messagesService: MessagesService, private crf: ChangeDetectorRef)
  { }
  
  ngAfterViewChecked()
  {
  
    if (this.tabSet && this.tabSet.tabs && this.categoryService.lastSelectedCategoryId != null)
        {
      for (let i = 0; i < this.tabSet.tabs.length; i++) {
        if (this.tabSet.tabs[i].id === null)
        {
          return;
        }
        if (this.tabSet.tabs[i].id === this.categoryService.lastSelectedMainCategoryId)
        {
          this.tabSet.tabs[i].active = true;
          this.crf.detectChanges();
            }
            if (this.tabSet.tabs[i].id === this.categoryService.lastSelectedCategoryId)
            {
              this.tabSet.tabs[i].active = true;
              this.crf.detectChanges();
            }    
          }
        }
  }

  ngOnInit(): void {
    this.checkHasUserMessagingId();
  }

  selectTab(event: TabDirective)
  {
    if (event.id)
    {
      if (event.customClass === 'false')
      {
        this.categoryService.lastSelectedMainCategoryId = event.id;
      }
      else
      {
        this.categoryService.lastSelectedCategoryId = event.id;
      }
      
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


    var categoryName = '';
    this.categoryService.getCategoryName(link.categoryId).subscribe({
      next: response => {
        if (response.isSubcategory)
        {
          categoryName = response.customName
          this.categoryService.lastSelectedCategoryId = categoryName;
        }
        this.categoryService.lastSelectedCategoryId = categoryName;
        this.bsLinkModalRef = this.modalService.show(DeleteLinkComponent, config)
        this.bsLinkModalRef.onHide?.subscribe({
          next: () => {
            this.categoryService.fetchCategories();
                  
          }
        })
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

    var categoryName = '';
    this.categoryService.getCategoryName(link.categoryId).subscribe({
      next: response => {
        if (response.isSubcategory)
        {
          categoryName = response.customName
          this.categoryService.lastSelectedCategoryId = categoryName;
        }
        this.bsChangeLinkCategoryModalRef = this.modalService.show(ChangeLinkCategoryComponent, config);
        this.bsChangeLinkCategoryModalRef.onHide?.subscribe({
          next: () => {
            this.categoryService.fetchCategories();
          }
        })
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
    this.bsChangeLinkNameModalRef.onHide?.subscribe({
      next: () => {
        this.categoryService.fetchCategories();
      }
    })
  }
}
