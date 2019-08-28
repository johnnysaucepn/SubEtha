-- User: matrix
-- DROP USER matrix;

CREATE USER matrix WITH
  LOGIN
  NOSUPERUSER
  INHERIT
  CREATEDB
  NOCREATEROLE
  NOREPLICATION;

-- Database: Matrix
-- DROP DATABASE "Matrix";

CREATE DATABASE "Matrix"
    WITH 
    OWNER = matrix
    ENCODING = 'UTF8'
    LC_COLLATE = 'English_United Kingdom.1252'
    LC_CTYPE = 'English_United Kingdom.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;
