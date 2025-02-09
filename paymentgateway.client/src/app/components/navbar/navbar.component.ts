import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: false,
  
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  constructor(private authService: AuthService, public router: Router) {}

  logout(): void {
    this.authService.logout();
  }

  redirectToDashboard() {
    this.router.navigate(['/dashboard']);
  }
}
