/*Table created for UserInfo*/
CREATE TABLE UserProfiles(
   UserId INT PRIMARY KEY IDENTITY(1,1),
   FirstName NVARCHAR(50) NOT NULL,
   LastName NVARCHAR(50) NOT NULL,
   Email NVARCHAR(100) NOT NULL UNIQUE,
   ContactNumber NVARCHAR(15) NOT NULL UNIQUE,
   UserName NVARCHAR(100) NOT NULL UNIQUE,
   HashedPassword NVARCHAR(255) NOT NULL,
   UserRoleId INT FOREIGN KEY REFERENCES UserRoles(UserRoleId),
   Gender CHAR(1) CONSTRAINT CHK_UserProfiles_Gender CHECK (Gender IN ('M','F','O')),
   DateOfBirth DATE,
   IsActive BIT NOT NULL DEFAULT 1,
   CreatedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
   CreatedOn DATETIME NOT NULL,
   LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
   LastModifiedOn DATETIME
)



/*Table created for UserRoles*/
CREATE TABLE UserRoles (
    UserRoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1,
	CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
	CreatedOn DATETIME NOT NULL,
	LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
	LastModifiedOn DATETIME
)


/*Table created for DoctorProfiles*/
CREATE TABLE DoctorProfiles (
   DoctorId INT IDENTITY(1,1) PRIMARY KEY,
   UserId INT FOREIGN KEY REFERENCES UserProfiles(UserId),
   ExperienceYears INT NOT NULL,
   ConsultationFees DECIMAL(10,2) NOT NULL,
   Description NVARCHAR(300),
   HospitalName NVARCHAR(200),
   AddressId INT NOT NULL FOREIGN KEY REFERENCES Addresses(AddressId),
   Rating Decimal(3,2),
   IsActive BIT NOT NULL DEFAULT 1,
   CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
   CreatedOn DATETIME NOT NULL,
   LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
   LastModifiedOn DATETIME
)

/*Table created for PatientProfiles*/
CREATE TABLE PatientProfiles (
    PatientId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(15) NOT NULL,
	AadhaarNumber NVARCHAR(12) UNIQUE NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender CHAR(1) NOT NULL CHECK (Gender IN ('M','F','O')),
    InsuranceInfo NVARCHAR(100) NULL,
    MedicalHistory NVARCHAR(300) NULL,
	AddressId INT NOT NULL FOREIGN KEY REFERENCES Addresses(AddressId),
    IsActive BIT NOT NULL DEFAULT 1,
	CreatedOn DATETIME NOT NULL,
	LastModifiedOn DATETIME
);

/*Table created for Specializations*/
CREATE TABLE Specializations (
  SpecializationId INT IDENTITY(1,1) PRIMARY KEY,
  SpecializationName NVARCHAR(150) NOT NULL UNIQUE,
  IsActive BIT NOT NULL DEFAULT 1,
  CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
  CreatedOn DATETIME NOT NULL,
  LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
  LastModifiedOn DATETIME
)

/*Table created for DoctorSpecializations*/
CREATE TABLE DoctorSpecializations (
    DoctorSpecializationId INT IDENTITY(1,1) PRIMARY KEY,
    DoctorId INT NOT NULL FOREIGN KEY REFERENCES DoctorProfiles(DoctorId),
    SpecializationId INT NOT NULL FOREIGN KEY REFERENCES Specializations(SpecializationId),
	CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
	CreatedOn DATETIME NOT NULL,

);


/*Table created for Qualifications*/
CREATE TABLE Qualifications (
  QualificationId INT IDENTITY(1,1) PRIMARY KEY,
  QualificationName NVARCHAR(150) NOT NULL UNIQUE,
  IsActive BIT NOT NULL DEFAULT 1,
  CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
  CreatedOn DATETIME NOT NULL,
  LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
  LastModifiedOn DATETIME
)


/*Table created for DoctorQualifications*/
CREATE TABLE DoctorQualifications (
    DoctorQualificationId INT IDENTITY(1,1) PRIMARY KEY,
    DoctorId INT NOT NULL FOREIGN KEY REFERENCES DoctorProfiles(DoctorId),
    QualificationId INT NOT NULL FOREIGN KEY REFERENCES Qualifications(QualificationId),
    CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
	CreatedOn DATETIME NOT NULL,
);

CREATE TABLE States (
    StateId INT IDENTITY(1,1) PRIMARY KEY,
    StateName NVARCHAR(100) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1,
	CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
	CreatedOn DATETIME NOT NULL,
	LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
	LastModifiedOn DATETIME
);

CREATE TABLE Districts (
    DistrictId INT IDENTITY(1,1) PRIMARY KEY,
    StateId INT NOT NULL FOREIGN KEY REFERENCES States(StateId),
    DistrictName NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
	CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
	CreatedOn DATETIME NOT NULL,
	LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
	LastModifiedOn DATETIME
);

