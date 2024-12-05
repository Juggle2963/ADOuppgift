
-------------------------------------
----------------Create DB------------
-------------------------------------


create database Slutuppgift

go

create table Kunder
(
ID int primary key identity not null,
Kundnummer nvarchar(16) unique null,
Namn nvarchar(32) not null,
Adress nvarchar(32) not null,
Kontaktuppgift nvarchar(32) not null,
)
go
-----------------------
-----------------------

create table [Order]
(
ID int primary key identity not null,
Ordernummer nvarchar(16) unique not null,
Orderdatum date not null,
KunderID int references Kunder(ID) not null
)
go

-----------------

create table [Produkter]
(
ID int primary key identity not null,
Produkt nvarchar(64) unique not null,
Pris money not null,


)
go

------------------



create table Products2Order
(
OrderID int references [Order](ID) not null,
ProdukterID int references Produkter(ID) not null,
Antal int not null
unique(OrderID, ProdukterID)
)
go
-----------
--Skapa kunder---
-----------
INSERT INTO 
	Kunder (Kundnummer, Namn, Adress, Kontaktuppgift)
VALUES 
	('12345', 'Anders', 'Andersgatan 1', 'idontknow@yet.com'),
	('23456', 'Bertil', 'Bertilstreet 2', '14523-23456'),
	('34567', 'Ceasar', 'Ceasarstreet 3', 'min@mejl.nu')
go
---------------
------Skapa ordrar-----
---------------
INSERT INTO 
	[Order] (Ordernummer, Orderdatum, KunderID)
VALUES 
	('ord1', GETDATE(), 4),
	('ord2', 2023-03-11, 5),
	('ord3', 2023-01-10, 6),
	('ord5', 2024-01-03, 4),
	('ord6', 2023-03-08, 5),
	('ord7', 2023-01-10, 6)
go

INSERT INTO
	Produkter(Produkt, Pris)
VALUES
('Cykel', 500),
('Bil', 1000),
('Motorcykel', 1000),
('Helikopter', 10000),
('Flygplan', 100000)
go

INSERT INTO	
	Products2Order(OrderID, ProdukterID, Antal)
Values
	(2, 1, 2),
	(2, 3, 3),
	(2, 5, 1),
	(3, 2, 2),
	(3, 5, 1),
	(5, 1, 4),
	(5, 4, 5),
	(6, 5, 8),
	(6, 2, 1),
	(7, 3, 3)
	

	go
	-------------------------------------------------------------
	-------------------------------------------------------------
	-------Querys----Procedures------Views

	
	
	
	-----------------
	----------------
	create procedure AddKund(
	
	@Kundnummer nvarchar(16),
	@Namn nvarchar(32),
	@Adress nvarchar(32),
	@Kontaktuppgift nvarchar(32),

	@ID int output
	)
	as begin
	
	insert into 
		Kunder
	values
	(@Kundnummer, @Namn, @Adress, @Kontaktuppgift)

	set @ID = SCOPE_IDENTITY();
	end
	go

	-----
	--uppdate Kund
	------
		create procedure UpdateKund(
	@ID int,
	@Kundnummer nvarchar(16),
	@Namn nvarchar(32),
	@Adress nvarchar(32),
	@Kontaktuppgift nvarchar(32)
	)
	as begin
	update 
		Kunder
	set
	Kundnummer = @Kundnummer,
	Namn = @Namn,
	Adress = @Adress,
	Kontaktuppgift = @Kontaktuppgift
	where
		ID = @ID
	end
	go

	
	-----------------
	----Delete Kund--
	-----------------
	create procedure DeleteKund(
	@ID int)
	
	as begin

	delete from
		Kunder
	where 
		ID = @ID
	AND NOT EXISTS(
		select 1
		from [Order] as o
		where o.KunderID = @ID
		)

	 end
	 go

	 

	--------------
	---PRODUKTER--
	----ADD Product--
	-----------------

		create procedure AddProdukt(
	
	@Produkt nvarchar(16),
	@Pris money,

	@ID int output
	)
	as begin
	
	insert into 
		Produkter
	values
	(@Produkt, @Pris)

	set @ID = SCOPE_IDENTITY();
	end
	go



	select * from Produkter
	go

	-----UPDATE PRODUKT----

	create procedure UpdateProdukt(
	@ID int,
	@Produkt nvarchar(64),
	@Pris money
	)
	as begin
	update 
		Produkter
	set
	Produkt = @Produkt,
	Pris = @Pris
	where
		ID = @ID 
		and not exists
		(select 1
		from 
		Products2Order
		where
		ProdukterID = @ID
		)
	end
	go
	

	----DELETE Produkt---
		create procedure DeleteProdukt(
	@ID int)
	
	as begin

	delete from
		Produkter
	where 
		ID = @ID
	AND NOT EXISTS(
		select 1
		from Products2Order as p2o
		where p2o.ProdukterID = @ID
		)

	 end
	 go

	 
	 --------------------
	 ----------AddOrder + en produkt---
	 ---------------------
	 create procedure AddOrder(
	 @Ordernummer nvarchar(16),
	 @Orderdatum date,     
	 @KunderID int,
	 @ID int output,

	 @ProdukterId int,
	 @Antal int
	
	)

  as begin 
	begin transaction
	 begin try
		declare @OrderID int
		insert into 
			[Order](Ordernummer, Orderdatum, KunderID)
			values(@Ordernummer, @Orderdatum, @KunderID)

			set @OrderID = SCOPE_IDENTITY()
			set @ID = SCOPE_IDENTITY()
		insert into
			Products2Order(OrderID, ProdukterID, Antal)
			values(@OrderID, @ProdukterId, @Antal)

   commit transaction
	  end try
	 
	 begin catch
		rollback transaction
	 end catch
 end
	 go


	 -------------------------
	 create procedure AddProduct2Order(
	 @ID int,
	 @ProdukterID int,
	 @Antal int
	 )
