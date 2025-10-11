CREATE OR ALTER   PROCEDURE DoctorsGetList  
@TotalCount INT OUTPUT
/*  
***********************************************************************************************  
 Date      Modified By     Purpose of Modification  
1 10Oct2025  Asmatali         Get List of Doctors with Full Address  
***********************************************************************************************  
DoctorsGetList  
*/  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 SELECT   
  D.DoctorId,  
  ISNULL(U.FirstName, '') AS FirstName,  
  ISNULL(U.LastName, '') AS LastName,  
  ISNULL(U.Gender, '') AS Gender,  
  ISNULL(D.ExperienceYears, 0) AS ExperienceYears,  
  ISNULL(D.ConsultationFees, 0) AS ConsultationFees,  
  ISNULL(D.Description, '') AS Description,  
  ISNULL(D.HospitalName, '') AS HospitalName,  
  
  -- Construct full address using related tables  
  ISNULL(CONCAT(  
   ISNULL(A.AddressLine, ''), ', ',  
   ISNULL(C.CityName, ''), ', ',  
   ISNULL(T.TalukaName, ''), ', ',  
   ISNULL(DI.DistrictName, ''), ', ',  
   ISNULL(S.StateName, ''), ' - ',  
   ISNULL(A.Pincode, '')  
  ), '') AS Address,  
  
  ISNULL(D.Rating, 0) AS Rating,  
  ISNULL(D.IsActive, 0) AS IsActive  
  
 FROM   
  dbo.DoctorProfiles D  
 INNER JOIN   
  dbo.UserProfiles U ON D.UserId = U.UserId  
 LEFT JOIN   
  dbo.Addresses A ON D.AddressId = A.AddressId  
 LEFT JOIN   
  dbo.States S ON A.StateId = S.StateId  
 LEFT JOIN   
  dbo.Districts DI ON A.DistrictId = DI.DistrictId  
 LEFT JOIN   
  dbo.Talukas T ON A.TalukaId = T.TalukaId  
 LEFT JOIN   
  dbo.Cities C ON A.CityId = C.CityId  
 WHERE   
  D.IsActive = 1  
 ORDER BY   
  U.FirstName, U.LastName;  

  SELECT @TotalCount = COUNT(*) FROM DoctorProfiles;
END  


CREATE OR ALTER PROCEDURE DoctorsGetList  
    @DoctorName NVARCHAR(100) = NULL,
    @SpecializationId INT = NULL,
    @CityId INT = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Create a temp table to hold filtered results
    CREATE TABLE #FilteredDoctors
    (
        DoctorId INT,
        FirstName NVARCHAR(50),
        LastName NVARCHAR(50),
        Gender CHAR(1),
        ExperienceYears INT,
        ConsultationFees DECIMAL(10,2),
        Description NVARCHAR(300),
        HospitalName NVARCHAR(200),
        Address NVARCHAR(500),
        Rating DECIMAL(3,2),
        IsActive BIT
    );

    -- Insert filtered data into temp table
    INSERT INTO #FilteredDoctors
    SELECT
        D.DoctorId,
        ISNULL(U.FirstName, '') AS FirstName,
        ISNULL(U.LastName, '') AS LastName,
        ISNULL(U.Gender, '') AS Gender,
        ISNULL(D.ExperienceYears, 0) AS ExperienceYears,
        ISNULL(D.ConsultationFees, 0) AS ConsultationFees,
        ISNULL(D.Description, '') AS Description,
        ISNULL(D.HospitalName, '') AS HospitalName,
        ISNULL(CONCAT(
            ISNULL(A.AddressLine, ''), ', ',
            ISNULL(C.CityName, ''), ', ',
            ISNULL(T.TalukaName, ''), ', ',
            ISNULL(DI.DistrictName, ''), ', ',
            ISNULL(S.StateName, ''), ' - ',
            ISNULL(A.Pincode, '')
        ), '') AS Address,
        ISNULL(D.Rating, 0) AS Rating,
        ISNULL(D.IsActive, 0) AS IsActive
    FROM dbo.DoctorProfiles D
    INNER JOIN dbo.UserProfiles U ON D.UserId = U.UserId
    LEFT JOIN dbo.Addresses A ON D.AddressId = A.AddressId
    LEFT JOIN dbo.States S ON A.StateId = S.StateId
    LEFT JOIN dbo.Districts DI ON A.DistrictId = DI.DistrictId
    LEFT JOIN dbo.Talukas T ON A.TalukaId = T.TalukaId
    LEFT JOIN dbo.Cities C ON A.CityId = C.CityId
    LEFT JOIN dbo.DoctorSpecializations DS ON D.DoctorId = DS.DoctorId
    WHERE D.IsActive = 1
      AND (@DoctorName IS NULL OR U.FirstName LIKE '%' + @DoctorName + '%' OR U.LastName LIKE '%' + @DoctorName + '%')
      AND (@SpecializationId IS NULL OR DS.SpecializationId = @SpecializationId)
      AND (@CityId IS NULL OR A.CityId = @CityId);

    -- Get total count for pagination
    SELECT @TotalCount = COUNT(*) FROM #FilteredDoctors;

    -- Return paginated results
    SELECT *
    FROM #FilteredDoctors
    ORDER BY FirstName, LastName
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Drop temp table
    DROP TABLE #FilteredDoctors;
