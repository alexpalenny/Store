import {Component, OnInit, ViewChild} from '@angular/core';
import {Yacht} from '../../shared/yacht.model';
import {YachtService} from '../../shared/yacht.service';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {MatDialog} from '@angular/material';
import {Router} from '@angular/router';
import {LoggerService} from '../../../../core/services/logger.service';
import {AppConfig} from '../../../../config/app.config';
//import {YachtRemoveComponent} from '../../components/yacht-remove/yacht-remove.component';

@Component({
  selector: 'app-yachts-list',
  templateUrl: './yachts-list.page.html',
  styleUrls: ['./yachts-list.page.scss']
})

export class YachtsListPage implements OnInit {

  yachts: Yacht[];
  newYachtForm: FormGroup;
  canVote = false;
  error: string;
  @ViewChild('form') myNgForm; // just to call resetForm method

  constructor(private yachtService: YachtService,
              private dialog: MatDialog,
              private router: Router,
              private formBuilder: FormBuilder) {
    this.canVote = YachtService.checkIfUserCanVote();

    this.newYachtForm = this.formBuilder.group({
      'name': new FormControl('', [Validators.required]),
      'alterEgo': new FormControl('', [Validators.required])
    });
  }

  ngOnInit() {
    this.yachtService.getYachts().subscribe((yachts: Array<Yacht>) => {
      this.yachts = yachts.sort((a, b) => {
        return b.length - a.length;
      });
    });
  }

  like(yacht: Yacht) {
    this.yachtService.like(yacht).subscribe(() => {
      this.canVote = YachtService.checkIfUserCanVote();
    }, (error: Response) => {
      LoggerService.error('maximum votes limit reached', error);
    });
  }

  createNewYacht(newYacht: Yacht) {
    this.yachtService.createYacht(newYacht).subscribe((newYachtWithId) => {
      this.yachts.push(newYachtWithId);
      this.myNgForm.resetForm();
    }, (response: Response) => {
      if (response.status === 500) {
        this.error = 'errorHasOcurred';
      }
    });
  }

  seeYachtDetails(yacht): void {
    if (yacht.default) {
      this.router.navigate([AppConfig.routes.yachts + '/' + yacht.id]);
    }
  }

  // remove(yachtToRemove: Yacht): void {
  //   const dialogRef = this.dialog.open(YachtRemoveComponent);
  //   dialogRef.afterClosed().subscribe(result => {
  //     if (result) {
  //       this.yachtService.deleteYachtById(yachtToRemove.id).subscribe(() => {
  //         this.yachtService.showSnackBar('yachtRemoved');
  //         this.yachts = this.yachts.filter(yacht => yacht.id !== yachtToRemove.id);
  //       }, (response: Response) => {
  //         if (response.status === 500) {
  //           this.error = 'yachtDefault';
  //         }
  //       });
  //     }
  //   });
  // }
}
