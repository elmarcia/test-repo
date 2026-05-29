INSERT INTO operations.flights (
    flight_id,
    flight_number,
    origin_iata,
    destination_iata,
    scheduled_departure,
    scheduled_arrival,
    estimated_departure,
    estimated_arrival,
    status,
    disruption_reason,
    aircraft_registration
) VALUES
    ('FLT-1001', 'OC1001', 'BOG', 'MDE', '2026-05-16 08:10:00+00', '2026-05-16 09:05:00+00', '2026-05-16 08:10:00+00', '2026-05-16 09:05:00+00', 'ACTIVE', NULL, 'HK-5301'),
    ('FLT-1002', 'OC2040', 'BOG', 'CTG', '2026-05-16 09:25:00+00', '2026-05-16 10:50:00+00', '2026-05-16 10:10:00+00', '2026-05-16 11:35:00+00', 'DELAYED', 'Weather hold at destination', 'HK-5314'),
    ('FLT-1003', 'OC3175', 'MDE', 'BOG', '2026-05-16 10:15:00+00', '2026-05-16 11:05:00+00', '2026-05-16 11:00:00+00', '2026-05-16 11:50:00+00', 'DELAYED', 'Inbound aircraft rotation delay', 'HK-5298'),
    ('FLT-1004', 'OC4422', 'BOG', 'CLO', '2026-05-16 11:40:00+00', '2026-05-16 12:45:00+00', NULL, NULL, 'CANCELLED', 'Aircraft technical inspection', 'HK-5270'),
    ('FLT-1005', 'OC5588', 'CTG', 'BOG', '2026-05-16 13:00:00+00', '2026-05-16 14:25:00+00', '2026-05-16 14:20:00+00', '2026-05-16 15:45:00+00', 'DISRUPTED', 'Crew legality review required', 'HK-5322'),
    ('FLT-1006', 'OC6124', 'CLO', 'ADZ', '2026-05-16 14:10:00+00', '2026-05-16 16:05:00+00', '2026-05-16 14:10:00+00', '2026-05-16 16:05:00+00', 'ACTIVE', NULL, 'HK-5330'),
    ('FLT-1007', 'OC7301', 'BOG', 'UIO', '2026-05-16 15:35:00+00', '2026-05-16 17:10:00+00', '2026-05-16 17:05:00+00', '2026-05-16 18:40:00+00', 'DISRUPTED', 'Air traffic flow restriction', 'HK-5255'),
    ('FLT-1008', 'OC8180', 'MDE', 'CTG', '2026-05-16 17:20:00+00', '2026-05-16 18:30:00+00', NULL, NULL, 'CANCELLED', 'Operational recovery scenario', 'HK-5266')
ON CONFLICT (flight_id) DO NOTHING;

INSERT INTO operations.crew_members (
    crew_member_id,
    employee_number,
    full_name,
    base_iata,
    crew_role,
    status,
    legality_note
) VALUES
    ('CREW-001', 'E1001', 'Ana Torres', 'BOG', 'CAPTAIN', 'ASSIGNED', NULL),
    ('CREW-002', 'E1002', 'Mateo Ruiz', 'BOG', 'FIRST_OFFICER', 'ASSIGNED', NULL),
    ('CREW-003', 'E1003', 'Laura Gomez', 'BOG', 'PURSER', 'ASSIGNED', NULL),
    ('CREW-004', 'E1004', 'Diego Alvarez', 'MDE', 'CAPTAIN', 'LEGALITY_REVIEW', 'Projected duty time exceeds demo threshold after delay'),
    ('CREW-005', 'E1005', 'Valentina Rojas', 'MDE', 'FIRST_OFFICER', 'LEGALITY_REVIEW', 'Minimum rest review needed for disrupted pairing'),
    ('CREW-006', 'E1006', 'Camilo Perez', 'CTG', 'CABIN_CREW', 'ASSIGNED', NULL),
    ('CREW-007', 'E1007', 'Sofia Herrera', 'BOG', 'CAPTAIN', 'STANDBY', NULL),
    ('CREW-008', 'E1008', 'Julian Moreno', 'BOG', 'FIRST_OFFICER', 'STANDBY', NULL),
    ('CREW-009', 'E1009', 'Natalia Cardenas', 'CLO', 'CABIN_CREW', 'ASSIGNED', NULL),
    ('CREW-010', 'E1010', 'Andres Silva', 'BOG', 'PURSER', 'AVAILABLE', NULL)