END


--Final Version
CREATE OR ALTER PROCEDURE DoctorsGetList  
    @DoctorName NVARCHAR(100) = NULL,
    @SpecializationId INT = NULL,
    @CityId INT = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Create temp table for filtered results
    CREATE TABLE #FilteredDoctors
    (
        DoctorId INT,
        FirstName NVARCHAR(50),
        LastName NVARCHAR(50),
        Gender CHAR(1),
        ExperienceYears INT,
        ConsultationFees DECIMAL(10,2),
        Description NVARCHAR(300),
        HospitalName NVARCHAR(200),
        Address NVARCHAR(500),
        Rating DECIMAL(3,2),
        IsActive BIT,
        SpecializationNames NVARCHAR(MAX)
    );

    -- Insert filtered data
    INSERT INTO #FilteredDoctors
    SELECT
        D.DoctorId,
        ISNULL(U.FirstName, '') AS FirstName,
        ISNULL(U.LastName, '') AS LastName,
        ISNULL(U.Gender, '') AS Gender,
        ISNULL(D.ExperienceYears, 0) AS ExperienceYears,
        ISNULL(D.ConsultationFees, 0) AS ConsultationFees,
        ISNULL(D.Description, '') AS Description,
        ISNULL(D.HospitalName, '') AS HospitalName,
        ISNULL(CONCAT(
            ISNULL(A.AddressLine, ''), ', ',
            ISNULL(C.CityName, ''), ', ',
            ISNULL(T.TalukaName, ''), ', ',
            ISNULL(DI.DistrictName, ''), ', ',
            ISNULL(S.StateName, ''), ' - ',
            ISNULL(A.Pincode, '')
        ), '') AS Address,
        ISNULL(D.Rating, 0) AS Rating,
        ISNULL(D.IsActive, 0) AS IsActive,
        STUFF((
            SELECT ', ' + SP.SpecializationName
            FROM DoctorSpecializations DS2
            INNER JOIN Specializations SP ON DS2.SpecializationId = SP.SpecializationId
            WHERE DS2.DoctorId = D.DoctorId AND DS2.IsActive = 1
            FOR XML PATH(''), TYPE
        ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS SpecializationNames
    FROM DoctorProfiles D
    INNER JOIN UserProfiles U ON D.UserId = U.UserId
    LEFT JOIN Addresses A ON D.AddressId = A.AddressId
    LEFT JOIN States S ON A.StateId = S.StateId
    LEFT JOIN Districts DI ON A.DistrictId = DI.DistrictId
    LEFT JOIN Talukas T ON A.TalukaId = T.TalukaId
    LEFT JOIN Cities C ON A.CityId = C.CityId
    WHERE D.IsActive = 1
      AND (@DoctorName IS NULL OR U.FirstName LIKE '%' + @DoctorName + '%' OR U.LastName LIKE '%' + @DoctorName + '%')
      AND (@CityId IS NULL OR A.CityId = @CityId)
      AND (
            @SpecializationId IS NULL
            OR EXISTS (
                SELECT 1
                FROM DoctorSpecializations DS
                WHERE DS.DoctorId = D.DoctorId
                  AND DS.SpecializationId = @SpecializationId
                  AND DS.IsActive = 1
            )
          );

    -- Total count
    SELECT @TotalCount = COUNT(*) FROM #FilteredDoctors;

    -- Paginated result
    SELECT *
    FROM #FilteredDoctors
    ORDER BY FirstName, LastName
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    DROP TABLE #FilteredDoctors;
END;



GO
CREATE   PROCEDURE DoctorSpecializationGetList  
  
/*  
***********************************************************************************************  
 Date      Modified By     Purpose of Modification  
1 10Oct2025  Asmatali         Get List for Doctor Specializations  
***********************************************************************************************  
DoctorSpecializationGetList  
  
*/  
  
AS  
BEGIN  
  
 SELECT   
  ISNULL(SpecializationId, '') AS SpecializationId,  
  ISNULL(SpecializationName, '') AS SpecializationName,  
  ISNULL(IsActive, '') AS IsActive,  
  ISNULL(CreatedBy, '') AS CreatedBy,  
  ISNULL(CreatedOn, '') AS CreatedOn,  
  ISNULL(LastModifiedBy, '') AS ModifiedBy,  
  ISNULL(LastModifiedOn, '') AS ModifiedOn  
 FROM  
  Specializations  
 WHERE IsActive = 1  
 ORDER BY SpecializationName  
  
END  
GO

GO
CREATE OR ALTER PROCEDURE DoctorQualificationsGetList

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
11Oct2025 Asmatali        Get List for Doctor Qualifications
***********************************************************************************************
DoctorQualificationsGetList
*/

AS
BEGIN

    SELECT
        ISNULL(QualificationId, '') AS QualificationId,
        ISNULL(QualificationName, '') AS QualificationName,
        ISNULL(IsActive, '') AS IsActive,
        ISNULL(CreatedBy, '') AS CreatedBy,
        ISNULL(CreatedOn, '') AS CreatedOn,
        ISNULL(LastModifiedBy, '') AS ModifiedBy,
        ISNULL(LastModifiedOn, '') AS ModifiedOn
    FROM
        Qualifications
    WHERE IsActive = 1
    ORDER BY QualificationName

END
GO

GO
CREATE OR ALTER PROCEDURE StatesGetList
AS
BEGIN
    SELECT
        ISNULL(StateId, '') AS StateId,
        ISNULL(StateName, '') AS StateName,
        ISNULL(IsActive, '') AS IsActive,
        ISNULL(CreatedBy, '') AS CreatedBy,
        ISNULL(CreatedOn, '') AS CreatedOn,
        ISNULL(LastModifiedBy, '') AS ModifiedBy,
        ISNULL(LastModifiedOn, '') AS ModifiedOn
    FROM
        States
    WHERE IsActive = 1
    ORDER BY StateName
END
GO

GO
CREATE OR ALTER PROCEDURE DistrictsGetList
AS
BEGIN
    SELECT
        ISNULL(DistrictId, '') AS DistrictId,
        ISNULL(DistrictName, '') AS DistrictName,
        ISNULL(StateId, '') AS StateId,
        ISNULL(IsActive, '') AS IsActive,
        ISNULL(CreatedBy, '') AS CreatedBy,
        ISNULL(CreatedOn, '') AS CreatedOn,
        ISNULL(LastModifiedBy, '') AS ModifiedBy,
        ISNULL(LastModifiedOn, '') AS ModifiedOn
    FROM
        Districts
    WHERE IsActive = 1
    ORDER BY DistrictName
END
GO


GO
CREATE OR ALTER PROCEDURE TalukasGetList
AS
BEGIN
    SELECT
        ISNULL(TalukaId, '') AS TalukaId,
        ISNULL(TalukaName, '') AS TalukaName,
        ISNULL(DistrictId, '') AS DistrictId,
        ISNULL(IsActive, '') AS IsActive,
        ISNULL(CreatedBy, '') AS CreatedBy,
        ISNULL(CreatedOn, '') AS CreatedOn,
        ISNULL(LastModifiedBy, '') AS ModifiedBy,
        ISNULL(LastModifiedOn, '') AS ModifiedOn
    FROM
        Talukas
    WHERE IsActive = 1
    ORDER BY TalukaName
END
GO


GO
CREATE OR ALTER PROCEDURE CitiesGetList
AS
BEGIN
    SELECT
        ISNULL(CityId, '') AS CityId,
        ISNULL(CityName, '') AS CityName,
        ISNULL(TalukaId, '') AS TalukaId,
        ISNULL(IsActive, '') AS IsActive,
        ISNULL(CreatedBy, '') AS CreatedBy,
        ISNULL(CreatedOn, '') AS CreatedOn,
        ISNULL(LastModifiedBy, '') AS ModifiedBy,
        ISNULL(LastModifiedOn, '') AS ModifiedOn
    FROM
        Cities
    WHERE IsActive = 1
    ORDER BY CityName
END
GO


GO
CREATE OR ALTER PROCEDURE InsertDoctorDetails
(
    -- Personal Info
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @ContactNumber NVARCHAR(15),
    @UserName NVARCHAR(100),
    @HashedPassword NVARCHAR(255),
    @Gender CHAR(1),
    @DateOfBirth DATE,
    @CreatedBy INT,

    -- Professional Info
    @ExperienceYears INT,
    @ConsultationFees DECIMAL(10,2),
    @Description NVARCHAR(300),
    @HospitalName NVARCHAR(200),
    @Rating DECIMAL(3,2),

    -- Address Info
    @AddressLine NVARCHAR(200),
    @StateId INT,
    @DistrictId INT,
    @TalukaId INT,
    @CityId INT,
    @Pincode NVARCHAR(10),

    -- Multi-selects (comma-separated IDs)
    @Qualifications NVARCHAR(MAX),
    @Specializations NVARCHAR(MAX),

	@CurrentDoctorId INT OUTPUT
)
AS
BEGIN

    DECLARE @UserId INT;
    DECLARE @AddressId INT;
    DECLARE @DoctorId INT;
    DECLARE @CurrentDate DATETIME = GETDATE();

    BEGIN TRY
        BEGIN TRANSACTION;

		DECLARE @DoctorRoleId INT;

		SELECT @DoctorRoleId = UserRoleId
		FROM UserRoles
		WHERE RoleName = 'Doctor';

        -- 1. Insert into UserProfiles
        INSERT INTO UserProfiles
        (FirstName, LastName, Email, ContactNumber, UserName, HashedPassword,UserRoleId, Gender, DateOfBirth, IsActive, CreatedBy, CreatedOn)
        VALUES
        (@FirstName, @LastName, @Email, @ContactNumber, @UserName, @HashedPassword,@DoctorRoleId, @Gender, @DateOfBirth, 1, @CreatedBy, @CurrentDate);

        SET @UserId = SCOPE_IDENTITY();

        -- 2. Insert into Addresses
        INSERT INTO Addresses
        (AddressLine, StateId, DistrictId, TalukaId, CityId, Pincode, IsActive, CreatedBy, CreatedOn)
        VALUES
        (@AddressLine, @StateId, @DistrictId, @TalukaId, @CityId, @Pincode, 1, 6, @CurrentDate);      -----Handle CreatedBy here----------

        SET @AddressId = SCOPE_IDENTITY();

        -- 3. Insert into DoctorProfiles
        INSERT INTO DoctorProfiles
        (UserId, ExperienceYears, ConsultationFees, Description, HospitalName, AddressId, Rating, IsActive, CreatedBy, CreatedOn)
        VALUES
        (@UserId, @ExperienceYears, @ConsultationFees, @Description, @HospitalName, @AddressId, @Rating, 1, 6, @CurrentDate);  -----Handle CreatedBy here----------

        SET @DoctorId = SCOPE_IDENTITY();

        

					-- 5. Insert into DoctorSpecializations (split comma-separated string)
				 -- Assuming @Specializations = '2,6,7' and @Qualifications = '1,3'
			-- And @DoctorId and @UserId are already set

			-- Insert DoctorSpecializations
			INSERT INTO DoctorSpecializations (DoctorId, SpecializationId, IsActive, CreatedBy, CreatedOn)
			SELECT 
				@DoctorId, 
				TRY_CAST(value AS INT),  -- convert string to int
				1, 
				6,                                  -----Handle CreatedBy here----------
				GETDATE()
			FROM STRING_SPLIT(@Specializations, ',');

			-- Insert DoctorQualifications
			INSERT INTO DoctorQualifications (DoctorId, QualificationId, IsActive, CreatedBy, CreatedOn)
			SELECT 
				@DoctorId, 
				TRY_CAST(value AS INT),
				1, 
				6,                          -----Handle CreatedBy here----------
				GETDATE()
			FROM STRING_SPLIT(@Qualifications, ',');


		SET @CurrentDoctorId = @DoctorId;
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO


