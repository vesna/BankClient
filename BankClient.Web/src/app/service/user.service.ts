import { Injectable } from '@angular/core';
import { inject } from '@angular/core';
import { User } from '../models/user.model';
import { HttpClient} from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  httpClient = inject(HttpClient);
  baseUrl = 'https://localhost:7265/';
  constructor() { }

  async getUserInfo(token : any){ 
    let httpOptions = {
      headers: {'Authorization': "Bearer " + token},
    }
    return await lastValueFrom(this.httpClient.get<User>(`${this.baseUrl}user/info`, httpOptions)
    .pipe(map((response : User) => response),
    catchError(error => {
      console.log('Caught in CatchError. Throwing error')
      throw new Error(error)
    })
  ));
  }
}
