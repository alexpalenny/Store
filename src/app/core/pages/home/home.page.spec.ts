import {async, fakeAsync, TestBed, tick} from '@angular/core/testing';
import {APP_BASE_HREF} from '@angular/common';
import {TranslateModule} from '@ngx-translate/core';
import {CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';
import {TestsModule} from '../../../shared/modules/tests.module';
import {APP_CONFIG, AppConfig} from '../../../config/app.config';
import {HeroService} from '../../../modules/heroes/shared/hero.service';
import {YachtService} from '../../../modules/yachts/shared/yacht.service';
import {HomePage} from './home.page';

describe('HomePage', () => {
  let fixture;
  let component;
  let heroService;
  let yachtService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        TestsModule,
        TranslateModule.forRoot(),
      ],
      declarations: [
        HomePage
      ],
      providers: [
        {provide: APP_CONFIG, useValue: AppConfig},
        {provide: APP_BASE_HREF, useValue: '/'},
        HeroService,
        YachtService
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(HomePage);
    fixture.detectChanges();
    component = fixture.debugElement.componentInstance;
    heroService = TestBed.get(HeroService);
    yachtService = TestBed.get(YachtService);
  }));

  it('should create hero top component', (() => {
    expect(component).toBeTruthy();
  }));

  it('should initialice component', fakeAsync(() => {
    fixture.detectChanges();
    spyOn(heroService, 'getHeroes').and.returnValue(Promise.resolve(true));
    spyOn(yachtService, 'getYachts').and.returnValue(Promise.resolve(true));
    tick();
    fixture.detectChanges();
    expect(component.heroes.length).toBe(AppConfig.topHeroesLimit);
  }));

  it('should like a hero', async(() => {
    localStorage.setItem('votes', String(AppConfig.votesLimit - 1));
    component.like({id: 1}).then((result) => {
      expect(result).toBe(true);
    });
  }));

  it('should not like a hero', async(() => {
    localStorage.setItem('votes', String(AppConfig.votesLimit));
    component.like({id: 1}).then(() => {
    }, (error) => {
      expect(error).toBe('maximum votes');
    });
    expect(HeroService.checkIfUserCanVote()).toBe(false);
    expect(YachtService.checkIfUserCanVote()).toBe(false);
  }));
});
