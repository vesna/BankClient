import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { inject } from '@angular/core';
import { Account } from '../models/account.model';
import { map, catchError } from 'rxjs/operators';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  httpClient = inject(HttpClient);
  baseUrl = 'https://localhost:7265/';
  constructor() { }

  async getAccounts(token : any) { 
    let httpOptions = {
      headers: {'Authorization': "Bearer " + token},
    }
    return await lastValueFrom(this.httpClient.get<Account>(`${this.baseUrl}account/accounts`, httpOptions)
    .pipe(map((response : Account) => response.accounts),
    catchError(error => {
      console.log('Caught in CatchError. Throwing error')
      throw new Error(error)
    })
  ));
  }
}