ON CONFLICT (crew_member_id) DO NOTHING;

INSERT INTO operations.pairings (
    pairing_id,
    pairing_code,
    pairing_date,
    crew_member_ids,
    flight_ids,
    status,
    legality_status,
    legality_note
) VALUES
    ('PAIR-001', 'BOG-AM-01', '2026-05-16', ARRAY['CREW-001', 'CREW-002', 'CREW-003'], ARRAY['FLT-1001', 'FLT-1002'], 'ACTIVE', 'OK', NULL),
    ('PAIR-002', 'MDE-AM-02', '2026-05-16', ARRAY['CREW-004', 'CREW-005'], ARRAY['FLT-1003', 'FLT-1008'], 'DISRUPTED', 'ISSUE', 'Delay plus cancelled segment creates a demo legality issue'),
    ('PAIR-003', 'CTG-PM-01', '2026-05-16', ARRAY['CREW-006'], ARRAY['FLT-1005'], 'DISRUPTED', 'WARNING', 'Crew reassignment may be needed after disruption'),
    ('PAIR-004', 'CLO-PM-01', '2026-05-16', ARRAY['CREW-009'], ARRAY['FLT-1006'], 'ACTIVE', 'OK', NULL),
    ('PAIR-005', 'BOG-INT-01', '2026-05-16', ARRAY['CREW-007', 'CREW-008'], ARRAY['FLT-1007'], 'PLANNED', 'WARNING', 'Standby crew candidates identified for disrupted international segment')
ON CONFLICT (pairing_id) DO NOTHING;

INSERT INTO operations.standby_assignments (
    standby_assignment_id,
    crew_member_id,
    base_iata,
    standby_start,
    standby_end,
    readiness_status,
    notes
) VALUES
    ('STBY-001', 'CREW-007', 'BOG', '2026-05-16 06:00:00+00', '2026-05-16 14:00:00+00', 'READY', 'Captain available for recovery pairing'),
    ('STBY-002', 'CREW-008', 'BOG', '2026-05-16 06:00:00+00', '2026-05-16 14:00:00+00', 'CONTACTED', 'First officer contacted for OC7301 scenario'),
    ('STBY-003', 'CREW-009', 'CLO', '2026-05-16 12:00:00+00', '2026-05-16 20:00:00+00', 'ASSIGNED', 'Assigned to active CLO departure'),
    ('STBY-004', 'CREW-010', 'BOG', '2026-05-16 13:00:00+00', '2026-05-16 21:00:00+00', 'READY', 'Purser available for disrupted CTG arrival')
ON CONFLICT (standby_assignment_id) DO NOTHING;

INSERT INTO operations.operational_journal (
    journal_entry_id,
    occurred_at,
    severity,
    category,
    flight_id,
    crew_member_id,
    message
) VALUES
    ('JRN-001', '2026-05-16 08:05:00+00', 'INFO', 'Flight Monitoring', 'FLT-1001', NULL, 'OC1001 marked active for demo continuity board.'),
    ('JRN-002', '2026-05-16 09:30:00+00', 'WARNING', 'Delay', 'FLT-1002', NULL, 'OC2040 delayed due to weather hold at destination.'),
    ('JRN-003', '2026-05-16 10:20:00+00', 'WARNING', 'Delay', 'FLT-1003', 'CREW-004', 'Inbound rotation delay may affect MDE pairing duty window.'),
    ('JRN-004', '2026-05-16 11:05:00+00', 'CRITICAL', 'Cancellation', 'FLT-1004', NULL, 'OC4422 cancelled for aircraft technical inspection demo scenario.'),
    ('JRN-005', '2026-05-16 12:45:00+00', 'CRITICAL', 'Crew Legality', 'FLT-1005', 'CREW-005', 'Crew legality issue flagged for disrupted CTG return.'),
    ('JRN-006', '2026-05-16 13:15:00+00', 'INFO', 'Standby', NULL, 'CREW-007', 'Standby captain remains ready at BOG.'),
    ('JRN-007', '2026-05-16 14:35:00+00', 'WARNING', 'Disruption', 'FLT-1007', 'CREW-008', 'OC7301 impacted by air traffic flow restriction; standby first officer contacted.'),
    ('JRN-008', '2026-05-16 16:50:00+00', 'CRITICAL', 'Cancellation', 'FLT-1008', NULL, 'OC8180 cancelled as part of operational recovery demo data.')
ON CONFLICT (journal_entry_id) DO NOTHING;
