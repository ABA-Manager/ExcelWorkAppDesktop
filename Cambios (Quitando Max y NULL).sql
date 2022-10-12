ALTER TABLE [dbo].[Contractor] ALTER COLUMN [Name] nvarchar(50)  NOT NULL;
ALTER TABLE [dbo].[Contractor] ALTER COLUMN [RenderingProvider] nvarchar(100)  NOT NULL;
ALTER TABLE [dbo].[Contractor] ALTER COLUMN [Extra] nvarchar(100)  NOT NULL;

ALTER TABLE [dbo].[Client] ALTER COLUMN [Name] nvarchar(50)  NOT NULL;
ALTER TABLE [dbo].[Client] ALTER COLUMN [RecipientID] nvarchar(15)  NOT NULL;
ALTER TABLE [dbo].[Client] ALTER COLUMN [PatientAccount] nvarchar(15)  NOT NULL;
ALTER TABLE [dbo].[Client] ALTER COLUMN [ReferringProvider] nvarchar(15)  NOT NULL;
ALTER TABLE [dbo].[Client] ALTER COLUMN [AuthorizationNumber] nvarchar(15)  NOT NULL;
	
ALTER TABLE [dbo].[UnitDetail] ALTER COLUMN [Modifiers] nvarchar(20) NOT NULL;
	
ALTER TABLE [dbo].[ReleaseInformation] ALTER COLUMN [Name] nvarchar(100)  NOT NULL;
	
ALTER TABLE [dbo].[Company] ALTER COLUMN [Name] nvarchar(100)  NOT NULL;
ALTER TABLE [dbo].[Company] ALTER COLUMN [Acronym] nvarchar(10) NULL;

ALTER TABLE [dbo].[Diagnosis] ALTER COLUMN [Name] nvarchar(10)  NOT NULL;
ALTER TABLE [dbo].[Diagnosis] ALTER COLUMN [Description] nvarchar(150) NULL;
	
ALTER TABLE [dbo].[PlaceOfService] ALTER COLUMN [Name] nvarchar(50)  NOT NULL;
ALTER TABLE [dbo].[PlaceOfService] ALTER COLUMN [Value] nvarchar(5)  NOT NULL;

ALTER TABLE [dbo].[ContractorType] ALTER COLUMN [Name] nvarchar(20)  NOT NULL;

ALTER TABLE [dbo].[Procedure] ALTER COLUMN [Name] nvarchar(10)  NOT NULL;

--DROP INDEX [IX_UnitDetail_DOS] ON [dbo].[UnitDetail]
CREATE NONCLUSTERED INDEX [IX_UnitDetail_DOS] ON [dbo].[UnitDetail] ( [DateOfService] ASC )

--DROP INDEX [IX_ServiceLog_ClientId_ContractorID] ON dbo.ServiceLog
CREATE NONCLUSTERED INDEX [IX_ServiceLog_ClientId_ContractorID] ON dbo.ServiceLog (ClientID ASC, ContractorId ASC)

