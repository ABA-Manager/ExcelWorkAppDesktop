-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE INS_CONTRACTOR_INFO
   (@ContractorName nvarchar(max),
	@RenderinProvide nvarchar(max),
	@CompanyId int,
	@PayRollRate float,
	@contractorTypeId int,
	@procedureId int)	   
AS
BEGIN
	DECLARE @ContId INT
		
	--INSERT INTO Contractor
	INSERT INTO Contractor  (Name, RenderingProvider, Enabled) 
	VALUES (@ContractorName, @RenderinProvide, 1);
	
	SET @ContId = @@IDENTITY

	--INSERT INTO CompanyContractor
	INSERT INTO dbo.CompanyContractor ( Company_Id, Contractor_Id ) 
	VALUES  ( @CompanyId, @ContId )

	--INSERT INTO Payroll
	INSERT INTO Payroll ( RateEmployees, ContractorId, ContractorTypeId, ProcedureId )
	VALUES (@PayRollRate, @ContId, @contractorTypeId, @procedureId)

	RETURN @ContId;
END



