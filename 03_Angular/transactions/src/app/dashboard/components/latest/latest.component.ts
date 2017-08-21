import { Component, OnInit } from '@angular/core';

import { Transaction } from '../../models';
import { TransactionService } from '../../services';

@Component({
  selector: 'app-latest',
  templateUrl: './latest.component.html',
  styleUrls: ['./latest.component.css']
})
export class LatestComponent implements OnInit {
  private transactions: Transaction[] = [];

  constructor(private transactionService: TransactionService) { }

  ngOnInit() {
    this.transactionService.transactionChange.subscribe((transactions) => {
      this.transactions = transactions;
    });

    this.transactionService.getLatest();
  }
}
