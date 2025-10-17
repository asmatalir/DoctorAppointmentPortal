/* Stored Procedure for ValidateUser*/
Go
CREATE OR ALTER PROCEDURE GetUserByUserName
@UserName NVARCHAR(100)
/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1	25June2025		Asmatali		      Validate the user
***********************************************************************************************
ValidateUser

*/
AS
BEGIN

        SELECT  
			ISNULL(uc.PrimaryPassword,'') AS PrimaryPassword,
			ISNULL(ui.Email,'') AS Email
        FROM 
		    UserCredentials uc
			JOIN UserInfo ui ON uc.UserId = ui.UserId
        WHERE
		    uc.UserName = @UserName AND
			uc.IsActive = 1	
END
GO


/* Stored Procedure for UpdateSecondaryPassword*/
Go
CREATE OR ALTER PROCEDURE UpdateSecondaryPassword
@UserName VARCHAR(100),
@SecondaryPassword VARCHAR(255)
/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1	26June2025		Asmatali		      Update the Secondary Password
***********************************************************************************************
UpdateSecondaryPassword

*/
AS
BEGIN

	UPDATE UserCredentials
	SET 
    SecondaryPassword = @SecondaryPassword,
	LastModifiedOn = GETDATE()
	WHERE
	UserName=@UserName	
END
GO

/* Stored Procedure for GetSecondaryPasswordByUserName*/
Go
CREATE OR ALTER PROCEDURE GetSecondaryPasswordByUserName
@UserName VARCHAR(100)
/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1	26June2025		Asmatali		      Get the Secondary Password
***********************************************************************************************
GetSecondaryPasswordByUserName

*/
AS
BEGIN
      SELECT  
		ISNULL(SecondaryPassword, '') AS SecondaryPassword,
		ISNULL(UserId, '') AS UserId
	FROM 
		UserCredentials

      WHERE
	    UserName = @UserName AND
		IsActive = 1	

END
GO

/* Stored Procedure for PurchaseOrderGetList*/
Go
CREATE OR ALTER PROCEDURE PurchaseOrderGetList
/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1	26Jun2025		Asmatali		       Get List of Purchase Order
***********************************************************************************************
PurchaseOrderGetList

*/

AS
BEGIN

	SELECT 
	    ISNULL(po.PurchaseOrderId,'') AS PurchaseOrderId,
		ISNULL(po.PurchaseOrderSerialNumber, '') AS PurchaseOrderSerialNumber,
		ISNULL(po.PurchaseOrderDate, '') AS PurchaseOrderDate,
		ISNULL(po.ExpectedDeliveryDate, '') AS ExpectedDeliveryDate,
		ISNULL(v.VendorId, '') AS VendorId,
		ISNULL(v.VendorName, '') AS VendorName,
		ISNULL(vc.VendorContactId, '') AS VendorContactId,
		ISNULL(vc.Name, '') AS Name,
        ISNULL(pos.PurchaseOrderStatus,'') AS PurchaseOrderStatus,

		 -- Total Order Price from PurchaseOrderDetails (Quantity * Price)
        (
            SELECT SUM(pod.Quantity * pod.Price)
            FROM PurchaseOrderDetails pod
            WHERE pod.PurchaseOrderId = po.PurchaseOrderId
        ) AS TotalOrderPrice

	FROM
		PurchaseOrders po
		JOIN Vendor v ON po.VendorId = v.VendorId
		JOIN VendorContacts vc ON po.VendorContactId = vc.VendorContactId
		JOIN LKPPurchaseOrderStatus pos ON po.LKPPurchaseOrderStatusId = pos.LKPPurchaseOrderStatusId
END
GO


/* Stored Procedure for CategoryGetList*/
Go
CREATE OR ALTER PROCEDURE ProductCategoryGetList

/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1	26June2025		Asmatali		       Get List for Categories
***********************************************************************************************
CategoryGetList

*/

