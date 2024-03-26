import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './home/home.component';
import { MembersComponent } from './members/members.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { RecipeIngredientsComponent } from './modals/recipe-ingredients/recipe-ingredients.component';
import { HasRoleDirective } from './_directives/has-role.directive';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { EditRecipeComponent } from './modals/edit-recipe/edit-recipe.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { AddRecipesComponent } from './recipe-components/add-recipes/add-recipes.component';
import { BrowseRecipesComponent } from './recipe-components/browse-recipes/browse-recipes.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    RegisterComponent,
    HomeComponent,
    MembersComponent,
    TextInputComponent,
    BrowseRecipesComponent,
    RecipeIngredientsComponent,
    HasRoleDirective,
    EditRecipeComponent,
    AddRecipesComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
    ModalModule.forRoot(),
    ToastrModule.forRoot({
    positionClass: 'toast-bottom-right'
    }),
    TabsModule.forRoot()
  ],
  providers: [
      { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
      { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
