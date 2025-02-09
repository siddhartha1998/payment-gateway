import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  transactions: any[] = [];
  transactionCount: number = 0;
  transactionAmount: number = 0;

  constructor(private authService: AuthService, private router: Router, private http: HttpClient) {}

  ngOnInit() {
    this.fetchTransactions();
  }

  fetchTransactions() {
    this.http.get('https://localhost:44390/api/payment')
    .subscribe({
      next: (res: any) =>{
        this.transactions = res;
        this.transactionCount = this.transactions.length;
        this.transactionAmount = this.transactions.reduce((sum, transaction) => sum + transaction.amount, 0);
      },
      error: err =>{
        console.error('Error fetching transactions', err);
      }
      
    })
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
