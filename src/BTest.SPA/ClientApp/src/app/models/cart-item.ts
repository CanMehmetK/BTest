import {Product} from "./product";

export class CartItem {
  public product: Product = {id: 0, name: "", price: 0, description: ""};
  public quantity: number = 0;

  public totalValue(): number {
    return this.quantity * this.product.price
  }
}
