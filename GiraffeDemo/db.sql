USE GiraffeDemo

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL 
  DROP TABLE dbo.Invoices;

CREATE TABLE dbo.Invoices (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Supplier nvarchar(255) SPARSE NULL,
    Sum numeric(18)
);

GO