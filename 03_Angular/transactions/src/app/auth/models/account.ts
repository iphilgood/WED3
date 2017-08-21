export class Account {
  constructor(public login: string,
              public firstname: string,
              public lastname: string,
              public accountNr: string) {
  }

  public static fromDto(data: any): Account {
    return new Account(data.login, data.firstname, data.lastname, data.accountNr);
  }

  toDto(): any {
    return {
      login: this.login,
      firstname: this.firstname,
      lastname: this.lastname,
      accountNr: this.accountNr
    };
  }
}
