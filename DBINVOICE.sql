USE master
GO

CREATE DATABASE DBINVOICES
GO

USE DBINVOICES
GO

--DROP DATABASE DBINVOICES

CREATE TABLE [dbo].[STATUS](
	StatusId INT IDENTITY (1,1) NOT NULL,
	StatusDescription VARCHAR (20) NOT NULL,

	CONSTRAINT PK_STATUS_StatusId PRIMARY KEY (StatusId)
)

CREATE TABLE [dbo].[SUPPLIER](
	SupplierId INT IDENTITY (1,1) NOT NULL,
	SupplierName VARCHAR (30) NOT NULL,

	CONSTRAINT PK_SUPPLIER_SupplierId PRIMARY KEY (SupplierId),
)

CREATE TABLE [dbo].[PRODUCT](
	ProductId   INT IDENTITY (1,1)  NOT NULL,
	ProductName VARCHAR      (30)   NOT NULL,
	Price       DECIMAL      (18,2) NOT NULL,

	CONSTRAINT PK_PRODUCT_ProductId PRIMARY KEY (ProductId)
)

CREATE TABLE [dbo].[USER](

	UserId          INT IDENTITY (1,1) NOT NULL,
	UserName        VARCHAR      (50)  NOT NULL,
	UserLastName    VARCHAR      (50)  NOT NULL,
	UserEmail	    VARCHAR      (30)  NOT NULL,
	UserPassword    VARCHAR      (30)  NOT NULL,

	StatusId INT NOT NULL,

	CONSTRAINT PK_USER_UserId PRIMARY KEY (UserId),
	CONSTRAINT FK_USER_StatusId FOREIGN KEY (StatusId) REFERENCES [STATUS] (StatusId)
)


CREATE TABLE [dbo].[INVOICE](

	InvoiceId   INT IDENTITY (1,1) NOT NULL,
	UserId      INT                NOT NULL,
	Comments    VARCHAR(MAX)       NULL,
	SupplierId  INT NOT NULL,

	StatusId      INT              NOT NULL,
	InvoiceDate   DATE             NOT NULL,
	InvoiceUpdate DATE			   NULL,
	InvoiceDelete DATE			   NULL,

	CONSTRAINT PK_INVOICE_InvoiceId PRIMARY KEY (InvoiceId),
	CONSTRAINT FK_INVOICE_UserId FOREIGN KEY (UserId) REFERENCES [USER] (UserId),
	CONSTRAINT FK_INVOICE_SupplierId FOREIGN KEY (SupplierId) REFERENCES [SUPPLIER] (SupplierId),
	CONSTRAINT FK_INVOICE_StatusId FOREIGN KEY (StatusId) REFERENCES [STATUS] (StatusId)
)


CREATE TABLE [dbo].[INVOICE_DETAIL](

	DetailId	  INT IDENTITY (1,1) NOT NULL,
	InvoiceId     INT                NOT NULL,
	ProductId     INT                NOT NULL,
	Amount        INT				 NULL,
	Price         DECIMAL (18,2)     NOT NULL,
	ProductStatus BIT                NOT NULL

	CONSTRAINT PK_DETAIL_InvoiceDetail PRIMARY KEY (DetailId),
	CONSTRAINT PK_DETAIL_ProductId FOREIGN KEY (ProductId) REFERENCES [PRODUCT] (ProductId),
	CONSTRAINT PK_DETAIL_InvoiceId FOREIGN KEY (InvoiceId) REFERENCES [INVOICE] (InvoiceId),
)


INSERT INTO [STATUS]
VALUES ('Active'),
       ('INACTIVE'),
	   ('CANCELLED')

INSERT INTO SUPPLIER
VALUES --('SampleSuppliers'),
	   ('TechSuppliers'),
	   ('DomesticSuppliers'),
	   ('SuppliersForLife :v')

INSERT INTO PRODUCT
VALUES   ('Product 1', 120.50),
		 ('Product 2', 2000),
		 ('Product 3', 150),
		 ('Product 4', 830),
		 ('Product 5', 240),
		 ('Product 6', 4000)



IF EXISTS ( SELECT 1 FROM SYSOBJECTS WHERE NAME = 'spCatalogue' )
BEGIN
	DROP PROCEDURE spCatalogue
END
GO
CREATE PROCEDURE [dbo].[spCatalogue](
	@Action INT = NULL,
	@ProductName VARCHAR(30) = NULL,
	@InitPrice DECIMAL (18,2) = NULL,
	@EndPrice DECIMAL (18,2) = NULL
)
AS
BEGIN
	IF @Action = 1
	BEGIN
		SELECT * FROM SUPPLIER
	END

	IF @Action = 2
	BEGIN
		SELECT * 
		FROM PRODUCT
		WHERE ( ProductName LIKE '%' + @ProductName + '%' OR @ProductName IS NULL )
			  
	END

	IF @Action = 3
	BEGIN
		SELECT * FROM STATUS
	END
END


---------------------USERS--------------------------------
IF EXISTS (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'spAuth')
BEGIN
	DROP PROCEDURE spAuth
