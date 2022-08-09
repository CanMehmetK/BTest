import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {ProductService} from "app/services/product.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Product} from "../../../../models";
import {DomSanitizer} from "@angular/platform-browser";

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.scss']
})
export class ProductEditComponent implements OnInit {
  productId;
  productForm: FormGroup;

  constructor(private _activatedRoute: ActivatedRoute,
              public domSanitizer: DomSanitizer,
              private _fb: FormBuilder,
              private _productService: ProductService) {
    this.productForm = this._fb.group(
      {
        id: [],
        name: ['', Validators.required],
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


  private getProduct() {
    if (this.productId) {
      let product: Product;
      this._productService.getItem(this.productId)
        .subscribe((response: any) => {
          console.log(response);
          product = response.data.items[0];
          this.productForm.patchValue(product)
          if (product && product.image) {
            this._productService.getImage(product.image).subscribe(imageBase64=>{
              this.productForm.get('image').setValue(imageBase64);
            })
          }
        });

    }
  }

  getImageBase64(): any {
    if (this.productForm.get('image')?.value) {
      return this.domSanitizer.bypassSecurityTrustResourceUrl(
        this.productForm.get('image').value);
    }
    return '';
  }

  onFileSelected($event: Event):void {
    const fileInput = document.getElementById('productImageFile') as HTMLInputElement;
    const reader = new FileReader();
    reader.onload = (x): void => {
      this.productForm.get('image').setValue(reader.result);
      this.getImageBase64();
    };
    reader.readAsDataURL(fileInput.files[0]);
  }
}
