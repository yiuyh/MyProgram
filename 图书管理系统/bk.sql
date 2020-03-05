/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2005                    */
/* Created on:     2019/12/17 15:54:47                          */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('"Order"') and o.name = 'FK_ORDER_ADD_CUSTOMER')
alter table "Order"
   drop constraint FK_ORDER_ADD_CUSTOMER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Purchase') and o.name = 'FK_PURCHASE_PURCHASE_BOOK')
alter table Purchase
   drop constraint FK_PURCHASE_PURCHASE_BOOK
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Purchase') and o.name = 'FK_PURCHASE_PURCHASE2_CUSTOMER')
alter table Purchase
   drop constraint FK_PURCHASE_PURCHASE2_CUSTOMER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Support') and o.name = 'FK_SUPPORT_SUPPORT_SUPPORTE')
alter table Support
   drop constraint FK_SUPPORT_SUPPORT_SUPPORTE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Support') and o.name = 'FK_SUPPORT_SUPPORT2_BOOK')
alter table Support
   drop constraint FK_SUPPORT_SUPPORT2_BOOK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Book')
            and   type = 'U')
   drop table Book
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Customer')
            and   type = 'U')
   drop table Customer
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Manager')
            and   type = 'U')
   drop table Manager
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('"Order"')
            and   name  = 'Add_FK'
            and   indid > 0
            and   indid < 255)
   drop index "Order".Add_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"Order"')
            and   type = 'U')
   drop table "Order"
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Purchase')
            and   name  = 'Purchase2_FK'
            and   indid > 0
            and   indid < 255)
   drop index Purchase.Purchase2_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Purchase')
            and   name  = 'Purchase_FK'
            and   indid > 0
            and   indid < 255)
   drop index Purchase.Purchase_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Purchase')
            and   type = 'U')
   drop table Purchase
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Support')
            and   name  = 'Support2_FK'
            and   indid > 0
            and   indid < 255)
   drop index Support.Support2_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Support')
            and   name  = 'Support_FK'
            and   indid > 0
            and   indid < 255)
   drop index Support.Support_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Support')
            and   type = 'U')
   drop table Support
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Supporter')
            and   type = 'U')
   drop table Supporter
go

/*==============================================================*/
/* Table: Book                                                  */
/*==============================================================*/
create table Book (
   Book_no              int                  not null,
   name                 varchar(20)          null,
   author               varchar(20)          null,
   publish              varchar(20)          null,
   price                float                null,
   origin_price         float                null,
   Stock				int,
   Book_category		varchar(20),
   constraint PK_BOOK primary key nonclustered (Book_no)
)
go

alter table Book add Stock int CHECK (Stock >= 0)
alter table Book add Book_category varchar(20)


/*==============================================================*/
/* Table: Customer                                              */
/*==============================================================*/
create table Customer (
   Cus_uername          varchar(20)          not null,
   Cus_password         varchar(20)          null,
   Cus_nickname         varchar(20)          null,
   Cus_sex              varchar(2)           null,
   Cus_age              int                  null,
   Cus_tel              varchar(20)          null,
   vip                  int                  null,
   Cus_remark           varchar(20)			null,
   Cus_address          varchar(20)          null,
   constraint PK_CUSTOMER primary key nonclustered (Cus_uername)
)
go



/*==============================================================*/
/* Table: Manager                                               */
/*==============================================================*/
create table Manager (
   M_username           varchar(20)          not null,
   M_password           varchar(20)          null,
   M_tel                varchar(20)          null,
   M_nickname           varchar(20)          null,
   M_address            varchar(20)          null,
   M_remark             varchar(20)          null,
   constraint PK_MANAGER primary key nonclustered (M_username)
)
go

insert into Manager(M_username,M_password,M_tel,M_nickname,M_address,M_remark)
values('xz','123','123456','С��','���ݴ�ѧ','��')

/*==============================================================*/
/* Table: "Order"                                               */
/*==============================================================*/
create table "Order" (
   Order_no             varchar(8)           not null,
   Cus_uername          varchar(20)          null,
   date                 datetime             null,
   status               int                  null,
   Book_no              int                  null,
   sellnum				int					null,
   constraint PK_ORDER primary key nonclustered (Order_no)
)
go

insert into "Order"(Order_no,Cus_uername,date,status,Book_no,sellnum) values('#001','xh','2019-09-09',0,'1',1);

alter table "Order" add sellnum int CHECK (sellnum >= 0)




/*==============================================================*/
/* Index: Add_FK                                                */
/*==============================================================*/
create index Add_FK on "Order" (
Cus_uername ASC
)
go

/*==============================================================*/
/* Table: Purchase                                              */
/*==============================================================*/
create table Purchase (
   Book_no              int                  not null,
   Cus_uername          varchar(20)          not null,
   constraint PK_PURCHASE primary key (Book_no, Cus_uername)
)
go

/*==============================================================*/
/* Index: Purchase_FK                                           */
/*==============================================================*/
create index Purchase_FK on Purchase (
Book_no ASC
)
go

/*==============================================================*/
/* Index: Purchase2_FK                                          */
/*==============================================================*/
create index Purchase2_FK on Purchase (
Cus_uername ASC
)
go

/*==============================================================*/
/* Table: Support                                               */
/*==============================================================*/
create table Support (
   Sup_no               varchar(20)          not null,
   Book_no              int                  not null,
   constraint PK_SUPPORT primary key (Sup_no, Book_no)
)
go

/*==============================================================*/
/* Index: Support_FK                                            */
/*==============================================================*/
create index Support_FK on Support (
Sup_no ASC
)
go

