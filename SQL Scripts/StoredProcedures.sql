


GO
CREATE OR ALTER PROCEDURE DoctorsGetList  
    @DoctorName NVARCHAR(100) = NULL,
    @SpecializationId INT = NULL,
    @CityId INT = NULL,
    @Gender CHAR(1) = NULL,
    @PageNumber INT,
    @PageSize INT,
    @TotalCount INT OUTPUT

/*  
***********************************************************************************************  
 Date      Modified By     Purpose of Modification  
1 10Oct2025  Asmatali         Get List of Doctors  
***********************************************************************************************  
DoctorsGetList  
*/
AS
BEGIN

    -- Create temp table for filtered results
    CREATE TABLE #FilteredDoctors
    (
        DoctorId INT,
        FirstName NVARCHAR(50),
        LastName NVARCHAR(50),
        Gender CHAR(1),
        Email NVARCHAR(100),
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
        ISNULL(U.Email, '') AS Email,
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
		(
			SELECT STRING_AGG(SP.SpecializationName, ', ')
			FROM DoctorSpecializations DS2
			INNER JOIN Specializations SP ON DS2.SpecializationId = SP.SpecializationId
			WHERE DS2.DoctorId = D.DoctorId
		) AS SpecializationNames,
		(
			SELECT STRING_AGG(Q.QualificationName, ', ')
			FROM DoctorQualifications DQ
			INNER JOIN Qualifications Q ON DQ.QualificationId = Q.QualificationId
			WHERE DQ.DoctorId = D.DoctorId AND Q.IsActive = 1
		) AS QualificationNames
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
      AND (@Gender IS NULL OR U.Gender = @Gender)
      AND (
            @SpecializationId IS NULL
            OR EXISTS (
                SELECT 1
                FROM DoctorSpecializations DS
                WHERE DS.DoctorId = D.DoctorId
                  AND DS.SpecializationId = @SpecializationId
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
CREATE OR ALTER   PROCEDURE DoctorSpecializationGetList  
  
/*  
***********************************************************************************************  
 Date      Modified By     Purpose of Modification  
10Oct2025  Asmatali         Get List for Doctor Specializations  
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
10Oct2025 Asmatali        Get List for Doctor Qualifications
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
/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
10Oct2025 Asmatali        Get List for States
***********************************************************************************************
StatesGetList
*/
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
/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
10Oct2025 Asmatali        Get List for Districts
***********************************************************************************************
DistrictsGetList
*/
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
/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
10Oct2025 Asmatali        Get List for Talukas
***********************************************************************************************
TalukasGetList
*/
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
/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
10Oct2025 Asmatali        Get List for Cities
***********************************************************************************************
CitiesGetList
*/
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
/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
13Oct2025 Asmatali        Get List for Statuses
***********************************************************************************************
StatusesGetList
*/
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
CREATE OR ALTER PROCEDURE DoctorAvailabilityGetList
    @DoctorId INT

	/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
13Oct2025 Asmatali        Get List for Doctor Availability
***********************************************************************************************
DoctorAvailabilityGetList
*/
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

 	/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
13Oct2025 Asmatali        Get List for Doctor Availability Exceptions
***********************************************************************************************
DoctorAvailabilityExceptionsGetList
*/
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
CREATE OR ALTER PROCEDURE InsertOrUpdateDoctorDetails
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

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
14Oct2025 Asmatali        Insert Or Update Doctor Details
***********************************************************************************************
InsertOrUpdateDoctorDetails
*/
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
            --  Insert UserProfiles
            INSERT INTO UserProfiles
            (
				FirstName,
				LastName,
				Email,
				ContactNumber,
				UserName,
				HashedPassword,
				UserRoleId,
				Gender,
				DateOfBirth,
				IsActive,
				CreatedBy,
				CreatedOn
			)
            VALUES
            (
				@FirstName,
				@LastName,
				@Email,
				@ContactNumber,
				@UserName,
				@HashedPassword,
				@DoctorRoleId,
				@Gender,
				@DateOfBirth,
				1,
				@CreatedBy,
				@CurrentDate
			);

            SET @UserId = SCOPE_IDENTITY();

            --  Insert Addresses
            INSERT INTO Addresses
            (
				AddressLine,
				StateId,
				DistrictId,
				TalukaId,
				CityId,
				Pincode,
				IsActive,
				CreatedOn
			)
            VALUES
            (
				@AddressLine,
				@StateId,
				@DistrictId,
				@TalukaId,
				@CityId,
				@Pincode,
				1,
				@CurrentDate
			);

            SET @AddressId = SCOPE_IDENTITY();

            --  Insert DoctorProfiles
            INSERT INTO
				DoctorProfiles
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
            (
				@UserId,
				@ExperienceYears,
				@ConsultationFees,
				@Description,
				@HospitalName,
				@AddressId,
				@Rating,
				1,
				@CreatedBy,
				@CurrentDate
			);

            SET @DoctorId = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            -- Get UserId and AddressId
            SELECT @UserId = UserId, @AddressId = AddressId
            FROM DoctorProfiles
            WHERE DoctorId = @DoctorId;

            --  Update UserProfiles
            UPDATE
				UserProfiles
            SET
			    FirstName = @FirstName,
                LastName = @LastName,
                Email = @Email,
                ContactNumber = @ContactNumber,
                UserName = @UserName,
                Gender = @Gender,
                DateOfBirth = @DateOfBirth,
                LastModifiedBy = @CreatedBy,
                LastModifiedOn = @CurrentDate
            WHERE
				UserId = @UserId;

            --  Update Addresses
            UPDATE
				Addresses
            SET 
				AddressLine = @AddressLine,
                StateId = @StateId,
                DistrictId = @DistrictId,
                TalukaId = @TalukaId,
                CityId = @CityId,
                Pincode = @Pincode,
                LastModifiedBy = @CreatedBy,
                LastModifiedOn = @CurrentDate
            WHERE
				AddressId = @AddressId;

            --  Update DoctorProfiles
            UPDATE
				DoctorProfiles
            SET
				ExperienceYears = @ExperienceYears,
                ConsultationFees = @ConsultationFees,
                Description = @Description,
                HospitalName = @HospitalName,
                Rating = @Rating,
                LastModifiedBy = @CreatedBy,
                LastModifiedOn = @CurrentDate
            WHERE 
				DoctorId = @DoctorId;
        END

        -- Handle Specializations

        DECLARE @TempSpecializations TABLE (SpecializationId INT);

        INSERT INTO @TempSpecializations(SpecializationId)
        SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@Specializations, ',');

        -- Delete removed specializations (hard delete)
        DELETE ds
        FROM
			DoctorSpecializations ds
        LEFT JOIN
			@TempSpecializations ts ON ds.SpecializationId = ts.SpecializationId
        WHERE
			ds.DoctorId = @DoctorId AND ts.SpecializationId IS NULL;

        -- Insert new specializations
        INSERT INTO
			DoctorSpecializations
			(
			DoctorId,
			SpecializationId,
			CreatedBy,
			CreatedOn
			)
        SELECT 
			@DoctorId,
			ts.SpecializationId,
			@CreatedBy,
			@CurrentDate
        FROM 
			@TempSpecializations ts
        WHERE NOT EXISTS (
            SELECT 1 FROM DoctorSpecializations ds
            WHERE ds.DoctorId = @DoctorId AND ds.SpecializationId = ts.SpecializationId
        );

        -- Handle Qualifications

        DECLARE @TempQualifications TABLE (QualificationId INT);

        INSERT INTO @TempQualifications(QualificationId)
        SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@Qualifications, ',');

        -- Delete removed qualifications (hard delete)
        DELETE
			dq
        FROM
			DoctorQualifications dq
        LEFT JOIN
			@TempQualifications tq ON dq.QualificationId = tq.QualificationId
        WHERE
			dq.DoctorId = @DoctorId AND tq.QualificationId IS NULL;


        -- Insert new qualifications
        INSERT INTO
			DoctorQualifications
			(
			DoctorId,
			QualificationId,
			CreatedBy,
			CreatedOn
			)
        SELECT
			@DoctorId,
			tq.QualificationId,
			@CreatedBy,
			@CurrentDate
        FROM
			@TempQualifications tq
        WHERE NOT EXISTS (
            SELECT 1 FROM DoctorQualifications dq
            WHERE dq.DoctorId = @DoctorId AND dq.QualificationId = tq.QualificationId
        );

        -- Set output
        SET @CurrentDoctorId = @DoctorId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		THROW;
    END CATCH
END
GO





GO
CREATE OR ALTER PROCEDURE GetDoctorDetailsById
    @DoctorId INT

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
14Oct2025 Asmatali        Get Doctor Details By DoctorId
***********************************************************************************************
GetDoctorDetailsById
*/
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
                WHERE DS.DoctorId = D.DoctorId
            ), ''
        ) AS SpecializationIds,

        ISNULL(
            (
                SELECT STRING_AGG(SP.SpecializationName, ',')
                FROM DoctorSpecializations DS
                INNER JOIN Specializations SP ON DS.SpecializationId = SP.SpecializationId
                WHERE DS.DoctorId = D.DoctorId
            ), ''
        ) AS SpecializationNames,

        -- Qualification IDs & Names
        ISNULL(
            (
                SELECT STRING_AGG(CAST(DQ.QualificationId AS NVARCHAR), ',')
                FROM DoctorQualifications DQ
                WHERE DQ.DoctorId = D.DoctorId
            ), '') AS QualificationIds,

        ISNULL(
            (
                SELECT STRING_AGG(Q.QualificationName, ',')
                FROM DoctorQualifications DQ
                INNER JOIN Qualifications Q ON DQ.QualificationId = Q.QualificationId
                WHERE DQ.DoctorId = D.DoctorId
            ), '') AS QualificationNames

    FROM DoctorProfiles D
    INNER JOIN UserProfiles U ON D.UserId = U.UserId
    LEFT JOIN Addresses A ON D.AddressId = A.AddressId
    WHERE D.DoctorId = @DoctorId;
