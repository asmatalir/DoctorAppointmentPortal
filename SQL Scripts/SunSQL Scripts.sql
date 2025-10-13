/*Stored procedure for GetUserPasswordByUsername*/
GO
CREATE OR ALTER PROCEDURE GetUserPasswordByUsername
    @UserName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        HashedPassword,
		Email
    FROM 
        UserProfiles
    WHERE 
        UserName = @UserName
        AND IsActive = 1;
END
GO