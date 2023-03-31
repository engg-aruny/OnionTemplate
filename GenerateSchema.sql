create database PingPongDB

GO

CREATE TABLE [dbo].[Manufacturers] (
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(MAX) NOT NULL,
    [Country] NVARCHAR(MAX) NOT NULL
)
GO
CREATE TABLE [dbo].[PingPong] (
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Model] NVARCHAR(MAX) NOT NULL,
    [ManufactureDate] DATETIME NOT NULL,
    [ManufacturerId] INT NOT NULL,
    CONSTRAINT [FK_PingPong_Manufacturers_ManufacturerId] 
        FOREIGN KEY ([ManufacturerId]) REFERENCES [dbo].[Manufacturers]([Id])
)

