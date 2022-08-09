import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {AdminLayoutComponent} from './admin-layout/admin-layout.component';
import {AuthGuard} from "../../core/auth/guards/auth.guard";

const routes: Routes = [
  {
    path: '', component: AdminLayoutComponent, children: [
      {path:'',redirectTo:'product',pathMatch:'full'},
      {
        path: 'product',
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        data: {roles: ['SuperAdmin', 'Admin']},
        loadChildren: () => import('app/modules/admin/product/product.module').then(m => m.ProductModule)
      }
    ]
  }
];

@NgModule({
  declarations: [
    AdminLayoutComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule
  ]
})
export class AdminModule {
}
