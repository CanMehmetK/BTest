import { Product } from "./product";

export interface ProductPayload {
    items: Product[];
    count: number;
}
