IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'security')
BEGIN
    EXEC('CREATE SCHEMA security');
	PRINT 'security schema cretaed.';
END

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'lookup')
BEGIN
    EXEC('CREATE SCHEMA lookup');
	PRINT 'lookup schema cretaed.';
END

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'audit')
BEGIN
    EXEC('CREATE SCHEMA audit');
	PRINT 'audit schema cretaed.';
END