import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Hero} from '../../../modules/heroes/shared/hero.model';
import {Yacht} from '../../../modules/yachts/shared/yacht.model';
import {HeroService} from '../../../modules/heroes/shared/hero.service';
import {YachtService} from '../../../modules/yachts/shared/yacht.service';
import {AppConfig} from '../../../config/app.config';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
})

export class HomePage implements OnInit {
  heroes: Hero[] = null;
  yachts: Yacht[] = null;
  canVote = false;

  constructor(private heroService: HeroService, private yachtService: YachtService,
              private router: Router) {
    this.canVote = HeroService.checkIfUserCanVote();
    this.canVote = YachtService.checkIfUserCanVote();
  }

  ngOnInit() {
    this.heroService.getHeroes().subscribe((heroes) => {
      this.heroes = heroes.sort((a, b) => {
        return b.likes - a.likes;
      }).slice(0, AppConfig.topHeroesLimit);
    });
    this.yachtService.getYachts().subscribe((yachts) => {
      this.yachts = yachts.sort((a, b) => {
        return b.likes - a.likes;
      }).slice(0, AppConfig.topYachtsLimit);
    });
  }

  like(hero: Hero): Promise<any> {
    return new Promise((resolve, reject) => {
      this.heroService.like(hero).subscribe(() => {
        this.canVote = HeroService.checkIfUserCanVote();
        resolve(true);
      }, (error) => {
        reject(error);
      });
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
