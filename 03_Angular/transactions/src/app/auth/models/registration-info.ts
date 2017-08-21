import {LoginInfo} from './login-info';

export class RegistrationInfo extends LoginInfo{
  constructor(login: string,
              password: string,
              public firstname: string,
              public lastname: string) {
    super(login, password);
  }

  public static fromDto(data: any): RegistrationInfo {
    return new RegistrationInfo(data.login, data.firstname, data.lastname, data.password);
  }

  toDto(): any {
    const dto = super.toDto();
    dto.firstname = this.firstname;
    dto.lastname = this.lastname;
    return dto;
  }
}
