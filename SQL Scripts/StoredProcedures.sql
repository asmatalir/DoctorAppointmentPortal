CREATE OR ALTER PROCEDURE DoctorsGetList
/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1	10Oct2025		Asmatali		       Get List of Doctors
***********************************************************************************************
DoctorsGetList
*/

AS
BEGIN
	SELECT 
		ISNULL(D.DoctorId, '') AS DoctorId,
		ISNULL(U.FirstName, '') AS FirstName,
		ISNULL(U.LastName, '') AS LastName,
		ISNULL(U.Gender, '') AS Gender,
		ISNULL(D.ExperienceYears, '') AS ExperienceYears,
		ISNULL(D.ConsultationFees, '') AS ConsultationFees,
		ISNULL(D.Description, '') AS Description,
		ISNULL(D.HospitalName, '') AS HospitalName,
		ISNULL(D.Address, '') AS Address,
		ISNULL(D.Rating,'') AS Rating,
		ISNULL(D.IsActive, '') AS IsActive
	FROM 
		DoctorProfiles D
	INNER JOIN 
		UserProfiles U ON D.UserId = U.UserId
	WHERE 
		D.IsActive = 1
	ORDER BY 
		U.FirstName, U.LastName;
END
GO
