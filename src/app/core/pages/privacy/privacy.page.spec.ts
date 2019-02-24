import {async, TestBed} from '@angular/core/testing';
import {CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';
import {TranslateModule} from '@ngx-translate/core';
import {APP_CONFIG, AppConfig} from '../../../config/app.config';
import {MaterialModule} from '../../../shared/modules/material.module';
import {TestsModule} from '../../../shared/modules/tests.module';
import {PrivacyPage} from './privacy.page';
import {YachtService} from '../../../modules/yachts/shared/yacht.service';
import {ProgressBarService} from '../../services/progress-bar.service';

describe('PrivacyPage', () => {
  let fixture;
  let component;
  let progressBarService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        TestsModule,
        TranslateModule.forRoot(),
        MaterialModule
      ],
      declarations: [
        PrivacyPage
      ],
      providers: [
        {provide: APP_CONFIG, useValue: AppConfig},
        YachtService,
        ProgressBarService
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(PrivacyPage);
    fixture.detectChanges();
    component = fixture.debugElement.componentInstance;
    progressBarService = TestBed.get(ProgressBarService);
  }));

  it('should create nav component', (() => {
    expect(component).toBeTruthy();
  }));
});
