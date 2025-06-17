import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://localhost:7153/api/Users'; // Replace with your API base URL

  constructor(private http: HttpClient) { }

    // Method to get the auth token from storage
  private getAuthToken(): string | null {
    return localStorage.getItem('authToken'); // or your token storage method
  }

  // Method to create headers with auth token
  private getAuthHeaders(): HttpHeaders {
    const token = this.getAuthToken();
    if (!token) {
      throw new Error('No authentication token available');
    }
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }
  
  getAllUsers(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/exportData`, { headers: this.getAuthHeaders() });
  }

}