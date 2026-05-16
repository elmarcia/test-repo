import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'ops-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  private readonly http = inject(HttpClient);

  readonly apiDisplayUrl = 'http://localhost:8080/api/health';
  readonly apiProxyUrl = '/api/health';

  healthJson = '';
  healthError = '';
  isLoadingHealth = false;

  checkHealth(): void {
    this.isLoadingHealth = true;
    this.healthError = '';
    this.healthJson = '';

    this.http.get<unknown>(this.apiProxyUrl).subscribe({
      next: (response) => {
        this.healthJson = JSON.stringify(response, null, 2);
        this.isLoadingHealth = false;
      },
      error: (error: unknown) => {
        this.healthError = error instanceof Error
          ? error.message
          : 'Unable to reach the Core API health endpoint.';
        this.isLoadingHealth = false;
      }
    });
  }
}
