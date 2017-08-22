// @flow

import React from 'react';

import { getTransactions, Transaction } from '../api';

export type Props = {
  token: string,
}

export class LatestTransactions extends React.Component {
  state: {
    transactions: Transaction[],
  }

  state = {
    transactions: [],
  }

  componentDidMount() {
    this.props.emitter.on('paymentCompleted', this.refresh);
    this.refresh();
  }

  componentWillUnmount() {
    this.props.emitter.off('paymentCompleted', this.refresh);
  }

  refresh = () => {
    console.log('Refresh transactions');
    getTransactions(this.props.token).then(({ result: transactions }) => {
      this.setState({ transactions });
    })
  }

  render() {
    const { transactions } = this.state;

    return (
      <div>
        <h2>Latest Transactions</h2>
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
          </tbody>
        </table>
      </div>
    )
  }
}

export default LatestTransactions;
