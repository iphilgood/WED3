import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { TransactionService } from '../../services';
import { Transaction } from '../../models';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {
  private transactions: Transaction[];
  private _selectedYear: number;
  private _selectedMonth: number;

  public years: number[];
  public months: string[];

  constructor(private transactionService: TransactionService) {
    this.years = Array.from(Array(3)).map((x, i) => new Date().getFullYear() - i);
    this.months = Array.from(Array(12)).map((x, i) => new Date(`${i + 1}/01/2000`).toLocaleString('en-us', { month: 'long' }));
  }

  ngOnInit() {
    this.transactionService.transactionChange.subscribe((transactions: Transaction[]) => {
      this.transactions = transactions;
    });

    const date = new Date();
    this.selectedYear = date.getFullYear();
    this._selectedMonth = date.getMonth();
    this.updateTransactions();
  }

  private updateTransactions(): void {
    const firstDay = new Date(this._selectedYear, this._selectedMonth, 1);
    const lastDay = new Date(this._selectedYear, this._selectedMonth + 1, 0);

    this.transactionService.getBetween(firstDay, lastDay);
  }

  public get selectedMonth(): number {
    return this._selectedMonth;
  }

  public set selectedMonth(month: number) {
    this._selectedMonth = month;
    this.updateTransactions();
  }

  public get selectedYear(): number {
    return this._selectedYear;
  }

  public set selectedYear(year: number) {
    this._selectedYear = year;
    this.updateTransactions();
  }
}
