import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard.component';
import { AuthGuard } from '../auth/services/';
import { ListComponent } from './components/list/list.component';
import { PanelComponent } from './components/panel/panel.component';

const appRoutes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    canLoad: [AuthGuard],
    children: [
      { path: 'transactions', component: ListComponent },
      { path: '', component: PanelComponent }
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(appRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class DashbaordRoutingModule {}
