import {InjectionToken} from '@angular/core';

import {IAppConfig} from './iapp.config';

export let APP_CONFIG = new InjectionToken('app.config');

export const AppConfig: IAppConfig = {
  routes: {
    heroes: 'heroes',
    yachts: 'yachts',
    error404: '404'
  },
  endpoints: {
    heroes: 'https://nodejs-example-app.herokuapp.com/heroes',
    yachts: ''
  },
  votesLimit: 3,
  topHeroesLimit: 4,
  topYachtsLimit: 4,
  snackBarDuration: 3000,
  repositoryURL: 'https://github.com/ismaestro/angular6-example-app'
};
