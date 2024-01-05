import { Component, OnInit, Inject, ViewChild  } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { Helper } from '../../../../helper'; 
import { PollService } from '../../../../services/poll.service';

@Component({
  selector: 'app-poll-dialog',
  templateUrl: './poll-dialog.component.html'
})
export class PollDialogComponent implements OnInit {
  formChanged: boolean = false;
  pageOneComplete: boolean = false;
  displayPollCreationError: boolean = false;
  name: string = "";
  urlShareable: boolean = false;

  constructor(
	  public PollDialogComponent: MatDialogRef<PollDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public dialogData: { /* Supply data to popup here. */ },
	  private helper: Helper,
	  private pollService: PollService) {
	    PollDialogComponent.disableClose = true;  
		this.PollDialogComponent.backdropClick().subscribe(async () => await this.safeClose());
	}
	
	ngOnInit() { }
	
	public async safeClose(result?: string): Promise<void> {
		this.cancel();
	}
	
	onChangeName(result: any) {
		var name = result.target.value.toString();
		this.helper.log("Name: '" + name + "'");
		this.formChanged = true;

		if(name.length > 50) 
		{ 
			name = name.substring(0, 50);
			result.target.value = name;
		}

		this.name = name;
		this.checkPageOneComplete();
	}
	
	onChangeUrlShareable(v: EventTarget) {
		let checked: boolean = ((v as Element) as HTMLInputElement).checked;
		this.helper.log("Url Shareable: " + checked);
		this.formChanged = true;
		this.urlShareable = checked;
		this.checkPageOneComplete();
	}
	
	checkPageOneComplete() {
		this.pageOneComplete = this.name.length > 1 && this.name.length < 50;
	}

    save() {
		this.helper.log(
			"Saving: " + 
			this.name.toString() + ", " +
			this.urlShareable.toString()
		);
		
		let PollForm: FormGroup = new FormGroup({  
			Title: new FormControl(this.name,[Validators.required]),        
			UrlShareable: new FormControl(this.urlShareable,[Validators.required])
		});    
		this.pollService.createPoll(PollForm.value).subscribe((data: any) => {  
			if(data != null && data.title != null) {
				this.PollDialogComponent.close();
			}
			else {
				this.helper.log("Poll Creation Error");
				this.displayPollCreationError = true;
			}
		});
    }
	
	cancelClicked(event: Event) {
		event.preventDefault();
		this.cancel();
	}
	cancel() {
		if(!this.formChanged || confirm("Are you sure you want to exit without saving?")) {
			this.PollDialogComponent.close();
		}
	}
}
