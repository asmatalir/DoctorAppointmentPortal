

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


