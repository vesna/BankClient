import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from '../../auth/auth.service';
import { UserService } from '../../service/user.service';
import { AccountService } from '../../service/account.service';
import { Router } from '@angular/router';
import { User } from '../../models/user.model';
import { Account } from '../../models/account.model';
import { CommonModule, NgForOf } from '@angular/common';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent implements OnInit{
  authService = inject(AuthService);
  userService = inject(UserService);
  accountService = inject(AccountService);
  router = inject(Router);
  user!: User;
  accounts: Account[] = []

  constructor() { }

  async ngOnInit() {
    const token = this.authService.getToken();
    if(token == "" || token == null){
       this.router.navigateByUrl('/login')
     }else {
        this.user = await this.userService.getUserInfo(token);
        this.accounts = await this.accountService.getAccounts(token);
     }
   }

  public logout(){
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}