import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: false,
  
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  currentYear: number = new Date().getFullYear();
  constructor(private router: Router) {}

  isActive(route: string): boolean {
    return this.router.url.includes(route);
  }

  navigateToUserDetails(): void {
    this.router.navigate(['/user']);
  }

  navigateToTransactionDetails(): void {
    this.router.navigate(['/transactions']);
  }
}
