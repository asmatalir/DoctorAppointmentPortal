USE [TrainingDB_AsmataliRauthar]
GO

INSERT INTO [dbo].[Books]
           ([BookName]
           ,[NoOfPages])
     VALUES
           ('Let Us C',500),
		   ('Think and Grow Rich',700);

DROP Table Books;
TRUNCATE Table Books;
SELECT * FROM Books;

