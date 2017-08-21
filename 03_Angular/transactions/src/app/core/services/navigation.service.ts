import {Injectable} from '@angular/core';
import {Router} from '@angular/router';

@Injectable()
export class NavigationService {

  public goToUrl(url: string): void {
    this.router.navigateByUrl(url);
  }

  public goToHome(): void {
    this.goToUrl('/'); // TODO: adjust routing according this URL
  }

  public goToDashboard(): void {
    this.goToUrl('/dashboard'); // TODO: adjust routing according this URL
  }

  constructor(private router: Router) {
  }
}
