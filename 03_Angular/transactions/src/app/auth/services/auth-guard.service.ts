import { Injectable } from '@angular/core';
import { CanLoad, ActivatedRouteSnapshot, RouterStateSnapshot, Route, Router } from '@angular/router';

import { AuthService } from './auth.service';
import { NavigationService } from '../../core/index';

@Injectable()
export class AuthGuard implements CanLoad {
  constructor(private authService: AuthService, private navService: NavigationService) {}

  canLoad(route: Route): boolean {
    const url = `/${route.path}`;
    return this.checkLogin(url);
  }

  checkLogin(url: string): boolean {
    if (this.authService.hasCredentials) { return true; }
    this.navService.goToHome();
    return false;
  }
}
