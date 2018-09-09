import {async, TestBed} from '@angular/core/testing';
import {YachtService} from './yacht.service';
import {APP_BASE_HREF} from '@angular/common';
import {APP_CONFIG, AppConfig} from '../../../config/app.config';
import {TestsModule} from '../../../shared/modules/tests.module';
import {TranslateModule} from '@ngx-translate/core';
import {HttpErrorResponse} from '@angular/common/http';

describe('YachtService', () => {
  let yachtService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        TestsModule,
        TranslateModule.forRoot()
      ],
      providers: [
        {provide: APP_CONFIG, useValue: AppConfig},
        {provide: APP_BASE_HREF, useValue: '/'},
        YachtService
      ]
    });

    yachtService = TestBed.get(YachtService);
  });

  it('should contains yachts', async(() => {
    yachtService.getYachts().subscribe((data: any) => {
      expect(data.length).toBeGreaterThan(AppConfig.topYachtsLimit);
    });
  }));

  it('should get yacht by id 1', async(() => {
    yachtService.getYachtById('1').subscribe((yacht) => {
      expect(yacht.id).toEqual(1);
    });
  }));

  it('should fail getting yacht by no id', async(() => {
    yachtService.getYachtById('noId').subscribe(() => {
    }, (error) => {
      expect(error).toEqual(jasmine.any(HttpErrorResponse));
    });
  }));

  it('should fail creating empty yacht', async(() => {
    yachtService.createYacht({}).subscribe(() => {
    }, (error) => {
      expect(error).toEqual(jasmine.any(HttpErrorResponse));
    });
  }));

  it('should fail deleting noId yacht', async(() => {
    yachtService.deleteYachtById('noId').subscribe(() => {
    }, (error) => {
      expect(error).toEqual(jasmine.any(HttpErrorResponse));
    });
  }));

  it('should fail like empty yacht', async(() => {
    localStorage.setItem('votes', String(0));
    yachtService.like('noId').subscribe(() => {
    }, (error) => {
      expect(error).toEqual(jasmine.any(HttpErrorResponse));
    });
  }));

  it('should create yacht', async(() => {
    yachtService.createYacht({
      'name': 'test',
      'alterEgo': 'test'
    }).subscribe((yacht) => {
      expect(yacht.id).not.toBeNull();
      yachtService.deleteYachtById(yacht.id).subscribe((response) => {
        expect(response).toEqual({});
      });
    });
  }));

  it('should not like a yacht because no votes', async(() => {
    localStorage.setItem('votes', String(AppConfig.votesLimit));
    expect(YachtService.checkIfUserCanVote()).toBe(false);
    yachtService.createYacht({
      'name': 'test',
      'alterEgo': 'test'
    }).subscribe((yacht) => {
      yachtService.like(yacht).subscribe(() => {
      }, (error) => {
        expect(error).toBe('maximum votes');
        yachtService.deleteYachtById(yacht.id).subscribe((response) => {
          expect(response).toEqual({});
        });
      });
    });
  }));

  it('should like a yacht', async(() => {
    localStorage.setItem('votes', String(0));
    expect(YachtService.checkIfUserCanVote()).toBe(true);
    yachtService.createYacht({
      'name': 'test',
      'alterEgo': 'test'
    }).subscribe((yacht) => {
      yachtService.like(yacht).subscribe((response) => {
        expect(response).toEqual({});
        yachtService.deleteYachtById(yacht.id).subscribe((res) => {
          expect(res).toEqual({});
        });
      });
    });
  }));

  it('should delete a yacht', async(() => {
    yachtService.createYacht({
      'name': 'test',
      'alterEgo': 'test'
    }).subscribe((yacht) => {
      yachtService.deleteYachtById(yacht.id).subscribe((response) => {
        expect(response).toEqual({});
      });
    });
  }));
});