END;
GO



---Final Version
GO
CREATE OR ALTER PROCEDURE InsertOrUpdateDoctorAvailability

    @DoctorId INT,
    @CreatedBy INT,
    @AvailabilitiesXml XML,
    @ExceptionsListXml XML,
    @ResultCode INT OUTPUT
/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
14Oct2025 Asmatali        Insert Or Update Doctor Availability
***********************************************************************************************
InsertOrUpdateDoctorAvailability
*/
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Doctor Availability
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
        UPDATE
			DA
        SET
			DA.IsActive = 0,
		    DA.LastModifiedBy = @CreatedBy,
            DA.LastModifiedOn = GETDATE()
        FROM
			DoctorAvailability DA
        LEFT JOIN
			@TempAvailabilities T ON DA.DayOfWeek = T.DayOfWeek
			AND DA.StartTime = T.StartTime AND
			DA.EndTime = T.EndTime AND
			DA.SlotDuration = T.SlotDuration
        WHERE
			DA.DoctorId = @DoctorId
            AND T.DayOfWeek IS NULL
            AND DA.IsActive = 1;

        -- Reactivate existing availabilities if present in XML
        UPDATE
			DA
        SET 
			DA.IsActive = 1,
		    DA.LastModifiedBy = @CreatedBy,
            DA.LastModifiedOn = GETDATE()
        FROM DoctorAvailability DA
        JOIN @TempAvailabilities T
            ON DA.DoctorId = @DoctorId
            AND DA.DayOfWeek = T.DayOfWeek
            AND DA.StartTime = T.StartTime
            AND DA.EndTime = T.EndTime
            AND DA.SlotDuration = T.SlotDuration
        WHERE
			DA.IsActive = 0;

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

        -- Doctor Availability Exceptions
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
        UPDATE
			DAE
        SET
			DAE.IsActive = 0,
		    DAE.LastModifiedBy = @CreatedBy,
            DAE.LastModifiedOn = GETDATE()
        FROM
			DoctorAvailabilityExceptions DAE
        LEFT JOIN @TempExceptions T
            ON DAE.ExceptionDate = T.ExceptionDate
            AND ((DAE.StartTime = T.StartTime) OR (DAE.StartTime IS NULL AND T.StartTime IS NULL))
            AND ((DAE.EndTime = T.EndTime) OR (DAE.EndTime IS NULL AND T.EndTime IS NULL))
        WHERE DAE.DoctorId = @DoctorId
          AND T.ExceptionDate IS NULL
          AND DAE.IsActive = 1;

        -- Reactivate existing exceptions if present in XML
        UPDATE
			DAE
        SET DAE.IsActive = 1,
            DAE.IsAvailable = T.IsAvailable,
            DAE.Reason = T.Reason,
			DAE.LastModifiedBy = @CreatedBy,
            DAE.LastModifiedOn = GETDATE()
        FROM 
			DoctorAvailabilityExceptions DAE
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


