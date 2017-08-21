import {Injectable, EventEmitter} from '@angular/core';

import {AuthResourceService} from '../resources';
import {LoginInfo, RegistrationInfo, Credential, Account} from '../models';

import {SecurityTokenStore} from './credential-management';

@Injectable()
export class AuthService {

  public authenticatedUserChange: EventEmitter<Account> = new EventEmitter<Account>();

  public get authenticatedUser(): Account {
    return this.authUser;
  }

  private authUser: Account = null;

  constructor(private resource: AuthResourceService, private tokenStore: SecurityTokenStore) {
    if (tokenStore.storedValue) {
      this.authUser = tokenStore.storedValue.owner;
    }
  }

  public get hasCredentials(): boolean {
    return !this.isBlank(this.authenticatedUser);
  }

  public register(registerModel: RegistrationInfo): void {
    this.resource.register(registerModel).subscribe(
      (data: Account) => {
        this.login(registerModel);
      });
  }

  public login(loginModel: LoginInfo): void {
    this.resource.login(loginModel).subscribe(
      (data: Credential) => {
        this.tokenStore.storedValue = data;
        this.authUser = !this.isBlank(data) ? data.owner : null;
        this.authenticatedUserChange.emit(this.authenticatedUser);
      } );
  }

  public logout(): void {
    this.tokenStore.storedValue = null;
    this.authUser = null;
    this.authenticatedUserChange.emit(null);
  }

  private isBlank(obj: any): boolean {
    return obj == null || obj === '';
  }
}
