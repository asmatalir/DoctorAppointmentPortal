/*Stored procedure for GetUserPasswordByUsername*/
GO
CREATE OR ALTER PROCEDURE GetUserPasswordByUsername
    @UserName NVARCHAR(100)
AS
BEGIN

    SELECT 
        up.HashedPassword,
        up.UserRoleId,
        ur.RoleName
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
