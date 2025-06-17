import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthResponse } from '../models/auth-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7153/api/Auth'; // ✅ Replace with your actual API URL

  constructor(private http: HttpClient) {}

  login(username: string, password : string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, 
        { 
            Name: username, 
            Password: password
        });
  }
}
