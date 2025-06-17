// storage-cleanup.service.ts
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageCleanupService {
  constructor() {
    this.cleanStorage();
  }

  cleanStorage() {
    localStorage.clear();
    // Optional: Clear specific items instead of all
    // localStorage.removeItem('authToken');
    // localStorage.removeItem('userData');
  }
}