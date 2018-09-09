import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {YachtDetailPage} from './pages/yacht-detail/yacht-detail.page';
import {YachtsListPage} from './pages/yachts-list/yachts-list.page';

const yachtsRoutes: Routes = [
  {path: '', component: YachtsListPage},
  {path: ':id', component: YachtDetailPage}
];

@NgModule({
  imports: [
    RouterModule.forChild(yachtsRoutes)
  ],
  exports: [
    RouterModule
  ]
})

export class YachtRoutingModule {
}
