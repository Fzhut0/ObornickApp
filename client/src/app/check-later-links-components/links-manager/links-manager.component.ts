import { Component, OnInit } from '@angular/core';
import { Category } from 'src/app/_models/category';
import { Link } from 'src/app/_models/link';
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

  constructor(private linksService: LinksService) { }

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

  addCategory() {
    
  }

  markAsWatched(link: Link) {
    
  }

  removeLink(index: number) {
    this.links.splice(index, 1);
  }

  filteredLinks(category: Category): Link[] {
    return this.links.filter(link => link.categoryName === category.customName);
  }

  getCategories()
  {
    this.linksService.getCategories().subscribe(
      response => {
        this.categories = response;
        console.log(response)
      }
    );
  }

}
