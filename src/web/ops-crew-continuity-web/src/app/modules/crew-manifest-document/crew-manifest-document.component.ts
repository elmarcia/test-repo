import { DatePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';

import { FlightDto } from '../../core/api/operational-api.models';
import { OperationalApiService } from '../../core/api/operational-api.service';

@Component({
  selector: 'ops-crew-manifest-document',
  standalone: true,
  imports: [DatePipe, RouterLink],
  templateUrl: './crew-manifest-document.component.html',
  styleUrl: './crew-manifest-document.component.css'
})
export class CrewManifestDocumentComponent implements OnInit {
  private readonly operationalApi = inject(OperationalApiService);

  flights: FlightDto[] = [];
  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    this.operationalApi.getFlights().subscribe({
      next: (flights) => {
        this.flights = flights;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Crew Manifest flight list is unavailable.';
        this.isLoading = false;
      }
    });
  }
}
