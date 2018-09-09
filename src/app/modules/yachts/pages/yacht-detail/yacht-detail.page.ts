import {Component, OnInit} from '@angular/core';
import {Yacht} from '../../shared/yacht.model';
import {YachtService} from '../../shared/yacht.service';
import {ActivatedRoute} from '@angular/router';
import {Location} from '@angular/common';

@Component({
  selector: 'app-yacht-detail',
  templateUrl: './yacht-detail.page.html',
  styleUrls: ['./yacht-detail.page.scss']
})

export class YachtDetailPage implements OnInit {

  yacht: Yacht;
  canVote: boolean;

  constructor(private yachtService: YachtService,
              private location: Location,
              private activatedRoute: ActivatedRoute) {
  }

  ngOnInit() {
    const yachtId = this.activatedRoute.snapshot.paramMap.get('id');
    this.yachtService.getYachtById(yachtId).subscribe((yacht: Yacht) => {
      this.yacht = yacht;
    });
  }

  like(yacht: Yacht) {
    return new Promise((resolve, reject) => {
      this.yachtService.like(yacht).subscribe(() => {
        this.canVote = YachtService.checkIfUserCanVote();
        resolve(true);
      }, (error) => {
        reject(error);
      });
    });
  }

  dynamicImport() {
    import('html2canvas').then((html2canvas: any) => {
      html2canvas.default(document.getElementById('yacht-detail')).then((canvas) => {
        window.open().document.write('<img src="' + canvas.toDataURL() + '" />');
      });
    });
  }

  goBack(): void {
    this.location.back();
  }
}
