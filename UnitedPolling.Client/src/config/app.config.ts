import { Injectable } from '@angular/core';
import ClientConfigJson from './client-config.json';
import { provideHttpClient, withFetch } from '@angular/common/http';

@Injectable()
export class AppConfig {
    settings: any;
    constructor() {
		this.settings = ClientConfigJson;
		provideHttpClient(withFetch());
	}
	
    load() {
	
    }
}
