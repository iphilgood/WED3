import {ActivatedRoute, Params} from '@angular/router';
import {Component, OnInit} from '@angular/core';
import {NgForm} from '@angular/forms';

import {NavigationService} from '../../core/services/navigation.service';

import {AuthService} from '../services';
import {LoginInfo} from '../models';

@Component({
  selector: 'wed-login',
  templateUrl: 'login.component.html',
  styleUrls: ['login.component.scss']
})
export class LoginComponent implements OnInit {
  private backUrl;

  public login: string;
  public password: string;
  public isProcessing = false;

  constructor(private autSvc: AuthService, private navigationSvc: NavigationService, route: ActivatedRoute) {
    route.params.subscribe((p: Params) => this.backUrl = p['backUrl']);
  }

  ngOnInit() {
    this.backUrl = '';
    this.autSvc.authenticatedUserChange.subscribe(
      (credentials) => {
        this.isProcessing = false;
        if (credentials) {
          if (this.backUrl) {
            this.navigationSvc.goToUrl(this.backUrl);
          } else {
            this.navigationSvc.goToDashboard();
          }
        }
      });
  }

  public doLogin(f: NgForm): boolean {
    if (f.valid) {
      this.isProcessing = true;
      this.autSvc.login(new LoginInfo(f.value.login, f.value.password));
    }
    return false;
  }
}
