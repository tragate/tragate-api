USE [master]
GO
CREATE DATABASE TragateDB
GO
use TragateDB
GO
create table [User]
(
	Id int identity
		primary key,
	Password varchar(100)  null,
	Email varchar(100)  null,
	ExternalUserId varchar(100),
	RegisterTypeId tinyint,
	UserTypeId tinyint not null,
	StatusId tinyint,
	CreatedDate datetime not null,
	EmailVerified bit default 0 not null,
	FullName varchar(max),
	ProfileBigImagePath varchar(100),
	ProfileSmallImagePath varchar(100),
	Salt varchar(200)
)
go

create unique index User_Id_uindex
	on [User] (Id)
go


create table TragateDB.dbo.CompanyAdmin
(
	Id int identity
		primary key,
	PersonId int not null
		constraint CompanyAdmin_Person_Id_fk
			references User,
	CompanyId int not null
		constraint CompanyAdmin_Company_Id_fk
			references User,
	CompanyAdminRoleId tinyint not null,
	StatusId tinyint not null,
	CreatedDate datetime not null
)
go



create table TragateDB.dbo.Parameter
(
	Id int identity
		constraint PK_Parameter
			primary key,
	ParameterGroupName nvarchar(max),
	ParameterType nvarchar(max),
	ParameterCode tinyint,
	ParameterValue1 nvarchar(max),
	ParameterValue2 nvarchar(max),
	ParameterValue3 nvarchar(max),
	StatusId tinyint,
	CreatedDate datetime
)
go




CREATE TABLE [dbo].[StoredEvent](
	Id int identity,
	[AggregateId] int NOT NULL,
	[Data] [nvarchar](max) NULL,
	[Action] [varchar](100) NULL,
	[CreationDate] [datetime2](7) NOT NULL,
	[User] [nvarchar](max) NULL,
 CONSTRAINT [PK_StoredEvent] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

CREATE TABLE TragateDB.dbo.Category
(
    Id INT NOT NULL IDENTITY,
    Title VARCHAR(100) NOT NULL,
    Slug VARCHAR(100) NOT NULL,
    ParentId INT,
    StatusId TINYINT NOT NULL,
    MetaKeyword VARCHAR(100),
    MetaDescription VARCHAR(MAX),
    CreatedUserId INT NOT NULL,
    CreatedDate DATETIME NOT NULL
)
CREATE UNIQUE INDEX Category_Id_uindex ON TragateDB.dbo.Category (Id)
GO
CREATE TABLE TragateDB.dbo.SystemAdmin
(
    Id INT NOT NULL IDENTITY,
    UserId INT NOT NULL,
    SystemAdminRoleId TINYINT NOT NULL,
    StatusId TINYINT NOT NULL,
    CreatedDate DATETIME NOT NULL
)
CREATE UNIQUE INDEX SystemAdmin_Id_uindex ON TragateDB.dbo.SystemAdmin (Id)
go
create table TragateDB.dbo.SystemAdmin
(
	Id int identity,
	UserId int not null,
	SystemAdminRoleId tinyint not null,
	StatusId tinyint not null,
	CreatedDate datetime not null
)
go

create unique index SystemAdmin_Id_uindex
	on SystemAdmin (Id)
go

