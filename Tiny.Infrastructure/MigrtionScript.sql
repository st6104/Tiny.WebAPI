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
    [Id] bigint NOT NULL IDENTITY,
    [Code] nvarchar(200) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[JournalEntryStatus] (
    [Id] int NOT NULL,
    [Name] nvarchar(200) NULL,
    CONSTRAINT [PK_JournalEntryStatus] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Postable] (
    [Id] int NOT NULL,
    [Name] nvarchar(200) NULL,
    CONSTRAINT [PK_Postable] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[User] (
    [Id] bigint NOT NULL,
    [Code] nvarchar(200) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [JournalEntry] (
    [Id] bigint NOT NULL,
    [PostingDate] datetime2 NOT NULL,
    [JournalEntryStatusId] int NOT NULL,
    [DepartmentId] bigint NOT NULL,
    [Description] nvarchar(200) NOT NULL,
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
    [AccountTypeId] int NOT NULL,
    [Balance] decimal(19,6) NOT NULL DEFAULT 0.0,
    CONSTRAINT [PK_GLAccount] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GLAccount_AccountingType_AccountTypeId] FOREIGN KEY ([AccountTypeId]) REFERENCES [dbo].[AccountingType] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_GLAccount_Postable_PostableId] FOREIGN KEY ([PostableId]) REFERENCES [Postable] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [dbo].[JournalEntryLine] (
    [Id] bigint NOT NULL IDENTITY,
    [GLAccountId] bigint NOT NULL,
    [DebitAmount] decimal(19,6) NOT NULL,
    [CreditAmount] decimal(19,6) NOT NULL,
    [Description] nvarchar(200) NOT NULL,
    [JournalEntryId] bigint NOT NULL,
    CONSTRAINT [PK_JournalEntryLine] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_JournalEntryLine_GLAccount_GLAccountId] FOREIGN KEY ([GLAccountId]) REFERENCES [dbo].[GLAccount] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_JournalEntryLine_JournalEntry_JournalEntryId] FOREIGN KEY ([JournalEntryId]) REFERENCES [JournalEntry] ([Id]) ON DELETE CASCADE
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

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Postable]'))
    SET IDENTITY_INSERT [Postable] ON;
INSERT INTO [Postable] ([Id], [Name])
VALUES (1, N'Yes'),
(2, N'No');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Postable]'))
    SET IDENTITY_INSERT [Postable] OFF;
GO

CREATE UNIQUE INDEX [IX_Department_Code] ON [dbo].[Department] ([Code]);
GO

CREATE INDEX [IX_GLAccount_AccountTypeId] ON [dbo].[GLAccount] ([AccountTypeId]);
GO

CREATE UNIQUE INDEX [IX_GLAccount_Code_Name] ON [dbo].[GLAccount] ([Code], [Name]);
GO

CREATE INDEX [IX_GLAccount_PostableId] ON [dbo].[GLAccount] ([PostableId]);
GO

CREATE INDEX [IX_JournalEntry_DepartmentId] ON [JournalEntry] ([DepartmentId]);
GO

CREATE INDEX [IX_JournalEntry_JournalEntryStatusId] ON [JournalEntry] ([JournalEntryStatusId]);
GO

CREATE INDEX [IX_JournalEntryLine_GLAccountId] ON [dbo].[JournalEntryLine] ([GLAccountId]);
GO

CREATE INDEX [IX_JournalEntryLine_JournalEntryId] ON [dbo].[JournalEntryLine] ([JournalEntryId]);
GO

CREATE UNIQUE INDEX [IX_User_Code] ON [dbo].[User] ([Code]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230320011956_Init', N'7.0.4');
GO

COMMIT;
GO

