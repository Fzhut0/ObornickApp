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

  username: string | undefined;

  newLink: Link = {
    customName: '',
    savedUrl: '',
    categoryName: 'string'
  };
  newCategory: string = '';

  constructor(private linksService: LinksService, private categoryService: CategoryService, private messagesService: MessagesService,
    public accountService: AccountService) { }

  ngOnInit(): void {
    this.accountService.currentUser$.subscribe({
      next: response => this.username = response?.username
    })
    this.getCategories(this.username!);
  }

  addLink(username: string) {
    this.linksService.addLink(this.newLink, username).subscribe({
      next: () => {
        this.getCategories(this.username!);
      }  
   })  
  }

  addCategory(name: string, username: string) {
    this.categoryService.addCategory(name, username).subscribe({
      next: () => {
        this.getCategories(this.username!)
      },
      error: error => console.log(error)
    });
  }

  markAsWatched(link: Link) {
    this.linksService.markLinkAsWatched(link).subscribe({
      next: () => {
        this.getCategories(this.username!)
      },
      error: error => console.log(error)
    })
  }

  removeLink(name: string, username: string) {
    this.linksService.deleteLink(name, username).subscribe({
      next: () => {
        this.getCategories(this.username!);
      }    
    })
  }

  filteredLinks(category: Category): Link[] {
    return this.links.filter(link => link.categoryName === category.customName);
  }

  getCategories(username: string)
  {
    this.categoryService.getCategories(username).subscribe(
      response => {
        this.categories = response;
        console.log(response)
      }
    );
  }

  openPopup(name: string, username: string)
  {
    this.categoryService.deleteCategory(name, username).subscribe({
      next: () => {
        this.getCategories(this.username!)
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

}
