import {map, startWith} from 'rxjs/operators';
import {Component, OnInit} from '@angular/core';
import {FormControl} from '@angular/forms';
import {Router} from '@angular/router';
import {AppConfig} from '../../../config/app.config';
import {LoggerService} from '../../services/logger.service';
import {Yacht} from '../../../modules/yachts/shared/yacht.model';
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
  defaultYachts: Array<Yacht>;
  heroFormControl: FormControl;
  yachtFormControl: FormControl;
  filteredHeroes: any;
  filteredYachts: any;

  constructor(private yachtService: YachtService,
              private router: Router) {
    this.heroFormControl = new FormControl();
    this.yachtFormControl = new FormControl();
  }

  ngOnInit() {
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

  filterYachts(val: string): Yacht[] {
    return val ? this.defaultYachts.filter(yacht => yacht.name.toLowerCase().indexOf(val.toLowerCase()) === 0 && yacht['default'])
      : this.defaultYachts;
  }

  searchYacht(yacht: Yacht): Promise<boolean> {
    LoggerService.log('Moved to yacht with id: ' + yacht.id);
    return this.router.navigate([AppConfig.routes.yachts + '/' + yacht.id]);
  }
}
