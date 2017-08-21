import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { NavigationService } from '../../core/services/navigation.service';

import { AuthService } from '../services';
import { RegistrationInfo } from '../models';

@Component({
  selector: 'wed-register',
  templateUrl: 'register.component.html',
  styleUrls: ['register.component.scss']
})
export class RegisterComponent implements OnInit {

  public login: string;
  public password: string;
  public firstname: string;
  public lastname: string;

  public isProcessing = false;

  constructor(private autSvc: AuthService, private navigationSvc: NavigationService) {
  }

  ngOnInit() {
    this.autSvc.authenticatedUserChange.subscribe(
      (credentials) => {
        this.isProcessing = false;
        if (credentials) {
          this.navigationSvc.goToDashboard();
        }
      });
  }

  public doRegister(f: NgForm): boolean {
    if (f.valid) {
      this.isProcessing = true;
      this.autSvc.register(new RegistrationInfo(
        f.value.login,
        f.value.password,
        f.value.firstname,
        f.value.lastname));
    }
    return false;
  }
}
