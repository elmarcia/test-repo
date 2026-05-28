import { FlightDto } from '../../core/api/operational-api.models';

export interface TimelineTick {
  label: string;
  offsetPercent: number;
}

export interface TimelineFlightBar {
  flight: FlightDto;
  leftPercent: number;
  widthPercent: number;
  delayMinutes: number;
  statusClass: string;
  departureLabel: string;
  arrivalLabel: string;
  turnMinutesAfterPrevious: number | null;
}

export interface TimelineRow {
  groupKey: string;
  groupLabel: string;
  bars: TimelineFlightBar[];
  connectors: TimelineConnector[];
}

export interface TimelineConnector {
  leftPercent: number;
  widthPercent: number;
  state: 'normal' | 'tight' | 'broken';
  label: string;
}

export interface TimelineViewModel {
  rows: TimelineRow[];
  ticks: TimelineTick[];
  windowStart: Date;
  windowEnd: Date;
  totalMinutes: number;
  operationalNow: Date;
  currentTimeOffsetPercent: number;
}
