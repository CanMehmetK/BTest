import {NgModule} from '@angular/core';
import {RouterModule} from '@angular/router';

import {SharedModule} from 'app/shared.module';
import {AuthRegisterComponent} from 'app/modules/auth/register/register.component';

@NgModule({
  declarations: [
    AuthRegisterComponent
  ],
  imports: [
    RouterModule.forChild([{path: '', component: AuthRegisterComponent}]),

    SharedModule
  ]
})
export class AuthRegisterModule {
}
