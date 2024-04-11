import { Link } from "./link";

export interface Category {
    customName: string;
    links: Link[];
    subcategories: Category[];
    categoryId: number
}