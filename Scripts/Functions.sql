--DROP PROCEDURE CalculateDiscount
--ALTER PROCEDURE CalculateDiscount(@p_idOrder INT, @p_idCustomer INT)
CREATE PROCEDURE CalculateDiscount(@p_idOrder INT, @p_idCustomer INT)
AS
	DECLARE @v_suma FLOAT;
	DECLARE @v_discount FLOAT;
	DECLARE @v_currOrderCost FLOAT;

BEGIN TRANSACTION CalculateDiscount;
	BEGIN TRY
		print('Start Commit block');
			IF EXISTS(SELECT idOrder FROM [ORDER] WHERE idOrder = @p_idOrder AND idCustomer =  @p_idCustomer)		 
			BEGIN
		    --1.  Zjisti celkovou utratu v aktualnim roce
			SELECT @v_suma = SUM(cost) FROM [ORDER] 
			WHERE idCustomer = @p_idCustomer AND YEAR(orderDate) = YEAR(CURRENT_TIMESTAMP)
			GROUP BY idCustomer;

			--2. Vypocita slevu pro objednavku
			IF (@v_suma >= 100000)
				SET @v_discount = 10;
			ELSE
				SET @v_discount = FLOOR(@v_suma/10000);
			print('Vypoctena sleva je: ' + CAST (@v_discount AS VARCHAR));	

			--4. Zjistime cenu objednavky

			SELECT @v_currOrderCost = cost FROM [ORDER] WHERE idOrder = @p_idOrder AND idCustomer = @p_idCustomer;
			print('Aktualni cena je: ' + CAST (@v_currOrderCost AS VARCHAR));	

			--5. Vypocitame cenu se slevou 
			SET @v_currOrderCost = @v_currOrderCost - (@v_currOrderCost * (@v_discount /100))
			print('Nova cena je: ' + CAST (@v_currOrderCost AS VARCHAR));	
			
			--6.UPDATE objednavky
			UPDATE [ORDER] 	
			SET  discount = @v_discount, cost = @v_currOrderCost
			WHERE idOrder = @p_idOrder AND idCustomer = @p_idCustomer; 
			print(' Commit block Executed');
			COMMIT
			END;
			ELSE
			BEGIN
				 RAISERROR ('Nebyl nalezen zadny zaznam', 15,1);
			END;
		--COMMIT;
	END TRY
	BEGIN CATCH
		print('Rollback block');
		ROLLBACK;
		THROW;
	END CATCH;


GO

--DROP FUNCTION getProductCostFromHistory
CREATE FUNCTION getProductCostFromHistory(@p_idproduct INT, @p_dateInvoice Date)
--ALTER FUNCTION getProductCostFromHistory(@p_idproduct INT, @p_dateInvoice Date)
	RETURNS FLOAT
AS
BEGIN
 -------DECLARE---------
 DECLARE @v_result FLOAT;
 DECLARE @v_cost FLOAT;
 ----------SET---------
 SET @v_result = -1; 
 
 SELECT  @v_cost = ch.COST FROM ProductCostHistory ch
 WHERE ch.idProduct = @p_idproduct AND (@p_dateInvoice BETWEEN ch.starDate AND ch.endDate);

 IF @v_cost IS NOT NULL
	SET @v_result = @v_cost;
 return @v_result; 
END;


GO

--DROP FUNCTION existPRoductInCostHistory
CREATE FUNCTION existPRoductInCostHistory(@p_idproduct INT)
--ALTER FUNCTION existPRoductInCostHistory(@p_idproduct INT)
	RETURNS BIT
AS
BEGIN
	DECLARE @v_counthistory INT ;
	DECLARE @v_bit BIT ;
	SELECT @v_counthistory = COUNT(*) FROM ProductCostHistory WHERE idProduct = @p_idproduct;    
	IF @v_counthistory = 0
		SET @v_bit = 0
	ELSE
		SET @v_bit = 1

	return @v_bit;
END;

GO

