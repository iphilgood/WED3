// @flow

import React from 'react'

import type { User } from '../api'

export type Props = {
  token: string,
  user: User,
}

class AllTransactions extends React.Component {

  props: Props

  render() {
    return (
      <div>All Transactions</div>
    )
  }
}

export default AllTransactions
