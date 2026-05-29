import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { environment } from '../../../environments/environment';

interface RuntimeAppConfig {
  apiBaseUrl?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {
  private readonly http = inject(HttpClient);
  private config: RuntimeAppConfig = {
    apiBaseUrl: environment.apiBaseUrl
  };

  get apiBaseUrl(): string {
    return this.normalizeApiBaseUrl(this.config.apiBaseUrl ?? environment.apiBaseUrl);
  }

  async load(): Promise<void> {
    try {
      const config = await firstValueFrom(
        this.http.get<RuntimeAppConfig>('/assets/app-config.json')
      );

      this.config = {
        apiBaseUrl: config.apiBaseUrl ?? environment.apiBaseUrl
      };
    } catch {
      this.config = {
        apiBaseUrl: environment.apiBaseUrl
      };
    }
  }

  private normalizeApiBaseUrl(apiBaseUrl: string): string {
    const trimmedApiBaseUrl = apiBaseUrl.trim();

    if (trimmedApiBaseUrl === '/') {
      return '';
    }

    return trimmedApiBaseUrl.replace(/\/+$/, '');
  }
}
