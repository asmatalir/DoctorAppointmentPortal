CREATE OR ALTER PROCEDURE SavePatientAppointment

    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(15),
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
    @CreatedBy INT
AS
BEGIN

    DECLARE @PatientId INT;
    DECLARE @AddressId INT;
	DECLARE @PendingStatusId INT;
    DECLARE @BookedStatusId INT;

    -- Check if patient exists by PhoneNumber
    SELECT @PatientId = PatientId 
    FROM PatientProfiles 
    WHERE PhoneNumber = @PhoneNumber;

    IF @PatientId IS NOT NULL
    BEGIN
        -- Patient exists, update Address
        SELECT @AddressId = AddressId
        FROM PatientProfiles
        WHERE PatientId = @PatientId;

        UPDATE Addresses
        SET AddressLine = @AddressLine,
            StateId = @StateId,
            DistrictId = @DistrictId,
            TalukaId = @TalukaId,
            CityId = @CityId,
            Pincode = @Pincode,
            LastModifiedBy = @CreatedBy,
            LastModifiedOn = GETDATE()
        WHERE AddressId = @AddressId;

        -- Update PatientProfiles
        UPDATE PatientProfiles
        SET FirstName = @FirstName,
            LastName = @LastName,
            Email = @Email,
            DateOfBirth = @DateOfBirth,
            Gender = @Gender,
            InsuranceInfo = @InsuranceInfo,
            MedicalHistory = @MedicalHistory,
            LastModifiedOn = GETDATE()
        WHERE PatientId = @PatientId;
    END
    ELSE
    BEGIN
        -- Patient does not exist, insert into Address
        INSERT INTO Addresses(AddressLine, StateId, DistrictId, TalukaId, CityId, Pincode, IsActive, CreatedBy, CreatedOn)
        VALUES (@AddressLine, @StateId, @DistrictId, @TalukaId, @CityId, @Pincode, 1, @CreatedBy, GETDATE());

        SET @AddressId = SCOPE_IDENTITY();

        -- Insert into PatientProfiles
        INSERT INTO PatientProfiles(FirstName, LastName, Email, PhoneNumber, DateOfBirth, Gender, InsuranceInfo, MedicalHistory, AddressId, IsActive, CreatedOn)
        VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @DateOfBirth, @Gender, @InsuranceInfo, @MedicalHistory, @AddressId, 1, GETDATE());

        SET @PatientId = SCOPE_IDENTITY();
    END


	SELECT TOP 1 @PendingStatusId = StatusId
	FROM Statuses
	WHERE StatusName = 'Pending';

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

	SELECT TOP 1 @BookedStatusId = StatusId
	FROM Statuses
	WHERE StatusName = 'Booked';

	-- Update the Doctor Slot as Booked
	UPDATE DoctorSlots
	SET StatusId = @BookedStatusId
	WHERE SlotId = @SlotId;

END