GO
CREATE OR ALTER PROCEDURE AppointmentRequestsGetList
    @PatientName NVARCHAR(100) = NULL,
    @DoctorName NVARCHAR(100) = NULL,
    @SpecializationId INT = NULL,
	@StatusId INT = NULL,
	@AppointmentType NVARCHAR(20) = 'Upcoming',
    @FromDate DATE = NULL,
    @ToDate DATE = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
13Oct2025 Asmatali        Get List for Appointment Requests
***********************************************************************************************
AppointmentRequestsGetList
*/
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
        ISNULL(AR.AppointmentRequestId, '') AS AppointmentRequestId,
		ISNULL(AR.PatientId, '') AS PatientId,
		ISNULL(PP.FirstName, '') + ' ' + ISNULL(PP.LastName, '') AS PatientName,
		ISNULL(AR.DoctorId, '') AS DoctorId,
		ISNULL(U.FirstName, '') + ' ' + ISNULL(U.LastName, '') AS DoctorName,
		ISNULL(AR.SpecializationId, '') AS SpecializationId,
		ISNULL(SP.SpecializationName, '') AS SpecializationName,
		ISNULL(AR.MedicalConcern, '') AS MedicalConcern,
		ISNULL(AR.PreferredDate, '') AS PreferredDate,
		ISNULL(AR.StartTime, '') AS StartTime,
		ISNULL(AR.EndTime, '') AS EndTime,
		ISNULL(AR.FinalStartTime, '') AS FinalStartTime,
		ISNULL(AR.FinalEndTime, '') AS FinalEndTime,
		ISNULL(AR.FinalDate, '') AS FinalDate,
		ISNULL(AR.StatusId, '') AS StatusId,
		ISNULL(S.StatusName, '') AS StatusName,
		ISNULL(AR.CreatedOn, '') AS CreatedOn,
		ISNULL(AR.LastModifiedBy, '') AS LastModifiedBy,
		ISNULL(UM.FirstName, '') + ' ' + ISNULL(UM.LastName, '') AS ModifiedBy,
		ISNULL(AR.LastModifiedOn, '') AS LastModifiedOn
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
		AND (@StatusId IS NULL OR AR.StatusId = @StatusId)
		AND (
			@AppointmentType = 'All'
			OR (
				@AppointmentType = 'Upcoming'
				AND DATEADD(SECOND, DATEDIFF(SECOND, 0, AR.FinalStartTime), CAST(AR.FinalDate AS DATETIME)) >= GETDATE()
			    )
			OR (
				@AppointmentType = 'Past'
				AND DATEADD(SECOND, DATEDIFF(SECOND, 0, AR.FinalEndTime), CAST(AR.FinalDate AS DATETIME)) < GETDATE()
			   )
		     )
		AND (@FromDate IS NULL OR  AR.FinalDate >= @FromDate)
        AND (@ToDate IS NULL OR  AR.FinalDate < DATEADD(DAY, 1, @ToDate))

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
	@StatusId INT = NULL,
	@AppointmentType NVARCHAR(20) = 'Upcoming',
    @FromDate DATE = NULL,
    @ToDate DATE = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
