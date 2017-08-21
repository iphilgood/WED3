import { Account } from '../../auth/models';

export class BankAccount {
  constructor(public ownerId: string,
              public accountNr: string,
              public amount: number,
              public owner: Account) {
  }

  public static fromDto(data: any): BankAccount {
    return new BankAccount(data.ownerId, data.accountNr, data.amount, Account.fromDto(data.owner));
  }

  toDto(): any {
    return {
      ownerId: this.ownerId,
      accountNr: this.accountNr,
      amount: this.amount,
      owner: this.owner
    };
  }
}
