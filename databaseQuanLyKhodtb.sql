create database QuanLyKho
go

use QuanLyKho
go

create table Unit
(
	Id int identity(1,1) primary key,
	DisplayName nvarchar(max)
)
go

create table Supplier
(
	Id int identity(1,1) primary key,
	DisplayName nvarchar(max),
	Address nvarchar(max),
	Phone nvarchar(20),
	Email nvarchar(200),
	MoreInfo nvarchar(max),
	ContractDate datetime
)
go

create table Customer
(
	Id int identity(1,1) primary key,
	DisplayName nvarchar(max),
	Address nvarchar(max),
	Phone nvarchar(20),
	Email nvarchar(200),
	MoreInfo nvarchar(max),
	ContractDate datetime
)
go

create table Objects
(
	Id nvarchar(128) primary key,
	DisplayName nvarchar(max),
	IdUnit int not null,
	IdSupplier int not null,
	QRCode nvarchar(max),
	BarCode nvarchar(max),

	foreign key (IdUnit) references Unit(Id),
	foreign key (IdSupplier) references Supplier(Id)
)
go

create table UserRole
(
	Id int identity(1,1) primary key,
	DisplayName nvarchar(max),
)
go

insert into UserRole(DisplayName)
values (N'Admin')

insert into UserRole(DisplayName)
values (N'Staff')


create table Users
(
	Id int identity(1,1) primary key,
	DisplayName nvarchar(max),
	UserName nvarchar(100),
	Password nvarchar(max),
	IdRole int not null,

	foreign key (IdRole) references UserRole(Id)
)
go

insert into Users(DisplayName, UserName, Password, IdRole) values(N'Thanhcute', N'admin', N'db69fc039dcbd2962cb4d28f5891aae1',1)
insert into Users(DisplayName, UserName, Password, IdRole) values(N'Nhân viên', N'staff', N'262ccc32f3017b74b1689018c348ea11',2)

insert into Users(DisplayName, UserName, Password, IdRole) values(N'Nhân viên', N'staff', N'admin',2)

create table Input
(
	Id nvarchar(128) primary key,
	DateInput datetime
)
go

create table InputInfo
(
	Id nvarchar(128) primary key,
	IdObject nvarchar(128) not null,
	IdInput nvarchar(128) not null,
	Counts int,
	InputPrice float default 0,
	OutputPrice float default 0,
	Status nvarchar(max)

	foreign key (IdObject) references Objects(Id),
	foreign key (IdInput) references Input(Id)
)
go

create table Output
(
	Id nvarchar(128) primary key,
	DateOutput datetime
)
go



create table OutputInfo
(
	Id nvarchar(128) primary key,
	IdObject nvarchar(128) not null,
	IdOutputInfo nvarchar(128) not null,
	IdCustomer int not null,
	Counts int,
	Status nvarchar(max)


	foreign key (IdObject) references Objects(Id),
	foreign key (IdOutputInfo) references Output(Id),
	foreign key (IdCustomer) references Customer(Id)
)
go