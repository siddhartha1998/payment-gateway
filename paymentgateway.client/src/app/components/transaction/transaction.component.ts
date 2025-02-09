import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { PaymentMethod } from '../../shared/enums/payment-method.enum';

@Component({
  selector: 'app-transaction',
  standalone: false,
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.css']
})
export class TransactionComponent implements OnInit {
  transactions: any[] = [];
  showModal: boolean = false;
  transactionForm: FormGroup;
  paymentMethods = PaymentMethod;
  viewTransactionDetails: boolean = false;
  selectedTransaction: any = null;
  paymentMethod: any = 1;

  constructor(
    private http: HttpClient,
    private formBuilder: FormBuilder,
    private authService: AuthService
  ) {
    this.transactionForm = this.formBuilder.group({
      Amount: ['', Validators.required, Validators.min(0)],
      Currency: ['NPR', Validators.required],
      PaymentMethod: [this.paymentMethods.BankTransfer, Validators.required],
      CardNumber: [''],
      ExpiryDate: [''],
      Cvv: [''],
      AccountNumber: [''],
      BankName: ['']
    });
    this.transactionForm.get('PaymentMethod')?.valueChanges.subscribe(value => {
        this.paymentMethod = +value;
        // Reset other fields if necessary
        if (+value !== this.paymentMethods.Card) {
          this.transactionForm.get('CardNumber')?.reset();
          this.transactionForm.get('ExpiryDate')?.reset();
          this.transactionForm.get('Cvv')?.reset();
        }
        if (+value !== this.paymentMethods.BankTransfer) {
          this.transactionForm.get('AccountNumber')?.reset();
          this.transactionForm.get('BankName')?.reset();
        }
      });
  }

  ngOnInit(): void {
    this.fetchTransactions();
  }

  fetchTransactions() {
    this.http.get('https://localhost:44390/api/payment')
    .subscribe({
      next: (res: any) =>{
        this.transactions = res;
      },
      error: err =>{
        console.error('Error fetching transactions', err);
      }
      
    })
  }

  openModal(): void {
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.transactionForm.reset();
  }

  async addTransaction(): Promise<void> {
    if (this.transactionForm.invalid) {
      return;
    }

    const createPayment = {
      UserId: this.authService.userId, // Fetch user ID from AuthService
      ...this.transactionForm.value
    };

    const requestData = JSON.stringify(createPayment);
   
    this.http.post('https://localhost:44390/api/payment/process', requestData)
      .subscribe({
        next: (res: any) => {
          this.transactions.push(res);
          this.closeModal();
        },
        error: err => {
          console.error('Error adding transaction', err);
        }
      });
  }

  viewTransaction(transaction: any): void {
    this.selectedTransaction = transaction;
    this.viewTransactionDetails = true;
  }

  closeViewModal(): void {
    this.viewTransactionDetails = false;
    this.selectedTransaction = null;
  }
}