14Oct2025 Asmatali        Get List for Appointment Requests By DoctorId
***********************************************************************************************
DoctorAppointmentRequestsGetList
*/
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
		DoctorEmail NVARCHAR(150),
		PatientEmail NVARCHAR(150),
        SpecializationId INT,
        SpecializationName NVARCHAR(150),
        MedicalConcern NVARCHAR(300),
		SlotId INT,
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
         ISNULL(AR.AppointmentRequestId, ' ') AS AppointmentRequestId,
		ISNULL(AR.PatientId, ' ') AS PatientId,
		ISNULL(PP.FirstName, ' ') + ' ' + ISNULL(PP.LastName, ' ') AS PatientName,		
		ISNULL(AR.DoctorId, ' ') AS DoctorId,
		ISNULL(U.FirstName, ' ') + ' ' + ISNULL(U.LastName, ' ') AS DoctorName,
		ISNULL(U.Email, ' ') AS DoctorEmail,
		ISNULL(PP.Email, ' ') AS PatientEmail,
		ISNULL(AR.SpecializationId, ' ') AS SpecializationId,
		ISNULL(SP.SpecializationName, ' ') AS SpecializationName,
		ISNULL(AR.MedicalConcern, ' ') AS MedicalConcern,
		ISNULL(DS.SlotId, ' ') AS SlotId, 
		ISNULL(AR.FinalStartTime, ' ') AS FinalStartTime,
		ISNULL(AR.FinalEndTime, ' ') AS FinalEndTime,
		ISNULL(AR.FinalDate, ' ') AS FinalDate,
		ISNULL(AR.StatusId, ' ') AS StatusId,
		ISNULL(S.StatusName, ' ') AS StatusName,
		ISNULL(AR.CreatedOn, ' ') AS CreatedOn,
		ISNULL(AR.LastModifiedBy, ' ') AS LastModifiedBy,
		ISNULL(UM.FirstName, ' ') + ' ' + ISNULL(UM.LastName, ' ') AS ModifiedBy,
		ISNULL(AR.LastModifiedOn, ' ') AS LastModifiedOn
    FROM AppointmentRequests AR
    LEFT JOIN PatientProfiles PP ON AR.PatientId = PP.PatientId
    LEFT JOIN DoctorProfiles DP ON AR.DoctorId = DP.DoctorId
    LEFT JOIN UserProfiles U ON DP.UserId = U.UserId
    LEFT JOIN Specializations SP ON AR.SpecializationId = SP.SpecializationId
    LEFT JOIN Statuses S ON AR.StatusId = S.StatusId
    LEFT JOIN UserProfiles UM ON AR.LastModifiedBy = UM.UserId
	    LEFT JOIN DoctorSlots DS 
        ON AR.DoctorId = DS.DoctorId
        AND AR.FinalDate = DS.SlotDate
        AND AR.FinalStartTime = DS.StartTime
        AND AR.FinalEndTime = DS.EndTime
    WHERE AR.DoctorId = @DoctorId                      
        AND (@PatientName IS NULL OR PP.FirstName + ' ' + PP.LastName LIKE '%' + @PatientName + '%')
		AND (@StatusId IS NULL OR AR.StatusId = @StatusId)
		AND (
			@AppointmentType = 'All'
			OR (
				@AppointmentType = 'Upcoming'
				AND DATEADD(SECOND, DATEDIFF(SECOND, 0, AR.FinalStartTime), CAST(AR.FinalDate AS DATETIME)) >= GETDATE()
			    )
			OR (
				@AppointmentType = 'Past'
				AND DATEADD(SECOND, DATEDIFF(SECOND, 0, AR.FinalEndTime), CAST(AR.FinalDate AS DATETIME)) < GETDATE()
			   )
		     )
        AND (@FromDate IS NULL OR AR.FinalDate >= @FromDate)
        AND (@ToDate IS NULL OR AR.FinalDate < DATEADD(DAY, 1, @ToDate))

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
    @StatusName NVARCHAR(50),
	@LastModifiedBy INT

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
15Oct2025 Asmatali        Update Appointment Request Status
***********************************************************************************************
UpdateAppointmentStatus
*/
AS
BEGIN

   
    DECLARE @StatusId INT;

	-- StatusId For the Status Change
    SELECT
		@StatusId = StatusId
    FROM
		Statuses
    WHERE
		StatusName = @StatusName;


    -- Update the appointment Status
    UPDATE
		AppointmentRequests
    SET 
		StatusId = @StatusId,
	    LastModifiedBy = @LastModifiedBy,
        LastModifiedOn = GETDATE()
    WHERE
		AppointmentRequestId = @AppointmentRequestId
		AND StatusId = (SELECT StatusId FROM Statuses WHERE StatusName = 'Pending');
