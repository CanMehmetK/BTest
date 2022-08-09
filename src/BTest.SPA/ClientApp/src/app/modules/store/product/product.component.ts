import {Component, Inject, OnInit, ViewEncapsulation} from '@angular/core';
import {skip} from "rxjs";

import {ProductService} from "app/services/product.service";
import {StoreService} from "app/services/store.service";
import {CartItem} from "app/models/cart-item";

@Component({
  selector: 'app-items',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProductComponent implements OnInit {

  constructor(
    private _productService: ProductService,
    public _storeService: StoreService,
    @Inject('IMAGES_URL') public _imagesUrl: string) {

  }

  ngOnInit(): void {
    this._storeService.filter$
      .pipe(skip(1))    //skip getting filter at component creation
      .subscribe(filter => {
        this._storeService.page = 1;
        this.getProducts();
      });

    this.getProducts();
  }

  getProducts(): void {
    this._productService.getItems(this._storeService.page, this._storeService.pageSize, this._storeService.filter)
      .subscribe((itemPayload: any) => {
        this._storeService.products = itemPayload.data.items;
        this._storeService.count = itemPayload.data.count;
      });
  }

  onPageChange(newPage: number): void {
    this._storeService.page = newPage;
    this.getProducts();
  }

  onPageSizeChange(): void {
    this._storeService._pageSizeSubject.next(this._storeService.pageSize);
    this.getProducts();
  }


  addToCart(product: any, quantity = 1) {
    var chartItem = new CartItem();
    chartItem.product = product;
    chartItem.quantity = quantity;
    this._storeService.cart.addItem(chartItem);
  }

  removeFromCart(item: CartItem) {
    this._storeService.cart.removeItem(item);
  }

  emptyCart() {
    this._storeService.cart.emptyCart();
  }
}
