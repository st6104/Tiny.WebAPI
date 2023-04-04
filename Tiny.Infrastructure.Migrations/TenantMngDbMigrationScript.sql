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

CREATE TABLE [TenantInfo] (
    [Id] nvarchar(200) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [ConnectionString] nvarchar(1000) NOT NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK_TenantInfo] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230404072204_Initial', N'7.0.4');
GO

COMMIT;
GO

