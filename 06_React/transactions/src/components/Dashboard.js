// @flow

import React from 'react';

import type { User } from '../api';
import NewPayment from './NewPayment';
import LatestTransactions from './LatestTransactions';
import mitt from './mitt';

/*
  Use the api functions to call the API server. For example, the transactions
  can be retrieved and stored in the state as follows:

  getTransactions(this.props.token)
    .then(({result: transactions}) =>
      this.setState({transactions})
    )

  import { getAccountDetails, getAccount, transfer, getTransactions } from '../api'
*/

export type Props = {
  token: string,
  user: ?User,
}

class Dashboard extends React.Component {

  props: Props;
  emitter = mitt();

  render() {

    return (
      <div>
        <h1>Account: {this.props.user.accountNr}</h1>
        <NewPayment emitter={this.emitter} {...this.props} />
        <LatestTransactions emitter={this.emitter} {...this.props} />
      </div>
    )
  }
}

export default Dashboard