AS
BEGIN

	SELECT 
		ISNULL(LKPProductCategoryId, '') AS LKPProductCategoryId,
		ISNULL(ProductCategory, '') AS ProductCategory,
		ISNULL(Description,'') AS Description,
		ISNULL(IsActive, '') AS IsActive,
		ISNULL(CreatedBy, '') AS CreatedBy,
		ISNULL(CreatedOn, '') AS CreatedOn,
		ISNULL(LastModifiedBy, '') AS ModifiedBy,
		ISNULL(LastModifiedOn, '') AS ModifiedOn
	FROM
		LKPProductCategories
	WHERE IsActive=1
	ORDER BY ProductCategory
	

	
END
GO


/* Stored Procedure for CategoryGetList*/
Go
CREATE OR ALTER PROCEDURE ProductGetList

/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1	26June2025		Asmatali		       Get List for Product
***********************************************************************************************
ProductGetList

*/

AS
BEGIN

	SELECT 
		ISNULL(ProductId, '') AS ProductId,
		ISNULL(ProductName, '') AS ProductName,
		ISNULL(ProductDescription,'') AS ProductDescription,
		ISNULL(ModelNumber,'') AS ModelNumber,
		ISNULL(IsActive, '') AS IsActive,
		ISNULL(CreatedBy, '') AS CreatedBy,
		ISNULL(CreatedOn, '') AS CreatedOn,
		ISNULL(LastModifiedBy, '') AS ModifiedBy,
		ISNULL(LastModifiedOn, '') AS ModifiedOn
	FROM
		Products
	WHERE IsActive=1
	ORDER BY ProductName
	
END
GO


/* Stored Procedure for VendorsGetList*/
Go
CREATE OR ALTER PROCEDURE VendorsGetList

/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1  30June2025		Asmatali		       Get List for Vendors
***********************************************************************************************
VendorsGetList

*/

AS
BEGIN

	SELECT 
		ISNULL(VendorId, '') AS VendorId,
		ISNULL(VendorName, '') AS VendorName,
		ISNULL(Address,'') AS Address,
		ISNULL(City,'') AS City,
		ISNULL(State,'') AS State,
		ISNULL(PostCode, '') AS PostCode,
		ISNULL(Email, '') AS Email,
		ISNULL(Mobile, '') AS Mobile,
		ISNULL(Website, '') AS Website,
		ISNULL(IsActive,'') AS IsActive,
		ISNULL(CreatedBy, '') AS CreatedBy,
		ISNULL(CreatedOn, '') AS CreatedOn,
		ISNULL(LastModifiedBy, '') AS ModifiedBy,
		ISNULL(LastModifiedOn, '') AS ModifiedOn
	FROM
		Vendor
	WHERE IsActive=1
	ORDER BY VendorName
	
END
GO


/* Stored Procedure for VendorContactsGetList*/
Go
CREATE OR ALTER PROCEDURE VendorContactsGetList

/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1  30June2025		Asmatali		       Get List for VendorContacts
***********************************************************************************************
VendorContactsGetList

*/

AS
BEGIN

	SELECT 
		ISNULL(VendorContactId, '') AS VendorContactId,
		ISNULL(Name, '') AS Name,
		ISNULL(Designation, '') AS Designation,
		ISNULL(Email, '') AS Email,
		ISNULL(Mobile, '') AS Mobile,
		ISNULL(IsActive,'') AS IsActive,
		ISNULL(CreatedBy, '') AS CreatedBy,
		ISNULL(CreatedOn, '') AS CreatedOn,
		ISNULL(LastModifiedBy, '') AS ModifiedBy,
		ISNULL(LastModifiedOn, '') AS ModifiedOn
	FROM
		VendorContacts
	WHERE IsActive=1
	ORDER BY Name
	
END
GO

SELECT * FROM PurchaseOrders

/* Stored Procedure for PurchaseOrderStatusGetList*/
Go
CREATE OR ALTER PROCEDURE PurchaseOrderStatusGetList

/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1	26June2025		Asmatali		       Get List for PurchaseOrderStatus
***********************************************************************************************
PurchaseOrderStatusGetList

*/

AS
BEGIN

	SELECT 
		ISNULL(LKPPurchaseOrderStatusId, '') AS LKPPurchaseOrderStatusId,
		ISNULL(PurchaseOrderStatus, '') AS PurchaseOrderStatus,
		ISNULL(IsActive, '') AS IsActive,
		ISNULL(CreatedBy, '') AS CreatedBy,
		ISNULL(CreatedOn, '') AS CreatedOn,
		ISNULL(LastModifiedBy, '') AS ModifiedBy,
		ISNULL(LastModifiedOn, '') AS ModifiedOn
	FROM
		LKPPurchaseOrderStatus
	WHERE IsActive=1
	ORDER BY PurchaseOrderStatus
	
