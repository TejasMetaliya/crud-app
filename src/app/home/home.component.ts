import { Component , OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../services/user.service';
import { AllUser, User } from '../models/user.model';
import { HeaderComponent } from "../header/header.component";
import { MproductComponent } from '../mproduct/mproduct.component';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';


import { DialogModule } from 'primeng/dialog';

import { LoadingService } from '../services/loading.service';
import { AppMessageService } from '../services/message.service';

@Component({
  selector: 'app-home',
  imports: [CommonModule, HeaderComponent, MproductComponent, TableModule, ButtonModule, DialogModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  users: AllUser[] = [];
  // isLoading = false;

  constructor(
    private userService: UserService, 
    private loadingService: LoadingService, 
    private messageService: AppMessageService
  ) { }
  
  loadUsers(): void {
    this.loadingService.show();
    
    this.userService.getAllUsers().subscribe({
      next: (data) => {
        this.users = data;
        this.loadingService.hide();
      },
      error: (err) => {
        if (err.status === 401) {
          this.messageService.showError('Please login to access user data');
        } else {
          this.messageService.showError('Error loading user data');
        }
        this.loadingService.hide();
      }
    });
  }

  dialogVisible: boolean = false;

  showDialog() {
    this.loadUsers();
    this.dialogVisible = true;
  }
}