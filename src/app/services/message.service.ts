import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class AppMessageService {
  constructor(private primeMessageService: MessageService) {}

  // Success message
  showSuccess(detail: string, summary: string = 'Success', life: number = 3000): void {
    this.primeMessageService.add({
      severity: 'success',
      summary,
      detail,
      life
    });
  }

  // Error message
  showError(detail: string, summary: string = 'Error', life: number = 3000): void {
    this.primeMessageService.add({
      severity: 'error',
      summary,
      detail,
      life
    });
  }

  // Info message
  showInfo(detail: string, summary: string = 'Info', life: number = 3000): void {
    this.primeMessageService.add({
      severity: 'info',
      summary,
      detail,
      life
    });
  }

  // Warning message
  showWarning(detail: string, summary: string = 'Warning', life: number = 3000): void {
    this.primeMessageService.add({
      severity: 'warn',
      summary,
      detail,
      life
    });
  }

  // Clear all messages
  clear(): void {
    this.primeMessageService.clear();
  }
}