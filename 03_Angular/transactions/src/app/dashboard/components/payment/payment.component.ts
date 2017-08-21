import { Component, OnInit, ViewChild } from '@angular/core';
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
  private targetBankAccountHasMinLength = false;
  private targetBankAccount: BankAccount;
  private successfulTransaction: Transaction;
  private isProcessed = false;

  @ViewChild('payForm') payForm: NgForm;

  constructor(private accountService: AccountService,
              private transactionService: TransactionService) { }

  ngOnInit() {
    this.accountService.bankAccountChange.subscribe((bankAccount: BankAccount) => {
      this.bankAccount = bankAccount;
    });

    this.accountService.transactionSuccessfulChange.subscribe((transaction: Transaction) => {
      this.successfulTransaction = transaction;
      this.isProcessed = true;
      this.payForm.controls['to'].setValue('');
      this.payForm.controls['amount'].setValue('');

      this.accountService.getMe();
      this.transactionService.getLatest();
    });

    this.accountService.targetBankAccountChange.subscribe((targetBankAccount: BankAccount) => {
      this.targetBankAccount = targetBankAccount;
      if (targetBankAccount && targetBankAccount.owner.accountNr === this.bankAccount.owner.accountNr) {
        this.targetBankAccount = null;
      }
    });

    this.accountService.getMe();
  }

  public transfer(form: NgForm): boolean {
    if (form.valid) {
      this.accountService.transfer(form.value.to, form.value.amount);
    }
    return false;
  }

  public targetChanged(targetControlValue): void {
    this.accountService.getAccountNr(targetControlValue);
    if (targetControlValue.length > 2) {
      this.targetBankAccountHasMinLength = true;
    } else {
      this.targetBankAccountHasMinLength = false;
    }
  }
}
