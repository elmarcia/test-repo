CREATE SCHEMA IF NOT EXISTS core;
CREATE SCHEMA IF NOT EXISTS operations;
CREATE SCHEMA IF NOT EXISTS crew;
CREATE SCHEMA IF NOT EXISTS continuity;
CREATE SCHEMA IF NOT EXISTS identity_access;

COMMENT ON SCHEMA core IS 'Shared database objects for the OPS&CREW Continuity modular monolith.';
COMMENT ON SCHEMA operations IS 'Schema reserved for the Operations module.';
COMMENT ON SCHEMA crew IS 'Schema reserved for the Crew module.';
COMMENT ON SCHEMA continuity IS 'Schema reserved for the Continuity module.';
COMMENT ON SCHEMA identity_access IS 'Schema reserved for future identity and access objects. No real Entra ID integration is configured.';
