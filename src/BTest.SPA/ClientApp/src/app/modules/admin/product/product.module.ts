import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ProductListComponent} from './product-list/product-list.component';
import {ProductEditComponent} from './product-edit/product-edit.component';
import {RouterModule, Routes} from "@angular/router";

const routes: Routes = [
  {path:'',redirectTo:'list',pathMatch:'full'},
  {path: 'list', component: ProductListComponent},
  {path: 'edit', component: ProductEditComponent},

]

@NgModule({
  declarations: [
    ProductListComponent,
    ProductEditComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule
  ]
})
export class ProductModule {
}
