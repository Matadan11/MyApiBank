CREATE DATABASE [My_Bank_DB];
SELECT 1
CREATE TABLE [__EFMigrationsHistory] (
    [MigrationId] nvarchar(150) NOT NULL,
    [ProductVersion] nvarchar(32) NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
);
SELECT 1
SELECT OBJECT_ID(N'[__EFMigrationsHistory]');
SELECT [MigrationId], [ProductVersion]
FROM [__EFMigrationsHistory]
ORDER BY [MigrationId];

CREATE TABLE [Accounts] (
    [AccountId] int NOT NULL IDENTITY,
    [AccountNumber] nvarchar(max) NOT NULL,
    [Balance] decimal(18,2) NOT NULL,
    [Transaction_list] int NOT NULL,
    [Transaction_list] int NOT NULL,
    [Deposits] int NOT NULL,
    [Withdrawals] int NOT NULL,
    [accrued_interest] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY ([AccountId])
);

CREATE TABLE [Transactions] (
    [TransId] int NOT NULL IDENTITY,
    [AccountId] int NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [TransactionDate] datetime2 NOT NULL,
    [Trantype] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY ([TransId])
);
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240328204735_Initial', N'8.0.3');