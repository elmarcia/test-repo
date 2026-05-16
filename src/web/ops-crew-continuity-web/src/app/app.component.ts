import { DatePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { forkJoin } from 'rxjs';

import {
  CrewMemberDto,
  FlightDto,
  JournalEntryDto,
  PairingDto,
  StandbyAssignmentDto
} from './core/api/operational-api.models';
import { OperationalApiService } from './core/api/operational-api.service';

@Component({
  selector: 'ops-root',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  private readonly operationalApi = inject(OperationalApiService);

  flights: FlightDto[] = [];
  disruptedFlights: FlightDto[] = [];
  crewMembers: CrewMemberDto[] = [];
  standbyAssignments: StandbyAssignmentDto[] = [];
  pairings: PairingDto[] = [];
  journal: JournalEntryDto[] = [];

  isLoading = true;
  isActionInProgress = false;
  errorMessage = '';
  successMessage = '';

  get activeFlights(): FlightDto[] {
    return this.flights.filter((flight) => flight.status === 'ACTIVE');
  }

  get legalityIssues(): CrewMemberDto[] {
    return this.crewMembers.filter((crewMember) => crewMember.status === 'LEGALITY_REVIEW');
  }

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.isLoading = true;
    this.errorMessage = '';

    forkJoin({
      flights: this.operationalApi.getFlights(),
      disruptedFlights: this.operationalApi.getDisruptedFlights(),
      crewMembers: this.operationalApi.getCrewMembers(),
      standbyAssignments: this.operationalApi.getStandbyAssignments(),
      pairings: this.operationalApi.getPairings(),
      journal: this.operationalApi.getJournal()
    }).subscribe({
      next: (dashboard) => {
        this.flights = dashboard.flights;
        this.disruptedFlights = dashboard.disruptedFlights;
        this.crewMembers = dashboard.crewMembers;
        this.standbyAssignments = dashboard.standbyAssignments;
        this.pairings = dashboard.pairings;
        this.journal = dashboard.journal;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Operational data is unavailable. Confirm the Core API and PostgreSQL containers are running.';
        this.isLoading = false;
      }
    });
  }

  delayFlight(flight: FlightDto): void {
    const minutesInput = window.prompt(`Delay ${flight.flightNumber} by how many minutes?`, '30');
    if (minutesInput === null) {
      return;
    }

    const minutes = Number(minutesInput);
    if (!Number.isInteger(minutes) || minutes <= 0) {
      this.errorMessage = 'Delay minutes must be a positive whole number.';
      return;
    }

    const reason = window.prompt('Delay reason', flight.disruptionReason || 'Operational continuity demo delay');
    if (!reason?.trim()) {
      return;
    }

    this.runAction(
      this.operationalApi.delayFlight(flight.flightId, { minutes, reason: reason.trim() }),
      `${flight.flightNumber} delayed by ${minutes} minutes.`
    );
  }

  cancelFlight(flight: FlightDto): void {
    const reason = window.prompt('Cancellation reason', flight.disruptionReason || 'Operational continuity demo cancellation');
    if (!reason?.trim()) {
      return;
    }

    this.runAction(
      this.operationalApi.cancelFlight(flight.flightId, { reason: reason.trim() }),
      `${flight.flightNumber} cancelled.`
    );
  }

  assignStandby(assignment: StandbyAssignmentDto): void {
    const flightId = window.prompt('Assign standby crew to flight ID', this.disruptedFlights[0]?.flightId || '');
    if (!flightId?.trim()) {
      return;
    }

    const notes = window.prompt('Assignment notes', 'Assigned from continuity dashboard') || '';

    this.runAction(
      this.operationalApi.assignStandby(assignment.standbyAssignmentId, {
        flightId: flightId.trim(),
        notes: notes.trim()
      }),
      `${assignment.fullName} assigned to ${flightId.trim()}.`
    );
  }

  private runAction(action$: ReturnType<OperationalApiService['delayFlight']>, successMessage: string): void {
    this.isActionInProgress = true;
    this.errorMessage = '';
    this.successMessage = '';

    action$.subscribe({
      next: () => {
        this.successMessage = successMessage;
        this.isActionInProgress = false;
        this.loadDashboard();
      },
      error: () => {
        this.errorMessage = 'The operational action could not be completed.';
        this.isActionInProgress = false;
      }
    });
  }
}
