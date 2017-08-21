import { Injectable, EventEmitter } from '@angular/core';

import { Transaction } from '../models';
import { TransactionResourceService } from '../resources/index';

@Injectable()
export class TransactionService {
  public transactionChange: EventEmitter<Transaction[]> =
    new EventEmitter<Transaction[]>();

  constructor(private resource: TransactionResourceService) { }

  public getBetween(from: Date, to: Date): void {
    this.resource.getBetween(from, to).subscribe((data: Transaction[]) => {
      this.transactionChange.emit(data);
    });
  }

  public getLatest(count: number = 3): void {
    this.resource.getLatest(count).subscribe((data: Transaction[]) => {
      this.transactionChange.emit(data);
    });
  }
}
