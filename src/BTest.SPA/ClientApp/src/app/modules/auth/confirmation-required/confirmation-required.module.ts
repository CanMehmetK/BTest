import {NgModule} from '@angular/core';
import {RouterModule} from '@angular/router';
import {SharedModule} from 'app/shared.module';
import {
  AuthConfirmationRequiredComponent
} from 'app/modules/auth/confirmation-required/confirmation-required.component';

@NgModule({
  declarations: [
    AuthConfirmationRequiredComponent
  ],
  imports: [
    RouterModule.forChild([
      {path: '', component: AuthConfirmationRequiredComponent}
    ]),
    SharedModule
  ]
})
export class AuthConfirmationRequiredModule {
}
