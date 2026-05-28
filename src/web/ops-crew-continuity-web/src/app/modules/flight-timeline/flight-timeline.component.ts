import { DatePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { FlightDto } from '../../core/api/operational-api.models';
import { OperationalApiService } from '../../core/api/operational-api.service';
import { TimelineViewModel } from './flight-timeline.models';
import { FlightTimelineService } from './flight-timeline.service';

@Component({
  selector: 'ops-flight-timeline',
  standalone: true,
  imports: [DatePipe, FormsModule, RouterLink],
  templateUrl: './flight-timeline.component.html',
  styleUrl: './flight-timeline.component.css'
})
export class FlightTimelineComponent implements OnInit {
  private readonly operationalApi = inject(OperationalApiService);
  private readonly timelineService = inject(FlightTimelineService);

  flights: FlightDto[] = [];
  viewModel: TimelineViewModel = this.timelineService.buildViewModel([]);
  selectedFlight: FlightDto | null = null;
  aircraftFilter = 'ALL';
  statusFilter = 'ALL';
  isLoading = true;
  isActionInProgress = false;
  errorMessage = '';
  successMessage = '';

  get timelineWidth(): number {
    return Math.max(1380, this.viewModel.totalMinutes * 4);
  }

  get aircraftOptions(): string[] {
    return Array.from(new Set(this.flights.map((flight) => flight.aircraftRegistration || `${flight.originIata}-${flight.destinationIata}`))).sort();
  }

  get filteredFlights(): FlightDto[] {
    return this.flights.filter((flight) => {
      const group = flight.aircraftRegistration || `${flight.originIata}-${flight.destinationIata}`;
      const aircraftMatches = this.aircraftFilter === 'ALL' || group === this.aircraftFilter;
      const statusMatches = this.statusFilter === 'ALL' || flight.status === this.statusFilter;
      return aircraftMatches && statusMatches;
    });
  }

  get delayedCount(): number {
    return this.flights.filter((flight) => flight.status === 'DELAYED').length;
  }

  get disruptedCount(): number {
    return this.flights.filter((flight) => flight.status === 'DISRUPTED').length;
  }

  get cancelledCount(): number {
    return this.flights.filter((flight) => flight.status === 'CANCELLED').length;
  }

  ngOnInit(): void {
    this.loadFlights();
  }

  loadFlights(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.operationalApi.getFlights().subscribe({
      next: (flights) => {
        this.flights = flights;
        this.refreshTimeline();
        this.selectedFlight = this.selectedFlight
          ? flights.find((flight) => flight.flightId === this.selectedFlight?.flightId) || null
          : this.filteredFlights[0] || null;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Flight timeline data is unavailable. Confirm the Core API and PostgreSQL containers are running.';
        this.isLoading = false;
      }
    });
  }

  selectFlight(flight: FlightDto): void {
    this.selectedFlight = flight;
  }

  applyFilters(): void {
    this.refreshTimeline();
    if (this.selectedFlight && !this.filteredFlights.some((flight) => flight.flightId === this.selectedFlight?.flightId)) {
      this.selectedFlight = this.filteredFlights[0] || null;
    }
  }

  clearFilters(): void {
    this.aircraftFilter = 'ALL';
    this.statusFilter = 'ALL';
    this.applyFilters();
  }

  delayFlight(flight: FlightDto | null): void {
    if (!flight) {
      return;
    }

    const minutesInput = window.prompt(`Delay ${flight.flightNumber} by how many minutes?`, '30');
    if (minutesInput === null) {
      return;
    }

    const minutes = Number(minutesInput);
    if (!Number.isInteger(minutes) || minutes <= 0) {
      this.errorMessage = 'Delay minutes must be a positive whole number.';
      return;
    }

    const reason = window.prompt('Delay reason', flight.disruptionReason || 'Timeline operational adjustment');
    if (!reason?.trim()) {
      return;
    }

    this.runAction(
      this.operationalApi.delayFlight(flight.flightId, { minutes, reason: reason.trim() }),
      `${flight.flightNumber} delayed by ${minutes} minutes.`
    );
  }

  cancelFlight(flight: FlightDto | null): void {
    if (!flight) {
      return;
    }

    const reason = window.prompt('Cancellation reason', flight.disruptionReason || 'Timeline operational cancellation');
    if (!reason?.trim()) {
      return;
    }

    this.runAction(
      this.operationalApi.cancelFlight(flight.flightId, { reason: reason.trim() }),
      `${flight.flightNumber} cancelled.`
    );
  }

  hasDelay(flight: FlightDto): boolean {
    return this.timelineService.getDelayMinutes(flight) !== 0;
  }

  getDelayMinutes(flight: FlightDto): number {
    return this.timelineService.getDelayMinutes(flight);
  }

  getEffectiveDeparture(flight: FlightDto): Date {
    return this.timelineService.getEffectiveDeparture(flight);
  }

  getEffectiveArrival(flight: FlightDto): Date {
    return this.timelineService.getEffectiveArrival(flight);
  }

  private runAction(action$: ReturnType<OperationalApiService['delayFlight']>, successMessage: string): void {
    this.isActionInProgress = true;
    this.errorMessage = '';
    this.successMessage = '';

    action$.subscribe({
      next: () => {
        this.successMessage = successMessage;
        this.isActionInProgress = false;
        this.loadFlights();
      },
      error: () => {
        this.errorMessage = 'The operational action could not be completed.';
        this.isActionInProgress = false;
      }
    });
  }

  private refreshTimeline(): void {
    this.viewModel = this.timelineService.buildViewModel(this.filteredFlights);
  }
}
