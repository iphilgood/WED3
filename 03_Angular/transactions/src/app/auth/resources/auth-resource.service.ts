import {Injectable} from '@angular/core';
import {Response, Http} from '@angular/http';

import {Observable} from 'rxjs';

import {LoginInfo, Account, RegistrationInfo, Credential} from '../models';
import {ResourceBase} from './resource-base';

@Injectable()
export class AuthResourceService extends ResourceBase {
  constructor(http: Http) {
    super(http);
  }

  public register(model: RegistrationInfo): Observable<Account> {
    return this.post('/auth/register', model.toDto())
      .map((response: Response) => {
        const result = response.json();
        if (result) {
          return Account.fromDto(result);
        }
        return null;
      })
      .catch((error: any) => {
        return Observable.of<Account>(null);
      });
  }

  public login(model: LoginInfo): Observable<Credential> {
    return this.post('/auth/login', model.toDto())
      .map((response: Response) => {
        const result = response.json();
        if (result) {
          return Credential.fromDto(result);
        }
        return null;
      })
      .catch((error: any) => {
        return Observable.of<Credential>(null);
      });
  }
}
