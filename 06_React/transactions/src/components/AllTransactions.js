// @flow
import React from 'react';

import type { User } from '../api';
import type { Transaction } from '../api';

import { getTransactions } from '../api';

export type Props = {
  token: string,
  user: User,
}

class AllTransactions extends React.Component {
  props: Props;
  years: Number[];
  months: String[];

  yearOptions: {
    key: Number,
    value: Number,
    text: Number,
  }[];

  monthOptions: {
    key: Number,
    text: String,
    value: Number,
  }[]

  state: {
    transactions: Transaction[],
    selectedYear: Number,
    selectedMonth: Number
  }

  constructor(props: any) {
    super(props);

    this.years = Array.from(Array(3)).map((x, i) => new Date().getFullYear() - i);
    this.yearOptions = this.years.map((value, index) => { return { key: index, text: value, value: value}})

    this.months = Array.from(Array(12)).map((x, i) => new Date(`${i + 1} / 01/2000`).toLocaleString('en-us', { month: 'long' }));
    this.monthOptions = this.months.map((value, index) => { return { key: index, text: value, value: index}})

    const date = new Date();
    this.state = {
      transactions: [],
      selectedYear: date.getFullYear(),
      selectedMonth: date.getMonth(),
    }
  }

  componentDidMount() {
    const { selectedYear, selectedMonth } = this.state;
    this.updateTransactions(selectedYear, selectedMonth);
  }

  yearChanged = (event: Event) => {
    const year = event.target.value;
    this.setState({ selectedYear: year });
    this.updateTransactions(year, this.state.selectedMonth);
  }

  monthChanged = (event: Event) => {
    const month = event.target.value;
    this.setState({ selectedMonth: month });
    this.updateTransactions(this.state.selectedYear, month);
  }

  updateTransactions = (year, month) => {
    const firstDay = new Date(year, month, 1);
    const lastDay = new Date(year, parseInt(month, 10) + 1, 0);

    getTransactions(this.props.token, firstDay, lastDay, null, null).then(({ result: transactions }) => {
      this.setState({ transactions });
    });
  }

  render() {
    const { selectedMonth, selectedYear, transactions } = this.state;

    return (
      <div>
        <label htmlFor="month">Select Month</label><br />
        <select id="month" onChange={this.monthChanged} value={selectedMonth}>
          {this.monthOptions.map(function(option, index) {
            return <option key={index} value={option.value}>{option.text}</option>
          })}
        </select>

        <br /><br />

        <label htmlFor="year">Select Year</label><br />
        <select id="year" onChange={this.yearChanged} value={selectedYear}>
          {this.yearOptions.map((option, index) => {
            return <option key={index} value={option.value}>{option.text}</option>
          })}
        </select>

        <h2>All Transaction</h2>
        <table>
          <thead>
            <tr>
              <th>Date</th>
              <th>Soruce</th>
              <th>Target</th>
              <th>Amount [CHF]</th>
              <th>Balance [CHF]</th>
            </tr>
          </thead>

          <tbody>
            {transactions.map(function(t, index) {
              return <tr key={index}><td>{t.date}</td><td>{t.from}</td><td>{t.target}</td><td>{t.amount}</td><td>{t.total}</td></tr>
            })}

            <tr hidden={transactions.length > 0}><td colSpan="5">No transanctions available</td></tr>
          </tbody>
        </table>
      </div>
    )
  }
}

export default AllTransactions