END
GO

GO
CREATE OR ALTER PROCEDURE GetUserPasswordByUsername
    @UserName NVARCHAR(100)

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
15Oct2025 Asmatali        Get User Details By Username
***********************************************************************************************
GetUserPasswordByUsername
*/
AS
BEGIN

    SELECT 
	    ISNULL(up.UserId, ' ') AS UserId,
		ISNULL(up.HashedPassword, ' ') AS HashedPassword,
		ISNULL(up.UserRoleId, ' ') AS UserRoleId,
		ISNULL(ur.RoleName, ' ') AS RoleName,
		ISNULL(up.Email, ' ') AS Email,
		ISNULL(up.UserName, ' ') AS UserName,
		ISNULL(dp.DoctorId, ' ') AS DoctorId
    FROM 
        UserProfiles up
    INNER JOIN 
        UserRoles ur
        ON up.UserRoleId = ur.UserRoleId
    LEFT JOIN 
        DoctorProfiles dp
        ON dp.UserId = up.UserId
    WHERE 
        up.UserName = @UserName
        AND up.IsActive = 1;
END
GO


GO
CREATE OR ALTER PROCEDURE GetDoctorAvailableSlots
    @DoctorId INT

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
15Oct2025 Asmatali        Get Doctor Slots By DoctorId
***********************************************************************************************
GetDoctorAvailableSlots
*/
AS
BEGIN

    SELECT 
        ISNULL(DS.SlotId, ' ') AS SlotId,
		ISNULL(DS.DoctorId, ' ') AS DoctorId,
		ISNULL(DS.SlotDate, ' ') AS SlotDate,
		ISNULL(DS.StartTime, ' ') AS StartTime,
		ISNULL(DS.EndTime, ' ') AS EndTime,
		ISNULL(DS.StatusId, ' ') AS StatusId,
		ISNULL(S.StatusName, ' ') AS StatusName
    FROM DoctorSlots DS
		INNER JOIN Statuses S ON DS.StatusId = S.StatusId
    WHERE DS.DoctorId = @DoctorId
		AND S.StatusName = 'Available' 
    ORDER BY
		DS.SlotDate, DS.StartTime;
END
GO

GO
CREATE OR ALTER PROCEDURE GetPatientPersonalInfoByAadhaar
    @AadhaarNumber NVARCHAR(12)

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
15Oct2025 Asmatali        Get Patient Info By Aadhaar Number
***********************************************************************************************
GetPatientPersonalInfoByAadhaar
*/
AS
BEGIN

    SELECT
        ISNULL(p.PatientId, ' ') AS PatientId,
		ISNULL(p.FirstName, ' ') AS FirstName,
		ISNULL(p.LastName, ' ') AS LastName,
		ISNULL(p.Email, ' ') AS Email,
		ISNULL(p.PhoneNumber, ' ') AS ContactNumber,
		ISNULL(p.AadhaarNumber, ' ') AS AadhaarNumber,
		ISNULL(p.DateOfBirth, ' ') AS DateOfBirth,
		ISNULL(p.Gender, ' ') AS Gender,
		ISNULL(a.AddressLine, ' ') AS AddressLine,
		ISNULL(a.StateId, ' ') AS StateId,
		ISNULL(a.DistrictId, ' ') AS DistrictId,
		ISNULL(a.TalukaId, ' ') AS TalukaId,
		ISNULL(a.CityId, ' ') AS CityId,
		ISNULL(a.Pincode, ' ') AS Pincode
    FROM
        PatientProfiles p
    LEFT JOIN
		Addresses a ON p.AddressId = a.AddressId AND a.IsActive = 1
    WHERE
        p.AadhaarNumber = @AadhaarNumber
        AND p.IsActive = 1
