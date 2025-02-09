import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user',
  standalone: false,
  
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit {
  user : any;

  constructor( private http: HttpClient ) {}

  ngOnInit(): void {
    this.fetchUserDetails();
  }

  fetchUserDetails() {
    this.http.get('https://localhost:44390/api/account')
    .subscribe({
      next: (res: any) =>{
        this.user = res;
        console.log(this.user);
      },
      error: err =>{
        console.error('Error fetching user details', err);
      }
      
    })
  }
}
