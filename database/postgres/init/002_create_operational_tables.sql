CREATE TABLE IF NOT EXISTS operations.flights (
    flight_id text PRIMARY KEY,
    flight_number text NOT NULL UNIQUE,
    origin_iata char(3) NOT NULL,
    destination_iata char(3) NOT NULL,
    scheduled_departure timestamptz NOT NULL,
    scheduled_arrival timestamptz NOT NULL,
    estimated_departure timestamptz,
    estimated_arrival timestamptz,
    status text NOT NULL CHECK (status IN ('ACTIVE', 'DELAYED', 'CANCELLED', 'DISRUPTED')),
    disruption_reason text,
    aircraft_registration text,
    created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS operations.crew_members (
    crew_member_id text PRIMARY KEY,
    employee_number text NOT NULL UNIQUE,
    full_name text NOT NULL,
    base_iata char(3) NOT NULL,
    crew_role text NOT NULL CHECK (crew_role IN ('CAPTAIN', 'FIRST_OFFICER', 'PURSER', 'CABIN_CREW')),
    status text NOT NULL CHECK (status IN ('AVAILABLE', 'ASSIGNED', 'STANDBY', 'LEGALITY_REVIEW', 'UNAVAILABLE')),
    legality_note text,
    created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS operations.pairings (
    pairing_id text PRIMARY KEY,
    pairing_code text NOT NULL UNIQUE,
    pairing_date date NOT NULL,
    crew_member_ids text[] NOT NULL DEFAULT '{}',
    flight_ids text[] NOT NULL DEFAULT '{}',
    status text NOT NULL CHECK (status IN ('PLANNED', 'ACTIVE', 'DISRUPTED', 'CANCELLED')),
    legality_status text NOT NULL CHECK (legality_status IN ('OK', 'WARNING', 'ISSUE')),
    legality_note text,
    created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS operations.standby_assignments (
    standby_assignment_id text PRIMARY KEY,
    crew_member_id text NOT NULL REFERENCES operations.crew_members (crew_member_id),
    base_iata char(3) NOT NULL,
    standby_start timestamptz NOT NULL,
    standby_end timestamptz NOT NULL,
    readiness_status text NOT NULL CHECK (readiness_status IN ('READY', 'CONTACTED', 'ASSIGNED', 'UNAVAILABLE')),
    notes text,
    created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS operations.operational_journal (
    journal_entry_id text PRIMARY KEY,
    occurred_at timestamptz NOT NULL,
    severity text NOT NULL CHECK (severity IN ('INFO', 'WARNING', 'CRITICAL')),
    category text NOT NULL,
    flight_id text REFERENCES operations.flights (flight_id),
    crew_member_id text REFERENCES operations.crew_members (crew_member_id),
    message text NOT NULL,
    created_by text NOT NULL DEFAULT 'continuity-demo',
    created_at timestamptz NOT NULL DEFAULT now()
);

CREATE INDEX IF NOT EXISTS ix_flights_status ON operations.flights (status);
CREATE INDEX IF NOT EXISTS ix_flights_departure ON operations.flights (scheduled_departure);
CREATE INDEX IF NOT EXISTS ix_crew_members_status ON operations.crew_members (status);
CREATE INDEX IF NOT EXISTS ix_pairings_status ON operations.pairings (status);
CREATE INDEX IF NOT EXISTS ix_operational_journal_occurred_at ON operations.operational_journal (occurred_at DESC);

COMMENT ON TABLE operations.flights IS 'Demo flight snapshot for the OPS&CREW Continuity PoC.';
COMMENT ON TABLE operations.crew_members IS 'Demo crew roster for continuity mockup scenarios.';
COMMENT ON TABLE operations.pairings IS 'Simple demo pairing summary. Not airline-grade scheduling logic.';
COMMENT ON TABLE operations.standby_assignments IS 'Demo standby crew availability for continuity scenarios.';
COMMENT ON TABLE operations.operational_journal IS 'Demo operational journal entries for continuity events.';
