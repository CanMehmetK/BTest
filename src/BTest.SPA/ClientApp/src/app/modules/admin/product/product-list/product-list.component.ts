import {Component, Inject, OnInit} from '@angular/core';
import {ProductService} from "app/services/product.service";
import {Product} from "app/models";

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {
  listoptions: any = {};
  products: Product[] = [];

  constructor(@Inject('IMAGES_URL') public _imagesUrl: string,
              private _productService: ProductService) {

  }

  ngOnInit(): void {
    this.listoptions.pageSize = 10;
    this.listoptions.page = 1;
    this.getProducts();
  }

  onPageChange($event: any) {
    this.listoptions.page = $event;
    this.getProducts();
  }

  getProducts(): void {
    this._productService.getItems(this.listoptions.page, this.listoptions.pageSize, this.listoptions.filter)
      .subscribe((itemPayload: any) => {
        this.products = itemPayload.data.items;
        this.listoptions.count = itemPayload.data.count;
      });
  }

  goToEditProductPage(item: Product) {

  }
}
