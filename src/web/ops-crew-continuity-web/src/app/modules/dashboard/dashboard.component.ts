import { DatePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';

import {
  CrewMemberDto,
  FlightDto,
  JournalEntryDto,
  PairingDto,
  StandbyAssignmentDto
} from '../../core/api/operational-api.models';
import { OperationalApiService } from '../../core/api/operational-api.service';

@Component({
  selector: 'ops-dashboard',
  standalone: true,
  imports: [DatePipe, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
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

  get crewMovements(): StandbyAssignmentDto[] {
    return this.standbyAssignments.filter((assignment) => assignment.readinessStatus === 'ASSIGNED');
  }

  get recentCrewMovements(): StandbyAssignmentDto[] {
    return this.crewMovements.slice(0, 4);
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
        this.errorMessage = 'Operational data is unavailable. Confirm API connectivity and database access.';
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
    const candidateFlights = this.disruptedFlights.length ? this.disruptedFlights : this.flights;
    const defaultFlight = candidateFlights[0];
    const options = candidateFlights
      .slice(0, 12)
      .map((flight, index) => `${index + 1}. ${this.getFlightLabel(flight)}`)
      .join('\n');
    const selection = window.prompt(
      `Assign ${assignment.fullName} (${assignment.employeeNumber}) to which flight?\n\n${options}\n\nEnter number or flight number.`,
      defaultFlight?.flightNumber || ''
    );

    if (!selection?.trim()) {
      return;
    }

    const selectedFlight = this.findFlightFromSelection(selection, candidateFlights);
    if (!selectedFlight) {
      this.errorMessage = 'Select a valid flight number from the current operations board.';
      return;
    }

    const notes = window.prompt('Assignment notes', 'Assigned from continuity dashboard') || '';
    const targetLabel = this.getFlightLabel(selectedFlight);

    this.runAction(
      this.operationalApi.assignStandby(assignment.standbyAssignmentId, {
        flightId: selectedFlight.flightId,
        notes: notes.trim()
      }),
      `${assignment.fullName} (${assignment.employeeNumber}) assigned to ${targetLabel}.`
    );
  }

  getFlightLabel(flight: FlightDto): string {
    const aircraft = flight.aircraftRegistration || 'Aircraft TBD';
    const departure = new Intl.DateTimeFormat(undefined, {
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    }).format(new Date(flight.scheduledDeparture));

    return `${flight.flightNumber} ${flight.originIata} -> ${flight.destinationIata} ${aircraft} STD ${departure}`;
  }

  getAssignmentTargetLabel(assignment: StandbyAssignmentDto): string {
    if (
      assignment.assignedFlightNumber &&
      assignment.assignedOriginIata &&
      assignment.assignedDestinationIata
    ) {
      const aircraft = assignment.assignedAircraftRegistration || 'Aircraft TBD';
      const departure = assignment.assignedScheduledDeparture
        ? new Intl.DateTimeFormat(undefined, {
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
          }).format(new Date(assignment.assignedScheduledDeparture))
        : 'STD TBD';

      return `${assignment.assignedFlightNumber} ${assignment.assignedOriginIata} -> ${assignment.assignedDestinationIata} ${aircraft} ${departure}`;
    }

    return 'No target flight assigned';
  }

  getJournalContext(entry: JournalEntryDto): string {
    const flight = entry.flightId ? this.flights.find((item) => item.flightId === entry.flightId) : null;
    const crew = entry.crewMemberId ? this.crewMembers.find((item) => item.crewMemberId === entry.crewMemberId) : null;

    if (flight && crew) {
      return `${flight.flightNumber} / ${crew.fullName} (${crew.employeeNumber})`;
    }

    if (flight) {
      return this.getFlightLabel(flight);
    }

    if (crew) {
      return `${crew.fullName} (${crew.employeeNumber})`;
    }

    return 'Network';
  }

  hasCrewMovement(flight: FlightDto): boolean {
    return this.standbyAssignments.some((assignment) => assignment.assignedFlightId === flight.flightId);
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

  private findFlightFromSelection(selection: string, candidates: FlightDto[]): FlightDto | null {
    const trimmed = selection.trim();
    const selectedIndex = Number(trimmed);

    if (Number.isInteger(selectedIndex) && selectedIndex > 0 && selectedIndex <= candidates.length) {
      return candidates[selectedIndex - 1];
    }

    return this.flights.find((flight) => flight.flightNumber.toLowerCase() === trimmed.toLowerCase()) || null;
  }
}
