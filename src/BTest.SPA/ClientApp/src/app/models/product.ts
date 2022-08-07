import {Image} from "./image";

export interface Product{
  id: number;
  name: string;
  price: number;
  description?: string;
  image?: Image;
  quantity?: number;
}
