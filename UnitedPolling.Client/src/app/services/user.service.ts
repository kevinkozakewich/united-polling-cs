import { Injectable } from '@angular/core';  
import { HttpClient,HttpHeaders }    from '@angular/common/http';  
import { AppConfig } from '../../config/app.config';
@Injectable({  
  providedIn: 'root'  
})  
  
export class UserService {  
  
constructor(private http: HttpClient, private appConfig: AppConfig) { }  
  httpOptions = {  
    headers: new HttpHeaders({  
      'Content-Type': 'application/json'  
    })  
  }    
  
  loginUser(){  
    return this.http.get('https://localhost:7294/api/User/loginUser');
  } 
}  
