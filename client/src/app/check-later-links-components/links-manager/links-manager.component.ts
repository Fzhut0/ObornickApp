import { Component, OnInit } from '@angular/core';
import { Category } from 'src/app/_models/category';
import { Link } from 'src/app/_models/link';
import { CategoryService } from 'src/app/_services/category.service';
import { LinksService } from 'src/app/_services/links.service';

@Component({
  selector: 'app-links-manager',
  templateUrl: './links-manager.component.html',
  styleUrls: ['./links-manager.component.css']
})
export class LinksManagerComponent implements OnInit {
  links: Link[] = [];
  categories: Category[] = [];

  newLink: Link = {
    customName: '',
    savedUrl: '',
    categoryName: 'string'
  };
  newCategory: string = '';

  constructor(private linksService: LinksService, private categoryService: CategoryService) { }

  ngOnInit(): void {
    this.getCategories();
  }

  addLink() {
    this.linksService.addLink(this.newLink).subscribe({
      next: () => {
        this.getCategories();
      }  
   })  
  }

  addCategory(name: string) {
    this.categoryService.addCategory(name).subscribe({
      error: error => console.log(error)
    });
  }

  markAsWatched(link: Link) {
    
  }

  removeLink(name: string) {
    this.linksService.deleteLink(name).subscribe({
      next: () => {
        this.getCategories();
      }    
    })
  }

  filteredLinks(category: Category): Link[] {
    return this.links.filter(link => link.categoryName === category.customName);
  }

  getCategories()
  {
    this.categoryService.getCategories().subscribe(
      response => {
        this.categories = response;
        console.log(response)
      }
    );
  }

  openPopup()
  {
    console.log("dupa")
  }

}