END
GO
CREATE PROCEDURE [dbo].[spAuth](
	@Action        INT            = NULL,
	@UserId        INT            = NULL,
	@UserName      VARCHAR   (50) = NULL,
	@UserLastName  VARCHAR   (30) = NULL,
	@UserEmail	   VARCHAR   (30) = NULL,
	@UserPassword  VARCHAR   (30) = NULL,
	@StatusId      INT            = NULL
)
AS
BEGIN
	IF @Action = 1 --LOG IN
	BEGIN 
		SELECT *
		FROM [USER]
		WHERE UserEmail = @UserEmail AND UserPassword = @UserPassword
	END

	--DELETE FROM [USER]

	IF @Action = 2 -- REEGISTRAR NUEVOS USUARIOS
	BEGIN
		INSERT INTO [USER] 
		VALUES ( @UserName, @UserLastName, @UserEmail, @UserPassword, 1)
	END

	IF @Action = 3 -- RECUPERAR CONTRASENIA
	BEGIN
		UPDATE [USER]
		SET UserPassword = @UserPassword
		WHERE UserId = @UserId
	END

	IF @Action = 4 -- sacar usuario con emal
	BEGIN
		SELECT *
		FROM [USER]
		WHERE UserEmail = @UserEmail
	END

END
GO

---------------------INVOICES--------------------------------
IF EXISTS (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'spInvoice')
BEGIN
	DROP PROCEDURE spInvoice
END
GO
CREATE PROCEDURE [dbo].[spInvoice](
	@Action        INT          = NULL,
	@InvoiceId     INT          = NULL,
	@UserId        INT          = NULL,
	@Comments      VARCHAR(MAX) = NULL,
	@SupplierId    INT			= NULL,
	@StatusId      INT          = NULL,
	@InvoiceDate   DATE         = NULL,
	@InvoiceUpdate DATE			= NULL,
	@InvoiceDelete DATE			= NULL,

	--FILTERS
	@SUPPPLIERNAME VARCHAR(30) = NULL,
	@USERNAME      VARCHAR(30) = NULL,
	@USERLASTNAME  VARCHAR(30) = NULL
)
AS
BEGIN
	IF @Action = 1 -- INVOICE SEARCH
	BEGIN 
		SELECT *
		FROM INVOICE INV 
		INNER JOIN [USER] US ON INV.UserId = US.UserId
		INNER JOIN [STATUS] STS ON INV.StatusId = STS.StatusId
		INNER JOIN [SUPPLIER] SP ON INV.SupplierId = SP.SupplierId
		WHERE
			  ( US.UserId 	= @UserId  OR @UserId IS NULL ) AND 
			  ( INV.InvoiceDate = @InvoiceDate OR @InvoiceDate IS NULL  ) AND
			  ( INV.SupplierId  = @SupplierId OR @SupplierId IS NULL  )
	END


	EXEC spInvoice @Action = 1, @SupplierId = 1

	IF @Action = 2 -- CREAR NUEVOS RECIBOS
	BEGIN
		INSERT INTO INVOICE ( UserId, COMMENTS ,StatusId, InvoiceDate )
		VALUES ( @UserId, @COMMENTS, 1, GETDATE() )

	END

	IF @Action = 3 -- ACTUALIZAR RECIBOS
	BEGIN
		UPDATE INVOICE
		SET InvoiceUpdate = GETDATE(),
			Comments = @Comments
		WHERE InvoiceId = @InvoiceId  
	END

	IF @Action = 4 -- CANCELAR RECIBOS
	BEGIN
		UPDATE INVOICE
		SET InvoiceDelete = GETDATE(),
			StatusId = 2
		WHERE InvoiceId = @InvoiceId  
	END

	IF @Action = 5 --invoice por id
	BEGIN 
	SELECT *
		FROM INVOICE INV 
		INNER JOIN [USER] US ON INV.UserId = US.UserId
		INNER JOIN [STATUS] STS ON INV.StatusId = STS.StatusId
		INNER JOIN [SUPPLIER] SP ON INV.SupplierId = SP.SupplierId
		WHERE InvoiceId = @InvoiceId
	END
END
GO

IF EXISTS (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'spInvoiceDetail')
BEGIN
	DROP PROCEDURE spInvoiceDetail
END
GO
CREATE PROCEDURE [dbo].[spInvoiceDetail](

	@Action        INT             = NULL,
	@DetailId      INT             = NULL,
	@InvoiceId     INT             = NULL,
	@ProductId     INT             = NULL,
	@Amount        INT			   = NULL,
	@Price         DECIMAL (18,2)  = NULL,
	@ProductStatus BIT             = NULL
)
AS
BEGIN
	IF @Action = 1 --obtiene todos los detalles de un recibo
	BEGIN
		SELECT DE.DetailId, P.ProductId, P.ProductName,DE.Amount ,DE.Price
		FROM INVOICE_DETAIL DE
		INNER JOIN PRODUCT P ON DE.ProductId = P.ProductId
		WHERE InvoiceId = @InvoiceId 
	END

	IF @Action = 2 --obtiene un detalle en especifico
	BEGIN
		SELECT *
		FROM INVOICE_DETAIL
		WHERE DetailId = @DetailId 
	END

	IF @Action = 3 --agregar detalles
	BEGIN
		INSERT INTO INVOICE_DETAIL (InvoiceId, ProductId, Amount, Price, ProductStatus)
		VALUES (@InvoiceId, @ProductId, @Amount, @Price ,1)
	END

	IF @Action = 4 --Actualizar detalles
	BEGIN
		UPDATE INVOICE_DETAIL
		SET Amount = @Amount,
			Price = @Price
		WHERE DetailId = @DetailId 
	END

	IF @Action = 5 --cancelar detalle
	BEGIN
		UPDATE INVOICE_DETAIL
		SET ProductStatus = 0
		WHERE DetailId = @DetailId 
	END
END
GO