import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'https://localhost:44390';
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  public isLoggedIn$ = this.isLoggedInSubject.asObservable();
  private _user: any;
  public userId: number = 0;

  constructor(private http: HttpClient, private router: Router) {}

  login(loginCredential: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, loginCredential, { responseType: "json" })
      .pipe(tap((response: any) => {
        localStorage.setItem('token', response.token); 
        localStorage.setItem('userId',response.userId);
        localStorage.setItem('sessionActive', 'true'); 
      }));
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.setItem('sessionActive', 'false'); 
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    //return !!localStorage.getItem('token') && !this.isTokenExpired();
    return !!localStorage.getItem('token');
  }

//   decodeToken(): any {
//     const rawToken = localStorage.getItem("token");
//     if (rawToken != null) {
//       const base64Url = rawToken.split('.')[1];
//       const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
//       const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
//         return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
//       }).join(''));
//       return JSON.parse(jsonPayload);
//     } else {
//       return null;
//     }
//   }

  showLoginPageIfTokenExpires(): void {
    if (this.isTokenExpired()) {
      this.isLoggedInSubject.next(false);
      this.logout();
    } else {
      this.isLoggedInSubject.next(true);
    }
  }

  getTokenExpirationDate(): Date {
    const date = localStorage.getItem("expiration");
    return new Date(date as string);
  }

  isTokenExpired(): boolean {
    const rawToken = localStorage.getItem("token");
    if (rawToken == null) return true;

    const date = this.getTokenExpirationDate();
    if (date === undefined) return false;
    return !(date.valueOf() > new Date().valueOf());
  }

  getUserId(): number {
    return (localStorage.getItem("userId")) ? parseInt(localStorage.getItem("userId") as string): 0;
  }
}