USE TrainingDB_AsmataliRauthar;

CREATE TABLE Tags
(
TagId INT PRIMARY KEY IDENTITY(1,1),
Tag VARCHAR (200),
IsActive BIT,
CreatedBy INT,
CreatedOn DATETIME,
ModifiedBy INT,
ModifiedOn DATETIME
)


CREATE PROCEDURE TagsGetList

/*
***********************************************************************************************
	Date   			Modified By   		Purpose of Modification
1	1Jan2020		Anuja Bhatkar		Get List for Tags
***********************************************************************************************
TagsGetList

*/

AS
BEGIN

	SELECT 
		ISNULL(TagId, '') AS TagId,
		ISNULL(Tag, '') AS Tag,
		ISNULL(IsActive, '') AS IsActive,
		ISNULL(CreatedBy, '') AS CreatedBy,
		ISNULL(CreatedOn, '') AS CreatedOn,
		ISNULL(ModifiedBy, '') AS ModifiedBy,
		ISNULL(ModifiedOn, '') AS ModifiedOn
	FROM
		Tags
	

	
END


CREATE PROCEDURE TagsInsert

@TagId INT OUTPUT,
@Tag VARCHAR (200),
@IsActive BIT,
@CreatedBy INT,
@CreatedOn DATETIME

/*
***********************************************************************************************
	Date   			Modified By   	Purpose of Modification
1	1Jan2020		Anuja Bhatkar	Insert Tags
***********************************************************************************************
*/

AS
BEGIN


	INSERT INTO Tags
	(
		Tag,
		IsActive,
		CreatedBy,
		CreatedOn
		
	)
	VALUES
	(
		@Tag,
		@IsActive,
		@CreatedBy,
		@CreatedOn
	)

	SET @TagId = @@IDENTITY;
END
EXECUTE TagsGetList;
SELECT * FROM Tags;