import { Component, OnInit } from '@angular/core';
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
    categoryName: ''
  };
  newCategory: string = '';
  newSubcategory: string = '';

  constructor(private linksService: LinksService, private categoryService: CategoryService, private messagesService: MessagesService,
    public accountService: AccountService) { }

  ngOnInit(): void {
    this.getCategories();
    this.categoryService.categorySelectedEvent.subscribe((data: Category) => {
      this.selectedCategory = data;
    })
  }

  addLink(username: string) {
    if (this.selectedCategory)
    {
        this.newLink.categoryName = this.selectedCategory.customName
        this.linksService.addLink(this.newLink, username).subscribe({
        next: () => {
          this.getCategories();
        }  
      }) 
    } 
  }

  addCategory(name: string, username: string) {
    this.categoryService.addCategory(name, username).subscribe({
      next: () => {
        this.getCategories()
      },
      error: error => console.log(error)
    });
  }

  hasNestedSubcategories(subcategories: Category[]): boolean {
    return subcategories && subcategories.length > 0;
  }

  addSubcategory(name: string, parentCategory: string, username: string)
  {
    console.log(name, parentCategory, username)
    this.categoryService.addSubcategory(name, parentCategory, username).subscribe({
      next: () => {
        this.getCategories()
      },
      error: error => console.log(error)
    })
  }

  markAsWatched(link: Link) {
    this.linksService.markLinkAsWatched(link).subscribe({
      next: () => {
        this.getCategories()
      },
      error: error => console.log(error)
    })
  }


  filteredLinks(category: Category): Link[] {
    return this.links.filter(link => link.categoryName === category.customName);
  }

  getCategories() {
    this.categoryService.getCategories().subscribe({
      next: response => {
        this.categories = response
        this.categories.forEach(category => {
          this.getSubcategories(category)
        })
      },   
      error: error => console.log(error)
    })
  }

  getSubcategories(category: Category)
  {
    this.categoryService.getSubcategories(category.customName).subscribe({
      next: (response: Category[]) => {
        category.subcategories = response;
        category.subcategories.forEach(subcategory => {
          this.getSubcategories(subcategory)
        });
        
      },
      error: error => console.log(error)
    })
  }

  

  sendMessage(link: Link, username: string)
  {
    var message = '';

    message = `Link o nazwie: ${link.customName} z kategorii: ${link.categoryName} \\n Link:${link.savedUrl}`
   
    console.log(message);
    console.log(username);

    message = encodeURIComponent(message);

    this.messagesService.sendMessage(message, username).subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  selectCategory(category: Category) {
    if (this.selectedCategory === category) {
      this.selectedCategory = null; 
    } else {
      this.selectedCategory = category;
      this.selectedSubcategory = null; 
    }
  }

  selectSubcategory(subcategory: Category) {
    if (this.selectedSubcategory === subcategory) {
      this.selectedSubcategory = null; 
    } else {
      this.selectedSubcategory = subcategory;
      this.selectedCategory = null; 
    }
  }

  isCategorySelected(category: Category): boolean {
    return this.selectedCategory === category;
  }

  isSubcategorySelected(subcategory: Category): boolean {
    return this.selectedSubcategory === subcategory;
  }

  toggleCategory(category: Category) {
    category.expanded = !category.expanded;
  }

  isCategoryExpanded(category: Category): boolean {
    return category.expanded ?? false;
  }

}
