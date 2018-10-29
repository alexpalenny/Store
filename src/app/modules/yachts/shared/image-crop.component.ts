import { Component, ViewEncapsulation } from '@angular/core';
import { CropperSettings } from 'ng2-img-cropper';

//https://www.npmjs.com/package/ng2-img-cropper
@Component({
    selector: 'crop-image',
    template: `<div class="crop-container">
        <img-cropper [image]="data" [settings]="cropperSettings"></img-cropper>
        <img *ngIf="false" [src]="data.image" [width]="cropperSettings.croppedWidth" [height]="cropperSettings.croppedHeight">        
        <button mat-raised-button (click)="saveImage(data)" color="primary">Сохранить</button>
    </div>`,
    styleUrls: ['./image-crop.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class CropComponent {
    data: any;
    cropperSettings: CropperSettings;

    constructor() {

        this.cropperSettings = new CropperSettings();
        this.cropperSettings.width = 300;
        this.cropperSettings.height = 230;
        this.cropperSettings.croppedWidth = 300;
        this.cropperSettings.croppedHeight = 230;
        this.cropperSettings.canvasWidth = 400;
        this.cropperSettings.canvasHeight = 300;

        this.data = {};
    }

    saveImage(data: any) {
        if (!data.image) return;
        console.log(data.image);
        debugger;
    }
}