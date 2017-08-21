import { Injectable, EventEmitter } from '@angular/core';

import { AccountResourceService } from '../resources';
import { BankAccount, Transaction } from '../../dashboard/models';

@Injectable()
export class AccountService {
  public bankAccountChange = new EventEmitter<BankAccount>();
  public targetBankAccountChange = new EventEmitter<BankAccount>();
  public transactionSuccessfulChange = new EventEmitter<Transaction>();

  constructor(private resource: AccountResourceService) { }

  public getMe(): void {
    this.resource.getMe().subscribe((data: BankAccount) => {
      this.bankAccountChange.emit(data);
    });
  }

  public getAccountNr(accountNr: number): void {
    this.resource.getAccountNr(accountNr).subscribe((data: BankAccount) => {
      this.targetBankAccountChange.emit(data);
    });
  }

  public transfer(target: string, amount: number): void {
    const transaction = new Transaction(null, target, amount, null, null);

    this.resource.transfer(transaction).subscribe((data: Transaction) => {
      this.transactionSuccessfulChange.emit(data);
    });
  }
}
