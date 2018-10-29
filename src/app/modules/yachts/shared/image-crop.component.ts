import { Component,ViewEncapsulation } from '@angular/core';
import { CropperSettings } from 'ng2-img-cropper';

//https://www.npmjs.com/package/ng2-img-cropper
@Component({
    selector: 'crop-image',
    template: `<div>
        <img-cropper [image]="data" [settings]="cropperSettings"></img-cropper><br>
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
}