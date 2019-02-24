import {NgModule, Optional, SkipSelf} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {throwIfAlreadyLoaded} from './module-import-guard';
import {SharedModule} from '../shared/shared.module';
import {RouterModule} from '@angular/router';
import {ProgressBarService} from './services/progress-bar.service';
import {LoggerService} from './services/logger.service';
import {HomePage} from './pages/home/home.page';
import {HeaderComponent} from './components/header/header.component';
import {FooterComponent} from './components/footer/footer.component';
import {Error404Page} from './pages/error404/error404.page';
import {PrivacyPage} from './pages/privacy/privacy.page';
import {SearchBarComponent} from './components/search-bar/search-bar.component';
import {YachtService} from '../modules/yachts/shared/yacht.service';

@NgModule({
  imports: [
    ReactiveFormsModule,
    RouterModule,
    SharedModule
  ],
  declarations: [
    HomePage,
    Error404Page,
    PrivacyPage,
    HeaderComponent,
    SearchBarComponent,
    FooterComponent
  ],
  exports: [
    HeaderComponent,
    SearchBarComponent,
    FooterComponent
  ],
  providers: [
    YachtService,
    LoggerService,
    ProgressBarService
  ]
})

export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    throwIfAlreadyLoaded(parentModule, 'CoreModule');
  }
}
