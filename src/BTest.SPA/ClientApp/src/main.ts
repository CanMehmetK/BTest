import {enableProdMode} from '@angular/core';
import {platformBrowserDynamic} from '@angular/platform-browser-dynamic';

import {AppModule} from './app/app.module';
import {environment} from './environments/environment';

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

const providers = [
  {provide: 'BASE_URL', useFactory: getBaseUrl, deps: []},
  {
    provide: 'IMAGES_URL', useFactory: () => {
      return 'https://localhost:7263/images/products/'
    }
  }
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.log(err));