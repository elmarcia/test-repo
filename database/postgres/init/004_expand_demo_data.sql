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
    ('FLT-2001', 'OC2101', 'BOG', 'MDE', '2026-05-17 06:10:00+00', '2026-05-17 07:05:00+00', '2026-05-17 06:10:00+00', '2026-05-17 07:05:00+00', 'ACTIVE', NULL, 'HK-5401'),
    ('FLT-2002', 'OC2102', 'MDE', 'BAQ', '2026-05-17 07:45:00+00', '2026-05-17 08:55:00+00', '2026-05-17 07:55:00+00', '2026-05-17 09:05:00+00', 'DELAYED', 'Late passenger connection from inbound BOG segment', 'HK-5401'),
    ('FLT-2003', 'OC2103', 'BAQ', 'BOG', '2026-05-17 09:35:00+00', '2026-05-17 11:00:00+00', '2026-05-17 10:05:00+00', '2026-05-17 11:30:00+00', 'DELAYED', 'Rotation delay carried from BAQ ground turn', 'HK-5401'),
    ('FLT-2004', 'OC2104', 'BOG', 'PEI', '2026-05-17 12:05:00+00', '2026-05-17 13:05:00+00', '2026-05-17 12:35:00+00', '2026-05-17 13:35:00+00', 'DISRUPTED', 'Crew duty review after accumulated rotation delay', 'HK-5401'),
    ('FLT-2005', 'OC2201', 'BOG', 'CTG', '2026-05-17 06:35:00+00', '2026-05-17 08:00:00+00', '2026-05-17 06:35:00+00', '2026-05-17 08:00:00+00', 'ACTIVE', NULL, 'HK-5402'),
    ('FLT-2006', 'OC2202', 'CTG', 'SMR', '2026-05-17 08:40:00+00', '2026-05-17 09:25:00+00', '2026-05-17 09:20:00+00', '2026-05-17 10:05:00+00', 'DISRUPTED', 'Coastal convective weather hold', 'HK-5402'),
    ('FLT-2007', 'OC2203', 'SMR', 'BOG', '2026-05-17 10:05:00+00', '2026-05-17 11:30:00+00', '2026-05-17 11:05:00+00', '2026-05-17 12:30:00+00', 'DELAYED', 'Downline recovery from coastal weather', 'HK-5402'),
    ('FLT-2008', 'OC2204', 'BOG', 'CLO', '2026-05-17 13:15:00+00', '2026-05-17 14:20:00+00', '2026-05-17 13:45:00+00', '2026-05-17 14:50:00+00', 'ACTIVE', NULL, 'HK-5402'),
    ('FLT-2009', 'OC2301', 'CLO', 'BOG', '2026-05-17 07:10:00+00', '2026-05-17 08:10:00+00', '2026-05-17 07:10:00+00', '2026-05-17 08:10:00+00', 'ACTIVE', NULL, 'HK-5403'),
    ('FLT-2010', 'OC2302', 'BOG', 'LET', '2026-05-17 09:00:00+00', '2026-05-17 10:50:00+00', '2026-05-17 09:25:00+00', '2026-05-17 11:15:00+00', 'DELAYED', 'Cargo weight and balance recalculation', 'HK-5403'),
    ('FLT-2011', 'OC2303', 'LET', 'BOG', '2026-05-17 11:45:00+00', '2026-05-17 13:35:00+00', '2026-05-17 12:25:00+00', '2026-05-17 14:15:00+00', 'DISRUPTED', 'Ground handling delay plus crew legality watch', 'HK-5403'),
    ('FLT-2012', 'OC2304', 'BOG', 'MDE', '2026-05-17 15:00:00+00', '2026-05-17 15:55:00+00', '2026-05-17 15:35:00+00', '2026-05-17 16:30:00+00', 'DELAYED', 'Late inbound aircraft from LET rotation', 'HK-5403'),
    ('FLT-2013', 'OC2401', 'MDE', 'BOG', '2026-05-17 06:50:00+00', '2026-05-17 07:45:00+00', '2026-05-17 06:50:00+00', '2026-05-17 07:45:00+00', 'ACTIVE', NULL, 'HK-5404'),
    ('FLT-2014', 'OC2402', 'BOG', 'AUA', '2026-05-17 08:45:00+00', '2026-05-17 10:35:00+00', NULL, NULL, 'CANCELLED', 'Aircraft technical inspection after overnight arrival', 'HK-5404'),
    ('FLT-2015', 'OC2403', 'AUA', 'BOG', '2026-05-17 11:25:00+00', '2026-05-17 13:10:00+00', NULL, NULL, 'CANCELLED', 'Return segment cancelled due aircraft unavailable', 'HK-5404'),
    ('FLT-2016', 'OC2404', 'BOG', 'UIO', '2026-05-17 15:20:00+00', '2026-05-17 16:55:00+00', '2026-05-17 16:10:00+00', '2026-05-17 17:45:00+00', 'DISRUPTED', 'Aircraft swap under recovery desk review', 'HK-5404'),
    ('FLT-2017', 'OC2501', 'BOG', 'PTY', '2026-05-17 07:35:00+00', '2026-05-17 09:10:00+00', '2026-05-17 07:35:00+00', '2026-05-17 09:10:00+00', 'ACTIVE', NULL, 'HK-5405'),
    ('FLT-2018', 'OC2502', 'PTY', 'BOG', '2026-05-17 10:05:00+00', '2026-05-17 11:40:00+00', '2026-05-17 10:05:00+00', '2026-05-17 11:40:00+00', 'ACTIVE', NULL, 'HK-5405'),
    ('FLT-2019', 'OC2503', 'BOG', 'MIA', '2026-05-17 13:00:00+00', '2026-05-17 16:50:00+00', '2026-05-17 13:45:00+00', '2026-05-17 17:35:00+00', 'DELAYED', 'International document reconciliation', 'HK-5405'),
    ('FLT-2020', 'OC2504', 'MIA', 'BOG', '2026-05-17 18:05:00+00', '2026-05-17 21:45:00+00', '2026-05-17 18:40:00+00', '2026-05-17 22:20:00+00', 'DISRUPTED', 'Inbound delay creates crew connection risk', 'HK-5405'),
    ('FLT-2021', 'OC2601', 'BOG', 'BGA', '2026-05-17 06:20:00+00', '2026-05-17 07:20:00+00', '2026-05-17 06:20:00+00', '2026-05-17 07:20:00+00', 'ACTIVE', NULL, 'HK-5406'),
    ('FLT-2022', 'OC2602', 'BGA', 'EOH', '2026-05-17 08:00:00+00', '2026-05-17 09:10:00+00', '2026-05-17 08:00:00+00', '2026-05-17 09:10:00+00', 'ACTIVE', NULL, 'HK-5406'),
    ('FLT-2023', 'OC2603', 'EOH', 'BOG', '2026-05-17 09:55:00+00', '2026-05-17 10:45:00+00', '2026-05-17 10:25:00+00', '2026-05-17 11:15:00+00', 'DELAYED', 'Airfield metering into BOG', 'HK-5406'),
    ('FLT-2024', 'OC2604', 'BOG', 'VVC', '2026-05-17 12:00:00+00', '2026-05-17 12:50:00+00', '2026-05-17 12:25:00+00', '2026-05-17 13:15:00+00', 'ACTIVE', NULL, 'HK-5406'),
    ('FLT-2025', 'OC2701', 'CLO', 'ADZ', '2026-05-17 08:25:00+00', '2026-05-17 10:20:00+00', '2026-05-17 08:25:00+00', '2026-05-17 10:20:00+00', 'ACTIVE', NULL, 'HK-5407'),
    ('FLT-2026', 'OC2702', 'ADZ', 'CLO', '2026-05-17 11:15:00+00', '2026-05-17 13:10:00+00', '2026-05-17 11:55:00+00', '2026-05-17 13:50:00+00', 'DELAYED', 'Late catering uplift at ADZ', 'HK-5407'),
    ('FLT-2027', 'OC2703', 'CLO', 'BOG', '2026-05-17 14:10:00+00', '2026-05-17 15:15:00+00', '2026-05-17 14:55:00+00', '2026-05-17 16:00:00+00', 'DISRUPTED', 'Standby crew evaluation after ADZ turn delay', 'HK-5407'),
    ('FLT-2028', 'OC2704', 'BOG', 'CTG', '2026-05-17 17:10:00+00', '2026-05-17 18:35:00+00', '2026-05-17 17:45:00+00', '2026-05-17 19:10:00+00', 'ACTIVE', NULL, 'HK-5407'),
    ('FLT-2029', 'OC2801', 'BOG', 'CLO', '2026-05-17 09:15:00+00', '2026-05-17 10:20:00+00', '2026-05-17 09:15:00+00', '2026-05-17 10:20:00+00', 'ACTIVE', NULL, 'HK-5408'),
    ('FLT-2030', 'OC2802', 'CLO', 'MDE', '2026-05-17 11:05:00+00', '2026-05-17 12:05:00+00', NULL, NULL, 'CANCELLED', 'Maintenance control cancelled sector after brake temperature alert', 'HK-5408'),
    ('FLT-2031', 'OC2803', 'MDE', 'BOG', '2026-05-17 13:00:00+00', '2026-05-17 13:55:00+00', NULL, NULL, 'CANCELLED', 'Aircraft unavailable after cancelled CLO-MDE sector', 'HK-5408'),
    ('FLT-2032', 'OC2804', 'BOG', 'BAQ', '2026-05-17 15:05:00+00', '2026-05-17 16:30:00+00', '2026-05-17 16:00:00+00', '2026-05-17 17:25:00+00', 'DISRUPTED', 'Aircraft swap and passenger reprotection required', 'HK-5408')
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
    ('CREW-101', 'E1101', 'Renata Salazar', 'BOG', 'CAPTAIN', 'ASSIGNED', NULL),
    ('CREW-102', 'E1102', 'Martin Pineda', 'BOG', 'FIRST_OFFICER', 'ASSIGNED', NULL),
    ('CREW-103', 'E1103', 'Clara Espinosa', 'BOG', 'PURSER', 'ASSIGNED', NULL),
    ('CREW-104', 'E1104', 'Rafael Cote', 'CTG', 'CAPTAIN', 'LEGALITY_REVIEW', 'Coastal weather delay extends projected duty period'),
    ('CREW-105', 'E1105', 'Lina Acosta', 'CTG', 'FIRST_OFFICER', 'ASSIGNED', NULL),
    ('CREW-106', 'E1106', 'Pablo Mejia', 'CLO', 'CAPTAIN', 'ASSIGNED', NULL),
    ('CREW-107', 'E1107', 'Monica Beltran', 'CLO', 'FIRST_OFFICER', 'STANDBY', NULL),
    ('CREW-108', 'E1108', 'Ivan Restrepo', 'BOG', 'CAPTAIN', 'STANDBY', NULL),
    ('CREW-109', 'E1109', 'Sara Quintero', 'BOG', 'FIRST_OFFICER', 'STANDBY', NULL),
    ('CREW-110', 'E1110', 'Emilia Vargas', 'BOG', 'PURSER', 'ASSIGNED', NULL),
    ('CREW-111', 'E1111', 'Hector Naranjo', 'MDE', 'CAPTAIN', 'ASSIGNED', NULL),
    ('CREW-112', 'E1112', 'Luisa Barrios', 'MDE', 'FIRST_OFFICER', 'LEGALITY_REVIEW', 'Late LET return creates minimum rest review'),
    ('CREW-113', 'E1113', 'Oscar Londoño', 'BOG', 'CAPTAIN', 'ASSIGNED', NULL),
    ('CREW-114', 'E1114', 'Diana Muñoz', 'BOG', 'FIRST_OFFICER', 'ASSIGNED', NULL),
    ('CREW-115', 'E1115', 'Veronica Ibarra', 'BOG', 'CABIN_CREW', 'AVAILABLE', NULL),
    ('CREW-116', 'E1116', 'Cristian Mora', 'BAQ', 'CABIN_CREW', 'STANDBY', NULL),
    ('CREW-117', 'E1117', 'Felipe Torres', 'BOG', 'CAPTAIN', 'ASSIGNED', NULL),
    ('CREW-118', 'E1118', 'Paula Rios', 'BOG', 'FIRST_OFFICER', 'ASSIGNED', NULL)
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
    ('PAIR-101', 'BOG-ROT-17A', '2026-05-17', ARRAY['CREW-101', 'CREW-102', 'CREW-103'], ARRAY['FLT-2001', 'FLT-2002', 'FLT-2003', 'FLT-2004'], 'DISRUPTED', 'WARNING', 'Four-sector rotation has compressed PEI turn after BAQ delay'),
    ('PAIR-102', 'CTG-COAST-17', '2026-05-17', ARRAY['CREW-104', 'CREW-105', 'CREW-006'], ARRAY['FLT-2005', 'FLT-2006', 'FLT-2007', 'FLT-2008'], 'DISRUPTED', 'ISSUE', 'Coastal weather creates duty review and standby monitoring'),
    ('PAIR-103', 'BOG-AMZ-17', '2026-05-17', ARRAY['CREW-111', 'CREW-112', 'CREW-115'], ARRAY['FLT-2009', 'FLT-2010', 'FLT-2011', 'FLT-2012'], 'DISRUPTED', 'ISSUE', 'LET delay creates projected minimum rest issue'),
    ('PAIR-104', 'BOG-INT-17A', '2026-05-17', ARRAY['CREW-113', 'CREW-114', 'CREW-110'], ARRAY['FLT-2013', 'FLT-2014', 'FLT-2015', 'FLT-2016'], 'CANCELLED', 'ISSUE', 'Aircraft cancellation disrupts outbound international sequence'),
    ('PAIR-105', 'BOG-INT-17B', '2026-05-17', ARRAY['CREW-001', 'CREW-002', 'CREW-003'], ARRAY['FLT-2017', 'FLT-2018', 'FLT-2019', 'FLT-2020'], 'DISRUPTED', 'WARNING', 'MIA delay creates crew connection risk'),
    ('PAIR-106', 'BOG-REG-17', '2026-05-17', ARRAY['CREW-117', 'CREW-118', 'CREW-116'], ARRAY['FLT-2021', 'FLT-2022', 'FLT-2023', 'FLT-2024'], 'ACTIVE', 'OK', NULL),
    ('PAIR-107', 'CLO-ISL-17', '2026-05-17', ARRAY['CREW-106', 'CREW-107', 'CREW-009'], ARRAY['FLT-2025', 'FLT-2026', 'FLT-2027', 'FLT-2028'], 'DISRUPTED', 'WARNING', 'ADZ delay triggers standby crew evaluation'),
    ('PAIR-108', 'BOG-TECH-17', '2026-05-17', ARRAY['CREW-108', 'CREW-109', 'CREW-010'], ARRAY['FLT-2029', 'FLT-2030', 'FLT-2031', 'FLT-2032'], 'CANCELLED', 'ISSUE', 'Technical cancellation requires aircraft swap and passenger protection')
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
    ('STBY-101', 'CREW-107', 'CLO', '2026-05-17 06:00:00+00', '2026-05-17 14:00:00+00', 'CONTACTED', 'First officer contacted for ADZ recovery sequence'),
    ('STBY-102', 'CREW-108', 'BOG', '2026-05-17 07:00:00+00', '2026-05-17 15:00:00+00', 'READY', 'Captain reserve for technical cancellation recovery'),
    ('STBY-103', 'CREW-109', 'BOG', '2026-05-17 07:00:00+00', '2026-05-17 15:00:00+00', 'READY', 'First officer reserve for aircraft swap recovery'),
    ('STBY-104', 'CREW-110', 'BOG', '2026-05-17 12:00:00+00', '2026-05-17 20:00:00+00', 'ASSIGNED', 'Purser assigned to protect international recovery pairing'),
    ('STBY-105', 'CREW-115', 'BOG', '2026-05-17 10:00:00+00', '2026-05-17 18:00:00+00', 'READY', 'Cabin reserve for MIA connection recovery'),
    ('STBY-106', 'CREW-116', 'BAQ', '2026-05-17 08:00:00+00', '2026-05-17 16:00:00+00', 'CONTACTED', 'Cabin crew contacted for BAQ downline delay')
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
    ('JRN-101', '2026-05-17 07:58:00+00', 'WARNING', 'Delay', 'FLT-2002', NULL, 'OC2102 held for late passenger connection; HK-5401 rotation buffer reduced.'),
    ('JRN-102', '2026-05-17 10:12:00+00', 'WARNING', 'Recovery', 'FLT-2003', 'CREW-116', 'BAQ standby cabin crew contacted while OC2103 delay propagates.'),
    ('JRN-103', '2026-05-17 12:28:00+00', 'CRITICAL', 'Crew Legality', 'FLT-2004', 'CREW-102', 'PEI continuation flagged for crew duty review.'),
    ('JRN-104', '2026-05-17 09:18:00+00', 'WARNING', 'Weather', 'FLT-2006', 'CREW-104', 'Coastal convective weather creates CTG-SMR disruption.'),
    ('JRN-105', '2026-05-17 11:08:00+00', 'WARNING', 'Recovery', 'FLT-2007', NULL, 'Recovery desk protecting BOG arrival connections after SMR delay.'),
    ('JRN-106', '2026-05-17 12:18:00+00', 'CRITICAL', 'Crew Legality', 'FLT-2011', 'CREW-112', 'LET return delay creates minimum rest review for MDE continuation.'),
    ('JRN-107', '2026-05-17 08:30:00+00', 'CRITICAL', 'Cancellation', 'FLT-2014', NULL, 'AUA outbound cancelled after maintenance control technical inspection.'),
    ('JRN-108', '2026-05-17 11:05:00+00', 'CRITICAL', 'Cancellation', 'FLT-2015', NULL, 'AUA return cancelled; passenger reprotection and aircraft swap opened.'),
    ('JRN-109', '2026-05-17 13:35:00+00', 'WARNING', 'Documents', 'FLT-2019', 'CREW-114', 'MIA departure delayed for document reconciliation.'),
    ('JRN-110', '2026-05-17 18:36:00+00', 'WARNING', 'Recovery', 'FLT-2020', 'CREW-115', 'MIA return monitored for inbound crew connection risk.'),
    ('JRN-111', '2026-05-17 10:27:00+00', 'WARNING', 'Airfield', 'FLT-2023', NULL, 'EOH-BOG delayed by airfield metering into BOG.'),
    ('JRN-112', '2026-05-17 11:52:00+00', 'INFO', 'Standby', NULL, 'CREW-107', 'CLO standby first officer contacted for ADZ recovery sequence.'),
    ('JRN-113', '2026-05-17 14:48:00+00', 'WARNING', 'Recovery', 'FLT-2027', 'CREW-107', 'ADZ delay triggers standby crew evaluation for CLO-BOG continuation.'),
    ('JRN-114', '2026-05-17 10:58:00+00', 'CRITICAL', 'Cancellation', 'FLT-2030', NULL, 'CLO-MDE cancelled after brake temperature alert.'),
    ('JRN-115', '2026-05-17 12:40:00+00', 'CRITICAL', 'Cancellation', 'FLT-2031', NULL, 'MDE-BOG cancelled due aircraft unavailable.'),
    ('JRN-116', '2026-05-17 15:42:00+00', 'WARNING', 'Recovery', 'FLT-2032', 'CREW-108', 'Aircraft swap and passenger reprotection required for BOG-BAQ.'),
    ('JRN-117', '2026-05-17 07:05:00+00', 'INFO', 'Flight Monitoring', 'FLT-2021', NULL, 'HK-5406 regional shuttle operating on plan.'),
    ('JRN-118', '2026-05-17 13:05:00+00', 'INFO', 'Standby', NULL, 'CREW-110', 'BOG purser reserve assigned to protect international recovery pairing.')
ON CONFLICT (journal_entry_id) DO NOTHING;