CREATE TABLE Talukas (
    TalukaId INT IDENTITY(1,1) PRIMARY KEY,
    DistrictId INT NOT NULL FOREIGN KEY REFERENCES Districts(DistrictId),
    TalukaName NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
	CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
	CreatedOn DATETIME NOT NULL,
	LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
	LastModifiedOn DATETIME
);

CREATE TABLE Cities (
    CityId INT IDENTITY(1,1) PRIMARY KEY,
    TalukaId INT NOT NULL FOREIGN KEY REFERENCES Talukas(TalukaId),
    CityName NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
	CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
	CreatedOn DATETIME NOT NULL,
	LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
	LastModifiedOn DATETIME
);

CREATE TABLE Addresses (
    AddressId INT IDENTITY(1,1) PRIMARY KEY,
    AddressLine NVARCHAR(200) NULL, 
    StateId INT NOT NULL FOREIGN KEY REFERENCES States(StateId),
    DistrictId INT NOT NULL FOREIGN KEY REFERENCES Districts(DistrictId),
    TalukaId INT NOT NULL FOREIGN KEY REFERENCES Talukas(TalukaId),
    CityId INT NULL FOREIGN KEY REFERENCES Cities(CityId),
    Pincode NVARCHAR(10) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
	CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
	CreatedOn DATETIME NOT NULL,
	LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
	LastModifiedOn DATETIME
);

CREATE TABLE DoctorAvailability (
    DoctorAvailabilityId INT IDENTITY(1,1) PRIMARY KEY,
    DoctorId INT NOT NULL FOREIGN KEY REFERENCES DoctorProfiles(DoctorId),
    DayOfWeek INT NOT NULL,                 
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    SlotDuration INT NOT NULL,                 
    IsActive BIT NOT NULL DEFAULT 1,
	CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
	CreatedOn DATETIME NOT NULL,
	LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
	LastModifiedOn DATETIME
);

CREATE TABLE DoctorSlots (
    SlotId INT IDENTITY(1,1) PRIMARY KEY,
    DoctorId INT NOT NULL FOREIGN KEY REFERENCES DoctorProfiles(DoctorId),
    SlotDate DATE NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    StatusId INT NULL FOREIGN KEY REFERENCES Statuses(StatusId),
	CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
	CreatedOn DATETIME NOT NULL,
	LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
	LastModifiedOn DATETIME
);


CREATE TABLE Statuses (
    StatusId INT IDENTITY(1,1) PRIMARY KEY,
    StatusName NVARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
	CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
	CreatedOn DATETIME NOT NULL,
	LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
	LastModifiedOn DATETIME
);

CREATE TABLE AppointmentRequests (
    AppointmentRequestId INT IDENTITY(1,1) PRIMARY KEY,
    PatientId INT NOT NULL FOREIGN KEY REFERENCES PatientProfiles(PatientId),
    DoctorId INT NULL FOREIGN KEY REFERENCES DoctorProfiles(DoctorId),
    SpecializationId INT NOT NULL FOREIGN KEY REFERENCES Specializations(SpecializationId),
    MedicalConcern NVARCHAR(300) NOT NULL,
    PreferredDate DATE NOT NULL,
	StartTime TIME NOT NULL,
	EndTime TIME NOT NULL,
	FinalStartTime TIME,
	FinalEndTime TIME,
	FinalDate DATE,
    StatusId INT NULL FOREIGN KEY REFERENCES Statuses(StatusId),
	CreatedOn DATETIME NOT NULL,
	LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
	LastModifiedOn DATETIME
);


/*Table created for PatientDocuments*/
CREATE TABLE PatientDocuments(
   PatientDocumentId INT PRIMARY KEY IDENTITY(1,1),
   AppointmentRequestId INT NOT NULL FOREIGN KEY REFERENCES AppointmentRequests(AppointmentRequestId),
   DocumentName VARCHAR(150) NOT NULL,
   DocumentFileName VARCHAR(255) NOT NULL,
   IsActive BIT NOT NULL DEFAULT 1,
   CreatedOn DATETIME NOT NULL,
   LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
   LastModifiedOn DATETIME
)



CREATE TABLE DoctorAvailabilityExceptions (
    ExceptionId INT IDENTITY(1,1) PRIMARY KEY,
    DoctorId INT NOT NULL FOREIGN KEY REFERENCES DoctorProfiles(DoctorId),
    ExceptionDate DATE NOT NULL,    
	StartTime TIME,
	EndTime TIME,
    IsAvailable BIT NOT NULL DEFAULT 0,      
    Reason NVARCHAR(255) NULL,                
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
    CreatedOn DATETIME NOT NULL,
    LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
    LastModifiedOn DATETIME       
);

/*Table created for ErrorLogs*/
CREATE TABLE DoctorAppointmentErrorLogs (
    LogId INT IDENTITY(1,1) PRIMARY KEY,
    ErrorMessage NVARCHAR(MAX),
    StackTrace NVARCHAR(MAX),
	CreatedBy INT NULL  FOREIGN KEY REFERENCES UserInfo(UserId),
    CreatedOn DATETIME
)