END
GO

SELECT * FROM PurchaseOrders
SELECT * FROM PurchaseOrderDetails
SELECT * FROM PurchaseOrdersDocuments

SELECT * FROM Products;
SELECT * FROM LKPProductCategories

SELECT * FROM LKPPurchaseOrderStatus



CREATE OR ALTER PROCEDURE PurchaseOrdersGetList
    @VendorName VARCHAR(255) = NULL,
    @CategoryIds VARCHAR(MAX) = NULL,
    @ProductIds VARCHAR(MAX) = NULL,
    @StatusIds VARCHAR(MAX) = NULL,
    @PageNo INT,
    @PageSize INT,
    @TotalCount INT OUTPUT
AS
BEGIN

    DECLARE @SQL NVARCHAR(MAX), @ParamDef NVARCHAR(MAX);

    SET @SQL = '
    -- Temp table with Row Numbers
    SELECT 
        po.PurchaseOrderId,
        po.PurchaseOrderSerialNumber,
        po.PurchaseOrderDate,
        po.ExpectedDeliveryDate,
        v.VendorName,
        vc.Name AS VendorContactName,
        s.PurchaseOrderStatus,
        ISNULL(SUM(pod.Quantity * pod.Price), 0) AS TotalOrderPrice,
        ROW_NUMBER() OVER (ORDER BY po.PurchaseOrderDate DESC) AS RowNum
    INTO #Temp
    FROM PurchaseOrders po
    INNER JOIN Vendor v ON po.VendorId = v.VendorId
    INNER JOIN VendorContacts vc ON po.VendorContactId = vc.VendorContactId
    INNER JOIN LKPPurchaseOrderStatus s ON po.LKPPurchaseOrderStatusId = s.LKPPurchaseOrderStatusId
    LEFT JOIN PurchaseOrderDetails pod ON po.PurchaseOrderId = pod.PurchaseOrderId
    LEFT JOIN Products p ON pod.ProductId = p.ProductId
    LEFT JOIN LKPProductCategories cat ON p.LKPProductCategoryId = cat.LKPProductCategoryId
    WHERE 
        (@VendorName IS NULL OR v.VendorName LIKE ''%'' + @VendorName + ''%'')
        AND (
            @CategoryIds IS NULL 
            OR EXISTS (
                SELECT 1 FROM PurchaseOrderDetails d
                INNER JOIN Products px ON d.ProductId = px.ProductId
                WHERE d.PurchaseOrderId = po.PurchaseOrderId
                  AND CHARINDEX('','' + CAST(px.LKPProductCategoryId AS VARCHAR) + '','', '','' + @CategoryIds + '','') > 0
            )
        )
        AND (
            @ProductIds IS NULL 
            OR EXISTS (
                SELECT 1 FROM PurchaseOrderDetails p2
                WHERE p2.PurchaseOrderId = po.PurchaseOrderId
                  AND CHARINDEX('','' + CAST(p2.ProductId AS VARCHAR) + '','', '','' + @ProductIds + '','') > 0
            )
        )
        AND (
            @StatusIds IS NULL 
            OR CHARINDEX('','' + CAST(po.LKPPurchaseOrderStatusId AS VARCHAR) + '','', '','' + @StatusIds + '','') > 0
        )
    GROUP BY 
        po.PurchaseOrderId,
        po.PurchaseOrderSerialNumber,
        po.PurchaseOrderDate,
        po.ExpectedDeliveryDate,
        v.VendorName,
        vc.Name,
        s.PurchaseOrderStatus;

    -- Total count
    SELECT @TotalCount = COUNT(*) FROM #Temp;

    -- Final paginated result
    SELECT * FROM #Temp
    WHERE RowNum BETWEEN ((@PageNo - 1) * @PageSize + 1) AND (@PageNo * @PageSize)
    ORDER BY RowNum;
    ';

    SET @ParamDef = '
        @VendorName VARCHAR(255),
        @CategoryIds VARCHAR(MAX),
        @ProductIds VARCHAR(MAX),
        @StatusIds VARCHAR(MAX),
        @PageNo INT,
        @PageSize INT,
        @TotalCount INT OUTPUT
    ';

    EXEC sp_executesql @SQL, @ParamDef,
        @VendorName = @VendorName,
        @CategoryIds = @CategoryIds,
        @ProductIds = @ProductIds,
        @StatusIds = @StatusIds,
        @PageNo = @PageNo,
        @PageSize = @PageSize,
        @TotalCount = @TotalCount OUTPUT;
