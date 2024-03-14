import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersComponent } from './members/members.component';
import { RegisterComponent } from './register/register.component';
import { BrowseRecipesComponent } from './browse-recipes/browse-recipes.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
    {path: '',
    runGuardsAndResolvers: 'always',
    children: [
      { path: 'members', component: MembersComponent },
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
