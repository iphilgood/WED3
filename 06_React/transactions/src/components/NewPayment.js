// @flow

import React from 'react';
import { transfer, getAccount, getAccountDetails, AccountNr } from '../api';
import type { User } from '../api';

export type Props = {
  token: string,
  user: ?User,
}

class NewPayment extends React.Component {
  props: Props;
  state: {
    srouce: AccountNr,
    balance?: Number,
    success: Boolean,
    error?: Error,
    target: AccountNr,
    targetValid: Boolean,
    targetMessage: String,
    amount: Number,
  };

  state = {
    source: this.props.user.accountNr,
    balance: 0,
    success: false,
    error: undefined,
    target: '',
    targetValid: false,
    targetMessage: '',
    amount: 0,
  };

  componentDidMount() {
    this.updateSource();
    this.validateTarget('');
  }

  updateSource = () => {
    getAccountDetails(this.props.token).then((result) => {
      this.setState({ balance: result.amount });
    });
  }

  targetChanged = (event: Event) => {
    if (event.target instanceof HTMLInputElement) {
      this.validateTarget(event.target.value);
    }
  }

  amountChanged = (event: Event) => {
    if (event.target instanceof HTMLInputElement) {
      this.setState({ amount: event.target.value });
    }
  }

  isAmountValid = () => {
    return this.state.amount >= 0.05 &&
           this.state.amount <= this.state.balance &&
           Math.round(Math.round(this.state.amount * 100) % 5) === 0;
  }

  isTargetValid = () => {
    return this.state.targetValid;
  }

  isFormValid = () => {
    return this.isAmountValid() && this.isTargetValid();
  }

  validateTarget = (target: String) => {
    if (target.length <= 2) {
      this.setState({
        target,
        targetValid: false,
        targetMessage: "Please specify the target account number."
      })
      return;
    }

    getAccount(target, this.props.token).then(result => {
      this.setState({
        target,
        targetValid: true,
        targetMessage: `${result.owner.firstname} ${result.owner.lastname}`
      })
    }).catch((error: Error) => {
      this.setState({
        target,
        targetValid: false,
        targetMessage: "Unknown account number specified."
      });
    });
  }

  handleSubmit = (event: Event) => {
    event.preventDefault();
    const { target, amount } = this.state;
    transfer(target, amount, this.props.token).then((result) => {
      this.setState({ balance: result.total, success: true});
      this.props.emitter.emit('paymentCompleted');
    }).catch((error: Error) => {
      this.setState({ error });
    });
  }

  startOver = (event: Event) => {
    this.setState({ success: false, amount: 0, error: null });
    this.validateTarget('');
  }

  render() {
    const { source, balance, target, amount, success, error } = this.state;
    const sourceText = `${source} ${balance === undefined ? "" : `[CHF ${balance}]`}`;

    return (
      <div>
        <h2>New Payment</h2>
        <form hidden={success} onSubmit={this.handleSubmit}>
          <div>
            <label htmlFor="from">Source</label><br />
            <input type="text" id="from" value={sourceText} required disabled />
          </div>

          <div>
            <label htmlFor="to">Target</label><br />
            <input type="text" id="to" value={target} onChange={this.targetChanged} required /><br />
            <small>{this.state.targetMessage}</small>
          </div>

          <div>
            <label htmlFor="amount">Amount [CHF]</label><br />
            <input type="number" id="amount" value={amount} onChange={this.amountChanged} required /><br />
            <small hidden={this.isAmountValid()}>Please specify the amount.</small>
          </div>

          { error && <p style={{fontColor: 'red'}}>Ooops!</p>}

          <input type="submit" value="Pay" />
        </form>

        <div hidden={!success}>
          <p>Transaction to {target} succeeded!</p>
          <p>New balance: {balance}</p>
          <button onClick={this.startOver}>Start over</button>
        </div>
      </div>
    )
  }
}

export default NewPayment;
