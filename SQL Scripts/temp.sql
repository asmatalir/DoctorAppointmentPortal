SELECT * FROM DoctorSlots WHERE DoctorId = 6

SELECT * FROM UserProfiles

SELECT * FROM PAtientDocuments 

EXEC GenerateDoctorSlots '2025-10-22', '2025-11-4',2;

DELETE FROM DoctorSlots
WHERE SlotDate >= CAST(GETDATE() AS DATE);



CREATE TABLE PatientDocuments(
   PatientDocumentId INT PRIMARY KEY IDENTITY(1,1),
   AppointmentRequestId INT NOT NULL FOREIGN KEY REFERENCES AppointmentRequests(AppointmentRequestId),
   DocumentName VARCHAR(150) NOT NULL,
   DocumentFileName VARCHAR(255) NOT NULL,
   IsActive BIT NOT NULL DEFAULT 1,
   CreatedBy INT NOT NULL FOREIGN KEY REFERENCES UserProfiles(UserId),
   CreatedOn DATETIME NOT NULL,
   LastModifiedBy INT FOREIGN KEY REFERENCES UserProfiles(UserId),
   LastModifiedOn DATETIME
)