END
GO

/* Stored Procedure for PurchaseOrdersGetList*/
CREATE OR ALTER PROCEDURE PurchaseOrdersGetList
    @VendorName VARCHAR(255) = NULL,
    @CategoryIds VARCHAR(MAX) = NULL,
    @ProductIds VARCHAR(MAX) = NULL,
    @StatusIds VARCHAR(MAX) = NULL,
    @PageNo INT,
    @PageSize INT,
    @SortColumn VARCHAR(100),
    @SortDirection VARCHAR(4),
	@FromDate DATETIME,
	@ToDate DATETIME,
    @TotalCount INT OUTPUT
/*
****************************************************************************************************
    Date            Modified By          Purpose of Modification
1   30June2025     Asmatali             Added custom sorting and dynamic SQL-based pagination
****************************************************************************************************
PurchaseOrdersGetList
*/
AS
BEGIN

    -- Set defaults
    IF @SortColumn IS NULL OR @SortColumn = ''
        SET @SortColumn = 'PurchaseOrderDate';

    IF @SortDirection IS NULL OR UPPER(@SortDirection) NOT IN ('ASC', 'DESC')
        SET @SortDirection = 'DESC';

    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @ParamDef NVARCHAR(MAX);

    SET @SQL = N'
    -- Step 1: First temp table with TotalOrderPrice calculated
    SELECT 
        po.PurchaseOrderId,
        po.PurchaseOrderSerialNumber,
        po.PurchaseOrderDate,
        po.ExpectedDeliveryDate,
        v.VendorName,
        vc.Name AS VendorContactName,
        s.PurchaseOrderStatus,
        ISNULL(SUM(pod.Quantity * pod.Price), 0) AS TotalOrderPrice
    INTO #BasePO
    FROM PurchaseOrders po
    INNER JOIN Vendor v ON po.VendorId = v.VendorId
    INNER JOIN VendorContacts vc ON po.VendorContactId = vc.VendorContactId
    INNER JOIN LKPPurchaseOrderStatus s ON po.LKPPurchaseOrderStatusId = s.LKPPurchaseOrderStatusId
    LEFT JOIN PurchaseOrderDetails pod ON po.PurchaseOrderId = pod.PurchaseOrderId
    LEFT JOIN Products p ON pod.ProductId = p.ProductId
    LEFT JOIN LKPProductCategories cat ON p.LKPProductCategoryId = cat.LKPProductCategoryId
    WHERE 
        (@VendorName IS NULL OR v.VendorName LIKE ''%'' + @VendorName + ''%'')
        AND (
            @CategoryIds IS NULL 
            OR EXISTS (
                SELECT 1 FROM PurchaseOrderDetails d
                INNER JOIN Products px ON d.ProductId = px.ProductId
                WHERE d.PurchaseOrderId = po.PurchaseOrderId
                  AND CHARINDEX('','' + CAST(px.LKPProductCategoryId AS VARCHAR) + '','', '','' + @CategoryIds + '','') > 0
            )
        )
        AND (
            @ProductIds IS NULL 
            OR EXISTS (
                SELECT 1 FROM PurchaseOrderDetails p2
                WHERE p2.PurchaseOrderId = po.PurchaseOrderId
                  AND CHARINDEX('','' + CAST(p2.ProductId AS VARCHAR) + '','', '','' + @ProductIds + '','') > 0
            )
        )
        AND (
            @StatusIds IS NULL 
            OR CHARINDEX('','' + CAST(po.LKPPurchaseOrderStatusId AS VARCHAR) + '','', '','' + @StatusIds + '','') > 0
        )
		AND (@FromDate IS NULL OR po.PurchaseOrderDate >= @FromDate)
        AND (@ToDate IS NULL OR po.PurchaseOrderDate < DATEADD(DAY, 1, @ToDate))

    GROUP BY 
        po.PurchaseOrderId,
        po.PurchaseOrderSerialNumber,
        po.PurchaseOrderDate,
        po.ExpectedDeliveryDate,
        v.VendorName,
        vc.Name,
        s.PurchaseOrderStatus;

    -- Step 2: Apply ROW_NUMBER with dynamic sort
    SELECT *,
        ROW_NUMBER() OVER (ORDER BY ' + QUOTENAME(@SortColumn) + ' ' + @SortDirection + ') AS RowNum
    INTO #FinalPO
    FROM #BasePO;

    -- Step 3: Get total count
    SELECT @TotalCount = COUNT(*) FROM #FinalPO;

    -- Step 4: Paginate
    SELECT * FROM #FinalPO
    WHERE RowNum BETWEEN ((@PageNo - 1) * @PageSize + 1) AND (@PageNo * @PageSize);
    ';

    -- Define parameter mappings
    SET @ParamDef = N'
        @VendorName VARCHAR(255),
        @CategoryIds VARCHAR(MAX),
        @ProductIds VARCHAR(MAX),
        @StatusIds VARCHAR(MAX),
        @PageNo INT,
        @PageSize INT,
		@FromDate DATETIME,
        @ToDate DATETIME,
        @TotalCount INT OUTPUT
    ';

    -- Execute the dynamic SQL
    EXEC sp_executesql 
        @SQL,
        @ParamDef,
        @VendorName = @VendorName,
        @CategoryIds = @CategoryIds,
        @ProductIds = @ProductIds,
        @StatusIds = @StatusIds,
        @PageNo = @PageNo,
        @PageSize = @PageSize,
		@FromDate = @FromDate,
        @ToDate = @ToDate,
        @TotalCount = @TotalCount OUTPUT;
