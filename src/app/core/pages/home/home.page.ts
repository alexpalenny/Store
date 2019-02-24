import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Yacht} from '../../../modules/yachts/shared/yacht.model';
import {YachtService} from '../../../modules/yachts/shared/yacht.service';
import {AppConfig} from '../../../config/app.config';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
})

export class HomePage implements OnInit {
  yachts: Yacht[] = null;
  canVote = false;

  constructor(private yachtService: YachtService,
              private router: Router) {
    this.canVote = YachtService.checkIfUserCanVote();
  }

  ngOnInit() {
    this.yachtService.getYachts().subscribe((yachts) => {
      this.yachts = yachts.sort((a, b) => {
        return 0;
      }).slice(0, AppConfig.topYachtsLimit);
    });
  }

  likeY(yacht: Yacht): Promise<any> {
    return new Promise((resolve, reject) => {
      this.yachtService.like(yacht).subscribe(() => {
        this.canVote = YachtService.checkIfUserCanVote();
        resolve(true);
      }, (error) => {
        reject(error);
      });
    });
  }

  seeHeroDetails(hero): void {
    if (hero.default) {
      this.router.navigate([AppConfig.routes.heroes + '/' + hero.id]);
    }
  }
  seeYachtDetails(yacht): void {
    if (yacht.default) {
      this.router.navigate([AppConfig.routes.yachts + '/' + yacht.id]);
    }
  }
}
