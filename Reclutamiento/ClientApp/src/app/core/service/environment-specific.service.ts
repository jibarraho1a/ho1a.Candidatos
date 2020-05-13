import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EnvSpecific } from '../model/env-specific';

@Injectable()
export class EnvironmentService {
  private appConfig: EnvSpecific;

  constructor(private http: HttpClient) {
  }

  loadAppConfig() {
    //return this.http.get('/miRHCandidato/assets/environment.json')
    return this.http.get('./assets/environment.json')
    .toPromise()
      .then(data => {
        this.appConfig = data as EnvSpecific;
      });
  }

  getConfig(): EnvSpecific {
    return this.appConfig;
  }
}
