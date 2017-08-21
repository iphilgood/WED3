import {Injectable} from '@angular/core';

/**
 * TODO: Add localStorage logic here...
 */
@Injectable()
export class SecurityTokenStore {
  private token: SecurityToken;

  constructor() {
  }

  public get storedValue(): SecurityToken {
    return this.token;
  }

  public set storedValue(value: SecurityToken) {
    this.token = value;
  }
}

export interface SecurityToken {
  token: string;
  owner: any;
}