END
GO

GO
CREATE OR ALTER PROCEDURE SavePatientAppointment

    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(15),
	@AadhaarNumber NVARCHAR(12),
    @DateOfBirth DATE,
    @Gender CHAR(1),
    @InsuranceInfo NVARCHAR(100),
    @MedicalHistory NVARCHAR(300),
    @AddressLine NVARCHAR(200),
	@SlotDate DATE,
	@StartTime TIME,
	@EndTime TIME,
	@SpecializationId INT,
    @StateId INT,
    @DistrictId INT,
    @TalukaId INT,
    @CityId INT,
    @Pincode NVARCHAR(10),
    @DoctorId INT,
    @SlotId INT,  
    @MedicalConcern NVARCHAR(300),
	@DocumentFileName NVARCHAR(255) = NULL,
    @DocumentFilePath NVARCHAR(500) = NULL,
	@AppointmentId INT OUTPUT 

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
15Oct2025 Asmatali        Save Appointment Requests
***********************************************************************************************
GetPatientPersonalInfoByPhone
*/
AS
BEGIN

    DECLARE @PatientId INT;
    DECLARE @AddressId INT;
	DECLARE @PendingStatusId INT;
    DECLARE @BookedStatusId INT;

    BEGIN TRY
      BEGIN TRANSACTION;

    -- Check if patient exists by PhoneNumber
    SELECT @PatientId = PatientId 
    FROM PatientProfiles 
    WHERE PhoneNumber = @PhoneNumber;

    IF @PatientId IS NOT NULL
    BEGIN
        -- Patient exists, update Address
        SELECT
			@AddressId = AddressId
        FROM
			PatientProfiles
        WHERE
			PatientId = @PatientId

        UPDATE
			Addresses
        SET
			AddressLine = @AddressLine,
            StateId = @StateId,
            DistrictId = @DistrictId,
            TalukaId = @TalukaId,
            CityId = @CityId,
            Pincode = @Pincode,
            LastModifiedOn = GETDATE()
        WHERE
			AddressId = @AddressId;

        -- Update PatientProfiles
        UPDATE
			PatientProfiles
        SET
			FirstName = @FirstName,
            LastName = @LastName,
			PhoneNumber = @PhoneNumber,
            Email = @Email,
            DateOfBirth = @DateOfBirth,
            Gender = @Gender,
            InsuranceInfo = @InsuranceInfo,
            MedicalHistory = @MedicalHistory,
            LastModifiedOn = GETDATE()
        WHERE
			PatientId = @PatientId;
    END
    ELSE
    BEGIN
        -- Patient does not exist, insert into Address
        INSERT INTO
			Addresses
			(
				AddressLine,
				StateId,
				DistrictId,
				TalukaId,
				CityId,
				Pincode,
				IsActive,
				CreatedOn
			)
			VALUES
			(
				@AddressLine,
				@StateId,
				@DistrictId,
				@TalukaId,
				@CityId,
				@Pincode,
				1,
				GETDATE()
			);

        SET @AddressId = SCOPE_IDENTITY();

        -- Insert into PatientProfiles
        INSERT INTO
			PatientProfiles
			(
				FirstName,
				LastName,
				Email,
				PhoneNumber,
				AadhaarNumber,
				DateOfBirth,
				Gender,
				InsuranceInfo,
				MedicalHistory,
				AddressId,
				IsActive,
				CreatedOn
			)
        VALUES
		(
			@FirstName,
			@LastName,
			@Email,
			@PhoneNumber,
			@AadhaarNumber,
			@DateOfBirth,
			@Gender,
			@InsuranceInfo,
			@MedicalHistory,
			@AddressId,
			1,
			GETDATE()
		);

        SET @PatientId = SCOPE_IDENTITY();
    END


	SELECT 
		@PendingStatusId = StatusId
	FROM
		Statuses
	WHERE
		StatusName = 'Pending';

	-- Insert into AppointmentRequests with fetched StatusId
	INSERT INTO AppointmentRequests
	(
		PatientId,
		DoctorId,
		SpecializationId,
		MedicalConcern,
		PreferredDate,
		StartTime,
		EndTime,
		FinalDate,
		FinalStartTime,
		FinalEndTime,		
		StatusId,
		CreatedOn
	)
	VALUES
	(
		@PatientId,
		@DoctorId,
		@SpecializationId,
		@MedicalConcern,
		@SlotDate,
		@StartTime,
		@EndTime,
		@SlotDate,
		@StartTime,
		@EndTime,
		@PendingStatusId,
		GETDATE()
	); 

	SET @AppointmentId = SCOPE_IDENTITY();

	IF @DocumentFileName IS NOT NULL AND @DocumentFilePath IS NOT NULL
