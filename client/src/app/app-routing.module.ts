import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { authGuard } from './_guards/auth.guard';
import { adminGuard } from './_guards/admin.guard';
import { AddRecipesComponent } from './recipe-components/add-recipes/add-recipes.component';
import { BrowseRecipesComponent } from './recipe-components/browse-recipes/browse-recipes.component';
import { BaseRecipesComponent } from './recipe-components/base-recipes/base-recipes.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
    {path: '',
      runGuardsAndResolvers: 'always',
      canActivate: [authGuard],
      children: [
        { path: 'add-recipes', component: AddRecipesComponent, canActivate: [adminGuard] },
        { path: 'register', component: RegisterComponent },
        { path: 'browse-recipes', component: BrowseRecipesComponent },
        { path: 'recipes', component: BaseRecipesComponent}
      ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
