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

GO
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
        SpecializationNames NVARCHAR(MAX),
		QualificationNames NVARCHAR(MAX)
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
        ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS SpecializationNames,
		 STUFF((
            SELECT ', ' + Q.QualificationName
            FROM DoctorQualifications DQ
			INNER JOIN Qualifications Q ON DQ.QualificationId = Q.QualificationId
            WHERE DQ.DoctorId = D.DoctorId AND Q.IsActive = 1
            FOR XML PATH(''), TYPE
        ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS QualificationNames
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
CREATE OR ALTER PROCEDURE StatusesGetList
AS
BEGIN
    SELECT
        ISNULL(StatusId, '') AS StatusId,
        ISNULL(StatusName, '') AS StatusName,
        ISNULL(IsActive, '') AS IsActive,
        ISNULL(CreatedBy, '') AS CreatedBy,
        ISNULL(CreatedOn, '') AS CreatedOn,
        ISNULL(LastModifiedBy, '') AS ModifiedBy,
        ISNULL(LastModifiedOn, '') AS ModifiedOn
    FROM
        Statuses
    WHERE IsActive = 1
    ORDER BY StatusName
END
GO

GO
CREATE OR ALTER PROCEDURE AppointmentRequestsGetList
AS
BEGIN
    SELECT
        ISNULL(AR.AppointmentRequestId, '') AS AppointmentRequestId,
        ISNULL(AR.PatientId, '') AS PatientId,
        ISNULL(PP.FirstName, '') + ' ' + ISNULL(PP.LastName, '') AS PatientName,
        ISNULL(AR.DoctorId, '') AS DoctorId,
        ISNULL(U.FirstName, '') + ' ' + ISNULL(U.LastName, '') AS DoctorName,
        ISNULL(AR.MedicalConcern, '') AS MedicalConcern,
        ISNULL(AR.PreferredDate, '') AS PreferredDate,
		ISNULL(AR.SpecializationId, '') AS SpecializationId,
	    ISNULL(SP.SpecializationName, '') AS SpecializationName,
        ISNULL(AR.StartTime, '') AS StartTime,
        ISNULL(AR.EndTime, '') AS EndTime,
        ISNULL(AR.FinalStartTime, '') AS FinalStartTime,
        ISNULL(AR.FinalEndTime, '') AS FinalEndTime,
        ISNULL(AR.FinalDate, '') AS FinalDate,
        ISNULL(AR.StatusId, '') AS StatusId,
        ISNULL(S.StatusName, '') AS StatusName,
        ISNULL(AR.CreatedOn, '') AS CreatedOn,
        ISNULL(AR.LastModifiedBy, '') AS LastModifiedBy,
        ISNULL(U.FirstName + ' ' + U.LastName, '') AS ModifiedBy,
        ISNULL(AR.LastModifiedOn, '') AS ModifiedOn
    FROM
        AppointmentRequests AR
        LEFT JOIN PatientProfiles PP ON AR.PatientId = PP.PatientId
        LEFT JOIN DoctorProfiles DP ON AR.DoctorId = DP.DoctorId
        LEFT JOIN Statuses S ON AR.StatusId = S.StatusId
        LEFT JOIN UserProfiles U ON DP.UserId = U.UserId
		LEFT JOIN Specializations  SP ON AR.SpecializationId = SP.SpecializationId
    ORDER BY
        AR.AppointmentRequestId DESC
END
GO




GO
CREATE OR ALTER PROCEDURE DoctorAvailabilityGetList
    @DoctorId INT
AS
BEGIN
    SELECT
        ISNULL(DoctorAvailabilityId, 0) AS DoctorAvailabilityId,
        ISNULL(DoctorId, 0) AS DoctorId,
        ISNULL(DayOfWeek, 0) AS DayOfWeek,
        ISNULL(StartTime, '') AS StartTime,
        ISNULL( EndTime,'') AS EndTime,
        ISNULL(SlotDuration, 0) AS SlotDuration,
        ISNULL(IsActive, '') AS IsActive,
        ISNULL(CreatedBy, '') AS CreatedBy,
        ISNULL(CreatedOn, '') AS CreatedOn,
        ISNULL(LastModifiedBy, '') AS ModifiedBy,
        ISNULL(LastModifiedOn, '') AS ModifiedOn
    FROM
        DoctorAvailability
    WHERE IsActive = 1 AND DoctorId = @DoctorId
    ORDER BY DoctorId, DayOfWeek, StartTime
END
GO


GO
CREATE OR ALTER PROCEDURE DoctorAvailabilityExceptionsGetList
 @DoctorId INT
AS
BEGIN
    SELECT
        ISNULL(ExceptionId, '') AS ExceptionId,
        ISNULL(DoctorId, '') AS DoctorId,
        ISNULL(ExceptionDate, '') AS ExceptionDate, 
        ISNULL(StartTime ,'') AS StartTime,      
        ISNULL(EndTime,'') AS EndTime,         
        ISNULL(IsAvailable, 0) AS IsAvailable,
        ISNULL(Reason, '') AS Reason,
        ISNULL(IsActive, '') AS IsActive,
        ISNULL(CreatedBy, '') AS CreatedBy,
        ISNULL(CreatedOn, '') AS CreatedOn,
        ISNULL(LastModifiedBy, '') AS ModifiedBy,
        ISNULL(LastModifiedOn, '') AS ModifiedOn
    FROM
        DoctorAvailabilityExceptions
    WHERE IsActive = 1 AND DoctorId = @DoctorId
    ORDER BY DoctorId, ExceptionDate, StartTime
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

GO
CREATE OR ALTER PROCEDURE InsertOrUpdateDoctorDetails
(
    -- Personal Info
    @DoctorId INT = NULL,  -- Pass NULL for insert, DoctorId for update
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
    DECLARE @CurrentDate DATETIME = GETDATE();

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @DoctorRoleId INT;
        SELECT @DoctorRoleId = UserRoleId FROM UserRoles WHERE RoleName = 'Doctor';

        -- If DoctorId is NULL, insert; otherwise, update
        IF @DoctorId IS NULL
        BEGIN
            -- 1. Insert UserProfiles
            INSERT INTO UserProfiles
            (FirstName, LastName, Email, ContactNumber, UserName, HashedPassword, UserRoleId, Gender, DateOfBirth, IsActive, CreatedBy, CreatedOn)
            VALUES
            (@FirstName, @LastName, @Email, @ContactNumber, @UserName, @HashedPassword, @DoctorRoleId, @Gender, @DateOfBirth, 1, @CreatedBy, @CurrentDate);

            SET @UserId = SCOPE_IDENTITY();

            -- 2. Insert Addresses
            INSERT INTO Addresses
            (AddressLine, StateId, DistrictId, TalukaId, CityId, Pincode, IsActive, CreatedBy, CreatedOn)
            VALUES
            (@AddressLine, @StateId, @DistrictId, @TalukaId, @CityId, @Pincode, 1, @CreatedBy, @CurrentDate);

            SET @AddressId = SCOPE_IDENTITY();

            -- 3. Insert DoctorProfiles
            INSERT INTO DoctorProfiles
            (UserId, ExperienceYears, ConsultationFees, Description, HospitalName, AddressId, Rating, IsActive, CreatedBy, CreatedOn)
            VALUES
            (@UserId, @ExperienceYears, @ConsultationFees, @Description, @HospitalName, @AddressId, @Rating, 1, @CreatedBy, @CurrentDate);

            SET @DoctorId = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            -- 1. Get UserId and AddressId
            SELECT @UserId = UserId, @AddressId = AddressId
            FROM DoctorProfiles
            WHERE DoctorId = @DoctorId;

            -- 2. Update UserProfiles
            UPDATE UserProfiles
            SET FirstName = @FirstName,
                LastName = @LastName,
                Email = @Email,
                ContactNumber = @ContactNumber,
                UserName = @UserName,
                HashedPassword = @HashedPassword,
                Gender = @Gender,
                DateOfBirth = @DateOfBirth,
                LastModifiedBy = @CreatedBy,
                LastModifiedOn = @CurrentDate
            WHERE UserId = @UserId;

            -- 3. Update Addresses
            UPDATE Addresses
            SET AddressLine = @AddressLine,
                StateId = @StateId,
                DistrictId = @DistrictId,
                TalukaId = @TalukaId,
                CityId = @CityId,
                Pincode = @Pincode,
                LastModifiedBy = @CreatedBy,
                LastModifiedOn = @CurrentDate
            WHERE AddressId = @AddressId;

            -- 4. Update DoctorProfiles
            UPDATE DoctorProfiles
            SET ExperienceYears = @ExperienceYears,
                ConsultationFees = @ConsultationFees,
                Description = @Description,
                HospitalName = @HospitalName,
                Rating = @Rating,
                LastModifiedBy = @CreatedBy,
                LastModifiedOn = @CurrentDate
            WHERE DoctorId = @DoctorId;
        END

        -- --------------------------
        -- Handle Specializations
        -- --------------------------
        DECLARE @TempSpecializations TABLE (SpecializationId INT);
        INSERT INTO @TempSpecializations(SpecializationId)
        SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@Specializations, ',');

        -- Delete removed specializations (hard delete)
        DELETE ds
        FROM DoctorSpecializations ds
        LEFT JOIN @TempSpecializations ts ON ds.SpecializationId = ts.SpecializationId
        WHERE ds.DoctorId = @DoctorId AND ts.SpecializationId IS NULL;

        -- Update existing specializations timestamps
        UPDATE ds
        SET LastModifiedBy = @CreatedBy,
            LastModifiedOn = @CurrentDate
        FROM DoctorSpecializations ds
        INNER JOIN @TempSpecializations ts ON ds.SpecializationId = ts.SpecializationId
        WHERE ds.DoctorId = @DoctorId;

        -- Insert new specializations
        INSERT INTO DoctorSpecializations (DoctorId, SpecializationId, IsActive, CreatedBy, CreatedOn)
        SELECT @DoctorId, ts.SpecializationId, 1, @CreatedBy, @CurrentDate
        FROM @TempSpecializations ts
        WHERE NOT EXISTS (
            SELECT 1 FROM DoctorSpecializations ds
            WHERE ds.DoctorId = @DoctorId AND ds.SpecializationId = ts.SpecializationId
        );

        -- --------------------------
        -- Handle Qualifications
        -- --------------------------
        DECLARE @TempQualifications TABLE (QualificationId INT);
        INSERT INTO @TempQualifications(QualificationId)
        SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@Qualifications, ',');

        -- Delete removed qualifications (hard delete)
        DELETE dq
        FROM DoctorQualifications dq
        LEFT JOIN @TempQualifications tq ON dq.QualificationId = tq.QualificationId
        WHERE dq.DoctorId = @DoctorId AND tq.QualificationId IS NULL;

        -- Update existing qualifications timestamps
        UPDATE dq
        SET LastModifiedBy = @CreatedBy,
            LastModifiedOn = @CurrentDate
        FROM DoctorQualifications dq
        INNER JOIN @TempQualifications tq ON dq.QualificationId = tq.QualificationId
        WHERE dq.DoctorId = @DoctorId;

        -- Insert new qualifications
        INSERT INTO DoctorQualifications (DoctorId, QualificationId, IsActive, CreatedBy, CreatedOn)
        SELECT @DoctorId, tq.QualificationId, 1, @CreatedBy, @CurrentDate
        FROM @TempQualifications tq
        WHERE NOT EXISTS (
            SELECT 1 FROM DoctorQualifications dq
            WHERE dq.DoctorId = @DoctorId AND dq.QualificationId = tq.QualificationId
        );

        -- Set output
        SET @CurrentDoctorId = @DoctorId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO





GO
CREATE OR ALTER PROCEDURE GetDoctorDetailsById
    @DoctorId INT
AS
BEGIN

    SELECT
        -- Doctor basic info
        ISNULL(D.DoctorId, 0) AS DoctorId,
        ISNULL(U.FirstName, '') AS FirstName,
        ISNULL(U.LastName, '') AS LastName,
        ISNULL(U.Email, '') AS Email,
		ISNULL(U.UserName,'') AS UserName,
		ISNULL(U.DateOfBirth,'') AS DateOfBirth,
        ISNULL(U.ContactNumber, '') AS ContactNumber,
        ISNULL(U.Gender, '') AS Gender,
        ISNULL(D.ExperienceYears, 0) AS ExperienceYears,
        ISNULL(D.ConsultationFees, 0) AS ConsultationFees,
        ISNULL(D.HospitalName, '') AS HospitalName,
        ISNULL(D.Description, '') AS Description,
        ISNULL(D.Rating, 0) AS Rating,

        -- Address info
		ISNULL(A.AddressLine,'') AS Address,
        ISNULL(A.AddressId, 0) AS AddressId,
        ISNULL(A.StateId, 0) AS StateId,
        ISNULL(A.DistrictId, 0) AS DistrictId,
        ISNULL(A.TalukaId, 0) AS TalukaId,
        ISNULL(A.CityId, 0) AS CityId,
		ISNULL(A.Pincode,'') AS Pincode,

        -- Specialization IDs & Names
        ISNULL(
            (
                SELECT STRING_AGG(CAST(DS.SpecializationId AS NVARCHAR), ',')
                FROM DoctorSpecializations DS
                WHERE DS.DoctorId = D.DoctorId AND DS.IsActive = 1
            ), ''
        ) AS SpecializationIds,

        ISNULL(
            (
                SELECT STRING_AGG(SP.SpecializationName, ',')
                FROM DoctorSpecializations DS
                INNER JOIN Specializations SP ON DS.SpecializationId = SP.SpecializationId
                WHERE DS.DoctorId = D.DoctorId AND DS.IsActive = 1
            ), ''
        ) AS SpecializationNames,

        -- Qualification IDs & Names
        ISNULL(
            (
                SELECT STRING_AGG(CAST(DQ.QualificationId AS NVARCHAR), ',')
                FROM DoctorQualifications DQ
                WHERE DQ.DoctorId = D.DoctorId AND DQ.IsActive = 1
            ), ''
        ) AS QualificationIds,

        ISNULL(
            (
                SELECT STRING_AGG(Q.QualificationName, ',')
                FROM DoctorQualifications DQ
                INNER JOIN Qualifications Q ON DQ.QualificationId = Q.QualificationId
                WHERE DQ.DoctorId = D.DoctorId AND DQ.IsActive = 1
            ), ''
        ) AS QualificationNames

    FROM DoctorProfiles D
    INNER JOIN UserProfiles U ON D.UserId = U.UserId
    LEFT JOIN Addresses A ON D.AddressId = A.AddressId
    WHERE D.DoctorId = @DoctorId;
END;
GO





GO
CREATE OR ALTER PROCEDURE InsertOrUpdateDoctorAvailability
    @DoctorId INT,
    @CreatedBy INT,
    @AvailabilitiesXml XML,
	@ExceptionsListXml XML, 
    @ResultCode INT OUTPUT
AS
BEGIN

    BEGIN TRY
        -- Begin a transaction
        BEGIN TRANSACTION;

        -- If DoctorId is not null or 0, optionally delete existing availabilities to replace with new ones
        IF @DoctorId IS NOT NULL
        BEGIN
            DELETE FROM DoctorAvailability
            WHERE DoctorId = @DoctorId;
        END

        -- Insert from XML
        IF @AvailabilitiesXml IS NOT NULL
        BEGIN
            INSERT INTO DoctorAvailability
            (
                DoctorId,
                DayOfWeek,
                StartTime,
                EndTime,
                SlotDuration,
                IsActive,
                CreatedBy,
                CreatedOn
            )
            SELECT
                @DoctorId AS DoctorId,
                T.c.value('(DayOfWeek)[1]', 'INT') AS DayOfWeek,
                T.c.value('(StartTime)[1]', 'TIME') AS StartTime,
                T.c.value('(EndTime)[1]', 'TIME') AS EndTime,
                T.c.value('(SlotDuration)[1]', 'INT') AS SlotDuration,
                1 AS IsActive,
                @CreatedBy AS CreatedBy,
                GETDATE() AS CreatedOn
            FROM @AvailabilitiesXml.nodes('/Availabilities/Availability') AS T(c);
        END

		 -- Delete existing exceptions for this doctor
        IF @DoctorId IS NOT NULL
        BEGIN
            DELETE FROM DoctorAvailabilityExceptions
            WHERE DoctorId = @DoctorId;
        END

        -- Insert from Exceptions XML
        IF @ExceptionsListXml IS NOT NULL
        BEGIN
            INSERT INTO DoctorAvailabilityExceptions
            (
                DoctorId,
                ExceptionDate,
                StartTime,
                EndTime,
                IsAvailable,
                Reason,
                IsActive,
                CreatedBy,
                CreatedOn
            )
            SELECT
                @DoctorId AS DoctorId,
                T.c.value('(ExceptionDate)[1]', 'DATE') AS ExceptionDate,
                T.c.value('(StartTime)[1]', 'TIME') AS StartTime,
                T.c.value('(EndTime)[1]', 'TIME') AS EndTime,
                T.c.value('(IsAvailable)[1]', 'INT') AS IsAvailable,
                T.c.value('(Reason)[1]', 'NVARCHAR(255)') AS Reason,
                1 AS IsActive,
                @CreatedBy AS CreatedBy,
                GETDATE() AS CreatedOn
            FROM @ExceptionsListXml.nodes('/Exceptions/Exception') AS T(c);
        END

        -- Set result code to success
        SET @ResultCode = 1;

        COMMIT TRANSACTION;
    END TRY
    --BEGIN CATCH
    --    -- Rollback on error
    --    IF @@TRANCOUNT > 0
    --        ROLLBACK TRANSACTION;

    --    -- Set result code to -1 for error
    --    SET @ResultCode = -1;
    --END CATCH

	BEGIN CATCH
    DECLARE @ErrMsg NVARCHAR(4000), @ErrSeverity INT;
    SELECT @ErrMsg = ERROR_MESSAGE(), @ErrSeverity = ERROR_SEVERITY();
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    RAISERROR('Error in InsertOrUpdateDoctorAvailability: %s', @ErrSeverity, 1, @ErrMsg);
    SET @ResultCode = -1;
END CATCH
END
GO


---Final Version
GO
CREATE OR ALTER PROCEDURE InsertOrUpdateDoctorAvailability
(
    @DoctorId INT,
    @CreatedBy INT,
    @AvailabilitiesXml XML,
    @ExceptionsListXml XML,
    @ResultCode INT OUTPUT
)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -------------------------
        -- Doctor Availability
        -------------------------
        DECLARE @TempAvailabilities TABLE
        (
            DayOfWeek INT,
            StartTime TIME,
            EndTime TIME,
            SlotDuration INT
        );

        -- Load XML into temp table
        IF @AvailabilitiesXml IS NOT NULL
        BEGIN
            INSERT INTO @TempAvailabilities (DayOfWeek, StartTime, EndTime, SlotDuration)
            SELECT
                T.c.value('(DayOfWeek)[1]', 'INT'),
                T.c.value('(StartTime)[1]', 'TIME'),
                T.c.value('(EndTime)[1]', 'TIME'),
                T.c.value('(SlotDuration)[1]', 'INT')
            FROM @AvailabilitiesXml.nodes('/Availabilities/Availability') AS T(c);
        END

        -- Soft delete removed availabilities
        UPDATE DA
        SET DA.IsActive = 0
        FROM DoctorAvailability DA
        LEFT JOIN @TempAvailabilities T
            ON DA.DayOfWeek = T.DayOfWeek
            AND DA.StartTime = T.StartTime
            AND DA.EndTime = T.EndTime
            AND DA.SlotDuration = T.SlotDuration
        WHERE DA.DoctorId = @DoctorId
          AND T.DayOfWeek IS NULL
          AND DA.IsActive = 1;

        -- Reactivate existing availabilities if present in XML
        UPDATE DA
        SET DA.IsActive = 1
        FROM DoctorAvailability DA
        JOIN @TempAvailabilities T
            ON DA.DoctorId = @DoctorId
            AND DA.DayOfWeek = T.DayOfWeek
            AND DA.StartTime = T.StartTime
            AND DA.EndTime = T.EndTime
            AND DA.SlotDuration = T.SlotDuration
        WHERE DA.IsActive = 0;

        -- Insert new availabilities
        INSERT INTO DoctorAvailability
        (
            DoctorId,
            DayOfWeek,
            StartTime,
            EndTime,
            SlotDuration,
            IsActive,
            CreatedBy,
            CreatedOn
        )
        SELECT
            @DoctorId,
            T.DayOfWeek,
            T.StartTime,
            T.EndTime,
            T.SlotDuration,
            1,
            @CreatedBy,
            GETDATE()
        FROM @TempAvailabilities T
        WHERE NOT EXISTS (
            SELECT 1 FROM DoctorAvailability DA
            WHERE DA.DoctorId = @DoctorId
              AND DA.DayOfWeek = T.DayOfWeek
              AND DA.StartTime = T.StartTime
              AND DA.EndTime = T.EndTime
              AND DA.SlotDuration = T.SlotDuration
        );

        -------------------------
        -- Doctor Availability Exceptions
        -------------------------
        DECLARE @TempExceptions TABLE
        (
            ExceptionDate DATE,
            StartTime TIME,
            EndTime TIME,
            IsAvailable INT,
            Reason NVARCHAR(255)
        );

        -- Load exceptions XML into temp table
        IF @ExceptionsListXml IS NOT NULL
        BEGIN
            INSERT INTO @TempExceptions (ExceptionDate, StartTime, EndTime, IsAvailable, Reason)
            SELECT
                T.c.value('(ExceptionDate)[1]', 'DATE'),
                T.c.value('(StartTime)[1]', 'TIME'),
                T.c.value('(EndTime)[1]', 'TIME'),
                T.c.value('(IsAvailable)[1]', 'INT'),
                T.c.value('(Reason)[1]', 'NVARCHAR(255)')
            FROM @ExceptionsListXml.nodes('/Exceptions/Exception') AS T(c);
        END

        -- Soft delete removed exceptions
        UPDATE DAE
        SET DAE.IsActive = 0
        FROM DoctorAvailabilityExceptions DAE
        LEFT JOIN @TempExceptions T
            ON DAE.ExceptionDate = T.ExceptionDate
            AND ((DAE.StartTime = T.StartTime) OR (DAE.StartTime IS NULL AND T.StartTime IS NULL))
            AND ((DAE.EndTime = T.EndTime) OR (DAE.EndTime IS NULL AND T.EndTime IS NULL))
        WHERE DAE.DoctorId = @DoctorId
          AND T.ExceptionDate IS NULL
          AND DAE.IsActive = 1;

        -- Reactivate existing exceptions if present in XML
        UPDATE DAE
        SET DAE.IsActive = 1,
            DAE.IsAvailable = T.IsAvailable,
            DAE.Reason = T.Reason
        FROM DoctorAvailabilityExceptions DAE
        JOIN @TempExceptions T
            ON DAE.DoctorId = @DoctorId
            AND DAE.ExceptionDate = T.ExceptionDate
            AND ((DAE.StartTime = T.StartTime) OR (DAE.StartTime IS NULL AND T.StartTime IS NULL))
            AND ((DAE.EndTime = T.EndTime) OR (DAE.EndTime IS NULL AND T.EndTime IS NULL));

        -- Insert new exceptions
        INSERT INTO DoctorAvailabilityExceptions
        (
            DoctorId,
            ExceptionDate,
            StartTime,
            EndTime,
            IsAvailable,
            Reason,
            IsActive,
            CreatedBy,
            CreatedOn
        )
        SELECT
            @DoctorId,
            T.ExceptionDate,
            T.StartTime,
            T.EndTime,
            T.IsAvailable,
            T.Reason,
            1,
            @CreatedBy,
            GETDATE()
        FROM @TempExceptions T
        WHERE NOT EXISTS (
            SELECT 1 FROM DoctorAvailabilityExceptions DAE
            WHERE DAE.DoctorId = @DoctorId
              AND DAE.ExceptionDate = T.ExceptionDate
              AND ((DAE.StartTime = T.StartTime) OR (DAE.StartTime IS NULL AND T.StartTime IS NULL))
              AND ((DAE.EndTime = T.EndTime) OR (DAE.EndTime IS NULL AND T.EndTime IS NULL))
        );

        SET @ResultCode = 1;
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @ResultCode = -1;
        THROW;
    END CATCH
END
GO


---Final Version
GO
CREATE OR ALTER PROCEDURE AppointmentRequestsGetList
    @PatientName NVARCHAR(100) = NULL,
    @DoctorName NVARCHAR(100) = NULL,
    @SpecializationId INT = NULL,
    @FromDate DATE = NULL,
    @ToDate DATE = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Temporary table to store filtered results
    CREATE TABLE #FilteredAppointments
    (
        AppointmentRequestId INT,
        PatientId INT,
        PatientName NVARCHAR(100),
        DoctorId INT,
        DoctorName NVARCHAR(100),
        SpecializationId INT,
        SpecializationName NVARCHAR(150),
        MedicalConcern NVARCHAR(300),
        PreferredDate DATE,
        StartTime TIME,
        EndTime TIME,
        FinalStartTime TIME,
        FinalEndTime TIME,
        FinalDate DATE,
        StatusId INT,
        StatusName NVARCHAR(50),
        CreatedOn DATETIME,
        LastModifiedBy INT,
        ModifiedBy NVARCHAR(200),
        LastModifiedOn DATETIME
    );

    -- Insert filtered data
    INSERT INTO #FilteredAppointments
    SELECT
        AR.AppointmentRequestId,
        AR.PatientId,
        ISNULL(PP.FirstName,'') + ' ' + ISNULL(PP.LastName,'') AS PatientName,
        AR.DoctorId,
        ISNULL(U.FirstName,'') + ' ' + ISNULL(U.LastName,'') AS DoctorName,
        AR.SpecializationId,
        ISNULL(SP.SpecializationName,'') AS SpecializationName,
        AR.MedicalConcern,
        AR.PreferredDate,
        AR.StartTime,
        AR.EndTime,
        AR.FinalStartTime,
        AR.FinalEndTime,
        AR.FinalDate,
        AR.StatusId,
        ISNULL(S.StatusName,'') AS StatusName,
        AR.CreatedOn,
        AR.LastModifiedBy,
        ISNULL(UM.FirstName,'') + ' ' + ISNULL(UM.LastName,'') AS ModifiedBy,
        AR.LastModifiedOn
    FROM AppointmentRequests AR
    LEFT JOIN PatientProfiles PP ON AR.PatientId = PP.PatientId
    LEFT JOIN DoctorProfiles DP ON AR.DoctorId = DP.DoctorId
    LEFT JOIN UserProfiles U ON DP.UserId = U.UserId
    LEFT JOIN Specializations SP ON AR.SpecializationId = SP.SpecializationId
    LEFT JOIN Statuses S ON AR.StatusId = S.StatusId
    LEFT JOIN UserProfiles UM ON AR.LastModifiedBy = UM.UserId
    WHERE 1=1
        AND (@PatientName IS NULL OR PP.FirstName + ' ' + PP.LastName LIKE '%' + @PatientName + '%')
        AND (@DoctorName IS NULL OR U.FirstName + ' ' + U.LastName LIKE '%' + @DoctorName + '%')
        AND (@SpecializationId IS NULL OR AR.SpecializationId = @SpecializationId)
		AND (@FromDate IS NULL OR  AR.PreferredDate >= @FromDate)
        AND (@ToDate IS NULL OR  AR.PreferredDate < DATEADD(DAY, 1, @ToDate))

    -- Total count
    SELECT @TotalCount = COUNT(*) FROM #FilteredAppointments;

    -- Paginated results
    SELECT *
    FROM #FilteredAppointments
    ORDER BY AppointmentRequestId DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    DROP TABLE #FilteredAppointments;
END
GO



GO
CREATE OR ALTER PROCEDURE DoctorAppointmentRequestsGetList
    @DoctorId INT,                         
    @PatientName NVARCHAR(100) = NULL,
    @SpecializationId INT = NULL,
	@StatusId INT = NULL,
    @FromDate DATE = NULL,
    @ToDate DATE = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN

    -- Temporary table to store filtered results
    CREATE TABLE #FilteredAppointments
    (
        AppointmentRequestId INT,
        PatientId INT,
        PatientName NVARCHAR(100),
        DoctorId INT,
        DoctorName NVARCHAR(100),
		PatientEmail NVARCHAR(150),
        SpecializationId INT,
        SpecializationName NVARCHAR(150),
        MedicalConcern NVARCHAR(300),
        FinalStartTime TIME,
        FinalEndTime TIME,
        FinalDate DATE,
        StatusId INT,
        StatusName NVARCHAR(50),
        CreatedOn DATETIME,
        LastModifiedBy INT,
        ModifiedBy NVARCHAR(200),
        LastModifiedOn DATETIME
    );

    -- Insert filtered data
    INSERT INTO #FilteredAppointments
    SELECT
        AR.AppointmentRequestId,
        AR.PatientId,
        ISNULL(PP.FirstName,'') + ' ' + ISNULL(PP.LastName,'') AS PatientName,		
        AR.DoctorId,
        ISNULL(U.FirstName,'') + ' ' + ISNULL(U.LastName,'') AS DoctorName,
		ISNULL(PP.Email,'') AS PatientEmail,
        AR.SpecializationId,
        ISNULL(SP.SpecializationName,'') AS SpecializationName,
        AR.MedicalConcern,
        AR.FinalStartTime,
        AR.FinalEndTime,
        AR.FinalDate,
        AR.StatusId,
        ISNULL(S.StatusName,'') AS StatusName,
        AR.CreatedOn,
        AR.LastModifiedBy,
        ISNULL(UM.FirstName,'') + ' ' + ISNULL(UM.LastName,'') AS ModifiedBy,
        AR.LastModifiedOn
    FROM AppointmentRequests AR
    LEFT JOIN PatientProfiles PP ON AR.PatientId = PP.PatientId
    LEFT JOIN DoctorProfiles DP ON AR.DoctorId = DP.DoctorId
    LEFT JOIN UserProfiles U ON DP.UserId = U.UserId
    LEFT JOIN Specializations SP ON AR.SpecializationId = SP.SpecializationId
    LEFT JOIN Statuses S ON AR.StatusId = S.StatusId
    LEFT JOIN UserProfiles UM ON AR.LastModifiedBy = UM.UserId
    WHERE AR.DoctorId = @DoctorId                       -- Filter for this doctor
        AND (@PatientName IS NULL OR PP.FirstName + ' ' + PP.LastName LIKE '%' + @PatientName + '%')
        AND (@SpecializationId IS NULL OR AR.SpecializationId = @SpecializationId)
		AND (@StatusId IS NULL OR AR.StatusId = @StatusId)
        AND (@FromDate IS NULL OR AR.PreferredDate >= @FromDate)
        AND (@ToDate IS NULL OR AR.PreferredDate < DATEADD(DAY, 1, @ToDate))

    -- Total count
    SELECT @TotalCount = COUNT(*) FROM #FilteredAppointments;

    -- Paginated results
    SELECT *
    FROM #FilteredAppointments
    ORDER BY CreatedOn DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    DROP TABLE #FilteredAppointments;
END
GO

GO
CREATE OR ALTER PROCEDURE UpdateAppointmentStatus
    @AppointmentRequestId INT,
    @StatusName NVARCHAR(50)
AS
BEGIN

   
    DECLARE @StatusId INT;
    SELECT @StatusId = StatusId
    FROM Statuses
    WHERE StatusName = @StatusName;

    UPDATE AppointmentRequests
    SET StatusId = @StatusId,
        LastModifiedOn = GETDATE()
    WHERE AppointmentRequestId = @AppointmentRequestId;
END
GO

GO
CREATE OR ALTER PROCEDURE GetUserPasswordByUsername
    @UserName NVARCHAR(100)
AS
BEGIN

    SELECT 
	    up.UserId,
        up.HashedPassword,
        up.UserRoleId,
        ur.RoleName,
		up.Email,
		up.UserName
    FROM 
        UserProfiles up
    INNER JOIN 
        UserRoles ur
        ON up.UserRoleId = ur.UserRoleId
    WHERE 
        up.UserName = @UserName
        AND up.IsActive = 1;
END
GO


GO
CREATE OR ALTER PROCEDURE GetDoctorAvailableSlots
    @DoctorId INT
AS
BEGIN

    SELECT 
        DS.SlotId,
        DS.DoctorId,
        DS.SlotDate,
        DS.StartTime,
        DS.EndTime,
        DS.StatusId,
        S.StatusName
    FROM DoctorSlots DS
    INNER JOIN Statuses S ON DS.StatusId = S.StatusId
    WHERE DS.DoctorId = @DoctorId
      AND S.StatusName = 'Available' 
    ORDER BY DS.SlotDate, DS.StartTime;
END
GO

GO
CREATE OR ALTER PROCEDURE GetPatientPersonalInfoByPhone
    @PhoneNumber NVARCHAR(15)
AS
BEGIN

    SELECT
        p.PatientId,
        p.FirstName,
        p.LastName,
        p.Email,
        p.PhoneNumber AS ContactNumber,
        p.DateOfBirth,
        p.Gender,
        a.AddressLine AS AddressLine,
        a.StateId,
        a.DistrictId,
        a.TalukaId,
        a.CityId,
        a.Pincode
    FROM
        PatientProfiles p
    INNER JOIN
        Addresses a ON p.AddressId = a.AddressId
    WHERE
        p.PhoneNumber = @PhoneNumber
        AND p.IsActive = 1
        AND a.IsActive = 1;
END





