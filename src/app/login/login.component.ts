import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule , FormBuilder, FormGroup, Validators} from '@angular/forms';
import { FloatLabelModule } from "primeng/floatlabel"
import { InputTextModule } from 'primeng/inputtext';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { MessageService } from 'primeng/api';
import { AuthService } from '../services/auth.service';
import { AuthResponse } from '../models/auth-response.model';
import { AppMessageService } from '../services/message.service';

@Component({
  selector: 'app-login',
  imports: [FloatLabelModule, InputTextModule, FormsModule, CardModule, ButtonModule],
  standalone: true,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  providers: [MessageService]
})

export class LoginComponent {
  username = '';
  password = '';

  constructor(
    private router: Router,
    private messageService: AppMessageService,
    private authService: AuthService
  ) {  }

  login() {
    // Basic input validation
    if (!this.username || !this.password) {
      this.messageService.showError('Username and password are required.');
      return;
    }
    this.authService.login(this.username, this.password).subscribe({
      next: (response: AuthResponse) => {
        if (response.isSuccess) {
          localStorage.setItem('authToken', response.token);
          localStorage.setItem('loggedInUser', this.username);
          this.router.navigate(['/home']);
        } else {
          this.messageService.showError(response.errorMessage || 'Login failed. Please try again.');
        }      
      }
    });
  }
  
}


