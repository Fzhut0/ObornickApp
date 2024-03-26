import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-base-recipes',
  templateUrl: './base-recipes.component.html',
  styleUrls: ['./base-recipes.component.css']
})
export class BaseRecipesComponent implements OnInit {

  constructor(public accountService: AccountService, private router: Router) {}

  addRecipe() {
    this.router.navigate(['/add-recipes']);
  }

  browseRecipes() {
    this.router.navigate(['/browse-recipes']);
  }

  ngOnInit(): void {
    
  }

}
