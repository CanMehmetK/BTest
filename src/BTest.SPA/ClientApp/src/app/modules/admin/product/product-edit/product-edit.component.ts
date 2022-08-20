import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {ProductService} from "app/services/product.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Product} from "../../../../models";
import {DomSanitizer} from "@angular/platform-browser";
import {StoreService} from "../../../../services/store.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.scss']
})
export class ProductEditComponent implements OnInit {
  productId;
  productForm: FormGroup;
  categories = [];

  constructor(private _toastr: ToastrService,
              private _activatedRoute: ActivatedRoute,
              public domSanitizer: DomSanitizer,
              private _fb: FormBuilder,
              private _productService: ProductService,
              private _storeService: StoreService,
              private _router: Router) {
    _storeService.categories$.subscribe(t => this.categories = t);
    this.productForm = this._fb.group(
      {
        id: [],
        name: ['', Validators.required],
        categoryId: ['', Validators.required],
        description: [],
        price: [],
        image: []
      }
    );
  }

  ngOnInit(): void {
    this.productId = this._activatedRoute.snapshot.paramMap.get('id');
    this.getProduct();
  }

  getImageBase64(): any {
    if (this.productForm.get('image')?.value) {
      return this.domSanitizer.bypassSecurityTrustResourceUrl(
        this.productForm.get('image').value);
    }
    return '';
  }

  onFileSelected($event: Event): void {
    const fileInput = document.getElementById('productImageFile') as HTMLInputElement;
    const reader = new FileReader();
    reader.onload = (x): void => {
      this.productForm.get('image').setValue(reader.result);
      this.getImageBase64();
    };
    reader.readAsDataURL(fileInput.files[0]);
  }

  save() {
    if (this.productForm.value.id == null)
      this._storeService.createProduct(this.productForm.value)
        .subscribe((response: any) => {
          this._router.navigate([`/admin/product/edit/${response.data}`]).then();
          this._toastr.success('#'+response.data+ ' created.');
        });
    else
      this._storeService.updateProduct(this.productForm.value)
        .subscribe((response: any) => {
          this._router.navigate([`/admin/product/edit/${response.data}`]).then();
          this._toastr.success('#'+response.data+ ' updated.');
        });
  }

  private getProduct() {
    if (this.productId) {
      let product: Product;
      this._productService.getItem(this.productId)
        .subscribe((response: any) => {
          product = response.data.items[0];
          const imgName = product.image;
          delete product.image;
          this.productForm.patchValue(product)
          if (imgName) {
            this._productService.getImage(imgName).subscribe(imageBase64 => {
              this.productForm.get('image').setValue(imageBase64);
            })
          }
        });
    }
  }
}
