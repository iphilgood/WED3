import { NgModule, ModuleWithProviders } from '@angular/core';
import { RequestOptions } from '@angular/http';

import { SharedModule } from '../shared/shared.module';
import { DashbaordRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './components/dashboard.component';
import { LatestComponent } from './components/latest/latest.component';
import { PaymentComponent } from './components/payment/payment.component';
import { TransactionResourceService } from './resources';
import { TransactionService } from './services';
import { AuthRequestOptions, SecurityTokenStore } from '../auth';
import { AuthModule } from '../auth/auth.module';
import { ListComponent } from './components/list/list.component';
import { PanelComponent } from './components/panel/panel.component';

@NgModule({
  declarations: [
    DashboardComponent,
    LatestComponent,
    PaymentComponent,
    ListComponent,
    PanelComponent
  ],
  imports: [
    SharedModule,
    DashbaordRoutingModule,
    AuthModule
  ],
  exports: [
    DashboardComponent
  ],
  providers: [
    TransactionResourceService,
    TransactionService
  ]
})
export class DashboardModule { }
