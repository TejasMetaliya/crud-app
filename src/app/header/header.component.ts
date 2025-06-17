import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit{

  constructor(private router: Router) {}
  reloadPage() {
      window.location.reload();
  }
  logout() {
    // Remove authentication data from localStorage
    localStorage.removeItem('authToken');
    localStorage.removeItem('loggedInUser');    
    this.router.navigate(['/login']); //redirect to login page
  }
  loggedInUser: string | null = null;
  ngOnInit() {
    this.loggedInUser = localStorage.getItem('loggedInUser');
  }
}
