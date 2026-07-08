IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'ticket')
BEGIN
    EXEC('CREATE SCHEMA ticket');
	PRINT 'ticket schema cretaed.';
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