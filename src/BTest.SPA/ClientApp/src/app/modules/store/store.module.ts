import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import {SharedModule} from "app/shared.module";
import {LayoutComponent} from './layout/layout.component';
import {ProductComponent} from './product/product.component';
import {FilterComponent} from "app/components/product-filter/filter.component";
import {ChartComponent} from "app/components/chart/chart.component";
import {CheckoutComponent} from './checkout/checkout.component';
import {OrderListComponent} from "app/components/order-list/order-list.component";


const routes: Routes = [
  {
    path: '', component: LayoutComponent, children: [
      {path: '', redirectTo: 'products', pathMatch: 'full'},
      {path: 'products', component: ProductComponent},
      {path: 'checkout', component: CheckoutComponent}
    ]
  },
];

@NgModule({
  declarations: [
    LayoutComponent,
    ProductComponent,
    FilterComponent,
    ChartComponent,
    CheckoutComponent,
    OrderListComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    NgbModule,
    SharedModule
  ]
})
export class StoreModule {
}