END
GO

/* Stored Procedure for PurchaseOrderGetDetails*/
Go
CREATE OR ALTER PROCEDURE PurchaseOrderGetDetails
@PurchaseOrderId INT
/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1	30June2025		Asmatali		       Get Details from PurchaseOrderId
***********************************************************************************************
PurchaseOrderGetDetails

*/

AS
BEGIN

    --First Result Set - Purchase Order Details
	SELECT 
		ISNULL(PurchaseOrderSerialNumber, '') AS PurchaseOrderSerialNumber,
		ISNULL(PurchaseOrderDate, '') AS PurchaseOrderDate,
		ISNULL(v.VendorName, '') AS VendorName,
		ISNULL(vc.Name, '') AS Name,
		ISNULL(OrderNotes, '') AS OrderNotes,
		ISNULL(ExpectedDeliveryDate, '') AS ExpectedDeliveryDate,
		ISNULL(pos.PurchaseOrderStatus, '') AS PurchaseOrderStatus,
		ISNULL(PaymentTerms, '') AS PaymentTerms,
		ISNULL(InvoiceReceived, '') AS InvoiceReceived,
		ISNULL(po.CreatedBy, '') AS CreatedBy,
		ISNULL(po.CreatedOn, '') AS CreatedOn,

		(
            SELECT SUM(pod.Quantity * pod.Price)
            FROM PurchaseOrderDetails pod
            WHERE pod.PurchaseOrderId = po.PurchaseOrderId
        ) AS TotalOrderPrice

	FROM
		PurchaseOrders po
		JOIN Vendor v ON po.VendorId = v.VendorId
		JOIN LKPPurchaseOrderStatus pos ON po.LKPPurchaseOrderStatusId = pos.LKPPurchaseOrderStatusId
		JOIN VendorContacts vc  ON po.VendorContactId = vc.VendorContactId
	 WHERE
	 po.PurchaseOrderId=@PurchaseOrderId
	

	--Second Result Set - Product Details
	SELECT 
		ISNULL(p.ProductName, '') AS ProductName,
		ISNULL(p.ModelNumber, '') AS ModelNumber,
		ISNULL(pod.Quantity, '') AS Quantity,
		ISNULL(pod.Price, '') AS Price

	FROM
		PurchaseOrderDetails pod
		JOIN Products p ON pod.ProductId = p.ProductId
	WHERE
	    PurchaseOrderId = @PurchaseOrderId


	--Third Result Set - Document Details
	SELECT 
		ISNULL(DocumentName, '') AS DocumentName,
		ISNULL(DocumentFileName, '') AS DocumentFileName,
		ISNULL(Notes, '') AS Notes

	FROM
		PurchaseOrdersDocuments
	WHERE
	    PurchaseOrderId = @PurchaseOrderId
    
	
