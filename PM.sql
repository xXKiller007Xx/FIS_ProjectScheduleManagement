create database ProjectManagement
use ProjectManagement
go
create table Status
(
	Id bigint primary key identity(1,1),
	Code nvarchar(50) not null,
	Name nvarchar(500) not null,
)
create table Project
(
	Id bigint primary key identity(1,1),
	Code nvarchar(50) not null,
	Name nvarchar(500) not null,
	Description nvarchar(1000),
	StartDate datetime not null,
	FinishDate datetime not null,
	Percentage int check(Percentage>=0 and Percentage<=100) default 0 not null,
	CreatedAt datetime not null,
	UpdatedAt datetime not null,
	DeletedAt datetime,
	Used bit,
	StatusId bigint foreign key references Status(Id)
)
create table TaskType
(
	Id bigint primary key identity(1,1),
	Code nvarchar(50) not null,
	Name nvarchar(500) not null,
	Description nvarchar(1000),
	CreatedAt datetime not null,
	UpdatedAt datetime not null,
	DeletedAt datetime,
	StatusId bigint foreign key references Status(Id)
)
create table Task
(
	Id bigint primary key identity(1,1),
	Code nvarchar(50) not null,
	Name nvarchar(500) not null,
	Description nvarchar(1000),
	StartDate datetime not null,
	FinishDate datetime not null,
	Percentage int check(Percentage>=0 and Percentage<=100) default 0 not null,
	ProjectId bigint foreign key references Project(Id),
	TaskTypeId bigint foreign key references TaskType(Id),
	CreatedAt datetime not null,
	UpdatedAt datetime not null,
	DeletedAt datetime,
	Used bit,
	StatusId bigint foreign key references Status(Id)
)
create table Job
(
	Id bigint primary key identity(1,1),
	Code nvarchar(50) not null,
	Name nvarchar(500) not null,
)
create table Gender
(
	Id bigint primary key identity(1,1),
	Code nvarchar(50) not null,
	Name nvarchar(500) not null
)
create table Employee
(
	Id bigint primary key identity(1,1),
	Code nvarchar(50) not null,
	Name nvarchar(500) not null,
	JobId bigint foreign key references Job(Id),
	GenderId bigint foreign key references Gender(Id),
	DateOfBirth datetime not null,
	Address nvarchar(500) not null,
	Phone nvarchar(10) not null,
	Email nvarchar(500) not null,
	CreatedAt datetime not null,
	UpdatedAt datetime not null,
	DeletedAt datetime,
	Used bit,
	StatusId bigint foreign key references Status(Id)
)
create table TaskEmployeeMapping
(
	TaskId bigint foreign key references Task(Id) not null,
	EmployeeId bigint foreign key references Employee(Id) not null,
	primary key (TaskId, EmployeeId)
)