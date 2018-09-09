import {async, TestBed} from '@angular/core/testing';
import {APP_BASE_HREF} from '@angular/common';
import {YachtsModule} from '../../yachts.module';
import {TestsModule} from '../../../../shared/modules/tests.module';
import {TranslateModule} from '@ngx-translate/core';
import {APP_CONFIG, AppConfig} from '../../../../config/app.config';
import {YachtsListPage} from './yachts-list.page';
import {YachtService} from '../../shared/yacht.service';

describe('YachtListComponent', () => {
  let fixture;
  let component;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        TestsModule,
        TranslateModule.forRoot(),
        YachtsModule
      ],
      providers: [
        {provide: APP_CONFIG, useValue: AppConfig},
        {provide: APP_BASE_HREF, useValue: '/'},
        YachtService
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(YachtsListPage);
    fixture.detectChanges();
    component = fixture.debugElement.componentInstance;
  }));

  it('should create yacht list component', (() => {
    expect(component).toBeTruthy();
  }));
});
