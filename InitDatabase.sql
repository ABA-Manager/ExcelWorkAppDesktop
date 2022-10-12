INSERT Company ( Name, Acronym, Enabled )
VALUES  
('Company 1', 'C1', 1),
('Company 2', 'C2', 1)

INSERT PlaceOfService ( Name, Value )
VALUES  
('Home', '12'),
('School', '03'),
('Community', '99')

INSERT Period (StartDate, EndDate, PayPeriod, [DocumentDeliveryDate], [PaymentDate], Active)
VALUES
('2022/01/09','2022/01/22', 'PP02', '2022/01/24', '2022/02/03',1),
('2022/01/23','2022/02/05', 'PP03', '2022/02/07', '2022/02/17',1),
('2022/02/06','2022/02/19', 'PP04', '2022/02/21', '2022/03/03',1)

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
('RBT 2', 'RP', 1, 'Algo 5'),		--5
('RBT 3', 'RP', 1, 'Algo 6'),		--6
('RBT 4', 'RP', 1, 'Algo 7'),		--7
('RBT 5', 'RP', 1, 'Algo 8')

INSERT Payroll (ContractorId, ContractorTypeId, [ProcedureId], CompanyId )
VALUES
(1,1,1, 1),
(2,1,1, 1),
(3,1,2, 2),
(4,2,3, 1),
(5,2,3, 1),
(6,2,3, 2),
(7,2,3, 2),
(8,2,3, 2)

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

INSERT Agreement ( ClientId, CompanyId, PayrollId,  RateEmployees )
VALUES  
( 1, 1, 1, 80 ),
( 1, 1, 4, 30 ),
( 2, 1, 2, 76 ),
( 2, 1, 5, 30 ),
( 2, 2, 6, 30 ),
( 3, 2, 2, 70 ),
( 3, 2, 3, 60 ),
( 3, 2, 7, 30 ),
( 3, 2, 8, 30 )

INSERT INTO ServiceLog ( PeriodId, ClientId, ContractorId, CreatedDate )
VALUES
(2,1,1, GETDATE()),
(2,1,4, GETDATE()),
(2,2,2, GETDATE()),
(2,2,5, GETDATE()),
(2,2,6, GETDATE()),
(2,3,2, GETDATE()),
(2,3,3, GETDATE()),
(1,3,7, GETDATE())


INSERT UnitDetail( ServiceLogId, PlaceOfServiceId, Unit, DateOfService, Modifiers )
VALUES  
( 1,1,8,'2022/01/24','BA'),
( 1,2,8,'2022/01/26','BA'),
( 1,1,8,'2022/01/28','BA'),
( 1,2,8,'2022/01/30','BA'),
( 2,1,8,'2022/01/25','BA'),
( 2,2,8,'2022/01/27','BA'),
( 2,1,8,'2022/01/29','BA'),
( 3,1,8,'2022/01/24','BA'),
( 3,2,8,'2022/01/26','BA'),
( 3,1,8,'2022/01/28','BA'),
( 4,2,8,'2022/01/25','BA'),
( 4,1,8,'2022/01/26','BA'),
( 5,2,8,'2022/01/27','BA'),
( 5,1,8,'2022/01/29','BA'),
( 6,1,8,'2022/01/24','BA'),
( 6,2,8,'2022/01/26','BA'),
( 7,1,8,'2022/01/28','BA'),
( 7,2,8,'2022/01/25','BA'),
( 8,1,8,'2022/01/16','BA'),
( 8,2,8,'2022/01/17','BA'),
( 8,1,8,'2022/01/19','BA')






