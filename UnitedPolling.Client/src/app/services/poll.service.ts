import { Injectable } from '@angular/core';  
import { HttpClient,HttpHeaders }    from '@angular/common/http';  
import { AppConfig } from '../../config/app.config';
import { protectedResources } from '../auth-config';

@Injectable({  
  providedIn: 'root'  
})  
  
export class PollService {  
  url = protectedResources.apiPolls.endpoint;
    
  constructor(private http: HttpClient, private appConfig: AppConfig) { }  
    
  httpOptions = {  
    headers: new HttpHeaders({  
      'Content-Type': 'application/json'  
    })  
  }    
  
  getPolls(){  
    return this.http.get(this.url + '/GetPolls');
  }  
  
  getPollQuestionList(id: any){  
    return this.http.get(this.url + '/GetPollQuestionList/' + id);
  } 

  createPoll(formData: any){  
    return this.http.post(this.url + '/CreatePoll', formData);
  }  
}  