BEGIN
    INSERT INTO PatientDocuments
    (
        AppointmentRequestId,
        DocumentName,
        DocumentFileName,
        IsActive,
        CreatedOn
    )
    VALUES
    (
        @AppointmentId,
        @DocumentFileName,  
        @DocumentFilePath,   
        1,
        GETDATE()
    )
END

	SELECT 
		@BookedStatusId = StatusId
	FROM 
		Statuses
	WHERE 
		StatusName = 'Booked';

	-- Update the Doctor Slot as Booked
	UPDATE
		DoctorSlots
	SET
		StatusId = @BookedStatusId
	WHERE
		SlotId = @SlotId;

	    COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

	SET @AppointmentId=0;
	THROW;
	END CATCH

END
GO

GO
CREATE OR ALTER PROCEDURE RescheduleAppointment
    @AppointmentRequestId INT,
    @OldSlotId INT,
    @NewSlotId INT,
    @NewStartTime TIME,
    @NewEndTime TIME,
    @NewDate DATE,
    @DoctorId INT

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
15Oct2025 Asmatali        Reschedule Appointment Status
***********************************************************************************************
RescheduleAppointment
*/
AS
BEGIN

    DECLARE @BookedStatusId INT;
    DECLARE @AvailableStatusId INT;
	DECLARE @RescheduledStatusId INT;


    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @BookedStatusId = StatusId FROM Statuses WHERE StatusName = 'Booked';
        SELECT @AvailableStatusId = StatusId FROM Statuses WHERE StatusName = 'Available';
		SELECT @RescheduledStatusId = StatusId FROM Statuses WHERE StatusName = 'Rescheduled';


        -- 🔹 Mark old slot as Rescheduled
        UPDATE
			DoctorSlots
        SET
			StatusId = @AvailableStatusId
        WHERE
			SlotId = @OldSlotId;

        -- 🔹 Mark new slot as Booked
        UPDATE
			DoctorSlots
        SET
			StatusId = @BookedStatusId
        WHERE
			SlotId = @NewSlotId;

        -- 🔹 Update final slot info in AppointmentRequests
        UPDATE
			AppointmentRequests
        SET 
            FinalStartTime = @NewStartTime,
            FinalEndTime = @NewEndTime,
            FinalDate = @NewDate,
			StatusId = @RescheduledStatusId, 
            LastModifiedOn = GETDATE(),
            LastModifiedBy = @DoctorId
        WHERE
			AppointmentRequestId = @AppointmentRequestId
		AND StatusId = (SELECT StatusId FROM Statuses WHERE StatusName = 'Pending');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
         IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

		THROW;
    END CATCH
END
GO

GO
CREATE OR ALTER PROCEDURE GenerateDoctorSlots
    @FromDate DATE,
    @ToDate DATE,
    @CreatedBy INT

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
16Oct2025 Asmatali        Generate Doctor Slots
***********************************************************************************************
GenerateDoctorSlots
*/
AS
BEGIN

    DECLARE @AvailableStatusId INT;
    SELECT @AvailableStatusId = StatusId
    FROM Statuses
    WHERE StatusName = 'Available';

    --  Generate regular availability slots
    INSERT INTO
		DoctorSlots
		(	
			DoctorId,
			SlotDate,
			StartTime,
			EndTime,
			StatusId,
			CreatedBy,
			CreatedOn
		)
    SELECT 
        DA.DoctorId,
        DATEADD(DAY, v.number, @FromDate) AS SlotDate,
        DATEADD(MINUTE, n.number * DA.SlotDuration, DA.StartTime) AS StartTime,
        DATEADD(MINUTE, (n.number + 1) * DA.SlotDuration, DA.StartTime) AS EndTime,
        @AvailableStatusId AS StatusId,
        @CreatedBy,
        GETDATE()
    FROM
		DoctorAvailability DA
    CROSS JOIN
		master.dbo.spt_values v
    CROSS JOIN
		master.dbo.spt_values n
    LEFT JOIN
		DoctorSlots S
        ON S.DoctorId = DA.DoctorId
        AND S.SlotDate = DATEADD(DAY, v.number, @FromDate)
        AND S.StartTime = DATEADD(MINUTE, n.number * DA.SlotDuration, DA.StartTime)
    WHERE v.type = 'P'
      AND n.type = 'P'
      AND DATEADD(DAY, v.number, @FromDate) <= @ToDate
      AND ((DATEPART(WEEKDAY, DATEADD(DAY, v.number, @FromDate)) + @@DATEFIRST - 2) % 7) + 1 = DA.DayOfWeek
      AND S.SlotId IS NULL
      AND n.number < DATEDIFF(MINUTE, DA.StartTime, DA.EndTime) / DA.SlotDuration
      AND NOT EXISTS (
            SELECT 1
            FROM DoctorAvailabilityExceptions E
            WHERE E.DoctorId = DA.DoctorId
              AND E.ExceptionDate = DATEADD(DAY, v.number, @FromDate)
              AND E.IsAvailable = 0
              AND DATEADD(MINUTE, n.number * DA.SlotDuration, DA.StartTime) < E.EndTime
              AND DATEADD(MINUTE, (n.number + 1) * DA.SlotDuration, DA.StartTime) > E.StartTime
        );

    --  Generate slots for special working days
    INSERT INTO
		DoctorSlots
		(
			DoctorId,
			SlotDate,
			StartTime,
			EndTime,
			StatusId,
			CreatedBy,
			CreatedOn
		)
    SELECT 
        E.DoctorId,
        E.ExceptionDate AS SlotDate,
        DATEADD(MINUTE, n.number * DA.SlotDuration, E.StartTime) AS StartTime,
        DATEADD(MINUTE, (n.number + 1) * DA.SlotDuration, E.StartTime) AS EndTime,
        @AvailableStatusId AS StatusId,
        @CreatedBy,
        GETDATE()
    FROM DoctorAvailabilityExceptions E
    JOIN DoctorAvailability DA
        ON DA.DoctorId = E.DoctorId
    CROSS JOIN master.dbo.spt_values n
    LEFT JOIN DoctorSlots S
        ON S.DoctorId = E.DoctorId
        AND S.SlotDate = E.ExceptionDate
        AND S.StartTime = DATEADD(MINUTE, n.number * DA.SlotDuration, E.StartTime)
    WHERE n.type = 'P'
      AND E.IsAvailable = 1
      AND E.StartTime IS NOT NULL
      AND n.number < DATEDIFF(MINUTE, E.StartTime, E.EndTime) / DA.SlotDuration
      AND S.SlotId IS NULL
      AND NOT EXISTS (
            SELECT 1
            FROM DoctorAvailabilityExceptions EX
            WHERE EX.DoctorId = E.DoctorId
              AND EX.IsAvailable = 0
              AND EX.ExceptionDate = E.ExceptionDate
              AND DATEADD(MINUTE, n.number * DA.SlotDuration, E.StartTime) < EX.EndTime
              AND DATEADD(MINUTE, (n.number + 1) * DA.SlotDuration, E.StartTime) > EX.StartTime
      );
