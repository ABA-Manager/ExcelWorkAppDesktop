
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/24/2022 21:22:13
-- Generated from EDMX file: D:\ExtraWork\clinic_app\clinicDOM\ClinicDOM\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [clinicbd_dev];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ClientAgreement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Agreement] DROP CONSTRAINT [FK_ClientAgreement];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyAgreement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Agreement] DROP CONSTRAINT [FK_CompanyAgreement];
GO
IF OBJECT_ID(N'[dbo].[FK_ReleaseInformationClient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Client] DROP CONSTRAINT [FK_ReleaseInformationClient];
GO
IF OBJECT_ID(N'[dbo].[FK_DiagnosisClient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Client] DROP CONSTRAINT [FK_DiagnosisClient];
GO
IF OBJECT_ID(N'[dbo].[FK_PlaceOfServiceUnitDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UnitDetail] DROP CONSTRAINT [FK_PlaceOfServiceUnitDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractorPayroll]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payroll] DROP CONSTRAINT [FK_ContractorPayroll];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractorTypePayroll]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payroll] DROP CONSTRAINT [FK_ContractorTypePayroll];
GO
IF OBJECT_ID(N'[dbo].[FK_ProcedurePayroll]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payroll] DROP CONSTRAINT [FK_ProcedurePayroll];
GO
IF OBJECT_ID(N'[dbo].[FK_PayrollAgreement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Agreement] DROP CONSTRAINT [FK_PayrollAgreement];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractorCompanyContractor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CompanyContractor] DROP CONSTRAINT [FK_ContractorCompanyContractor];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyCompanyContractor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CompanyContractor] DROP CONSTRAINT [FK_CompanyCompanyContractor];
GO
IF OBJECT_ID(N'[dbo].[FK_PeriodServiceLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceLog] DROP CONSTRAINT [FK_PeriodServiceLog];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceLogUnitDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UnitDetail] DROP CONSTRAINT [FK_ServiceLogUnitDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractorServiceLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceLog] DROP CONSTRAINT [FK_ContractorServiceLog];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientServiceLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceLog] DROP CONSTRAINT [FK_ClientServiceLog];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Contractor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contractor];
GO
IF OBJECT_ID(N'[dbo].[Client]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Client];
GO
IF OBJECT_ID(N'[dbo].[Period]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Period];
GO
IF OBJECT_ID(N'[dbo].[UnitDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UnitDetail];
GO
IF OBJECT_ID(N'[dbo].[ReleaseInformation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReleaseInformation];
GO
IF OBJECT_ID(N'[dbo].[Company]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Company];
GO
IF OBJECT_ID(N'[dbo].[Agreement]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Agreement];
GO
IF OBJECT_ID(N'[dbo].[Diagnosis]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Diagnosis];
GO
IF OBJECT_ID(N'[dbo].[PlaceOfService]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlaceOfService];
GO
IF OBJECT_ID(N'[dbo].[ContractorType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContractorType];
GO
IF OBJECT_ID(N'[dbo].[Procedure]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Procedure];
GO
IF OBJECT_ID(N'[dbo].[Payroll]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Payroll];
GO
IF OBJECT_ID(N'[dbo].[CompanyContractor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyContractor];
GO
IF OBJECT_ID(N'[dbo].[ServiceLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceLog];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Contractor'
CREATE TABLE [dbo].[Contractor] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [RenderingProvider] nvarchar(max)  NOT NULL,
    [Enabled] bit  NOT NULL,
    [Extra] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Client'
CREATE TABLE [dbo].[Client] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [RecipientID] nvarchar(max)  NOT NULL,
    [PatientAccount] nvarchar(max)  NOT NULL,
    [ReleaseInformationId] int  NOT NULL,
    [ReferringProvider] nvarchar(max)  NOT NULL,
    [AuthorizationNUmber] nvarchar(max)  NOT NULL,
    [Sequence] int  NOT NULL,
    [DiagnosisId] int  NOT NULL,
    [Enabled] bit  NOT NULL,
    [WeeklyApprovedRBT] int  NOT NULL,
    [WeeklyApprovedAnalyst] int  NOT NULL
);
GO

-- Creating table 'Period'
CREATE TABLE [dbo].[Period] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [Active] bit  NOT NULL
);
GO

-- Creating table 'UnitDetail'
CREATE TABLE [dbo].[UnitDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Modifiers] nvarchar(max)  NULL,
    [PlaceOfServiceId] int  NOT NULL,
    [DateOfService] datetime  NOT NULL,
    [Unit] int  NOT NULL,
    [ServiceLogId] int  NOT NULL
);
GO

-- Creating table 'ReleaseInformation'
CREATE TABLE [dbo].[ReleaseInformation] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Company'
CREATE TABLE [dbo].[Company] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Acronym] nvarchar(max)  NULL,
    [Enabled] bit  NOT NULL
);
GO

-- Creating table 'Agreement'
CREATE TABLE [dbo].[Agreement] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClientId] int  NOT NULL,
    [CompanyId] int  NOT NULL,
    [PayrollId] int  NOT NULL
);
GO

-- Creating table 'Diagnosis'
CREATE TABLE [dbo].[Diagnosis] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'PlaceOfService'
CREATE TABLE [dbo].[PlaceOfService] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Value] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ContractorType'
CREATE TABLE [dbo].[ContractorType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Procedure'
CREATE TABLE [dbo].[Procedure] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Rate] float  NOT NULL
);
GO

-- Creating table 'Payroll'
CREATE TABLE [dbo].[Payroll] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RateEmployees] float  NOT NULL,
    [ContractorId] int  NOT NULL,
    [ContractorTypeId] int  NOT NULL,
    [ProcedureId] int  NOT NULL
);
GO

-- Creating table 'CompanyContractor'
CREATE TABLE [dbo].[CompanyContractor] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContractorId] int  NOT NULL,
    [CompanyId] int  NOT NULL
);
GO

-- Creating table 'ServiceLog'
CREATE TABLE [dbo].[ServiceLog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PeriodId] int  NOT NULL,
    [ContractorId] int  NOT NULL,
    [ClientId] int  NOT NULL,
    [CreatedDate] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Contractor'
ALTER TABLE [dbo].[Contractor]
ADD CONSTRAINT [PK_Contractor]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Client'
ALTER TABLE [dbo].[Client]
ADD CONSTRAINT [PK_Client]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Period'
ALTER TABLE [dbo].[Period]
ADD CONSTRAINT [PK_Period]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UnitDetail'
ALTER TABLE [dbo].[UnitDetail]
ADD CONSTRAINT [PK_UnitDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReleaseInformation'
ALTER TABLE [dbo].[ReleaseInformation]
ADD CONSTRAINT [PK_ReleaseInformation]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Company'
ALTER TABLE [dbo].[Company]
ADD CONSTRAINT [PK_Company]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Agreement'
ALTER TABLE [dbo].[Agreement]
ADD CONSTRAINT [PK_Agreement]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Diagnosis'
ALTER TABLE [dbo].[Diagnosis]
ADD CONSTRAINT [PK_Diagnosis]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PlaceOfService'
ALTER TABLE [dbo].[PlaceOfService]
ADD CONSTRAINT [PK_PlaceOfService]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ContractorType'
ALTER TABLE [dbo].[ContractorType]
ADD CONSTRAINT [PK_ContractorType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Procedure'
ALTER TABLE [dbo].[Procedure]
ADD CONSTRAINT [PK_Procedure]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Payroll'
ALTER TABLE [dbo].[Payroll]
ADD CONSTRAINT [PK_Payroll]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CompanyContractor'
ALTER TABLE [dbo].[CompanyContractor]
ADD CONSTRAINT [PK_CompanyContractor]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ServiceLog'
ALTER TABLE [dbo].[ServiceLog]
ADD CONSTRAINT [PK_ServiceLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ClientId] in table 'Agreement'
ALTER TABLE [dbo].[Agreement]
ADD CONSTRAINT [FK_ClientAgreement]
    FOREIGN KEY ([ClientId])
    REFERENCES [dbo].[Client]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientAgreement'
CREATE INDEX [IX_FK_ClientAgreement]
ON [dbo].[Agreement]
    ([ClientId]);
GO

-- Creating foreign key on [CompanyId] in table 'Agreement'
ALTER TABLE [dbo].[Agreement]
ADD CONSTRAINT [FK_CompanyAgreement]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyAgreement'
CREATE INDEX [IX_FK_CompanyAgreement]
ON [dbo].[Agreement]
    ([CompanyId]);
GO

-- Creating foreign key on [ReleaseInformationId] in table 'Client'
ALTER TABLE [dbo].[Client]
ADD CONSTRAINT [FK_ReleaseInformationClient]
    FOREIGN KEY ([ReleaseInformationId])
    REFERENCES [dbo].[ReleaseInformation]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReleaseInformationClient'
CREATE INDEX [IX_FK_ReleaseInformationClient]
ON [dbo].[Client]
    ([ReleaseInformationId]);
GO

-- Creating foreign key on [DiagnosisId] in table 'Client'
ALTER TABLE [dbo].[Client]
ADD CONSTRAINT [FK_DiagnosisClient]
    FOREIGN KEY ([DiagnosisId])
    REFERENCES [dbo].[Diagnosis]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DiagnosisClient'
CREATE INDEX [IX_FK_DiagnosisClient]
ON [dbo].[Client]
    ([DiagnosisId]);
GO

-- Creating foreign key on [PlaceOfServiceId] in table 'UnitDetail'
ALTER TABLE [dbo].[UnitDetail]
ADD CONSTRAINT [FK_PlaceOfServiceUnitDetail]
    FOREIGN KEY ([PlaceOfServiceId])
    REFERENCES [dbo].[PlaceOfService]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlaceOfServiceUnitDetail'
CREATE INDEX [IX_FK_PlaceOfServiceUnitDetail]
ON [dbo].[UnitDetail]
    ([PlaceOfServiceId]);
GO

-- Creating foreign key on [ContractorId] in table 'Payroll'
ALTER TABLE [dbo].[Payroll]
ADD CONSTRAINT [FK_ContractorPayroll]
    FOREIGN KEY ([ContractorId])
    REFERENCES [dbo].[Contractor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractorPayroll'
CREATE INDEX [IX_FK_ContractorPayroll]
ON [dbo].[Payroll]
    ([ContractorId]);
GO

-- Creating foreign key on [ContractorTypeId] in table 'Payroll'
ALTER TABLE [dbo].[Payroll]
ADD CONSTRAINT [FK_ContractorTypePayroll]
    FOREIGN KEY ([ContractorTypeId])
    REFERENCES [dbo].[ContractorType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractorTypePayroll'
CREATE INDEX [IX_FK_ContractorTypePayroll]
ON [dbo].[Payroll]
    ([ContractorTypeId]);
GO

-- Creating foreign key on [ProcedureId] in table 'Payroll'
ALTER TABLE [dbo].[Payroll]
ADD CONSTRAINT [FK_ProcedurePayroll]
    FOREIGN KEY ([ProcedureId])
    REFERENCES [dbo].[Procedure]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProcedurePayroll'
CREATE INDEX [IX_FK_ProcedurePayroll]
ON [dbo].[Payroll]
    ([ProcedureId]);
GO

-- Creating foreign key on [PayrollId] in table 'Agreement'
ALTER TABLE [dbo].[Agreement]
ADD CONSTRAINT [FK_PayrollAgreement]
    FOREIGN KEY ([PayrollId])
    REFERENCES [dbo].[Payroll]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PayrollAgreement'
CREATE INDEX [IX_FK_PayrollAgreement]
ON [dbo].[Agreement]
    ([PayrollId]);
GO

-- Creating foreign key on [ContractorId] in table 'CompanyContractor'
ALTER TABLE [dbo].[CompanyContractor]
ADD CONSTRAINT [FK_ContractorCompanyContractor]
    FOREIGN KEY ([ContractorId])
    REFERENCES [dbo].[Contractor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractorCompanyContractor'
CREATE INDEX [IX_FK_ContractorCompanyContractor]
ON [dbo].[CompanyContractor]
    ([ContractorId]);
GO

-- Creating foreign key on [CompanyId] in table 'CompanyContractor'
ALTER TABLE [dbo].[CompanyContractor]
ADD CONSTRAINT [FK_CompanyCompanyContractor]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyCompanyContractor'
CREATE INDEX [IX_FK_CompanyCompanyContractor]
ON [dbo].[CompanyContractor]
    ([CompanyId]);
GO

-- Creating foreign key on [PeriodId] in table 'ServiceLog'
ALTER TABLE [dbo].[ServiceLog]
ADD CONSTRAINT [FK_PeriodServiceLog]
    FOREIGN KEY ([PeriodId])
    REFERENCES [dbo].[Period]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PeriodServiceLog'
CREATE INDEX [IX_FK_PeriodServiceLog]
ON [dbo].[ServiceLog]
    ([PeriodId]);
GO

-- Creating foreign key on [ServiceLogId] in table 'UnitDetail'
ALTER TABLE [dbo].[UnitDetail]
ADD CONSTRAINT [FK_ServiceLogUnitDetail]
    FOREIGN KEY ([ServiceLogId])
    REFERENCES [dbo].[ServiceLog]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceLogUnitDetail'
CREATE INDEX [IX_FK_ServiceLogUnitDetail]
ON [dbo].[UnitDetail]
    ([ServiceLogId]);
GO

-- Creating foreign key on [ContractorId] in table 'ServiceLog'
ALTER TABLE [dbo].[ServiceLog]
ADD CONSTRAINT [FK_ContractorServiceLog]
    FOREIGN KEY ([ContractorId])
    REFERENCES [dbo].[Contractor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractorServiceLog'
CREATE INDEX [IX_FK_ContractorServiceLog]
ON [dbo].[ServiceLog]
    ([ContractorId]);
GO

-- Creating foreign key on [ClientId] in table 'ServiceLog'
ALTER TABLE [dbo].[ServiceLog]
ADD CONSTRAINT [FK_ClientServiceLog]
    FOREIGN KEY ([ClientId])
    REFERENCES [dbo].[Client]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientServiceLog'
CREATE INDEX [IX_FK_ClientServiceLog]
ON [dbo].[ServiceLog]
    ([ClientId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------