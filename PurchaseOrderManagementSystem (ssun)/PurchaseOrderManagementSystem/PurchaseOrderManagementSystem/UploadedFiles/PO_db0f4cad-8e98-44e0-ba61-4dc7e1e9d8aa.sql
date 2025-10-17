
CREATE TABLE UserRole(
	UserRoleId INT PRIMARY KEY IDENTITY(1,1),
	UserRole VARCHAR(50) NOT NULL,
	IsActive BIT DEFAULT 1,
	CreatedBy INT NOT NULL,
	CreatedOn DATETIME,
	ModifiedBy INT NOT NULL,
	ModifiedOn DATETIME
)

CREATE TABLE Author(
   AuthorId INT PRIMARY KEY IDENTITY(1,1),
   AuthorName VARCHAR(50) NOT NULL,
   IsActive BIT DEFAULT 1,
   CreatedBy INT NOT NULL,
   CreatedOn DATETIME,
   ModifiedBy INT NOT NULL,
   ModifiedOn DATETIME
)


CREATE TABLE Publisher(
   PublisherId INT PRIMARY KEY IDENTITY(1,1),
   PublisherName VARCHAR(50) NOT NULL,
   IsActive BIT DEFAULT 1,
   CreatedBy INT NOT NULL,
   CreatedOn DATETIME,
   ModifiedBy INT NOT NULL,
   ModifiedOn DATETIME
)



CREATE TABLE Department(
   DepartmentId INT PRIMARY KEY IDENTITY(1,1),
   DepartmentName VARCHAR(50) NOT NULL,
   IsActive BIT DEFAULT 1,
   CreatedBy INT NOT NULL,
   CreatedOn DATETIME,
   ModifiedBy INT NOT NULL,
   ModifiedOn DATETIME
)



CREATE TABLE UserDetails(
   UserId INT PRIMARY KEY IDENTITY(1,1),
   UserName VARCHAR(50) NOT NULL,
   Gender VARCHAR(20) NOT NULL,
   Email VARCHAR(50) UNIQUE,
   Passwrd VARCHAR(255) NOT NULL,
   UserRoleId INT,
   FOREIGN KEY(UserRoleId) REFERENCES UserRole(UserRoleId),
   IsActive BIT DEFAULT 1,
   CreatedBy INT NOT NULL,
   CreatedOn DATETIME,
   ModifiedBy INT NOT NULL,
   ModifiedOn DATETIME
)

CREATE TABLE Members(
   MemberId INT PRIMARY KEY IDENTITY(1,1),
   MemberName VARCHAR(50) NOT NULL,
   Gender VARCHAR(20) NOT NULL,
   DepartmentId INT,
   MemberShipIsssuedDate Datetime NOT NULL,
   MemberShipStatus BIT,
   FOREIGN KEY(DepartmentId) REFERENCES Department(DepartmentId),
   IsActive BIT DEFAULT 1,
   CreatedBy INT NOT NULL,
   CreatedOn DATETIME,
   ModifiedBy INT NOT NULL,
   ModifiedOn DATETIME
)

CREATE TABLE Supplier(
   SupplierId INT PRIMARY KEY IDENTITY(1,1),
   SupplierName VARCHAR(50) NOT NULL,
   SupplierLocation VARCHAR(100),
   SupplierContact VARCHAR(15) CHECK (LEN(SupplierContact)>=10) NOT NULL,
   IsActive BIT DEFAULT 1,
   CreatedBy INT NOT NULL,
   CreatedOn DATETIME,
   ModifiedBy INT NOT NULL,
   ModifiedOn DATETIME
)



CREATE TABLE Book(
   BookId INT PRIMARY KEY IDENTITY(1,1),
   BookName VARCHAR(50) NOT NULL,
   NoOfCopies INT NOT NULL,
   PublisherId INT,
   DepartmentId INT,
   SupplierId INT,
   FOREIGN KEY(PublisherId) REFERENCES Publisher(PublisherId),
   FOREIGN KEY(DepartmentId) REFERENCES Department(DepartmentId),
   FOREIGN KEY(SupplierId) REFERENCES Supplier(SupplierId),
   IsActive BIT DEFAULT 1,
   CreatedBy INT NOT NULL,
   CreatedOn DATETIME,
   ModifiedBy INT NOT NULL,
   ModifiedOn DATETIME
)


DROP TABLE UserDetails;

CREATE TABLE BookCopy(
   BookCopyId INT PRIMARY KEY IDENTITY(1,1),
   BookId INT NOT NULL,
   BookLocation VARCHAR(50) NOT NULL,
   BookCondition VARCHAR(20) NOT NULL,
   IsActive BIT DEFAULT 1,
   CreatedBy INT NOT NULL,
   CreatedOn DATETIME,
   ModifiedBy INT NOT NULL,
   ModifiedOn DATETIME
)


CREATE TABLE BookAuthor(
   BookAuthorId INT PRIMARY KEY IDENTITY(1,1),
   BookId INT NOT NULL,
   AuthorId INT NOT NULL,
   FOREIGN KEY(BookId) REFERENCES Book(BookId),
   FOREIGN KEY(AuthorId) REFERENCES Author(AuthorId),
   
)

DROP TABLE BookAuthor;

/*SELECT * FROM UserRole;

INSERT INTO UserRole(UserRole) VALUES('Student');
INSERT INTO UserRole(UserRole) VALUES('Librarian');*/


CREATE TABLE Transactions(
   TransactionId INT PRIMARY KEY IDENTITY(1,1),
   BookCopyId INT,
   MemberId INT,
   IssueDate Datetime,
   ReturnDate DateTime,
   NoOfRenewals INT,
   FOREIGN KEY(BookCopyId) REFERENCES BookCopy(BookCopyId),
   FOREIGN KEY(MemberId) REFERENCES Members(MemberId),
   IsActive BIT DEFAULT 1,
   CreatedBy INT NOT NULL,
   CreatedOn DATETIME,
   ModifiedBy INT NOT NULL,
   ModifiedOn DATETIME
)

CREATE TABLE Fine(
   FineId INT PRIMARY KEY IDENTITY(1,1),
   TransactionId INT,
   OverDueDays INT,
   Amount INT,
   StatusID INT,
   NoOfRenewals INT,
   FOREIGN KEY(TransactionId) REFERENCES Transactions(TransactionId),
   FOREIGN KEY(StatusId) REFERENCES Statuss(StatusId),
   IsActive BIT DEFAULT 1,
   CreatedBy INT NOT NULL,
   CreatedOn DATETIME,
   ModifiedBy INT NOT NULL,
   ModifiedOn DATETIME
)

CREATE TABLE Statuss(
   StatusId INT PRIMARY KEY IDENTITY(1,1),
   Statuss VARCHAR(20) NOT NULL,
   IsActive BIT DEFAULT 1,
   CreatedBy INT NOT NULL,
   CreatedOn DATETIME,
   ModifiedBy INT NOT NULL,
   ModifiedOn DATETIME
)
