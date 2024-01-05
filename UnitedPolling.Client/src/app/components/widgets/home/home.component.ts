import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AppConfig } from '../../../../config/app.config'; 
import { Helper } from '../../../helper'; 
import { PollService } from '../../../services/poll.service';

import { PollDialogComponent } from '../../../shared/components/modals/poll-dialog/poll-dialog.component';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
	pollLoaded: boolean = false;
	pollData: any[] = [];
	
	currentDate: any = new Date(); 	
	
	constructor(
		private dialog: MatDialog,
		public appConfig: AppConfig,
		public helper: Helper,
		public pollService: PollService) {
			
		}  
		
	
	ngOnInit() { this.load(); }

    load() {
		this.helper.log("Retrieving information from asp.");

		this.pollService.getPolls().subscribe((pollData: any) => {  
			this.pollData = pollData.pollList;
			this.pollLoaded = true;
		});
	}
	
	createPollOnClick(event: Event): boolean {
        event.preventDefault();
		let dialogRef = this.dialog.open(PollDialogComponent, { data: { } });
		dialogRef.afterClosed().subscribe(result => {
			this.ngOnInit();
		});
		return false;
	}
}
