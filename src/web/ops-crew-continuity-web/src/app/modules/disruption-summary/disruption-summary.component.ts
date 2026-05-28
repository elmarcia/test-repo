import { DatePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';

import { FlightDto } from '../../core/api/operational-api.models';
import { OperationalApiService } from '../../core/api/operational-api.service';

@Component({
  selector: 'ops-disruption-summary',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './disruption-summary.component.html',
  styleUrl: './disruption-summary.component.css'
})
export class DisruptionSummaryComponent implements OnInit {
  private readonly operationalApi = inject(OperationalApiService);

  flights: FlightDto[] = [];
  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    this.operationalApi.getDisruptedFlights().subscribe({
      next: (flights) => {
        this.flights = flights;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Disruption summary is unavailable.';
        this.isLoading = false;
      }
    });
  }
}
