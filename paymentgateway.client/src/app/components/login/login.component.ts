import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  siginInsubmitted: boolean = false;
  isLoggedIn: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private formBuilder: FormBuilder,
  ) {
    this.loginForm = this.formBuilder.group({
      UserName: ["", Validators.required],
      Password: ["", Validators.required]
    });
  }

  get f() {
    return this.loginForm.controls;
  }

  login(): void {
    this.siginInsubmitted = true;
    if (this.loginForm.invalid) {
      return;
    }

    const loginCredential = this.loginForm.value;

    this.authService.login(loginCredential).subscribe(
      (res) => {
        this.isLoggedIn = true;
        this.router.navigate(['/dashboard']); // Navigate to dashboard
      },
      (err) => {
        this.isLoggedIn = false;
        this.handleError(err);
      }
    );
  }

  private handleError(err: any): void {
    if (err.status === 401) {
      // Handle unauthorized error
      this.router.navigate(['/login']);
    } else {
      // Handle other errors
      console.error(err);
    }
  }
}
