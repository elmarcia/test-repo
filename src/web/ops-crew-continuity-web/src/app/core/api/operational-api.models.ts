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

export interface GendecDocumentDto {
  flightId: string;
  documentNumber: string;
  generatedAtUtc: string;
  flightInformation: {
    flightNumber: string | null;
    scheduledDeparture: string | null;
    scheduledArrival: string | null;
    estimatedDeparture: string | null;
    estimatedArrival: string | null;
    status: string | null;
    disruptionReason: string | null;
  };
  aircraft: {
    registration: string | null;
  };
  route: {
    originIata: string | null;
    destinationIata: string | null;
  };
  crew: Array<{
    crewMemberId: string;
    employeeNumber: string;
    fullName: string;
    crewRole: string;
    status: string;
  }>;
  declaration: string;
  warnings: string[];
  rulesApplied: string[];
  fieldVisibility: {
    showEstimatedTimes: boolean;
    showDisruptionReason: boolean;
    showCrewStatus: boolean;
    showWarnings: boolean;
    showRulesApplied: boolean;
  };
  html: string;
}

export interface CrewManifestDto {
  flightId: string;
  documentNumber: string;
  generatedAtUtc: string;
  flight: {
    flightNumber: string;
    originIata: string;
    destinationIata: string;
    scheduledDeparture: string;
    scheduledArrival: string;
    status: string;
    aircraftRegistration: string | null;
  };
  crew: Array<{
    crewMemberId: string;
    employeeNumber: string;
    fullName: string;
    baseIata: string;
    crewRole: string;
    status: string;
    legalityNote: string | null;
  }>;
  warnings: string[];
  rulesApplied: string[];
  html: string;
}

export interface RecoveryActionsReportDto {
  reportId: string;
  generatedAtUtc: string;
  actions: Array<{
    disruptionType: string;
    impactedFlights: string[];
    assignedStandbyCrew: string[];
    recoveryAction: string;
    firstObservedAt: string;
    lastUpdatedAt: string;
    operationalNotes: string;
  }>;
  warnings: string[];
  rulesApplied: string[];
  html: string;
}
