import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  CrewMemberDto,
  AssignStandbyRequest,
  CancelFlightRequest,
  CreateJournalEntryRequest,
  DelayFlightRequest,
  FlightDto,
  JournalEntryDto,
  PairingDto,
  StandbyAssignmentDto
} from './operational-api.models';

@Injectable({
  providedIn: 'root'
})
export class OperationalApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = environment.apiBaseUrl.replace(/\/$/, '');

  getFlights(): Observable<FlightDto[]> {
    return this.http.get<FlightDto[]>(`${this.apiBaseUrl}/flights`);
  }

  getDisruptedFlights(): Observable<FlightDto[]> {
    return this.http.get<FlightDto[]>(`${this.apiBaseUrl}/flights/disrupted`);
  }

  getCrewMembers(): Observable<CrewMemberDto[]> {
    return this.http.get<CrewMemberDto[]>(`${this.apiBaseUrl}/crew-members`);
  }

  getStandbyAssignments(): Observable<StandbyAssignmentDto[]> {
    return this.http.get<StandbyAssignmentDto[]>(`${this.apiBaseUrl}/standby-assignments`);
  }

  getPairings(): Observable<PairingDto[]> {
    return this.http.get<PairingDto[]>(`${this.apiBaseUrl}/pairings`);
  }

  getJournal(): Observable<JournalEntryDto[]> {
    return this.http.get<JournalEntryDto[]>(`${this.apiBaseUrl}/journal`);
  }

  delayFlight(flightId: string, request: DelayFlightRequest): Observable<void> {
    return this.http.post<void>(`${this.apiBaseUrl}/flights/${flightId}/delay`, request);
  }

  cancelFlight(flightId: string, request: CancelFlightRequest): Observable<void> {
    return this.http.post<void>(`${this.apiBaseUrl}/flights/${flightId}/cancel`, request);
  }

  assignStandby(standbyAssignmentId: string, request: AssignStandbyRequest): Observable<void> {
    return this.http.post<void>(`${this.apiBaseUrl}/standby-assignments/${standbyAssignmentId}/assign`, request);
  }

  addJournalEntry(request: CreateJournalEntryRequest): Observable<void> {
    return this.http.post<void>(`${this.apiBaseUrl}/journal`, request);
  }
}
