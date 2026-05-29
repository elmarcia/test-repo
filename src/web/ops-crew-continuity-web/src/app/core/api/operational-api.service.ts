import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { AppConfigService } from '../config/app-config.service';
import {
  CrewManifestDto,
  CrewMemberDto,
  AssignStandbyRequest,
  CancelFlightRequest,
  CreateJournalEntryRequest,
  DelayFlightRequest,
  FlightDto,
  GendecDocumentDto,
  JournalEntryDto,
  PairingDto,
  RecoveryActionsReportDto,
  StandbyAssignmentDto
} from './operational-api.models';

@Injectable({
  providedIn: 'root'
})
export class OperationalApiService {
  private readonly http = inject(HttpClient);
  private readonly appConfig = inject(AppConfigService);

  private get apiBaseUrl(): string {
    return this.appConfig.apiBaseUrl;
  }

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

  getGendec(flightId: string): Observable<GendecDocumentDto> {
    return this.http.get<GendecDocumentDto>(`${this.apiBaseUrl}/documents/gendec/${flightId}`);
  }

  getCrewManifest(flightId: string): Observable<CrewManifestDto> {
    return this.http.get<CrewManifestDto>(`${this.apiBaseUrl}/documents/crew-manifest/${flightId}`);
  }

  getRecoveryActionsReport(): Observable<RecoveryActionsReportDto> {
    return this.http.get<RecoveryActionsReportDto>(`${this.apiBaseUrl}/reports/recovery-actions`);
  }
}
