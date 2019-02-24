import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AppConfig} from './config/app.config';
import {Error404Page} from './core/pages/error404/error404.page';
import {PrivacyPage} from './core/pages/privacy/privacy.page';

const routes: Routes = [
  {path: '', redirectTo: AppConfig.routes.yachts, pathMatch: 'full'},
  // {path: '', loadChildren: './modules/yachts/yachts.module#YachtsModule'},
  {path: AppConfig.routes.error404, component: Error404Page},
  {path: AppConfig.routes.privacy, component: PrivacyPage, },
  {path: AppConfig.routes.yachts, loadChildren: './modules/yachts/yachts.module#YachtsModule'},

  // otherwise redirect to 404
  {path: '**', redirectTo: '/' + AppConfig.routes.error404}
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { useHash: true, scrollPositionRestoration: 'enabled'})
  ],
  exports: [
    RouterModule
  ]
})

export class AppRoutingModule {
}
