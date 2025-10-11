INSERT INTO UserRoles (RoleName,CreatedBy, CreatedOn)
VALUES 
('Admin',1, GETDATE()),
('Doctor',1, GETDATE());


INSERT INTO UserProfiles 
(FirstName, LastName, Email, ContactNumber, UserName, HashedPassword, UserRoleId, Gender, DateOfBirth,CreatedBy, CreatedOn)
VALUES
('Amit', 'Sharma', 'amit.sharma@example.com', '9876543210', 'amit.admin', 'hashed_pwd_admin', 4, 'M', '1985-06-15',1, GETDATE());

INSERT INTO UserProfiles 
(FirstName, LastName, Email, ContactNumber, UserName, HashedPassword, UserRoleId, Gender, DateOfBirth,CreatedBy, CreatedOn)
VALUES
('Rohit', 'Patel', 'rohit.patel@hospital.com', '9823012345', 'rohit.doc', 'hashed_pwd_doc1', 5, 'M', '1988-03-22',6, GETDATE()),
('Sneha', 'Iyer', 'sneha.iyer@hospital.com', '9812098765', 'sneha.doc', 'hashed_pwd_doc2', 5, 'F', '1990-08-10',6, GETDATE()),
('Priya', 'Mehta', 'priya.mehta@hospital.com', '9876501234', 'priya.doc', 'hashed_pwd_doc3', 5, 'F', '1987-11-05',6, GETDATE()),
('Arjun', 'Reddy', 'arjun.reddy@hospital.com', '9898123456', 'arjun.doc', 'hashed_pwd_doc4', 5, 'M', '1992-02-28',6, GETDATE()),
('Kiran', 'Desai', 'kiran.desai@hospital.com', '9845123456', 'kiran.doc', 'hashed_pwd_doc5', 5, 'M', '1989-09-12',6, GETDATE());

INSERT INTO States (StateName, IsActive, CreatedBy, CreatedOn)
VALUES 
('Maharashtra', 1, 6, GETDATE()),
('Gujarat', 1, 6, GETDATE()),
('Karnataka', 1, 6, GETDATE());


INSERT INTO Districts (StateId, DistrictName, IsActive, CreatedBy, CreatedOn)
VALUES 
(1, 'Pune', 1, 6, GETDATE()),
(1, 'Mumbai', 1, 6, GETDATE()),
(2, 'Ahmedabad', 1, 6, GETDATE()),
(3, 'Bangalore Urban', 1, 6, GETDATE());


INSERT INTO Talukas (DistrictId, TalukaName, IsActive, CreatedBy, CreatedOn)
VALUES 
(1, 'Haveli', 1, 6, GETDATE()),
(1, 'Mulshi', 1, 6, GETDATE()),
(2, 'Andheri', 1, 6, GETDATE()),
(3, 'Daskroi', 1, 6, GETDATE()),
(4, 'Yelahanka', 1, 6, GETDATE());


INSERT INTO Cities (TalukaId, CityName, IsActive, CreatedBy, CreatedOn)
VALUES 
(1, 'Pune City', 1, 6, GETDATE()),
(2, 'Lavasa', 1, 6, GETDATE()),
(3, 'Mumbai City', 1, 6, GETDATE()),
(4, 'Ahmedabad City', 1, 6, GETDATE()),
(5, 'Bangalore City', 1, 6, GETDATE());

INSERT INTO Addresses (AddressLine, StateId, DistrictId, TalukaId, CityId, Pincode, IsActive, CreatedBy, CreatedOn)
VALUES 
('123 MG Road', 1, 1, 1, 1, '411001', 1, 6, GETDATE()),
('45 Hill View, Lavasa', 1, 1, 2, 2, '412107', 1, 6, GETDATE()),
('10 Marine Drive', 1, 2, 3, 3, '400020', 1, 6, GETDATE()),
('22 CG Road', 2, 3, 4, 4, '380009', 1, 6, GETDATE()),
('56 MG Layout', 3, 4, 5, 5, '560064', 1, 6, GETDATE());

INSERT INTO DoctorProfiles 
(
    UserId, 
    ExperienceYears, 
    ConsultationFees, 
    Description, 
    HospitalName, 
    AddressId, 
    Rating, 
    IsActive, 
    CreatedBy, 
    CreatedOn
)
VALUES
(9, 10, 500.00, 'Specialist in General Medicine and Family Health.', 'Ruby Hall Clinic', 1, 4.5, 1, 6, GETDATE()),
(10, 8, 750.00, 'Cardiologist with expertise in non-invasive treatments.', 'Apollo Hospitals', 2, 4.7, 1, 6, GETDATE()),
(11, 12, 1000.00, 'Orthopedic surgeon with focus on knee and spine care.', 'Lilavati Hospital', 3, 4.8, 1, 6, GETDATE()),
(12, 6, 400.00, 'Dermatologist specializing in cosmetic and laser treatments.', 'Civil Hospital', 4, 4.3, 1, 6, GETDATE()),
(13, 15, 1200.00, 'Neurologist experienced in neurorehabilitation.', 'NIMHANS Hospital', 5, 4.9, 1, 6, GETDATE());


INSERT INTO Specializations (SpecializationName, IsActive, CreatedBy, CreatedOn)
VALUES
('General Physician', 1, 6, GETDATE()),
('Cardiologist', 1, 6, GETDATE()),
('Orthopedic Surgeon', 1, 6, GETDATE()),
('Dermatologist', 1, 6, GETDATE()),
('Neurologist', 1, 6, GETDATE()),
('Pediatrician', 1, 6, GETDATE()),
('Gynecologist', 1, 6, GETDATE()),
('ENT Specialist', 1, 6, GETDATE());

INSERT INTO Qualifications (QualificationName, IsActive, CreatedBy, CreatedOn)
VALUES
('MBBS', 1, 6, GETDATE()),
('MD - General Medicine', 1, 6, GETDATE()),
('MD - Cardiology', 1, 6, GETDATE()),
('MS - Orthopedics', 1, 6, GETDATE()),
('MD - Dermatology', 1, 6, GETDATE()),
('DM - Neurology', 1, 6, GETDATE()),
('DCH - Pediatrics', 1, 6, GETDATE()),
('MS - ENT', 1, 6, GETDATE());


INSERT INTO DoctorSpecializations (DoctorId, SpecializationId, IsActive, CreatedBy, CreatedOn)
VALUES
(6, 1, 1, 6, GETDATE()),  
(2, 2, 1, 6, GETDATE()),  
(3, 3, 1, 6, GETDATE()),  
(4, 4, 1, 6, GETDATE()), 
(5, 5, 1, 6, GETDATE());  

INSERT INTO DoctorQualifications (DoctorId, QualificationId, IsActive, CreatedBy, CreatedOn)
VALUES
(6, 2, 1, 6, GETDATE()), 
(2, 3, 1, 6, GETDATE()),  
(3, 4, 1, 6, GETDATE()),  
(4, 5, 1, 6, GETDATE()), 
(5, 6, 1, 6, GETDATE());  




