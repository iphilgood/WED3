import {NgModule, ModuleWithProviders, SkipSelf, Optional, forwardRef} from '@angular/core';

import {SharedModule} from '../shared/shared.module';

import {NavigationService} from './services';

@NgModule({
  declarations: [ ],
  imports: [ SharedModule ],
  exports: [ /* nothing to add here */ ],
  providers: [ /* nothing to add here */ ]
})
export class CoreModule {
  static forRoot(config?: {}): ModuleWithProviders {

    return {
      ngModule: CoreModule,
      providers: [
        // DI Providers (Services, Tokens, Factories...) to be used globally and instantiated only once
        NavigationService
      ]
    };
  }

  // Only the root AppModule should import the CoreModule. Bad things happen if a lazy loaded module imports it.
  constructor (@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error('CoreModule is already loaded. Import it in the AppModule only');
    }
  }
}
