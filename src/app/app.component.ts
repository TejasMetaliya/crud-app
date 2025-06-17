import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { StorageCleanupService } from './services/storage-cleanup.service';
import { ToastModule } from 'primeng/toast';
import { LoadingComponent } from "./loading/loading.component";

@Component({
  selector: 'app-root',
  imports: [CommonModule, RouterOutlet, ToastModule, LoadingComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})

export class AppComponent implements OnInit{
  //title = 'CRUD-app';

  constructor(private storageCleanup: StorageCleanupService) {}

  ngOnInit(): void {
    // this.router.events
    //   .pipe(filter(event => event instanceof NavigationEnd))
    //   .subscribe((event: any) => {
    //     // Only show header on non-login routes
    //     this.showHeader = !event.url.includes('/login');
    //   });

    this.storageCleanup.cleanStorage(); // Clean storage using service on app initialization
  }
}
