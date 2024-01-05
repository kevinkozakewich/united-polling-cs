import { Component, OnInit } from '@angular/core';
import {  Router } from '@angular/router';
import { Helper } from './helper'; 

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'United Polling';
  constructor(public helper: Helper, 
	public router: Router) {}

  ngOnInit() { }
}