END
GO

SELECT * FROM PurchaseOrders

/* Stored Procedure for InsertOrUpdatePurchaseOrder */
GO
CREATE OR ALTER PROCEDURE InsertOrUpdatePurchaseOrder
    @PurchaseOrderId INT,
    @VendorId INT,
    @VendorContactId INT,
    @PurchaseOrderDate DATETIME,
    @ExpectedDeliveryDate DATETIME,
    @LKPPurchaseOrderStatusId INT,
    @OrderNotes NVARCHAR(MAX) = NULL,
	@PaymentTerms VARCHAR(200),
	@InvoiceReceived BIT,
    @CreatedBy INT,
    @ProductsXml XML = NULL,
    @DocumentsXml XML = NULL,
    @CurrentPurchaseOrderId INT OUTPUT

/*
***********************************************************************************************
    Date              Modified By           Purpose of Modification
1   30June2025       Asmatali              Insert/Update Purchase Order, Products, and Documents
***********************************************************************************************
InsertOrUpdatePurchaseOrder
*/

AS
BEGIN

    BEGIN TRY
	   BEGIN TRANSACTION;
    -- Insert or update Purchase Order
    IF @PurchaseOrderId = 0
    BEGIN
	    DECLARE @NewSerialNumber VARCHAR(20);
		DECLARE @YearPart VARCHAR(4) = CAST(YEAR(GETDATE()) AS VARCHAR);
		DECLARE @NextNumber INT;

		-- Find max number for current year like 'PO-2025-%'
		SELECT @NextNumber = ISNULL(MAX(CAST(RIGHT(PurchaseOrderSerialNumber, 3) AS INT)), 0) + 1
		FROM PurchaseOrders
		WHERE PurchaseOrderSerialNumber LIKE 'PO-' + @YearPart + '-%';

		-- Format to 'PO-YYYY-XXX'
		SET @NewSerialNumber = 'PO-' + @YearPart + '-' + RIGHT('000' + CAST(@NextNumber AS VARCHAR), 3);
        INSERT INTO PurchaseOrders
        (
		    PurchaseOrderSerialNumber,
            VendorId,
            VendorContactId,
            PurchaseOrderDate,
            ExpectedDeliveryDate,
            LKPPurchaseOrderStatusId,
            OrderNotes,
			PaymentTerms,
			InvoiceReceived,
            CreatedBy,
            CreatedOn
        )
        VALUES
        (
		    @NewSerialNumber,
            @VendorId,
            @VendorContactId,
            GETDATE(),
            @ExpectedDeliveryDate,
            @LKPPurchaseOrderStatusId,
            @OrderNotes,
			@PaymentTerms,
			@InvoiceReceived,
            @CreatedBy,
            GETDATE()
        );

        SET @PurchaseOrderId = SCOPE_IDENTITY();
        SET @CurrentPurchaseOrderId = @PurchaseOrderId;
    END
    ELSE
    BEGIN
        UPDATE PurchaseOrders
        SET
            VendorId = @VendorId,
            VendorContactId = @VendorContactId,
            PurchaseOrderDate = @PurchaseOrderDate,
            ExpectedDeliveryDate = @ExpectedDeliveryDate,
            LKPPurchaseOrderStatusId = @LKPPurchaseOrderStatusId,
            OrderNotes = @OrderNotes,
			PaymentTerms=@PaymentTerms,
			InvoiceReceived = @InvoiceReceived,
            LastModifiedBy = @CreatedBy,
            LastModifiedOn = GETDATE()
        WHERE PurchaseOrderId = @PurchaseOrderId;

        SET @CurrentPurchaseOrderId = @PurchaseOrderId;
    END


    -- Handle Products
    IF @ProductsXml IS NOT NULL
    BEGIN
        CREATE TABLE #TempProducts (
            ProductId INT,
            Quantity INT,
            Price DECIMAL(18, 2)
        );

        INSERT INTO #TempProducts (ProductId, Quantity, Price)
        SELECT
            Product.value('(ProductId)[1]', 'INT'),
            Product.value('(Quantity)[1]', 'INT'),
            Product.value('(Price)[1]', 'DECIMAL(18,2)')
        FROM @ProductsXml.nodes('/Products/Product') AS X(Product);

        -- Delete removed products
        DELETE FROM PurchaseOrderDetails
        WHERE PurchaseOrderId = @CurrentPurchaseOrderId
        AND ProductId NOT IN (SELECT ProductId FROM #TempProducts);

        -- Update existing
        UPDATE pod
        SET
            pod.Quantity = tp.Quantity,
            pod.Price = tp.Price,
		    pod.LastModifiedBy = @CreatedBy,
            pod.LastModifiedOn = GETDATE()
        FROM PurchaseOrderDetails pod
        JOIN #TempProducts tp ON pod.ProductId = tp.ProductId
        WHERE pod.PurchaseOrderId = @CurrentPurchaseOrderId;

        -- Insert new
        INSERT INTO PurchaseOrderDetails (PurchaseOrderId, ProductId, Quantity, Price,CreatedBy,CreatedOn)
        SELECT
            @CurrentPurchaseOrderId,
            tp.ProductId,
            tp.Quantity,
            tp.Price,
			@CreatedBy,
			GetDate()
        FROM #TempProducts tp
        WHERE NOT EXISTS (
            SELECT 1 FROM PurchaseOrderDetails pod
            WHERE pod.PurchaseOrderId = @CurrentPurchaseOrderId AND pod.ProductId = tp.ProductId
        );
    END

    -- Handle Documents
    IF @DocumentsXml IS NOT NULL
    BEGIN
        CREATE TABLE #TempDocuments (
            DocumentName NVARCHAR(255),
            DocumentFileName NVARCHAR(255),
            Notes NVARCHAR(MAX)
        );

        INSERT INTO #TempDocuments (DocumentName, DocumentFileName, Notes)
        SELECT
            DocumentNode.value('(DocumentName)[1]', 'NVARCHAR(255)'),
            DocumentNode.value('(DocumentFileName)[1]', 'NVARCHAR(255)'),
            DocumentNode.value('(Notes)[1]', 'NVARCHAR(MAX)')
        FROM @DocumentsXml.nodes('/Documents/Document') AS X(DocumentNode);

        -- Delete removed documents
        DELETE FROM PurchaseOrdersDocuments
        WHERE PurchaseOrderId = @CurrentPurchaseOrderId
        AND DocumentFileName NOT IN (SELECT DocumentFileName FROM #TempDocuments);

		-- Update existing documents
		UPDATE pod
		SET
			pod.Notes = td.Notes,
			pod.DocumentName = td.DocumentName,
			pod.LastModifiedBy = @CreatedBy,
            pod.LastModifiedOn = GETDATE()
		FROM PurchaseOrdersDocuments pod
		JOIN #TempDocuments td
			ON pod.DocumentFileName = td.DocumentFileName
		WHERE pod.PurchaseOrderId = @CurrentPurchaseOrderId;

        -- Insert new documents
        INSERT INTO PurchaseOrdersDocuments (PurchaseOrderId, DocumentName, DocumentFileName, Notes, CreatedBy, CreatedOn)
        SELECT
            @CurrentPurchaseOrderId,
            td.DocumentName,
            td.DocumentFileName,
            td.Notes,
            @CreatedBy,
            GETDATE()
        FROM #TempDocuments td
        WHERE NOT EXISTS (
            SELECT 1 FROM PurchaseOrdersDocuments pod
            WHERE pod.PurchaseOrderId = @CurrentPurchaseOrderId AND pod.DocumentFileName = td.DocumentFileName
        );
    END

	COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
	  IF @@TRANCOUNT > 0
	      ROLLBACK TRANSACTION;

    THROW;
    END CATCH
END
GO

/* Stored Procedure for PurchaseOrderGetEditData */
GO
CREATE OR ALTER PROCEDURE PurchaseOrderGetEditData
@PurchaseOrderId INT

/*
***********************************************************************************************
    Date              Modified By           Purpose of Modification
1   30June2025        Asmatali              Get Full PurchaseOrder Details for Edit Operation
***********************************************************************************************
PurchaseOrderGetEditData
*/

AS
BEGIN

    -- Purchase Order Main Details
    SELECT 
        ISNULL(PurchaseOrderId, '') AS PurchaseOrderId,
        ISNULL(PurchaseOrderSerialNumber, '') AS PurchaseOrderSerialNumber,
        ISNULL(VendorId, '') AS VendorId,
        ISNULL(VendorContactId, '') AS VendorContactId,
        ISNULL(PurchaseOrderDate, '') AS PurchaseOrderDate,
        ISNULL(ExpectedDeliveryDate, '') AS ExpectedDeliveryDate,
        ISNULL(LKPPurchaseOrderStatusId, '') AS LKPPurchaseOrderStatusId,
        ISNULL(OrderNotes, '') AS OrderNotes,
        ISNULL(PaymentTerms, '') AS PaymentTerms,
        ISNULL(InvoiceReceived, '') AS InvoiceReceived,
        ISNULL(CreatedBy, '') AS CreatedBy,
        ISNULL(CreatedOn, '') AS CreatedOn,
        ISNULL(LastModifiedBy, '') AS LastModifiedBy,
        ISNULL(LastModifiedOn, '') AS LastModifiedOn
    FROM 
        PurchaseOrders
    WHERE 
        PurchaseOrderId = @PurchaseOrderId;

    -- Products Linked to the Purchase Order
    SELECT 
        ISNULL(pod.ProductId, '') AS ProductId,
        ISNULL(p.ProductName, '') AS ProductName,
        ISNULL(pod.Quantity, '') AS Quantity,
        ISNULL(pod.Price, '') AS Price
    FROM 
        PurchaseOrderDetails pod
    INNER JOIN 
        Products p ON pod.ProductId = p.ProductId
    WHERE 
        pod.PurchaseOrderId = @PurchaseOrderId;

    -- Documents Linked to the Purchase Order
    SELECT 
        ISNULL(DocumentName, '') AS DocumentName,
        ISNULL(DocumentFileName, '') AS DocumentFileName,
        ISNULL(Notes, '') AS Notes
    FROM 
        PurchaseOrdersDocuments 
    WHERE 
        PurchaseOrderId = @PurchaseOrderId;

END
GO

/* Stored Procedure for PurchaseOrderErrorInsert*/
Go
CREATE OR ALTER PROCEDURE PurchaseOrderErrorLogsInsert
@ErrorMessage VARCHAR(MAX),
@StackTrace VARCHAR(MAX),
@CreatedBy INT

/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1	5May2025		Asmatali		       Insert ErrorLogs
***********************************************************************************************
PurchaseOrderErrorInsert

*/

AS
BEGIN

	INSERT INTO PurchaseOrderErrorLogs
	(
	   ErrorMessage,
	   StackTrace,
	   CreatedBy,
	   CreatedOn
	)
	VALUES
	(
	   @ErrorMessage,
	   @StackTrace,
	   @CreatedBy,
	   GETDATE()
	)	
END
GO

/* Stored Procedure for GetVendorContactsByVendorId*/
Go
CREATE OR ALTER PROCEDURE GetVendorContactsByVendorId
@VendorId INT
/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1  30June2025		Asmatali		       Get VendorContacts By VendorId
***********************************************************************************************
GetVendorContactsByVendorId

*/

AS
BEGIN

	SELECT 
		ISNULL(VendorContactId, '') AS VendorContactId,
		ISNULL(Name, '') AS Name
	FROM
		VendorContacts
	WHERE IsActive=1 AND
	      VendorId = @VendorId
	ORDER BY Name
	
END
GO


SELECT * FROM PurchaseOrdersDocuments
SELECT * FROM PurchaseOrders
SELECT * FROM VendorContacts
SELECT * FROM Vendor

SELECT * FROM PurchaseOrderErrorLogs