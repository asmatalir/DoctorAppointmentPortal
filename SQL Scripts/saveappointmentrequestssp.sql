CREATE PROCEDURE SavePatientAppointment
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(15),
    @DateOfBirth DATE,
    @Gender CHAR(1),
    @InsuranceInfo NVARCHAR(100),
    @MedicalHistory NVARCHAR(300),
    @AddressLine NVARCHAR(200),
    @StateId INT,
    @DistrictId INT,
    @TalukaId INT,
    @CityId INT,
    @Pincode NVARCHAR(10),
    @DoctorId INT,
    @SlotId INT,  -- The doctor slot being booked
    @MedicalConcern NVARCHAR(300),
    @CreatedBy INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PatientId INT;
    DECLARE @AddressId INT;

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

    -- Insert into AppointmentRequests
    INSERT INTO AppointmentRequests(PatientId, DoctorId, MedicalConcern, AppointmentDate, StatusId, CreatedOn)
    VALUES (@PatientId, @DoctorId, @MedicalConcern, GETDATE(), 1, GETDATE());  -- StatusId = 1 => Pending

    -- Update the Doctor Slot as Booked
    UPDATE DoctorAvailabilities
    SET Status = 'Booked'  -- Or use IsBooked = 1 if that’s the column
    WHERE DoctorAvailabilityId = @SlotId;

END