/*==============================================================*/
/* Index: Support2_FK                                           */
/*==============================================================*/
create index Support2_FK on Support (
Book_no ASC
)
go

/*==============================================================*/
/* Table: Supporter                                             */
/*==============================================================*/
create table Supporter (
   Sup_no               varchar(20)          not null,
   Sup_name             varchar(20)          null,
   Sup_address          varchar(20)          null,
   Sup_tel              varchar(20)          null,
   constraint PK_SUPPORTER primary key nonclustered (Sup_no)
)
go

alter table "Order"
   add constraint FK_ORDER_ADD_CUSTOMER foreign key (Cus_uername)
      references Customer (Cus_uername)
go

alter table Purchase
   add constraint FK_PURCHASE_PURCHASE_BOOK foreign key (Book_no)
      references Book (Book_no)
go

alter table Purchase
   add constraint FK_PURCHASE_PURCHASE2_CUSTOMER foreign key (Cus_uername)
      references Customer (Cus_uername)
go

alter table Support
   add constraint FK_SUPPORT_SUPPORT_SUPPORTE foreign key (Sup_no)
      references Supporter (Sup_no)
go

alter table Support
   add constraint FK_SUPPORT_SUPPORT2_BOOK foreign key (Book_no)
      references Book (Book_no)
go





--xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

alter table "Order"
   add constraint FK_ORDER_ADD2_CUSTOMER foreign key (Book_no)
      references Book (Book_no)
go

alter table "Order"
	add constraint CK_ORD2 CHECK("status" in (0, 1))
go

alter table Customer
	add constraint CK_CUS CHECK(Cus_sex in ('��','Ů'))
go

alter table Customer
	add constraint CK_CUS2 CHECK(vip in (0,1))
go


--����鼮
insert into Book("name",author,publish,Book_category,price,origin_price,Stock) 
values 
('Ų����ɭ��','���ϴ���','�Ϻ����ĳ�����','��ѧ',21,10,1),
('���ߵĿ���','���ϴ���','�Ϻ����ĳ�����','��ѧ',29,15,1),
('���μ�','��ж�','�������������','��ѧ',49,30,1),
('ˮ䰴�','ʩ���� ','������ѧ������','��ѧ',53,30,1),
('��������','�޹���','������ѧ������','��ѧ',25,10,1),
('��¥��','��ѩ��','������ѧ������','��ѧ',38,20,1),
('����־','����','�л����','��ѧ',55,40,1)
select * from Book

insert into Book("name",author,publish,Book_category,price,origin_price,Stock) 
values ('�㷨���','���ϴ���','�Ϻ����ĳ�����','��ѧ',21,10,1)

--��ӹ�Ӧ��
insert into Supporter(Sup_no,Sup_name,Sup_address,Sup_tel)
values('#001','���ݹ�Ӧ��','����','123'),
('#002','�Ĵ���Ӧ��','�ɶ�','124'),
('#003','xx��Ӧ��','xx','125')

insert into Support(Sup_no,Book_no)
values('#001',1),
('#001',2),
('#001',3),
('#002',4),
('#002',5)

--�û���ѯ������ͼ
drop view cus_orderSerch
create view cus_orderSerch
as 
select Customer.Cus_uername,Book.name ,"Order".date, "Order".sellnum ,"Order".status
from Book,"Order",Customer
where Book.Book_no="Order".Book_no and Customer.Cus_uername="Order".Cus_uername

select * from cus_orderSerch





--�û���ѯ�鼮��ͼ
drop view cus_bookSerch
create view cus_bookSerch
as 
select Book_no'�鼮���', name'����',author'����',publish'������', price '�ۼ�', Book_category'�鼮���'
from Book

select * from cus_bookSerch


--�ṩ���ṩ�鼮��ѯ
drop view sup_Serch
create view sup_Serch
as
select  Supporter.Sup_no, Sup_name,Book.Book_no, Book.name
from Book,Support,Supporter
where Book.Book_no = Support.Book_no and Support.Sup_no = Supporter.Sup_no

select * from sup_Serch

update Book set Stock = Stock-1 where Book_no = 1


delete Book where Book_no = 2014
delete Support where Book_no = 1008

create trigger del_book
on Book
instead of delete
as
	begin
		delete Support where Book_no = (select Book_no from deleted)
		delete "Order" where Book_no = (select Book_no from deleted)
		delete Book where Book_no = (select Book_no from deleted)
	end

drop trigger del_book


create trigger del_order
on "Order"
after delete
as
	begin
		delete Purchase 
		where Book_no = (select Book_no from deleted) and
		Cus_uername = (select Cus_uername from deleted)
	end

drop trigger del_order


insert into Book("name",author,publish,Book_category,price,origin_price,Stock) 
values 
('Ų����ɭ��22','���ϴ���','�Ϻ����ĳ�����','��ѧ',21,10,1)



select * from Book
select * from Manager
select * from Customer
select * from Support
select * from "Order"
select * from Purchase
select * from "Order" where Order_no = ''
delete "Order" where "Order".Order_no = '#00315'
select Stock from Book where Book_no = 0
select Order_no from "Order" where Book_no = 0 and Cus_uername = 'xh'
update "Order" set sellnum = sellnum+1 where Order_no = ''
select Order_no from "Order" where Book_no = '' and Cus_uername = ''
update Book set Stock = Stock+1 where Book_no = 1
insert into Purchase(Book_no,Cus_uername)
values(1,'xh')

delete from Book
where name = '�㷨���'