import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';

import { FlightDto } from '../../core/api/operational-api.models';
import { OperationalApiService } from '../../core/api/operational-api.service';

@Component({
  selector: 'ops-documents-hub',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './documents-hub.component.html',
  styleUrl: './documents-hub.component.css'
})
export class DocumentsHubComponent implements OnInit {
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
        this.errorMessage = 'Document module data is unavailable.';
        this.isLoading = false;
      }
    });
  }
}
