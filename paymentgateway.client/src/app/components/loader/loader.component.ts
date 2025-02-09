import { Component, OnInit } from '@angular/core';
import { LoaderService } from '../../services/loader.service';

@Component({
  selector: 'app-loader',
  template: `
    <div *ngIf="isLoading$ | async" class="loading-overlay">
      <div class="spinner-border text-primary"></div>
    </div>
  `,
  styles: [`
    .loading-overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: rgba(255, 255, 255, 0.7);
      display: flex;
      justify-content: center;
      align-items: center;
      z-index: 1000;
    }
  `]
})
export class LoaderComponent {
 // isLoading$ = this.loaderService.loading$;

  constructor(private loaderService: LoaderService) { }
}
