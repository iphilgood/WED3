import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class DataService {

  constructor(public http: Http) {
  }

  getPosts() {
    return this.http.get('http://jsonplaceholder.typicode.com/posts').map(res => res.json());
  }
}
