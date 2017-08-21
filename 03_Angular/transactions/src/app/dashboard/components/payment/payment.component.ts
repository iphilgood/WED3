import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { AccountService } from '../../../auth/services';
import { TransactionService } from '../../services';
import { BankAccount, Transaction } from '../../models';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent implements OnInit {
  private bankAccount: BankAccount;

  constructor(private accountService: AccountService,
              private transactionService: TransactionService) { }

  ngOnInit() {
    this.accountService.bankAccountChange.subscribe((bankAccount: BankAccount) => {
      this.bankAccount = bankAccount;
    });

    this.accountService.transactionSuccessfulChange.subscribe((transaction: Transaction) => {
      this.transactionService.getLatest();
    });

    this.accountService.getMe();
  }

  public transfer(form: NgForm): boolean {
    if (form.valid) {
      this.accountService.transfer(form.value.to, form.value.amount);
    }
    return false;
  }
}
