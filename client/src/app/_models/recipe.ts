import { Ingredient } from "./ingredient";
import { RecipeDescriptionStep } from "./recipedescriptionstep";

export interface Recipe {
  name: string;
  ingredients: Ingredient[];
  recipeId: number;
  recipeDescriptionSteps: RecipeDescriptionStep[];
}
