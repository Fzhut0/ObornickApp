import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersComponent } from './members/members.component';
import { RegisterComponent } from './register/register.component';
import { BrowseRecipesComponent } from './browse-recipes/browse-recipes.component';
import { authGuard } from './_guards/auth.guard';
import { adminGuard } from './_guards/admin.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
    {path: '',
      runGuardsAndResolvers: 'always',
      canActivate: [authGuard],
      children: [
        { path: 'members', component: MembersComponent, canActivate: [adminGuard] },
        { path: 'register', component: RegisterComponent },
        { path: 'browse-recipes', component: BrowseRecipesComponent}
      ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
