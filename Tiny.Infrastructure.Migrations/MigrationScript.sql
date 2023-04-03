IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE SEQUENCE [EntityFrameworkHiLoSequence] START WITH 1 INCREMENT BY 10 NO MINVALUE NO MAXVALUE NO CYCLE;
GO

CREATE TABLE [dbo].[AccountingType] (
    [Id] int NOT NULL,
    [Name] nvarchar(200) NULL,
    CONSTRAINT [PK_AccountingType] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[Department] (
    [Id] bigint NOT NULL,
    [Code] nvarchar(200) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [TenantId] nvarchar(200) NOT NULL,
    [Deleted] bit NOT NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[JournalEntryStatus] (
    [Id] int NOT NULL,
    [Name] nvarchar(200) NULL,
    CONSTRAINT [PK_JournalEntryStatus] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[Postable] (
    [Id] int NOT NULL,
    [Name] nvarchar(200) NULL,
    CONSTRAINT [PK_Postable] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[User] (
    [Id] bigint NOT NULL,
    [Code] nvarchar(200) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [TenantId] nvarchar(200) NOT NULL,
    [Deleted] bit NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[JournalEntry] (
    [Id] bigint NOT NULL,
    [PostingDate] datetime2 NOT NULL,
    [JournalEntryStatusId] int NOT NULL,
    [DepartmentId] bigint NOT NULL,
    [Description] nvarchar(200) NOT NULL DEFAULT N'',
    [Deleted] bit NOT NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK_JournalEntry] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_JournalEntry_Department_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Department] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_JournalEntry_JournalEntryStatus_JournalEntryStatusId] FOREIGN KEY ([JournalEntryStatusId]) REFERENCES [dbo].[JournalEntryStatus] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [dbo].[GLAccount] (
    [Id] bigint NOT NULL,
    [Code] nvarchar(50) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [PostableId] int NOT NULL,
    [AccountingTypeId] int NOT NULL,
    [Balance] decimal(19,6) NOT NULL DEFAULT 0.0,
    [TenantId] nvarchar(200) NOT NULL,
    [Deleted] bit NOT NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK_GLAccount] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GLAccount_AccountingType_AccountingTypeId] FOREIGN KEY ([AccountingTypeId]) REFERENCES [dbo].[AccountingType] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_GLAccount_Postable_PostableId] FOREIGN KEY ([PostableId]) REFERENCES [dbo].[Postable] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [dbo].[JournalEntryLine] (
    [Id] bigint NOT NULL IDENTITY,
    [GLAccountId] bigint NOT NULL,
    [DebitAmount] decimal(19,6) NOT NULL DEFAULT 0.0,
    [CreditAmount] decimal(19,6) NOT NULL DEFAULT 0.0,
    [Description] nvarchar(200) NOT NULL DEFAULT N'',
    [JournalEntryId] bigint NOT NULL,
    CONSTRAINT [PK_JournalEntryLine] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_JournalEntryLine_GLAccount_GLAccountId] FOREIGN KEY ([GLAccountId]) REFERENCES [dbo].[GLAccount] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_JournalEntryLine_JournalEntry_JournalEntryId] FOREIGN KEY ([JournalEntryId]) REFERENCES [dbo].[JournalEntry] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[dbo].[AccountingType]'))
    SET IDENTITY_INSERT [dbo].[AccountingType] ON;
INSERT INTO [dbo].[AccountingType] ([Id], [Name])
VALUES (1, N'Asset'),
(2, N'Liability'),
(3, N'Equity'),
(4, N'Revenue'),
(5, N'Expense');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[dbo].[AccountingType]'))
    SET IDENTITY_INSERT [dbo].[AccountingType] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[dbo].[JournalEntryStatus]'))
    SET IDENTITY_INSERT [dbo].[JournalEntryStatus] ON;
INSERT INTO [dbo].[JournalEntryStatus] ([Id], [Name])
VALUES (1, N'신청'),
(2, N'승인'),
(3, N'반려');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[dbo].[JournalEntryStatus]'))
    SET IDENTITY_INSERT [dbo].[JournalEntryStatus] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[dbo].[Postable]'))
    SET IDENTITY_INSERT [dbo].[Postable] ON;
INSERT INTO [dbo].[Postable] ([Id], [Name])
VALUES (1, N'Yes'),
(2, N'No');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[dbo].[Postable]'))
    SET IDENTITY_INSERT [dbo].[Postable] OFF;
GO

CREATE UNIQUE INDEX [IX_Department_TenantId_Code] ON [dbo].[Department] ([TenantId], [Code]);
GO

CREATE INDEX [IX_GLAccount_AccountingTypeId] ON [dbo].[GLAccount] ([AccountingTypeId]);
GO

CREATE INDEX [IX_GLAccount_PostableId] ON [dbo].[GLAccount] ([PostableId]);
GO

CREATE UNIQUE INDEX [IX_GLAccount_TenantId_Code_Name] ON [dbo].[GLAccount] ([TenantId], [Code], [Name]);
GO

CREATE INDEX [IX_JournalEntry_DepartmentId] ON [dbo].[JournalEntry] ([DepartmentId]);
GO

CREATE INDEX [IX_JournalEntry_JournalEntryStatusId] ON [dbo].[JournalEntry] ([JournalEntryStatusId]);
GO

CREATE INDEX [IX_JournalEntryLine_GLAccountId] ON [dbo].[JournalEntryLine] ([GLAccountId]);
GO

CREATE INDEX [IX_JournalEntryLine_JournalEntryId] ON [dbo].[JournalEntryLine] ([JournalEntryId]);
GO

CREATE UNIQUE INDEX [IX_User_TenantId_Code] ON [dbo].[User] ([TenantId], [Code]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230403064926_Initial', N'7.0.4');
GO

COMMIT;
GO

