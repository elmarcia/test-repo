-- Depends on 001_create_schemas.sql and 002_create_operational_tables.sql.
-- Depends on 003/004 seed data for the demo assignment backfill rows below.
-- Keeps existing PostgreSQL volumes aligned with the Crew Movement V1 model.

ALTER TABLE operations.standby_assignments
ADD COLUMN IF NOT EXISTS assigned_flight_id text REFERENCES operations.flights (flight_id);

CREATE INDEX IF NOT EXISTS ix_standby_assignments_assigned_flight
ON operations.standby_assignments (assigned_flight_id);

UPDATE operations.standby_assignments
SET assigned_flight_id = 'FLT-1006'
WHERE standby_assignment_id = 'STBY-003'
  AND assigned_flight_id IS NULL;

UPDATE operations.crew_members
SET status = 'ASSIGNED'
WHERE crew_member_id = 'CREW-009';

UPDATE operations.standby_assignments
SET assigned_flight_id = 'FLT-2016'
WHERE standby_assignment_id = 'STBY-104'
  AND assigned_flight_id IS NULL;

UPDATE operations.crew_members
SET status = 'ASSIGNED'
WHERE crew_member_id = 'CREW-110';

UPDATE operations.pairings
SET crew_member_ids = array_append(crew_member_ids, 'CREW-009')
WHERE pairing_id = 'PAIR-004'
  AND NOT 'CREW-009' = ANY(crew_member_ids);

UPDATE operations.pairings
SET crew_member_ids = array_append(crew_member_ids, 'CREW-110')
WHERE pairing_id = 'PAIR-104'
  AND NOT 'CREW-110' = ANY(crew_member_ids);

INSERT INTO operations.operational_journal (
    journal_entry_id,
    occurred_at,
    severity,
    category,
    flight_id,
    crew_member_id,
    message
) VALUES
    ('JRN-009', '2026-05-16 13:50:00+00', 'INFO', 'Standby', 'FLT-1006', 'CREW-009', 'Natalia Cardenas (E1009) assigned from standby to OC6124 CLO->ADZ HK-5330 STD 2026-05-16 14:10.'),
    ('JRN-119', '2026-05-17 13:05:00+00', 'INFO', 'Standby', 'FLT-2016', 'CREW-110', 'Emilia Vargas (E1110) assigned from standby to OC2404 BOG->UIO HK-5404 STD 2026-05-17 15:20.')
ON CONFLICT (journal_entry_id) DO NOTHING;
