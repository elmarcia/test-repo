import { Injectable } from '@angular/core';

import { FlightDto } from '../../core/api/operational-api.models';
import { TimelineConnector, TimelineFlightBar, TimelineRow, TimelineTick, TimelineViewModel } from './flight-timeline.models';

const MINUTE_MS = 60_000;

@Injectable({
  providedIn: 'root'
})
export class FlightTimelineService {
  buildViewModel(flights: FlightDto[]): TimelineViewModel {
    const usableFlights = flights.filter((flight) => this.getStartTime(flight) && this.getEndTime(flight));
    const bounds = this.getTimelineBounds(usableFlights);
    const operationalNow = this.getOperationalNow(bounds.windowStart, bounds.windowEnd);
    const rows = this.groupFlights(usableFlights, bounds.windowStart, bounds.totalMinutes);

    return {
      rows,
      ticks: this.buildTicks(bounds.windowStart, bounds.windowEnd, bounds.totalMinutes),
      windowStart: bounds.windowStart,
      windowEnd: bounds.windowEnd,
      totalMinutes: bounds.totalMinutes,
      operationalNow,
      currentTimeOffsetPercent: this.clamp(((operationalNow.getTime() - bounds.windowStart.getTime()) / MINUTE_MS / bounds.totalMinutes) * 100, 0, 100)
    };
  }

  getEffectiveDeparture(flight: FlightDto): Date {
    return new Date(flight.estimatedDeparture || flight.scheduledDeparture);
  }

  getEffectiveArrival(flight: FlightDto): Date {
    return new Date(flight.estimatedArrival || flight.scheduledArrival);
  }

  getDelayMinutes(flight: FlightDto): number {
    if (!flight.estimatedDeparture) {
      return 0;
    }

    return Math.round((new Date(flight.estimatedDeparture).getTime() - new Date(flight.scheduledDeparture).getTime()) / MINUTE_MS);
  }

  private groupFlights(flights: FlightDto[], windowStart: Date, totalMinutes: number): TimelineRow[] {
    const grouped = new Map<string, TimelineFlightBar[]>();

    for (const flight of flights) {
      const key = this.getGroupKey(flight);
      const bars = grouped.get(key) || [];
      bars.push(this.buildBar(flight, windowStart, totalMinutes));
      grouped.set(key, bars);
    }

    return Array.from(grouped.entries())
      .map(([groupLabel, bars]) => ({
        groupKey: groupLabel,
        groupLabel,
        bars: this.withTurnHints(
          bars.sort((left, right) => this.getEffectiveDeparture(left.flight).getTime() - this.getEffectiveDeparture(right.flight).getTime())
        ),
        connectors: this.buildConnectors(
          bars.sort((left, right) => this.getEffectiveDeparture(left.flight).getTime() - this.getEffectiveDeparture(right.flight).getTime())
        )
      }))
      .sort((left, right) => left.groupLabel.localeCompare(right.groupLabel));
  }

  private buildBar(flight: FlightDto, windowStart: Date, totalMinutes: number): TimelineFlightBar {
    const departure = this.getEffectiveDeparture(flight);
    const arrival = this.getEffectiveArrival(flight);
    const leftPercent = this.clamp(((departure.getTime() - windowStart.getTime()) / MINUTE_MS / totalMinutes) * 100, 0, 100);
    const widthPercent = this.clamp(((arrival.getTime() - departure.getTime()) / MINUTE_MS / totalMinutes) * 100, 3, 100 - leftPercent);

    return {
      flight,
      leftPercent,
      widthPercent,
      delayMinutes: this.getDelayMinutes(flight),
      statusClass: flight.status.toLowerCase(),
      departureLabel: this.formatTime(departure),
      arrivalLabel: this.formatTime(arrival),
      turnMinutesAfterPrevious: null
    };
  }

  private withTurnHints(bars: TimelineFlightBar[]): TimelineFlightBar[] {
    return bars.map((bar, index) => {
      if (index === 0) {
        return bar;
      }

      const previousArrival = this.getEffectiveArrival(bars[index - 1].flight);
      const departure = this.getEffectiveDeparture(bar.flight);
      return {
        ...bar,
        turnMinutesAfterPrevious: Math.round((departure.getTime() - previousArrival.getTime()) / MINUTE_MS)
      };
    });
  }

