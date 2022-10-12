
INSERT [Procedure] (Name, Rate) VALUES ('Undefined', 0)

CREATE TABLE [dbo].[SubProcedure] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Rate] nvarchar(max)  NOT NULL,
    [ProcedureId] int  NOT NULL
);
GO

ALTER TABLE [dbo].[SubProcedure]
ADD CONSTRAINT [PK_SubProcedure]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

ALTER TABLE [dbo].[SubProcedure]
ADD CONSTRAINT [FK_ProcedureSubProcedure]
    FOREIGN KEY ([ProcedureId])
    REFERENCES [dbo].[Procedure]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

CREATE INDEX [IX_FK_ProcedureSubProcedure]
ON [dbo].[SubProcedure]
    ([ProcedureId]);
GO

INSERT [SubProcedure] (ProcedureId, Name, Rate)
VALUES
(4, 'Undefined', '0.00'),
(1, 'H2019-97155','19.05'),
(1, 'H2019-97156','19.05'),
(1, 'H2019-97153','19.05'),
(2, 'H2012-97155HN','15.24'),
(2, 'H2012-97156HN','15.24'),
(2, 'H2012-97153','15.24'),
(3, 'H2014-97153','12.19');
GO

ALTER TABLE [dbo].[UnitDetail] ADD [SubProcedureId] int NOT NULL DEFAULT 1;
GO

ALTER TABLE [dbo].[UnitDetail]
ADD CONSTRAINT [FK_SubProcedureUnitDetail]
    FOREIGN KEY ([SubProcedureId])
    REFERENCES [dbo].[SubProcedure]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

CREATE INDEX [IX_FK_SubProcedureUnitDetail]
ON [dbo].[UnitDetail]
    ([SubProcedureId]);
GO