--DROP PROCEDURE checkCountOrder
CREATE PROCEDURE checkCountOrder(@p_idOrder INT, @p_calcCost Float OUTPUT)
--ALTER PROCEDURE checkCountOrder(@p_idOrder INT, @p_calcCost Float OUTPUT))
AS
	DECLARE @v_idproduct INT;
	DECLARE @v_costInvoice FLOAT;

	DECLARE @v_historyCost FLOAT;
	DECLARE @v_amountProduct INT;
	DECLARE @v_ProductPrice FLOAT;

	DECLARE @v_invoiceDate DATE;
	DECLARE @v_productHasHistory BIT;
	DECLARE @v_invoiceCostFromInvoiceTable FLOAT;

	DECLARE @v_deliveryID INT;
	DECLARE @v_deliveryCost FLOAT;

	DECLARE @v_discount FLOAT;

BEGIN
	--print ('Startuji propocet. ');
	
	SET @v_costInvoice = 0;


	--1. Kurzor na prochazeni produktu z detailu objednavky
	DECLARE c_cursor CURSOR FOR
		SELECT  pr.idproduct, ord.orderDate
			FROM [Order]  ord
				JOIN OrderDetail pr ON pr.idOrder = ord.idOrder
			WHERE ord.idOrder = @p_idOrder;	

	
	--2. Zjisteni IDdelivery, slevy a COSTdelivery
	SELECT @v_deliveryID = idDelivery, @v_discount = discount FROM [ORDER] WHERE idOrder = @p_idOrder;
	SELECT @v_deliveryCost = cost FROM TypeDelivery WHERe idDelivery = @v_deliveryID;
	

	OPEN c_cursor;
	FETCH NEXT FROM c_cursor INTO @v_idproduct, @v_invoiceDate;
	WHILE (@@FETCH_STATUS=0)
		BEGIN
			SET @v_ProductPrice = -1;
			--print cast(@v_idproduct as varchar) + '  ' + cast(@v_invoiceDate as varchar);	

			--OVERENI ZDA PRODUCT MA HISTORII CEN
			SET @v_productHasHistory =  dbo.existPRoductInCostHistory(@v_idproduct);
			IF @v_productHasHistory = 1	
				BEGIN
					--print @v_productHasHistory;
					SET @v_ProductPrice = dbo.getProductCostFromHistory(@v_idproduct, @v_invoiceDate);
					--print 'Product ma zaznam v historii, ale ne v danem intervalu casu.  ' + cast(@v_ProductPrice as varchar);
				END

			--Jestli se cena rovna -1 tak produkct nema cenu v daném intervalu
			-- JE NUTNE VYBRAT PRVKY Z AKTUALNI CENY
			IF (@v_ProductPrice = -1	)
				BEGIN
					SELECT @v_ProductPrice = pr.currentCost FROM Product pr WHERE pr.IDproduct = @v_idproduct;
				END;
			
			--Ziska pro konkretni produkt konkretni mnozstvi
			SELECT @v_amountProduct = pr.quantity FROM OrderDetail pr 
			WHERE pr.IDproduct = @v_idproduct AND  pr.idOrder = @p_idOrder;
				 
			--CENA FAKTURY
			SET @v_costInvoice = @v_costInvoice + (@v_ProductPrice * @v_amountProduct);	
			
			FETCH NEXT FROM c_cursor INTO @v_idproduct, @v_invoiceDate;
		END;		
	CLOSE c_cursor;
	DEALLOCATE c_cursor;
	--print 'Cena produktu pred dopravou ' + cast(@v_costInvoice AS varchar); 
	SET @v_costInvoice = @v_costInvoice + @v_deliveryCost;
	--print 'Cena objednavky s dobravou ' + cast(@v_costInvoice AS varchar); 	
	IF @v_discount IS NOT NULL  OR @v_discount > 0
	SET @v_costInvoice = @v_costInvoice - (@v_costInvoice*(@v_discount/100)) ;
	print 'Cena objednavky s dopravou a slevou ' + cast(@v_costInvoice AS varchar); 
	-- bude tadz return..""
	
	SET @p_calcCost = @v_costInvoice;
END;
	

GO


