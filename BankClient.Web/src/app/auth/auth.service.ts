import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  httpClient = inject(HttpClient);
  baseUrl = 'https://localhost:7265/';
  constructor() { }

  signup(data: any) {
    return this.httpClient.post(`${this.baseUrl}Auth/register`, data, {responseType: 'text'})
    .pipe(map((result) => {
      const token = result;
      if (token) {
        localStorage.setItem('token', token);
      }
    }));
  }

  login(data: any){
    return this.httpClient.post(`${this.baseUrl}Auth/login`, data, {responseType: 'text'})
      .pipe(map((result) => {
        const token = result;
        if (token) {
          console.log(token);
          localStorage.setItem('token', token);
        }
      }));
    }

  logout() {
    localStorage.removeItem('token');
  }

  isLoggedIn() {
    return localStorage.getItem('token') !== null;
  }

  getToken(){
    return localStorage.getItem('token');
  }
}
