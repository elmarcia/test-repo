export interface FlightDto {
  flightId: string;
  flightNumber: string;
  originIata: string;
  destinationIata: string;
  scheduledDeparture: string;
  scheduledArrival: string;
  estimatedDeparture: string | null;
  estimatedArrival: string | null;
  status: 'ACTIVE' | 'DELAYED' | 'CANCELLED' | 'DISRUPTED';
  disruptionReason: string | null;
  aircraftRegistration: string | null;
}

export interface CrewMemberDto {
  crewMemberId: string;
  employeeNumber: string;
  fullName: string;
  baseIata: string;
  crewRole: 'CAPTAIN' | 'FIRST_OFFICER' | 'PURSER' | 'CABIN_CREW';
  status: 'AVAILABLE' | 'ASSIGNED' | 'STANDBY' | 'LEGALITY_REVIEW' | 'UNAVAILABLE';
  legalityNote: string | null;
}

export interface StandbyAssignmentDto {
  standbyAssignmentId: string;
  crewMemberId: string;
  fullName: string;
  baseIata: string;
  standbyStart: string;
  standbyEnd: string;
  readinessStatus: 'READY' | 'CONTACTED' | 'ASSIGNED' | 'UNAVAILABLE';
  notes: string | null;
}

export interface PairingDto {
  pairingId: string;
  pairingCode: string;
  pairingDate: string;
  crewMemberIds: string[];
  flightIds: string[];
  status: 'PLANNED' | 'ACTIVE' | 'DISRUPTED' | 'CANCELLED';
  legalityStatus: 'OK' | 'WARNING' | 'ISSUE';
  legalityNote: string | null;
}

export interface JournalEntryDto {
  journalEntryId: string;
  occurredAt: string;
  severity: 'INFO' | 'WARNING' | 'CRITICAL';
  category: string;
  flightId: string | null;
  crewMemberId: string | null;
  message: string;
  createdBy: string;
}

export interface DelayFlightRequest {
  minutes: number;
  reason: string;
}

export interface CancelFlightRequest {
  reason: string;
}

export interface AssignStandbyRequest {
  flightId: string;
  notes: string;
}

export interface CreateJournalEntryRequest {
  severity: 'INFO' | 'WARNING' | 'CRITICAL';
  category: string;
  flightId: string | null;
  crewMemberId: string | null;
  message: string;
}
