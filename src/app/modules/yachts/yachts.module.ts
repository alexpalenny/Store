import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {YachtRoutingModule} from './yachts-routing.module';
import {SharedModule} from '../../shared/shared.module';
import {YachtService} from './shared/yacht.service';
import {YachtsListPage} from './pages/yachts-list/yachts-list.page';
import {YachtDetailPage} from './pages/yacht-detail/yacht-detail.page';
// import {YachtRemoveComponent} from './components/yacht-remove/yacht-remove.component';

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    YachtRoutingModule
  ],
  declarations: [
    YachtsListPage,
    YachtDetailPage,
    // YachtRemoveComponent
  ],
  // entryComponents: [
  //   YachtRemoveComponent
  // ]
})

export class YachtsModule {
}
