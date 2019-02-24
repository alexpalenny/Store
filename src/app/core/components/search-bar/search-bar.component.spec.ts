import {async, fakeAsync, TestBed, tick} from '@angular/core/testing';
import {SearchBarComponent} from './search-bar.component';
import {CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';
import {TranslateModule} from '@ngx-translate/core';
import {APP_CONFIG, AppConfig} from '../../../config/app.config';
import {AppRoutingModule} from '../../../app-routing.module';
import {APP_BASE_HREF} from '@angular/common';
import {TestsModule} from '../../../shared/modules/tests.module';
import {Error404Page} from '../../pages/error404/error404.page';
import {PrivacyPage} from '../../pages/privacy/privacy.page';
import {YachtService} from '../../../modules/yachts/shared/yacht.service';
import {Router} from '@angular/router';

describe('SearchComponent', () => {
  let fixture;
  let component;
  let yachtService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        TestsModule,
        TranslateModule.forRoot(),
        AppRoutingModule
      ],
      declarations: [
        SearchBarComponent,
        Error404Page,
        PrivacyPage
      ],
      providers: [
        {
          provide: Router,
          useClass: class {
            navigate = jasmine.createSpy('navigate');
          }
        },
        {provide: APP_CONFIG, useValue: AppConfig},
        {provide: APP_BASE_HREF, useValue: '/'},
        YachtService
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(SearchBarComponent);
    fixture.detectChanges();
    component = fixture.debugElement.componentInstance;
    yachtService = TestBed.get(YachtService);
  }));

  it('should create hero search component', (() => {
    expect(component).toBeTruthy();
  }));

  it('should get all heroes', fakeAsync(() => {
    spyOn(yachtService, 'getYachts').and.returnValue(Promise.resolve(true));
    tick();
    fixture.detectChanges();
    expect(component.defaultHeroes.length).toBeGreaterThan(0);
  }));

  it('should filter heroes array', (() => {
    component.defaultHeroes = [
      {
        'id': 1,
        'name': 'batman',
        'default': true
      },
      {
        'id': 2,
        'name': 'spiderman',
        'default': false
      }
    ];
    expect(component.filterHeroes('batman').length).toBe(1);
    expect(component.filterHeroes('spiderman').length).toBe(0);
    expect(component.filterHeroes().length).toBe(2);
  }));

  it('should navigate to hero detail', (() => {
    const router = fixture.debugElement.injector.get(Router);
    expect(router.navigate).toHaveBeenCalledWith(['heroes/5']);
  }));
});
