import { Injectable } from '@angular/core';
import { Response, Http } from '@angular/http';

import { Observable } from 'rxjs';

import { ResourceBase } from '../../auth/resources';
import { Transaction } from '../models';
import { SecurityTokenStore } from '../../auth';

@Injectable()
export class TransactionResourceService extends ResourceBase {
  constructor(http: Http, private tokenStore: SecurityTokenStore) {
    super(http);
  }

  public getBetween(from: Date, to: Date): Observable<Transaction[]> {
    return this.get(`/accounts/transactions?fromDate=${from}&toDate=${to}`)
      .map((res: Response) => {
        const result = res.json().result;
        if (result) {
          return result.map(transaction => Transaction.fromDto(transaction));
        }
        return null;
      })
      .catch((error: Response | any) => {
        return Observable.throw(error.message);
      });
  }

  public getLatest(count: number = 3): Observable<Transaction[]> {
    return this.get(`/accounts/transactions?count=${count}`)
      .map((res: Response) => {
        const result = res.json().result;
        if (result) {
          return result.map(transaction => Transaction.fromDto(transaction));
        }
      })
      .catch((error: Response | any) => {
        return Observable.throw(error.message);
      });
  }
}
