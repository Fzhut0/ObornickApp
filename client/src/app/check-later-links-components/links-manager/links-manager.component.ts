import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Category } from 'src/app/_models/category';
import { Link } from 'src/app/_models/link';
import { AccountService } from 'src/app/_services/account.service';
import { CategoryService } from 'src/app/_services/category.service';
import { LinksService } from 'src/app/_services/links.service';
import { MessagesService } from 'src/app/_services/messages.service';

@Component({
  selector: 'app-links-manager',
  templateUrl: './links-manager.component.html',
  styleUrls: ['./links-manager.component.css']
})
export class LinksManagerComponent implements OnInit {
  links: Link[] = [];
  categories: Category[] = [];
  subcategories: Category[] = [];

  selectedCategory: Category | null = null;
  selectedSubcategory: Category | null = null;

  oneAtATime = true;

  username: string | undefined;

  newLink: Link = {
    customName: '',
    savedUrl: '',
    categoryName: '',
    categoryId: 0
  };
  newCategory: string = '';
  newSubcategory: string = '';


  constructor(private linksService: LinksService, private categoryService: CategoryService, private toastr: ToastrService,
    public accountService: AccountService) { }

  ngOnInit(): void {
    this.getCategories();
    this.categoryService.fetchCategoriesEvent.subscribe({
      next: () => {
        this.getCategories()
      }
    })
    this.categoryService.categorySelectedEvent.subscribe((data: Category) => {
      this.selectedCategory = data;
    })
    this.categoryService.categoriesFetched.emit();
  }

  addLink() {
    if (this.selectedCategory)
    {
      this.newLink.categoryName = this.selectedCategory.customName;
      this.newLink.categoryId = this.selectedCategory.categoryId;
      var toastMsg = `Dodano link o nazwie: ${this.newLink.customName} do kategorii: ${this.newLink.categoryName}`
        this.linksService.addLink(this.newLink).subscribe({
        next: () => {
            this.getCategories();
            this.toastr.success(toastMsg);
            this.resetLinkForm();      
        }  
      }) 
    } 
  }

  resetLinkForm()
  {
    this.newLink = {
      customName: '',
      savedUrl: '',
      categoryName: '',
      categoryId: 0
    }
    this.selectedCategory = null;
  }

  addCategory(name: string) {
    var toastMsg = `Dodano kategorię o nazwie: ${name}`
    this.categoryService.addCategory(name).subscribe({
      next: () => {
        this.getCategories();
        this.toastr.success(toastMsg);
        this.newCategory = '';
      }
    });
  }

  hasNestedSubcategories(subcategories: Category[]): boolean {
    return subcategories && subcategories.length > 0;
  }

  addSubcategory(name: string, id: number)
  {
    var toastMsg = `Dodano podkategorię o nazwie: ${name}`
    this.categoryService.addSubcategory(name, id).subscribe({
      next: () => {
        this.getCategories();
        this.toastr.success(toastMsg);
        this.newSubcategory = '';
        this.newCategory = '';
      }
    })
  }

  markAsWatched(link: Link) {
    this.linksService.markLinkAsWatched(link).subscribe({
      next: () => {
        this.getCategories()
      }
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
     this.categoryService.categoriesFetched.emit();
  }

  getSubcategories(category: Category)
  {
    this.categoryService.getSubcategories(category.categoryId).subscribe({
      next: (response: Category[]) => {
        category.subcategories = response;
        category.subcategories.forEach(subcategory => {
          this.getSubcategories(subcategory)
        }); 
      },
      error: error => console.log(error),
    })
    this.categoryService.categoriesFetched.emit();
  }
}
