import {Component, OnInit} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';
import {Meta, Title} from '@angular/platform-browser';
import {NavigationEnd, Router} from '@angular/router';
import {AppConfig} from './config/app.config';
import {MatSnackBar} from '@angular/material';
import {_} from '@biesbjerg/ngx-translate-extract/dist/utils/utils';

declare const require;
declare const Modernizr;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})

export class AppComponent implements OnInit {

  isOnline: boolean;

  constructor(private translateService: TranslateService,
              private title: Title,
              private meta: Meta,
              private snackBar: MatSnackBar,
              private router: Router) {
    this.isOnline = navigator.onLine;
  }

  ngOnInit() {
    this.translateService.setDefaultLang('en');
    this.translateService.use('en');

    // With this we load the default language in the main bundle (cache busting)
    this.translateService.setTranslation('en', require('../assets/i18n/en.json'));

    this.title.setTitle('U-Sail');
    this.router.events.subscribe((event: any) => {
      if (event instanceof NavigationEnd) {
        switch (event.urlAfterRedirects) {
          case '/':
            this.meta.updateTag({
              name: 'description',
              content: 'U-Sail with Angular CLI, Angular Material and more'
            });
            break;
          case '/' + AppConfig.routes.yachts:
            this.title.setTitle('Yachts list');
            this.meta.updateTag({
              name: 'description',
              content: 'List of yachts'
            });
            break;
        }
      }
    });

    this.checkBrowserFeatures();
  }

  checkBrowserFeatures() {
    let supported = true;
    for (const feature in Modernizr) {
      if (Modernizr.hasOwnProperty(feature) &&
        typeof Modernizr[feature] === 'boolean' && Modernizr[feature] === false) {
        supported = false;
        break;
      }
    }

    if (!supported) {
      this.translateService.get([String(_('updateBrowser'))]).subscribe((texts) => {
        this.snackBar.open(texts['updateBrowser'], 'OK');
      });
    }

    return supported;
  }
}
