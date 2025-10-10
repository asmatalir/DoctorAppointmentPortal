
/*Get Foriegn keys of UserRoles*/
SELECT 
    name AS ConstraintName,
    type_desc AS ConstraintType
FROM sys.objects
WHERE parent_object_id = OBJECT_ID('UserRoles');


/*Get Foriegn keys of UserProfiles*/
SELECT 
    fk.name AS ForeignKeyName,
    tp.name AS TableName
FROM sys.foreign_keys AS fk
INNER JOIN sys.tables AS tp ON fk.parent_object_id = tp.object_id
WHERE tp.name = 'UserProfiles';


/*Drop Foreign keys of UserProfiles*/
ALTER TABLE UserProfiles DROP CONSTRAINT FK__UserProfi__UserR__09946309;
ALTER TABLE UserProfiles DROP CONSTRAINT FK__UserProfi__Creat__2630A1B7;
ALTER TABLE UserProfiles DROP CONSTRAINT FK__UserProfi__LastM__0C70CFB4;

/*Drop Foreign keys of UserRoles*/
ALTER TABLE UserRoles DROP CONSTRAINT FK_UserRoles_CreatedBy;
ALTER TABLE UserRoles DROP CONSTRAINT FK_UserRoles_LastModifiedBy;

/* Table created for UserRoles without reference and then refernce added */
CREATE TABLE UserRoles (
    UserRoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedBy INT NULL,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    LastModifiedBy INT NULL,
    LastModifiedOn DATETIME NULL
);
ALTER TABLE UserRoles
ADD CONSTRAINT FK_UserRoles_CreatedBy
FOREIGN KEY (CreatedBy) REFERENCES UserProfiles(UserId);

ALTER TABLE UserRoles
ADD CONSTRAINT FK_UserRoles_LastModifiedBy
FOREIGN KEY (LastModifiedBy) REFERENCES UserProfiles(UserId);

ALTER TABLE UserProfiles
ADD CONSTRAINT FK_UserProfiles_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES UserProfiles(UserId);
