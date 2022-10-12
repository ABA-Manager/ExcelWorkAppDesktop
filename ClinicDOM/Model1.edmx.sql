
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/23/2022 16:39:08
-- Generated from EDMX file: D:\Personal\Proyectos\Software fidelito\v1\software\clinic_app\clinicDOM\ClinicDOM\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [clinicbdRef];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AspNetRoleClaims_AspNetRoles_RoleId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetRoleClaims] DROP CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserClaims_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserLogins_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRoles_RoleId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserTokens_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserTokens] DROP CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientAgreement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Agreement] DROP CONSTRAINT [FK_ClientAgreement];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientBilling]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Billing] DROP CONSTRAINT [FK_ClientBilling];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientPatientAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PatientAccount] DROP CONSTRAINT [FK_ClientPatientAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientServiceLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceLog] DROP CONSTRAINT [FK_ClientServiceLog];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyAgreement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Agreement] DROP CONSTRAINT [FK_CompanyAgreement];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyPayroll]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payroll] DROP CONSTRAINT [FK_CompanyPayroll];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractorBilling]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Billing] DROP CONSTRAINT [FK_ContractorBilling];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractorPayroll]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payroll] DROP CONSTRAINT [FK_ContractorPayroll];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractorServiceLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceLog] DROP CONSTRAINT [FK_ContractorServiceLog];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractorTypePayroll]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payroll] DROP CONSTRAINT [FK_ContractorTypePayroll];
GO
IF OBJECT_ID(N'[dbo].[FK_DiagnosisClient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Client] DROP CONSTRAINT [FK_DiagnosisClient];
GO
IF OBJECT_ID(N'[dbo].[FK_PayrollAgreement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Agreement] DROP CONSTRAINT [FK_PayrollAgreement];
GO
IF OBJECT_ID(N'[dbo].[FK_PeriodBilling]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Billing] DROP CONSTRAINT [FK_PeriodBilling];
GO
IF OBJECT_ID(N'[dbo].[FK_PeriodServiceLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceLog] DROP CONSTRAINT [FK_PeriodServiceLog];
GO
IF OBJECT_ID(N'[dbo].[FK_PlaceOfServiceUnitDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UnitDetail] DROP CONSTRAINT [FK_PlaceOfServiceUnitDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_ProcedurePayroll]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payroll] DROP CONSTRAINT [FK_ProcedurePayroll];
GO
IF OBJECT_ID(N'[dbo].[FK_ProcedureSubProcedure]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubProcedure] DROP CONSTRAINT [FK_ProcedureSubProcedure];
GO
IF OBJECT_ID(N'[dbo].[FK_ReleaseInformationClient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Client] DROP CONSTRAINT [FK_ReleaseInformationClient];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceLogUnitDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UnitDetail] DROP CONSTRAINT [FK_ServiceLogUnitDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_SubProcedureUnitDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UnitDetail] DROP CONSTRAINT [FK_SubProcedureUnitDetail];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Agreement]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Agreement];
GO
IF OBJECT_ID(N'[dbo].[AspNetRoleClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoleClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserTokens]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserTokens];
GO
IF OBJECT_ID(N'[dbo].[Billing]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Billing];
GO
IF OBJECT_ID(N'[dbo].[Client]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Client];
GO
IF OBJECT_ID(N'[dbo].[Company]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Company];
GO
IF OBJECT_ID(N'[dbo].[Contractor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contractor];
GO
IF OBJECT_ID(N'[dbo].[ContractorType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContractorType];
GO
IF OBJECT_ID(N'[dbo].[Diagnosis]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Diagnosis];
GO
IF OBJECT_ID(N'[dbo].[PatientAccount]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PatientAccount];
GO
IF OBJECT_ID(N'[dbo].[Payroll]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Payroll];
GO
IF OBJECT_ID(N'[dbo].[Period]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Period];
GO
IF OBJECT_ID(N'[dbo].[PlaceOfService]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlaceOfService];
GO
IF OBJECT_ID(N'[dbo].[Procedure]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Procedure];
GO
IF OBJECT_ID(N'[dbo].[ReleaseInformation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReleaseInformation];
GO
IF OBJECT_ID(N'[dbo].[ServiceLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceLog];
GO
IF OBJECT_ID(N'[dbo].[SubProcedure]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubProcedure];
GO
IF OBJECT_ID(N'[dbo].[UnitDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UnitDetail];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Contractor'
CREATE TABLE [dbo].[Contractor] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [RenderingProvider] nvarchar(max)  NULL,
    [Enabled] bit  NOT NULL,
    [Extra] nvarchar(max)  NULL
);
GO

-- Creating table 'Client'
CREATE TABLE [dbo].[Client] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [RecipientID] nvarchar(max)  NULL,
    [PatientAccount] nvarchar(max)  NULL,
    [ReleaseInformationId] int  NOT NULL,
    [ReferringProvider] nvarchar(max)  NULL,
    [AuthorizationNUmber] nvarchar(max)  NULL,
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
    [Active] bit  NOT NULL,
    [PayPeriod] nvarchar(max)  NOT NULL,
    [DocumentDeliveryDate] datetime  NOT NULL,
    [PaymentDate] datetime  NOT NULL
);
GO

-- Creating table 'UnitDetail'
CREATE TABLE [dbo].[UnitDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Modifiers] nvarchar(max)  NULL,
    [PlaceOfServiceId] int  NOT NULL,
    [DateOfService] datetime  NOT NULL,
    [Unit] int  NOT NULL,
    [ServiceLogId] int  NOT NULL,
    [SubProcedureId] int  NOT NULL
);
GO

-- Creating table 'ReleaseInformation'
CREATE TABLE [dbo].[ReleaseInformation] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL
);
GO

-- Creating table 'Company'
CREATE TABLE [dbo].[Company] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Acronym] nvarchar(max)  NULL,
    [Enabled] bit  NOT NULL
);
GO

-- Creating table 'Agreement'
CREATE TABLE [dbo].[Agreement] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClientId] int  NOT NULL,
    [CompanyId] int  NOT NULL,
    [PayrollId] int  NOT NULL,
    [RateEmployees] float  NOT NULL
);
GO

-- Creating table 'Diagnosis'
CREATE TABLE [dbo].[Diagnosis] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'PlaceOfService'
CREATE TABLE [dbo].[PlaceOfService] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Value] nvarchar(max)  NULL
);
GO

-- Creating table 'ContractorType'
CREATE TABLE [dbo].[ContractorType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL
);
GO

-- Creating table 'Procedure'
CREATE TABLE [dbo].[Procedure] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Rate] float  NOT NULL
);
GO

-- Creating table 'Payroll'
CREATE TABLE [dbo].[Payroll] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContractorId] int  NOT NULL,
    [ContractorTypeId] int  NOT NULL,
    [ProcedureId] int  NOT NULL,
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

-- Creating table 'SubProcedure'
CREATE TABLE [dbo].[SubProcedure] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Rate] float  NOT NULL,
    [ProcedureId] int  NOT NULL
);
GO

-- Creating table 'AspNetRoleClaims'
CREATE TABLE [dbo].[AspNetRoleClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoleId] nvarchar(450)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(450)  NOT NULL,
    [Name] nvarchar(256)  NULL,
    [NormalizedName] nvarchar(256)  NULL,
    [ConcurrencyStamp] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(450)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(450)  NOT NULL,
    [ProviderKey] nvarchar(450)  NOT NULL,
    [ProviderDisplayName] nvarchar(max)  NULL,
    [UserId] nvarchar(450)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(450)  NOT NULL,
    [UserName] nvarchar(256)  NULL,
    [NormalizedUserName] nvarchar(256)  NULL,
    [Email] nvarchar(256)  NULL,
    [NormalizedEmail] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [ConcurrencyStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEnd] datetimeoffset  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL
);
GO

-- Creating table 'AspNetUserTokens'
CREATE TABLE [dbo].[AspNetUserTokens] (
    [UserId] nvarchar(450)  NOT NULL,
    [LoginProvider] nvarchar(450)  NOT NULL,
    [Name] nvarchar(450)  NOT NULL,
    [Value] nvarchar(max)  NULL
);
GO

-- Creating table 'PatientAccount'
CREATE TABLE [dbo].[PatientAccount] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LicenseNumber] nvarchar(max)  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [ExpireDate] datetime  NOT NULL,
    [Auxiliar] nvarchar(max) NULL,
    [ClientId] int  NOT NULL
);
GO

-- Creating table 'Billing'
CREATE TABLE [dbo].[Billing] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BillingDate] datetime  NOT NULL,
    [ClientId] int  NOT NULL,
    [ContractorId] int  NOT NULL,
    [PeriodId] int  NOT NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [AspNetRoles_Id] nvarchar(450)  NOT NULL,
    [AspNetUsers_Id] nvarchar(450)  NOT NULL
);
GO

-- Creating table 'AspNetUserRoles1'
CREATE TABLE [dbo].[AspNetUserRoles1] (
    [AspNetRoles1_Id] nvarchar(450)  NOT NULL,
    [AspNetUsers1_Id] nvarchar(450)  NOT NULL
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

-- Creating primary key on [Id] in table 'ServiceLog'
ALTER TABLE [dbo].[ServiceLog]
ADD CONSTRAINT [PK_ServiceLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SubProcedure'
ALTER TABLE [dbo].[SubProcedure]
ADD CONSTRAINT [PK_SubProcedure]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetRoleClaims'
ALTER TABLE [dbo].[AspNetRoleClaims]
ADD CONSTRAINT [PK_AspNetRoleClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserId], [LoginProvider], [Name] in table 'AspNetUserTokens'
ALTER TABLE [dbo].[AspNetUserTokens]
ADD CONSTRAINT [PK_AspNetUserTokens]
    PRIMARY KEY CLUSTERED ([UserId], [LoginProvider], [Name] ASC);
GO

-- Creating primary key on [Id] in table 'PatientAccount'
ALTER TABLE [dbo].[PatientAccount]
ADD CONSTRAINT [PK_PatientAccount]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Billing'
ALTER TABLE [dbo].[Billing]
ADD CONSTRAINT [PK_Billing]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AspNetRoles_Id], [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([AspNetRoles_Id], [AspNetUsers_Id] ASC);
GO

-- Creating primary key on [AspNetRoles1_Id], [AspNetUsers1_Id] in table 'AspNetUserRoles1'
ALTER TABLE [dbo].[AspNetUserRoles1]
ADD CONSTRAINT [PK_AspNetUserRoles1]
    PRIMARY KEY CLUSTERED ([AspNetRoles1_Id], [AspNetUsers1_Id] ASC);
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
    ON DELETE CASCADE ON UPDATE NO ACTION;
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

-- Creating foreign key on [CompanyId] in table 'Payroll'
ALTER TABLE [dbo].[Payroll]
ADD CONSTRAINT [FK_CompanyPayroll]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyPayroll'
CREATE INDEX [IX_FK_CompanyPayroll]
ON [dbo].[Payroll]
    ([CompanyId]);
GO

-- Creating foreign key on [ProcedureId] in table 'SubProcedure'
ALTER TABLE [dbo].[SubProcedure]
ADD CONSTRAINT [FK_ProcedureSubProcedure]
    FOREIGN KEY ([ProcedureId])
    REFERENCES [dbo].[Procedure]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProcedureSubProcedure'
CREATE INDEX [IX_FK_ProcedureSubProcedure]
ON [dbo].[SubProcedure]
    ([ProcedureId]);
GO

-- Creating foreign key on [SubProcedureId] in table 'UnitDetail'
ALTER TABLE [dbo].[UnitDetail]
ADD CONSTRAINT [FK_SubProcedureUnitDetail]
    FOREIGN KEY ([SubProcedureId])
    REFERENCES [dbo].[SubProcedure]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubProcedureUnitDetail'
CREATE INDEX [IX_FK_SubProcedureUnitDetail]
ON [dbo].[UnitDetail]
    ([SubProcedureId]);
GO

-- Creating foreign key on [RoleId] in table 'AspNetRoleClaims'
ALTER TABLE [dbo].[AspNetRoleClaims]
ADD CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetRoleClaims_AspNetRoles_RoleId'
CREATE INDEX [IX_FK_AspNetRoleClaims_AspNetRoles_RoleId]
ON [dbo].[AspNetRoleClaims]
    ([RoleId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserClaims_AspNetUsers_UserId'
CREATE INDEX [IX_FK_AspNetUserClaims_AspNetUsers_UserId]
ON [dbo].[AspNetUserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserLogins_AspNetUsers_UserId'
CREATE INDEX [IX_FK_AspNetUserLogins_AspNetUsers_UserId]
ON [dbo].[AspNetUserLogins]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserTokens'
ALTER TABLE [dbo].[AspNetUserTokens]
ADD CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetRoles_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles]
    FOREIGN KEY ([AspNetRoles_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers]
    FOREIGN KEY ([AspNetUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUsers'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUsers]
ON [dbo].[AspNetUserRoles]
    ([AspNetUsers_Id]);
GO

-- Creating foreign key on [ClientId] in table 'PatientAccount'
ALTER TABLE [dbo].[PatientAccount]
ADD CONSTRAINT [FK_ClientPatientAccount]
    FOREIGN KEY ([ClientId])
    REFERENCES [dbo].[Client]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientPatientAccount'
CREATE INDEX [IX_FK_ClientPatientAccount]
ON [dbo].[PatientAccount]
    ([ClientId]);
GO

-- Creating foreign key on [AspNetRoles1_Id] in table 'AspNetUserRoles1'
ALTER TABLE [dbo].[AspNetUserRoles1]
ADD CONSTRAINT [FK_AspNetUserRoles1_AspNetRoles]
    FOREIGN KEY ([AspNetRoles1_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUsers1_Id] in table 'AspNetUserRoles1'
ALTER TABLE [dbo].[AspNetUserRoles1]
ADD CONSTRAINT [FK_AspNetUserRoles1_AspNetUsers]
    FOREIGN KEY ([AspNetUsers1_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles1_AspNetUsers'
CREATE INDEX [IX_FK_AspNetUserRoles1_AspNetUsers]
ON [dbo].[AspNetUserRoles1]
    ([AspNetUsers1_Id]);
GO

-- Creating foreign key on [ClientId] in table 'Billing'
ALTER TABLE [dbo].[Billing]
ADD CONSTRAINT [FK_ClientBilling]
    FOREIGN KEY ([ClientId])
    REFERENCES [dbo].[Client]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientBilling'
CREATE INDEX [IX_FK_ClientBilling]
ON [dbo].[Billing]
    ([ClientId]);
GO

-- Creating foreign key on [ContractorId] in table 'Billing'
ALTER TABLE [dbo].[Billing]
ADD CONSTRAINT [FK_ContractorBilling]
    FOREIGN KEY ([ContractorId])
    REFERENCES [dbo].[Contractor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractorBilling'
CREATE INDEX [IX_FK_ContractorBilling]
ON [dbo].[Billing]
    ([ContractorId]);
GO

-- Creating foreign key on [PeriodId] in table 'Billing'
ALTER TABLE [dbo].[Billing]
ADD CONSTRAINT [FK_PeriodBilling]
    FOREIGN KEY ([PeriodId])
    REFERENCES [dbo].[Period]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PeriodBilling'
CREATE INDEX [IX_FK_PeriodBilling]
ON [dbo].[Billing]
    ([PeriodId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------