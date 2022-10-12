INSERT Company ( Name, Acronym, Enabled )
VALUES  
('Company 1', 'C1', 1),
('Company 2', 'C2', 1)

INSERT PlaceOfService ( Name, Value )
VALUES  
('Home', '12'),
('School', '03'),
('Community', '99')

INSERT Period (StartDate, EndDate, Active)
VALUES
('2022/01/01','2022/01/14',1)

INSERT ContractorType ( Name )
VALUES  
('Analyst'),
('RBT')

INSERT [Procedure] (Name, Rate)
VALUES
('H2019',19.05),
('H2012',15.24),
('H2014',12.19)

INSERT Contractor (Name, RenderingProvider, Enabled, Extra)
VALUES
('Analyst 1', 'RP', 1, 'Algo 1'),	--1
('Analyst 2', 'RP', 1, 'Algo 2'),	--2
('Analyst 3', 'RP', 1, 'Algo 3'),	--3
('RBT 1', 'RP', 1, 'Algo 4'),		--4
('RBT 2', 'RP', 1, ''),		--5
('RBT 3', 'RP', 1, ''),		--6
('RBT 4', 'RP', 1, '')		--7

INSERT Payroll (ContractorId, ContractorTypeId, [ProcedureId], RateEmployees )
VALUES
(1,1,1,80.0),
(2,1,1,76.0),
(3,1,2,60.0),
(4,2,3,30.0),
(5,2,3,30.0),
(6,2,3,30.0),
(7,2,3,30.0)

INSERT CompanyContractor ( Company_Id, Contractor_Id )
VALUES  
(1, 1 ),
(2, 2 ),
(2, 3 ),
(1, 4 ),
(2, 5 ),
(2, 6 ),
(2, 7 )

INSERT ReleaseInformation ( Name )
VALUES  
('Bla bla bla')

INSERT Diagnosis ( Name )
VALUES  
( 'Enfermo' )

INSERT dbo.Client
        (Name,RecipientID,PatientAccount,ReleaseInformationId,ReferringProvider,AuthorizationNUmber,Sequence,DiagnosisId,Enabled, WeeklyApprovedRBT, WeeklyApprovedAnalyst)
VALUES  
('Client 1', 'Recipt', 'Account1', 1, 'Provider', '12345', 1, 1, 1, 50, 30),
('Client 2', 'Recipt', 'Account2', 1, 'Provider', '12345', 2, 1, 1, 24, 2),
('Client 3', 'Recipt', 'Account3', 1, 'Provider', '12345', 2, 1, 1, 31, 4)

INSERT Agreement ( ClientId, CompanyId, PayrollId )
VALUES  
( 1, 1, 1 ),
( 1, 1, 4 ),
( 2, 2, 2 ),
( 2, 2, 5 ),
( 2, 2, 6 ),
( 3, 2, 2 ),
( 3, 2, 3 ),
( 3, 2, 7 )

INSERT INTO ServiceLog ( PeriodId, ClientId, ContractorId, InsertDate )
VALUES
(1,1,1, GETDATE()),
(1,1,4, GETDATE()),
(1,2,2, GETDATE()),
(1,2,5, GETDATE()),
(1,2,6, GETDATE()),
(1,3,2, GETDATE()),
(1,3,3, GETDATE()),
(1,3,7, GETDATE())


INSERT UnitDetail( ServiceLogId, PlaceOfServiceId, Unit, DateOfService, Modifiers )
VALUES  
( 1,1,8,'2022/01/04','BA'),
( 1,2,8,'2022/01/06','BA'),
( 1,1,8,'2022/01/08','BA'),
( 1,2,8,'2022/01/10','BA'),
( 2,1,8,'2022/01/05','BA'),
( 2,2,8,'2022/01/07','BA'),
( 2,1,8,'2022/01/09','BA'),
( 3,1,8,'2022/01/04','BA'),
( 3,2,8,'2022/01/06','BA'),
( 3,1,8,'2022/01/08','BA'),
( 4,2,8,'2022/01/05','BA'),
( 4,1,8,'2022/01/06','BA'),
( 5,2,8,'2022/01/07','BA'),
( 5,1,8,'2022/01/09','BA'),
( 6,1,8,'2022/01/04','BA'),
( 6,2,8,'2022/01/06','BA'),
( 7,1,8,'2022/01/08','BA'),
( 7,2,8,'2022/01/05','BA'),
( 8,1,8,'2022/01/06','BA'),
( 8,2,8,'2022/01/07','BA'),
( 8,1,8,'2022/01/09','BA')






