import {InjectionToken} from '@angular/core';

import {IAppConfig} from './iapp.config';

export let APP_CONFIG = new InjectionToken('app.config');

export const AppConfig: IAppConfig = {
  routes: {
    yachts: 'yachts',
    error404: '404',
    privacy: 'privacy',
  },
  endpoints: {
    yachts: 'http://www.u-sail.com.ua/api/api/boats',
    //yachts: 'api/yachtList.json'
  },
  votesLimit: 3,
  topHeroesLimit: 4,
  topYachtsLimit: 4,
  snackBarDuration: 3000,
  repositoryURL: 'https://github.com/ismaestro/angular6-example-app'
};
