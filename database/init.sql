-- Am Creating the DB
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LinkShortenerDb')
BEGIN
    CREATE DATABASE LinkShortenerDb;
END
GO

-- Make LinkShortenerDb the main schema
USE LinkShortenerDb;
GO

-- Create the Users table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' and xtype='U')
BEGIN
    CREATE TABLE Users (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        FirstName NVARCHAR(100) NOT NULL,
        Surname NVARCHAR(100) NOT NULL,
        Email NVARCHAR(256) NOT NULL,
        PasswordHash NVARCHAR(MAX) NOT NULL
    );
    -- Made the Email column  a unique constraint to avoid duplicates
    CREATE UNIQUE INDEX IX_Users_Email ON Users(Email);
END
GO

-- Create the Urls table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Urls' and xtype='U')
BEGIN
    CREATE TABLE Urls (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        LongUrl NVARCHAR(MAX) NOT NULL,
        ShortCode NVARCHAR(50) NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UserId UNIQUEIDENTIFIER NOT NULL,
        --Foreign key relationship to the Users table
        CONSTRAINT FK_Urls_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
    );
    -- Add a Shortcode as a constraint
    CREATE UNIQUE INDEX IX_Urls_ShortCode ON Urls(ShortCode);
END
GO

-- Create the Clicks table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Clicks' and xtype='U')
BEGIN
    CREATE TABLE Clicks (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        UrlId UNIQUEIDENTIFIER NOT NULL,
        ClickedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        -- Foreign key relationship to the Urls table
        CONSTRAINT FK_Clicks_Urls FOREIGN KEY (UrlId) REFERENCES Urls(Id) ON DELETE CASCADE
    );
END
GO

PRINT 'Database and tables created successfully.';