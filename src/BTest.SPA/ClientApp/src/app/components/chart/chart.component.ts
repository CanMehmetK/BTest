import {Component, OnInit, ViewEncapsulation} from '@angular/core';

import {StoreService} from "app/services/store.service";
import {CartItem} from "app/models/cart-item";

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChartComponent implements OnInit {

  constructor(public _storeService: StoreService) { }



  removeFromCart(item: CartItem){
    this._storeService.cart.removeItem(item);
  }

  emptyCart(){
    this._storeService.cart.emptyCart();
  }

  ngOnInit(): void {
  }

  onQuantityChange(event:any, itemId: number) {
    let newQuantity = parseInt(event.target.value);
    if(Number.isNaN(newQuantity) || newQuantity < 0){
      newQuantity = 0;
      event.target.value = 0;
    }

    this._storeService.cart.cartItems = this._storeService.cart.cartItems.map(item => {
      if(item.product.id === itemId )
        item.quantity = newQuantity;
      return item;
    });
    this._storeService.cart.updateLocalStorage();
  }

  checkOut() {

  }
}
