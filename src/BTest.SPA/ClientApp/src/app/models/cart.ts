import {CartItem} from "./cart-item";

export class Cart {
  cartItems: CartItem[] = [];

  constructor(public cartAsJson: string) {
    if (cartAsJson !== '')
      this.cartItems = JSON.parse(cartAsJson) as CartItem[];
  }

  addItem(cartItem: CartItem) {
    let found: boolean = false;
    this.cartItems = this.cartItems.map(ci => {
      if (ci.product?.id == cartItem.product?.id) {
        ci.quantity++;
        found = true;
      }
      return ci;
    });

    if (!found) {
      this.cartItems.push(cartItem);
    }
    this.updateLocalStorage();
  }

  removeItem(item: CartItem) {
    const index = this.cartItems.indexOf(item, 0);
    if (index > -1) {
      this.cartItems.splice(index, 1);
    }
    this.updateLocalStorage();
  }

  emptyCart() {
    this.cartItems = [];
    this.updateLocalStorage();
  }

  get totalValue(): number {
    let sum = this.cartItems.reduce(
      (acc, chartItem) => {
        acc = acc + chartItem.quantity * chartItem.product.price;
        return acc;
      }, 0);
    return this.round(sum);
  }

  isCartValid(): boolean {
    if (this.cartItems.find(cartitem => (cartitem.quantity == null || cartitem.quantity <= 0)) === undefined)
      return true;
    return false;
  }

  updateLocalStorage() {
    localStorage.setItem('cart', JSON.stringify(this.cartItems));
  }
  round(value: number, direction: string = 'down', digits: number = 2): any {
    const round = 'down' === direction ? Math.floor : Math.ceil;
    return round(value * (10 ** digits)) / (10 ** digits);
  }
}
