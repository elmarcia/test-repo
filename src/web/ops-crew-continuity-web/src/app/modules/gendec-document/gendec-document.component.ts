import { DatePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';

import { FlightDto } from '../../core/api/operational-api.models';
import { OperationalApiService } from '../../core/api/operational-api.service';

@Component({
  selector: 'ops-gendec-document',
  standalone: true,
  imports: [DatePipe, RouterLink],
  templateUrl: './gendec-document.component.html',
  styleUrl: './gendec-document.component.css'
})
export class GendecDocumentComponent implements OnInit {
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
        this.errorMessage = 'GENDEC flight list is unavailable.';
        this.isLoading = false;
      }
    });
  }
}
