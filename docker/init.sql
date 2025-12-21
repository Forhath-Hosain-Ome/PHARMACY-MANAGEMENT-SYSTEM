-- Initial Database Setup Script for Pharmacy Management System
-- This script runs automatically when the PostgreSQL container starts for the first time

-- Enable required extensions
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Create schema (optional, using public by default)
-- CREATE SCHEMA IF NOT EXISTS pharmacy;

-- Grant privileges
GRANT ALL PRIVILEGES ON DATABASE pharmacy_db TO pharmacy_user;

-- Create custom types/enums (optional, can also use VARCHAR with CHECK constraints)
DO $$ BEGIN
    CREATE TYPE user_role AS ENUM ('Admin', 'Pharmacist', 'Patient', 'Employee');
EXCEPTION
    WHEN duplicate_object THEN null;
END $$;

DO $$ BEGIN
    CREATE TYPE medication_category AS ENUM ('Antibiotic', 'Painkiller', 'Vitamin', 'Supplement', 'Cardiovascular', 'Respiratory', 'Gastrointestinal', 'Other');
EXCEPTION
    WHEN duplicate_object THEN null;
END $$;

DO $$ BEGIN
    CREATE TYPE prescription_status AS ENUM ('Pending', 'Dispensed', 'Cancelled', 'Expired');
EXCEPTION
    WHEN duplicate_object THEN null;
END $$;

DO $$ BEGIN
    CREATE TYPE transaction_status AS ENUM ('Pending', 'Completed', 'Refunded', 'Cancelled');
EXCEPTION
    WHEN duplicate_object THEN null;
END $$;

-- Create initial audit trigger function (optional)
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW."UpdatedAt" = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ language 'plpgsql';

-- Log successful initialization
DO $$
BEGIN
    RAISE NOTICE 'Pharmacy Database initialized successfully!';
END $$;