END;
GO


GO
CREATE OR ALTER PROCEDURE GetStates
/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
16Oct2025 Asmatali        Get States List
***********************************************************************************************
GetStates
*/
AS
BEGIN
    SELECT
		ISNULL(StateId, ' ') AS StateId,
		ISNULL(StateName, ' ') AS StateName
	FROM
		States
	WHERE
		IsActive = 1
	ORDER BY
		StateName;
END
GO

GO
CREATE OR ALTER PROCEDURE GetDistrictsByState
    @StateId INT

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
16Oct2025 Asmatali        Get Districts By State
***********************************************************************************************
GetDistrictsByState
*/
AS
BEGIN
    SELECT
		ISNULL(DistrictId, ' ') AS DistrictId,
		ISNULL(DistrictName, ' ') AS DistrictName,
		ISNULL(StateId, ' ') AS StateId
	FROM
		Districts 
    WHERE
		StateId = @StateId
		AND IsActive = 1 
    ORDER BY
		DistrictName;
END
GO

GO
CREATE OR ALTER PROCEDURE GetTalukasByDistrict
    @DistrictId INT

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
16Oct2025 Asmatali        Get Taluka By District
***********************************************************************************************
GetTalukasByDistrict
*/
AS
BEGIN
    SELECT
		ISNULL(TalukaId, ' ') AS TalukaId,
		ISNULL(TalukaName, ' ') AS TalukaName,
		ISNULL(DistrictId, ' ') AS DistrictId
	FROM
		Talukas 
    WHERE
		DistrictId = @DistrictId
		AND IsActive = 1 
    ORDER BY
		TalukaName;
END
GO

GO
CREATE OR ALTER PROCEDURE GetCitiesByTaluka
    @TalukaId INT

/*
***********************************************************************************************
 Date      Modified By     Purpose of Modification
16Oct2025 Asmatali        Get Cities By Taluka
***********************************************************************************************
GetCitiesByTaluka
*/
AS
BEGIN
    SELECT
		ISNULL(CityId, ' ') AS CityId,
		ISNULL(CityName, ' ') AS CityName,
		ISNULL(TalukaId, ' ') AS TalukaId
	FROM
		Cities 
    WHERE
		TalukaId = @TalukaId
		AND IsActive = 1 
    ORDER BY
		CityName;
END
GO


Go
CREATE OR ALTER PROCEDURE DoctorAppointmentErrorLogsInsert
@ErrorMessage VARCHAR(MAX),
@StackTrace VARCHAR(MAX),
@CreatedBy INT

/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1	16Oct2025		Asmatali		       Insert ErrorLogs
***********************************************************************************************
DoctorAppointmentErrorLogsInsert

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








