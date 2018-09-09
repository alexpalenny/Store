import {map, startWith} from 'rxjs/operators';
import {Component, OnInit} from '@angular/core';
import {FormControl} from '@angular/forms';
import {Router} from '@angular/router';
import {AppConfig} from '../../../config/app.config';
import {LoggerService} from '../../services/logger.service';
import {Hero} from '../../../modules/heroes/shared/hero.model';
import {Yacht} from '../../../modules/yachts/shared/yacht.model';
import {HeroService} from '../../../modules/heroes/shared/hero.service';
import {YachtService} from '../../../modules/yachts/shared/yacht.service';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss'],
  providers: [
    LoggerService
  ]
})

export class SearchBarComponent implements OnInit {

  defaultHeroes: Array<Hero>;
  defaultYachts: Array<Yacht>;
  heroFormControl: FormControl;
  yachtFormControl: FormControl;
  filteredHeroes: any;
  filteredYachts: any;

  constructor(private heroService: HeroService, private yachtService: YachtService,
              private router: Router) {
    this.defaultHeroes = [];
    this.heroFormControl = new FormControl();
    this.yachtFormControl = new FormControl();
  }

  ngOnInit() {
    this.heroService.getHeroes().subscribe((heroes: Array<Hero>) => {
      this.defaultHeroes = heroes.filter(hero => hero['default']);

      this.heroFormControl.valueChanges.pipe(
        startWith(null),
        map(value => this.filterHeroes(value)))
        .subscribe(heroesFiltered => {
          this.filteredHeroes = heroesFiltered;
        });
    });
    
    this.yachtService.getYachts().subscribe((yachts: Array<Yacht>) => {
      this.defaultYachts = yachts.filter(yacht => yacht['default']);

      this.yachtFormControl.valueChanges.pipe(
        startWith(null),
        map(value => this.filterYachts(value)))
        .subscribe(yachtsFiltered => {
          this.filteredYachts = yachtsFiltered;
        });
    });
  }

  filterHeroes(val: string): Hero[] {
    return val ? this.defaultHeroes.filter(yacht => yacht.name.toLowerCase().indexOf(val.toLowerCase()) === 0 && yacht['default'])
      : this.defaultHeroes;
  }
  filterYachts(val: string): Yacht[] {
    return val ? this.defaultYachts.filter(yacht => yacht.name.toLowerCase().indexOf(val.toLowerCase()) === 0 && yacht['default'])
      : this.defaultYachts;
  }

  searchHero(hero: Hero): Promise<boolean> {
    LoggerService.log('Moved to hero with id: ' + hero.id);
    return this.router.navigate([AppConfig.routes.heroes + '/' + hero.id]);
  }
  searchYacht(yacht: Yacht): Promise<boolean> {
    LoggerService.log('Moved to yacht with id: ' + yacht.id);
    return this.router.navigate([AppConfig.routes.yachts + '/' + yacht.id]);
  }
}
