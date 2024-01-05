import { Component, OnInit } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Helper } from '../../../helper'; 

@Component({
  selector: 'loading',
  templateUrl: './loading.component.html',
})
export class LoadingComponent implements OnInit {
	displaySpinner: boolean = true;
	interval: any;

	constructor(public helper: Helper) {}

	ngOnInit() {
		this.helper.log("Loading");
		this.displaySpinner = true;
	}
}
