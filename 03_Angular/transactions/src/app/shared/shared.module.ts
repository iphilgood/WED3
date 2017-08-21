import {NgModule, ModuleWithProviders} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {CommonModule} from '@angular/common';

@NgModule({
  declarations: [
    // TODO: Add declarations here, if additional components are placed within the shared module
  ],
  imports: [
    CommonModule
  ],
  exports: [
    CommonModule, FormsModule
    // TODO: Add declarations here, if additional components are placed within the shared module
  ],
  providers: [ ]
})
export class SharedModule {
  // forRoot() isn't needed here...
}