as begin
	 if exists(
	 select 
		1
	 from
		[Order]
	 where
		ID = @ID
	 )

	 insert into
		Products2Order(OrderID, ProdukterID, Antal)
	values(@ID, @ProdukterID, @Antal)


	end
		



	 
	 go

-------------
--Get all orders  ---- Finns en POCO "Plain Old Class Object" för att Sqlreader ska kunna skapa en list av dessa -- POCON heter ViewOrdrar
--------------
create view Getallorders
as
 	 select 
		 k.Kundnummer,
		 k.Namn,
		 o.Ordernummer,
		 o.Orderdatum,
		 o.KunderID,  
		
		 STRING_AGG(p.Produkt +  ' x' +CAST(p2o.Antal as nvarchar)+ ' ', ', '  ) as Produkter,
	     SUM(p.Pris * p2o.Antal ) as Totalt

	 from Kunder as k
		join [Order] as o on k.ID = o.KunderID
		join Products2Order as p2o on o.ID = p2o.OrderID
		join Produkter as p on p2o.ProdukterID = p.ID

     group by
		k.Kundnummer,
		k.Namn,
		o.Ordernummer, 
		o.Orderdatum, 
		o.KunderID
	

	 go
	 ---------------------------------------------------------------------------------------------------------------------
	 --Get Order by ID -----Samma som ovan men lagt till Order ID - Används för att hämta order by ID med metod GetOrderID
	 ---------------------------------------------------------------------------------------------------------------------

	 create view GetOrderID
as
 	 select 
		
		 k.Kundnummer,
		 k.Namn,
		 o.Ordernummer,
		 o.Orderdatum,
		 o.KunderID,
		 o.ID as orderID, 
		
		 STRING_AGG(p.Produkt +  ' x' +CAST(p2o.Antal as nvarchar)+ ' ', ', '  ) as Produkter,
	     SUM(p.Pris * p2o.Antal ) as Totalt

	 from Kunder as k
		join [Order] as o on k.ID = o.KunderID
		join Products2Order as p2o on o.ID = p2o.OrderID
		join Produkter as p on p2o.ProdukterID = p.ID

     group by
		k.Kundnummer,
		k.Namn,
		o.Ordernummer, 
		o.Orderdatum, 
		o.KunderID,
		o.ID

	
	 go
	




-------------------
---UPDATE ORDER----
-------------------
create procedure UpDateOrder( --Inte testad 
@ID int,
@Ordernummer nvarchar(16),
@ProdukterID int,
@Antal int
)
as begin
	begin transaction
		begin try
	Update
		[Order]
	set
	Ordernummer = @Ordernummer
	where 
		ID = @ID

	Update
		Products2Order
	set
	Antal = @Antal
	where 
		OrderID = @ID and
		ProdukterID = @ProdukterID
	commit transaction
		end try
	begin catch
		rollback transaction
	end catch
end
go



-----------
--UPDATE Product2Order
---------- 
create procedure UpDateProduct2Order(  --inte uppdaterad, testad eller C#
@ID int,
@ProdukterID int,
@Antal int
)
as begin
	Update
		Products2Order
	set
	ProdukterID = @ProdukterID,
	@Orderdatum = @Antal

	where 
		OrderID = @ID
end

go

-----------------
	--Delete Order--
	-----------------
	create procedure DeleteOrder( 
	@ID int
	)
	as begin 

	begin transaction
		begin try

			delete from 
				Products2Order
			where
				OrderID = @ID

			delete from
				[Order]
			where
				ID = @ID

	commit transaction
		end try

		begin catch
			rollback transaction
		end catch
	end
go

 



	





	

	 
-----------------------------------------------END---------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
--------------------------------------END---------------END-------------------------------------------------------------

