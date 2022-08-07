import {NgModule, Optional, SkipSelf} from '@angular/core';
import {AuthModule} from 'app/core/auth/auth.module';
import {ToastrModule} from 'ngx-toastr';
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";

@NgModule({
  imports: [
    AuthModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 10000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    }),
  ]
})
export class CoreModule {

  constructor(
    @Optional() @SkipSelf() parentModule?: CoreModule
  ) {
    // Do not allow multiple injections
    if (parentModule) {
      throw new Error('CoreModule has already been loaded. Import this module in the AppModule only.');
    }
  }
}
