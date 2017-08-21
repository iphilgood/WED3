import { NgModule, ModuleWithProviders } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RequestOptions } from '@angular/http';

import { AuthService, SecurityTokenStore, AuthGuard, AccountService } from './services';
import { AuthResourceService, AuthRequestOptions, AccountResourceService } from './resources';

import { LoginComponent, LogoutComponent, RegisterComponent } from './components';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    // TODO: Add declarations here, if additional components are placed within the Auth module
    LoginComponent, LogoutComponent, RegisterComponent
  ],
  imports: [
    SharedModule
  ],
  exports: [
    // TODO: Add declarations here, if additional components are placed within the Auth module
    LoginComponent, LogoutComponent, RegisterComponent
  ],
  providers: [ ]
})
export class AuthModule {
  static forRoot(config?: {}): ModuleWithProviders {
    return {
      ngModule: AuthModule,
      providers: [
        AccountService,
        AccountResourceService,
        AuthResourceService,
        AuthService,
        AuthGuard,
        SecurityTokenStore,
        {
          provide: RequestOptions,
          useFactory: AuthRequestOptions.createFromTokenStore,
          deps: [SecurityTokenStore]
        }]
    };
  }
}