  private buildConnectors(bars: TimelineFlightBar[]): TimelineConnector[] {
    const connectors: TimelineConnector[] = [];

    for (let index = 0; index < bars.length - 1; index += 1) {
      const current = bars[index];
      const next = bars[index + 1];
      const gap = next.leftPercent - (current.leftPercent + current.widthPercent);
      const turnMinutes = Math.round((this.getEffectiveDeparture(next.flight).getTime() - this.getEffectiveArrival(current.flight).getTime()) / MINUTE_MS);

      connectors.push({
        leftPercent: current.leftPercent + current.widthPercent,
        widthPercent: this.clamp(gap, 0.4, 100),
        state: this.getConnectorState(current.flight, next.flight, turnMinutes),
        label: `${turnMinutes}m turn`
      });
    }

    return connectors;
  }

  private buildTicks(windowStart: Date, windowEnd: Date, totalMinutes: number): TimelineTick[] {
    const ticks: TimelineTick[] = [];
    const cursor = new Date(windowStart);
    cursor.setMinutes(0, 0, 0);

    while (cursor <= windowEnd) {
      const offsetPercent = ((cursor.getTime() - windowStart.getTime()) / MINUTE_MS / totalMinutes) * 100;
      if (offsetPercent >= 0 && offsetPercent <= 100) {
        ticks.push({
          label: this.formatTime(cursor),
          offsetPercent
        });
      }
      cursor.setHours(cursor.getHours() + 1);
    }

    return ticks;
  }

  private getTimelineBounds(flights: FlightDto[]): { windowStart: Date; windowEnd: Date; totalMinutes: number } {
    if (flights.length === 0) {
      const now = new Date();
      const windowStart = new Date(now);
      windowStart.setMinutes(0, 0, 0);
      const windowEnd = new Date(windowStart.getTime() + 8 * 60 * MINUTE_MS);
      return { windowStart, windowEnd, totalMinutes: 480 };
    }

    const starts = flights.map((flight) => this.getEffectiveDeparture(flight).getTime());
    const ends = flights.map((flight) => this.getEffectiveArrival(flight).getTime());
    const windowStart = new Date(Math.min(...starts) - 60 * MINUTE_MS);
    const windowEnd = new Date(Math.max(...ends) + 60 * MINUTE_MS);
    windowStart.setMinutes(0, 0, 0);
    windowEnd.setMinutes(59, 0, 0);

    return {
      windowStart,
      windowEnd,
      totalMinutes: Math.max(60, Math.round((windowEnd.getTime() - windowStart.getTime()) / MINUTE_MS))
    };
  }

  private getConnectorState(currentFlight: FlightDto, nextFlight: FlightDto, turnMinutes: number): 'normal' | 'tight' | 'broken' {
    if (currentFlight.status === 'CANCELLED' || nextFlight.status === 'CANCELLED' || turnMinutes < 0) {
      return 'broken';
    }

    if (turnMinutes < 35 || currentFlight.status !== 'ACTIVE' || nextFlight.status !== 'ACTIVE') {
      return 'tight';
    }

    return 'normal';
  }

  private getOperationalNow(windowStart: Date, windowEnd: Date): Date {
    const now = new Date();
    if (now >= windowStart && now <= windowEnd) {
      return now;
    }

    return new Date(windowStart.getTime() + (windowEnd.getTime() - windowStart.getTime()) * 0.52);
  }

  private getGroupKey(flight: FlightDto): string {
    return flight.aircraftRegistration || `${flight.originIata}-${flight.destinationIata}`;
  }

  private getStartTime(flight: FlightDto): Date | null {
    return flight.estimatedDeparture || flight.scheduledDeparture ? this.getEffectiveDeparture(flight) : null;
  }

  private getEndTime(flight: FlightDto): Date | null {
    return flight.estimatedArrival || flight.scheduledArrival ? this.getEffectiveArrival(flight) : null;
  }

  private formatTime(value: Date): string {
    return value.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }

  private clamp(value: number, min: number, max: number): number {
    return Math.min(Math.max(value, min), max);
  }
}