--DROP PROCEDURE CreateCustomer
CREATE PROCEDURE CreateCustomer(
--ALTER PROCEDURE CreateCustomer(
	@p_email VARCHAR(100), @p_password VARCHAR(300), 
	@p_fname VARCHAR(100) ,@p_lname  VARCHAR(100),
	@p_company VARCHAR(100) = null,@p_phone  VARCHAR(50)= null
)
AS
	DECLARE @v_tempID INT;
 
BEGIN TRANSACTION CreateCustomerTran;
        BEGIN TRY			
				INSERT INTO CUSTOMER(fname, lname, company, phone) VALUES(@p_fname, @p_lname,@p_company, @p_phone);
				SELECT @v_tempID = SCOPE_IDENTITY();
				--print('Vytvorene ID je: ' + CAST(@v_tempID as VARCHAR));
				INSERT INTO	Login (idCustomer, email, password)	VALUES (@v_tempID, @p_email, @p_password)				
				--print(' ENDCommit block');
            COMMIT
        END TRY
        BEGIN CATCH
                --rint('Rollback bloc');
				THROW
                ROLLBACK
        END CATCH;


GO

--drop TYPE createOrderTable
CREATE TYPE createOrderTable AS TABLE 
( 
  idProduct INT,
  numProduct INT 
);

GO

--drop PROCEDURE createOrder
 CREATE PROCEDURE createOrder(
--ALTER PROCEDURE createOrder(
@tableProduct createOrderTable READONLY, @p_idCustomer INT, 
@p_adress VARCHAR(100), @p_city VARCHAR(100), @p_postalcode VARCHAR(20),@p_idDelivery INT)
AS	

	DECLARE @v_currdate Datetime;
		SET @v_currdate = CURRENT_TIMESTAMP;
	DECLARE @v_idOrder INT;
	DECLARE @v_costDelivery FLOAT;
	DECLARE @v_calcSumOrder FLOAT;


    --PROMENNE PRO CURSOR VLOZENYCH PRODUKTU
	DECLARE @v_idproduct INT;
	DECLARE @v_num INT;
	DECLARE c_cursor CURSOR FOR SELECT * FROM @tableProduct;
BEGIN
	--###################################################################--
	--Zjisteni ceny dopravy
	SELECT @v_costDelivery = cost FROM TypeDelivery WHERE idDelivery = @p_idDelivery;

	--Vytvoreni zaznamu v tabulce order
	INSERT INTO [ORDER](idCustomer, idDelivery, orderDate, cost, adress,city,postalcode)
	VALUES(@p_idCustomer, @p_idDelivery, @v_currdate, @v_costDelivery, @p_adress, @p_city, @p_postalcode)
	 
	 --Zjisteni noveho ID - order
	SELECT @v_idOrder = SCOPE_IDENTITY();

	--###################################################################--


	--###################################################################--
	--V cyklu provadene vkladani dat z tabulky do do propojovaci tabulky OrderDetail

	OPEN c_cursor;
	FETCH NEXT FROM c_cursor INTO @v_idproduct, @v_num;
	WHILE (@@FETCH_STATUS=0)
		BEGIN
			--print ('Product: ' + cast(@v_idproduct as varchar) + 'Pocet: ' +  cast(@v_num as varchar) ) ;
			
			INSERT INTO OrderDetail(idOrder, idProduct, quantity) VALUES(@v_idOrder, @v_idproduct, @v_num)
			
			FETCH NEXT FROM c_cursor INTO @v_idproduct, @v_num;
		END;
	
    CLOSE c_cursor;
	DEALLOCATE c_cursor;	
	--###################################################################--


	--###################################################################--	
	SELECT @v_calcSumOrder = SUM(de.quantity * pr.currentCost)
	FROM [ORDER] ordr
	JOIN Orderdetail de ON de.idOrder = ordr.idOrder
	JOIN Product pr On pr.idProduct = de.idProduct
	WHERE ordr.idOrder = @v_idOrder
	GROUP BY ordr.idOrder;
	--###################################################################--
	UPDATE [ORDER] SET cost = cost + @v_calcSumOrder, status = 'prijata' WHERE idOrder = @v_idOrder;
	--###################################################################--

	print('createOrder');
END;


GO


--DROP PROCEDURE createWatchDog
CREATE PROCEDURE createWatchDog(@p_idCustomer INT, @p_idProduct INT, @p_cost FLOAT)
--ALTER PROCEDURE createWatchDog(@p_idCustomer INT, @p_idProduct INT, @p_cost FLOAT)
AS
	DECLARE @v_date DateTime;
BEGIN TRANSACTION createWatchDogTran;
	BEGIN TRY
		SET @v_date = CURRENT_TIMESTAMP;
		INSERT INTO WatchDog(idCustomer, idProduct, createDate, cost, notify)
		VALUES (@p_idCustomer, @p_idProduct, @v_date, @p_cost, 0 );
		COMMIT
	END TRY
	BEGIN CATCH 
			THROW
			ROLLBACK
END CATCH


GO

--DROP TRIGGER BackupOldCostProduct
--ALTER TRIGGER BackupOldCostProduct
CREATE TRIGGER BackupOldCostProduct

ON Product
AFTER UPDATE 
AS 
	DECLARE @v_beforeCost FLOAT;
	DECLARE @v_currProduct INT	
	DECLARE @v_newCost FLOAT;
	DECLARE @v_lastDate DateTime;
	DECLARE @v_createProduct DateTime;
BEGIN
	print('trigger backupOldCostProduct');
	IF UPDATE (currentCost)
		BEGIN			
			--SELECT @v_currProduct=idProduct, @v_beforeCost = currentCost FROM DELETED;			  
			DECLARE c_deleted CURSOR FOR SELECT idproduct,currentCost FROM DELETED;

			OPEN c_deleted;
			FETCH NEXT FROM c_deleted INTO @v_currProduct, @v_beforeCost;
			WHILE (@@FETCH_STATUS=0)
			BEGIN
				--print('bylo spusteno na sloupci cost');
				--print('ID ' + cast(@v_currProduct as varchar) + '  OldCost: ' + cast(@v_beforeCost as varchar));
				SELECT TOP 1 @v_lastDate = endDate FROM ProductCostHistory WHERE idproduct = @v_currProduct ORDER BY endDate DESC;	
					IF 	@v_lastDate IS NOT NULL
						BEGIN
							--print('NOT NULL datum: ');
							--print('Posledni datum: ' +  cast(@v_lastDate as VARCHAR));
							SELECT @v_lastDate = DATEADD(mi, 1, @v_lastDate );
							INSERT INTO ProductCostHistory(idProduct, starDate, endDate, cost) VALUES (@v_currProduct, @v_lastDate, CURRENT_TIMESTAMP, @v_beforeCost);
						END;
					ELSE
						BEGIN
							--print('IS NULL datum: ');
							--print('Posledni datum: ' +  cast(@v_lastDate as VARCHAR));
							/*
							 MUSIM ZJISTIT datum vlozeni produktu do DB a nasledne tohle pouzit 
							 TEDKA POUZIJI NEJAKOU PROMENOU
							*/
							 SELECT @v_createProduct = create_at FROM Product Where idProduct = @v_currProduct;
							 --SET @v_createProduct = CURRENT_TIMESTAMP - 2000;
							INSERT INTO ProductCostHistory(idProduct, starDate, endDate, cost) VALUES (@v_currProduct, @v_createProduct, CURRENT_TIMESTAMP, @v_beforeCost);
						END;
				--print('Posledni datum: ' +  cast(@v_lastDate as VARCHAR));
				FETCH NEXT FROM c_deleted INTO @v_currProduct, @v_beforeCost;
			END
			CLOSE c_deleted;
			DEALLOCATE c_deleted;			
		END 
END;


GO 
--NUTNE TOHO NASTAVENI

EXEC sp_settriggerorder @triggername= 'BackupOldCostProduct', @order='First', @stmttype = 'UPDATE';


GO

--DROP TRIGGER startWatchDogTrigger
--ALTER TRIGGER startWatchDogTrigger
CREATE TRIGGER startWatchDogTrigger
ON Product
AFTER UPDATE 
AS 
	DECLARE @v_idProduct INT;	
	DECLARE @v_newCost FLOAT;
	DECLARE @v_idCustomer INT;
	DECLARE @v_watchDogCost FLOAT;	
BEGIN
	/*Zjisti nove vlozenou cenu po UPDATU, zjisti take idproduktu, ktery byl updatovan*/
	--SELECT @v_idProduct = idProduct, @v_newCost = currentCost FROM INSERTED; --prepsat na cursor 
	DECLARE c_inserted CURSOR FOR SELECT idProduct, currentCost FROM INSERTED; --primarni cyklus
	OPEN c_inserted;
	FETCH NEXT FROM c_inserted INTO @v_idProduct, @v_newCost;
	WHILE (@@FETCH_STATUS=0)
		BEGIN	
			--PRINT('PRODUCT ID ' + CAST(@v_idProduct AS VARCHAR));	
			--PRINT('NOVA CENA  ' + CAST(@v_newCost AS VARCHAR));	

			DECLARE c_watchdog CURSOR FOR SELECT idCustomer, cost FROM WatchDog WHERE idProduct = @v_idProduct;
			OPEN c_watchdog;
			FETCH NEXT FROM c_watchdog INTO @v_idCustomer, @v_watchDogCost;
			WHILE (@@FETCH_STATUS=0)
				BEGIN
					--PRINT('Zakaznik ID ' + CAST(@v_idCustomer AS VARCHAR));	
					--PRINT('Cena spusteni ' + CAST(@v_watchDogCost AS VARCHAR));					
					IF (@v_watchDogCost >= @v_newCost)
						BEGIN
							UPDATE WatchDog SET notify = 1 WHERE idCustomer = @v_idCustomer AND idProduct = @v_idProduct;
						END
					ELSE
						BEGIN
							FETCH NEXT FROM c_watchdog INTO @v_idCustomer, @v_watchDogCost;
						END
					FETCH NEXT FROM c_watchdog INTO @v_idCustomer, @v_watchDogCost;	
				END;
			CLOSE c_watchdog;
			DEALLOCATE c_watchdog;
		FETCH NEXT FROM c_inserted INTO @v_idProduct, @v_newCost;

		END;
	CLOSE c_inserted;
	DEALLOCATE c_inserted;	
	print('startWatchDogTrigger')
END;

GO

--Zkompilovat jako PRVNI
CREATE FUNCTION CheckUserLoginExist(@p_email VARCHAR(100), @p_password VARCHAR(300))
--ALTER FUNCTION CheckUserLoginExist(@p_email VARCHAR(100), @p_password VARCHAR(300))
RETURNS INT
AS	
BEGIN	
	DECLARE @v_existLogin INT;
	DECLARE @v_foundLogin INT;
	SET @v_existLogin = -1; --Nebylo nalezeno
	SELECT @v_foundLogin = COUNT(*) FROM LOGIN WHERE email=@p_email AND password=@p_password;
	IF @v_foundLogin = 1
		BEGIN
			SET @v_existLogin=@v_foundLogin;
			--SELECT @v_loginCustomerID = idCustomer FROM LOGIN WHERE email=@p_email AND password=@p_password;
			--UPDATE Customer SET lastvisit = CURRENT_TIMESTAMP WHERE idCustomer=@v_loginCustomerID;
			--EXEC UpdateCustomerLastVisit @v_loginCustomerID;
		END
	RETURN @v_existLogin;
END;

GO

--Zkompilovat jako DRUHE
CREATE PROCEDURE UserLoginProcedure(@p_email VARCHAR(100), @p_password VARCHAR(300), @p_status INT OUTPUT, @p_idcustomer INT OUTPUT )
--ALTER PROCEDURE UserLoginProcedure(@p_email VARCHAR(100), @p_password VARCHAR(300), @p_status INT OUTPUT, @p_idcustomer INT OUTPUT  )
AS	
	DECLARE @v_loginCustomerID INT;
BEGIN
	SET @p_idcustomer = -1;
	SET @p_status = dbo.CheckUserLoginExist(@p_email,@p_password);
	IF(@p_status = 1)
	BEGIN
		SELECT @p_idcustomer = idCustomer FROM LOGIN WHERE email=@p_email AND password=@p_password;
		UPDATE Customer SET lastvisit = CURRENT_TIMESTAMP WHERE idCustomer=@p_idcustomer;
	END
END





