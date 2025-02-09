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
  isSubmitted: boolean = false;

  constructor(
    private http: HttpClient,
    private formBuilder: FormBuilder,
    private authService: AuthService
  ) {
    this.transactionForm = this.formBuilder.group({
      Amount: [0, Validators.required],
      Currency: ['NPR', Validators.required],
      PaymentMethod: [this.paymentMethods.BankTransfer, Validators.required],
      CardNumber: [''],
      ExpiryDate: [''],
      Cvv: [0],
      AccountNumber: ['',[Validators.required, Validators.minLength(10), Validators.maxLength(18)]],
      BankName: ['',[Validators.required]]
    });
    this.transactionForm.get('PaymentMethod')?.valueChanges.subscribe(value => {
        this.paymentMethod = +value;
        this.transactionForm.get('PaymentMethod')?.setValue(this.paymentMethod, { emitEvent: false });
        this.updateFormAndValidator(this.paymentMethod);
      });
  }

  ngOnInit(): void {
    this.fetchTransactions();
  }

  get f() {
    return this.transactionForm.controls;
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
    this.isSubmitted = true;
    if (this.transactionForm.invalid) {
        console.log('Invalid form');
      return;
    }

    const createPayment = {
      UserId: this.authService.getUserId(),
      ...this.transactionForm.value
    };

    const headers = new HttpHeaders()
  .set('Content-Type', 'application/json');

    const requestData = JSON.stringify(createPayment);
   
    this.http.post('https://localhost:44390/api/payment/process', requestData, { headers })
      .subscribe({
        next: (res: any) => {
        this.fetchTransactions();
          this.closeModal();
        },
        error: err => {
          console.error('Error adding transaction', err);
        },
        complete: () => {
          this.isSubmitted = false;
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

  updateFormAndValidator(value:number){
    if (value === this.paymentMethods.Card) {
        // Enable and set validators for Card fields
        this.transactionForm.get('CardNumber')?.setValidators([Validators.required, Validators.minLength(13), Validators.maxLength(19)]);
        this.transactionForm.get('ExpiryDate')?.setValidators([Validators.required]);
        this.transactionForm.get('Cvv')?.setValidators([Validators.required, Validators.minLength(3), Validators.maxLength(4)]);
    
        // Clear Bank fields & remove validators
        this.transactionForm.get('AccountNumber')?.reset();
        this.transactionForm.get('AccountNumber')?.clearValidators();
        this.transactionForm.get('BankName')?.reset();
        this.transactionForm.get('BankName')?.clearValidators();
      } 
      else {
        // Enable and set validators for Bank fields
        this.transactionForm.get('AccountNumber')?.setValidators([Validators.required, Validators.minLength(10), Validators.maxLength(18)]);
        this.transactionForm.get('BankName')?.setValidators([Validators.required]);
    
        // Clear Card fields & remove validators
        this.transactionForm.get('CardNumber')?.reset();
        this.transactionForm.get('CardNumber')?.clearValidators();
        this.transactionForm.get('ExpiryDate')?.reset();
        this.transactionForm.get('ExpiryDate')?.clearValidators();
        this.transactionForm.get('Cvv')?.reset();
        this.transactionForm.get('Cvv')?.clearValidators();
      }
    
      // Update form controls to reflect validation changes
      this.transactionForm.get('CardNumber')?.updateValueAndValidity();
      this.transactionForm.get('ExpiryDate')?.updateValueAndValidity();
      this.transactionForm.get('Cvv')?.updateValueAndValidity();
      this.transactionForm.get('AccountNumber')?.updateValueAndValidity();
      this.transactionForm.get('BankName')?.updateValueAndValidity();
  }
}
