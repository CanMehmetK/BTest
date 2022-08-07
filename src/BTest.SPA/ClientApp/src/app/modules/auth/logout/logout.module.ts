import {NgModule} from '@angular/core';
import {RouterModule} from '@angular/router';

import {SharedModule} from 'app/shared.module';
import {AuthLogOutComponent} from 'app/modules/auth/logout/logout.component';

@NgModule({
  declarations: [
    AuthLogOutComponent
  ],
  imports: [
    RouterModule.forChild([{path: '', component: AuthLogOutComponent}]),
    SharedModule
  ]
})
export class AuthLogoutModule {
}
