import {Component, OnInit} from '@angular/core';
import { ActivatedRoute} from '@angular/router';

import {NavigationService} from '../../core/services/navigation.service';
import {AuthService} from '../services';
import {Account} from '../models';

@Component({
  selector: 'wed-logout',
  templateUrl: 'logout.component.html',
  styleUrls: ['logout.component.scss']
})
export class LogoutComponent implements OnInit {

  public user: Account;

  constructor(private autSvc: AuthService, private navigationSvc: NavigationService, route: ActivatedRoute) {
  }

  ngOnInit() {
    this.user = this.autSvc.authenticatedUser;
    this.autSvc.authenticatedUserChange.subscribe(
      (credentials) => {
        if (!credentials) {
          this.navigationSvc.goToHome();
        }
      });
  }

  public doLogout() {
    this.autSvc.logout();
  }
}
