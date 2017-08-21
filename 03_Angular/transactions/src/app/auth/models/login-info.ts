export class LoginInfo {
  constructor(public login: string,
              public password: string) {
  }

  public static fromDto(data: any): LoginInfo {
    return new LoginInfo(data.login, data.password);
  }

  toDto(): any {
    return {
      login: this.login,
      password: this.password
    };
  }
}
