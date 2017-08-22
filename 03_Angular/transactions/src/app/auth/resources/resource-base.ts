import { Injectable } from '@angular/core';
import { Response, Http, Headers, RequestOptionsArgs } from '@angular/http';

import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

export abstract class ResourceBase {
  static JSON_HEADERS: RequestOptionsArgs = { headers: new Headers({ 'content-type': 'application/json' }) };

  constructor(private http: Http) {
  }

  protected get<T>(path: string): Observable<Response> {
    return this.http.get(environment.serverBaseUrl + path);
  }

  protected post<T>(path: string, dto: any): Observable<Response> {
    return this.http.post(
      environment.serverBaseUrl + path,
      JSON.stringify(dto),
      ResourceBase.JSON_HEADERS);
  }
}
