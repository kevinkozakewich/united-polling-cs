import { Injectable } from '@angular/core';
import { AppConfig } from '../config/app.config';

@Injectable()
export class Helper {
    settings: any;
	logging: boolean = true;
  
    constructor(public appConfig: AppConfig) {
	
	}

	log(data: any) {
		if(this.appConfig.settings.debugMode) {
			console.log(data);
		}
	}

	addDays(date: any, days: any) {
		return new Date(date.getTime() + (1000 * 60 * 60 * 24) * days);
	}
}
