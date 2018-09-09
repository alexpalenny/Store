import {async, TestBed} from '@angular/core/testing';
import {APP_BASE_HREF} from '@angular/common';
import {YachtsModule} from '../../yachts.module';
import {TestsModule} from '../../../../shared/modules/tests.module';
import {TranslateModule} from '@ngx-translate/core';
import {YachtService} from '../../shared/yacht.service';
import {ActivatedRoute, convertToParamMap} from '@angular/router';
import {APP_CONFIG, AppConfig} from '../../../../config/app.config';
import {YachtDetailPage} from './yacht-detail.page';

describe('YachtDetailPage', () => {
  let fixture;
  let component;
  let yachtService;

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
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: convertToParamMap({
                id: '1'
              })
            }
          }
        },
        YachtService
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(YachtDetailPage);
    fixture.detectChanges();
    component = fixture.debugElement.componentInstance;
    yachtService = TestBed.get(YachtService);
  }));

  it('should create yacht detail component', (() => {
    expect(component).toBeTruthy();
  }));

  it('should like a yacht', async(() => {
    localStorage.setItem('votes', String(AppConfig.votesLimit - 1));
    component.like({id: 1}).then((result) => {
      expect(result).toBe(true);
    });
  }));
});
