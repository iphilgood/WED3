import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { Observable } from 'rxjs';

import { ResourceBase } from './resource-base';
import { SecurityTokenStore } from '../services/credential-management';
import { BankAccount } from '../../dashboard/models';
import { Transaction } from '../../dashboard/models';

@Injectable()
export class AccountResourceService extends ResourceBase {
  constructor(http: Http, private tokenStore: SecurityTokenStore) {
    super(http);
  }

  public getMe(): Observable<BankAccount> {
    return this.get('/accounts')
      .map((res: Response) => {
        const result = res.json();
        if (result) {
          return BankAccount.fromDto(result);
        }

        return null;
      })
      .catch((error: Response | any) => {
        return Observable.throw(error.message);
      });
  }

  public getAccountNr(accountNr: number): Observable<BankAccount> {
    return this.get(`/accounts/${accountNr}`)
      .map((res: Response) => {
        const result = res.json();
        if (result) {
          return BankAccount.fromDto(result);
        }

        return null;
      })
      .catch((error: Response | any) => {
        return Observable.throw(error.message);
      });
  }

  public transfer(transaction: Transaction): Observable<Transaction> {
    return this.post('/accounts/transactions', transaction.toDto())
      .map((res: Response) => {
        const result = res.json();
        if (result) {
          return Transaction.fromDto(result);
        }

        return null;
      })
      .catch((error: Response | any) => {
        return Observable.throw(error.message);
      });
  }
}
