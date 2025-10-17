

/*Table created for UserInfo*/
CREATE TABLE UserInfo(
   UserId INT PRIMARY KEY IDENTITY(1,1),
   FirstName VARCHAR(50) NOT NULL,
   LastName VARCHAR(50) NOT NULL,
   Email VARCHAR(50) NOT NULL UNIQUE,
   Mobile VARCHAR(15) NOT NULL UNIQUE,
   IsActive BIT NOT NULL DEFAULT 1,
   CreatedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   CreatedOn DATETIME,
   LastModifiedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   LastModifiedOn DATETIME
)

/*Table created for UserCredentials*/
CREATE TABLE UserCredentials(
   UserCredentialsId INT PRIMARY KEY IDENTITY(1,1),
   UserId INT FOREIGN KEY REFERENCES UserInfo(UserId),
   UserName VARCHAR(100) NOT NULL UNIQUE,
   PrimaryPassword VARCHAR(255) NOT NULL,
   SecondaryPassword VARCHAR(255) NOT NULL,
   IsActive BIT NOT NULL DEFAULT 1,
   CreatedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   CreatedOn DATETIME,
   LastModifiedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   LastModifiedOn DATETIME
)

/*Table created for LKPPurchaseOrderStatus*/
CREATE TABLE LKPPurchaseOrderStatus(
   LKPPurchaseOrderStatusId INT PRIMARY KEY IDENTITY(1,1),
   PurchaseOrderStatus VARCHAR(100) NOT NULL UNIQUE,
   IsActive BIT NOT NULL DEFAULT 1,
   CreatedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   CreatedOn DATETIME,
   LastModifiedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   LastModifiedOn DATETIME
)


/*Table created for LKPProductCategories*/
CREATE TABLE LKPProductCategories(
   LKPProductCategoryId INT PRIMARY KEY IDENTITY(1,1),
   ProductCategory VARCHAR(100) NOT NULL UNIQUE,	
   Description VARCHAR(255),
   IsActive BIT NOT NULL DEFAULT 1,
   CreatedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   CreatedOn DATETIME,
   LastModifiedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   LastModifiedOn DATETIME
)

/*Table created for Products*/
CREATE TABLE Products(
   ProductId INT PRIMARY KEY IDENTITY(1,1),
   LKPProductCategoryId INT NOT NULL FOREIGN KEY REFERENCES LKPProductCategories(LKPProductCategoryId),	
   ProductName VARCHAR(150) NOT NULL UNIQUE,
   ProductDescription VARCHAR(255),
   ModelNumber VARCHAR(150),
   IsActive BIT NOT NULL DEFAULT 1,
   CreatedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   CreatedOn DATETIME,
   LastModifiedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   LastModifiedOn DATETIME
)

/*Table created for Vendor*/
CREATE TABLE Vendor(
   VendorId INT PRIMARY KEY IDENTITY(1,1),
   VendorName VARCHAR(200) NOT NULL UNIQUE,	
   Address VARCHAR(300),
   City VARCHAR(100),
   State VARCHAR(100),
   PostCode VARCHAR(20),
   Email VARCHAR(100),
   Mobile VARCHAR(15),
   Website VARCHAR(200),
   IsActive BIT NOT NULL DEFAULT 1,
   CreatedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   CreatedOn DATETIME,
   LastModifiedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   LastModifiedOn DATETIME
)


/*Table created for VendorContacts*/
CREATE TABLE VendorContacts(
   VendorContactId INT PRIMARY KEY IDENTITY(1,1),
   Name VARCHAR(200) NOT NULL UNIQUE,	
   Designation VARCHAR(150),
   Email VARCHAR(100),
   Mobile VARCHAR(15),
   VendorId INT FOREIGN KEY REFERENCES Vendor(VendorId),
   IsActive BIT NOT NULL DEFAULT 1,
   CreatedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   CreatedOn DATETIME,
   LastModifiedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   LastModifiedOn DATETIME
)

/*Table created for PurchaseOrders*/
CREATE TABLE PurchaseOrders(
   PurchaseOrderId INT PRIMARY KEY IDENTITY(1,1),
   PurchaseOrderDate Date NOT NULL,
   PurchaseOrderSerialNumber VARCHAR(50) NOT NULL UNIQUE,
   VendorId INT NOT NULL FOREIGN KEY REFERENCES Vendor(VendorId),
   VendorContactId INT NOT NULL FOREIGN KEY REFERENCES VendorContacts(VendorContactId),
   OrderNotes VARCHAR(200),
   ExpectedDeliveryDate DATE NOT NULL,
   ActualDeliveryDate DATE,
   LKPPurchaseOrderStatusId INT NOT NULL FOREIGN KEY REFERENCES LKPPurchaseOrderStatus(LKPPurchaseOrderStatusId),
   PaymentTerms VARCHAR(200),
   InvoiceReceived BIT NOT NULL DEFAULT 0,
   CreatedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   CreatedOn DATETIME,
   LastModifiedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   LastModifiedOn DATETIME
)

/*Table created for PurchaseOrdersDetails*/
CREATE TABLE PurchaseOrderDetails(
   PurchaseOrderDetailId INT PRIMARY KEY IDENTITY(1,1),
   PurchaseOrderId INT NOT NULL FOREIGN KEY REFERENCES PurchaseOrders(PurchaseOrderId),
   ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(ProductId),
   Quantity INT NOT NULL CHECK(Quantity > 0),
   Price INT NOT NULL CHECK(Price >=0),
   CreatedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   CreatedOn DATETIME,
   LastModifiedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   LastModifiedOn DATETIME
)

/*Table created for PurchaseOrdersDocuments*/
CREATE TABLE PurchaseOrdersDocuments(
   PurchaseOrdersDocumentId INT PRIMARY KEY IDENTITY(1,1),
   PurchaseOrderId INT NOT NULL FOREIGN KEY REFERENCES PurchaseOrders(PurchaseOrderId),
   DocumentName VARCHAR(150) NOT NULL,
   DocumentFileName VARCHAR(255) NOT NULL,
   Notes VARCHAR(300),
   CreatedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   CreatedOn DATETIME,
   LastModifiedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
   LastModifiedOn DATETIME
)

/*Table created for ErrorLogs*/
CREATE TABLE PurchaseOrderErrorLogs (
    LogId INT IDENTITY(1,1) PRIMARY KEY,
    ErrorMessage NVARCHAR(MAX),
    StackTrace NVARCHAR(MAX),
	CreatedBy INT FOREIGN KEY REFERENCES UserInfo(UserId),
    CreatedOn DATETIME
)

SELECT * FROM UserCredentials
SELECT * FROM UserInfo


