CREATE PROCEDURE dbo.InsertBook
	@Title NVARCHAR(300),
	@Author NVARCHAR(200),
	@Classification VARCHAR(20),
	@Genre VARCHAR(20),
	@Isbn VARCHAR(20),
	@Submitter INT
AS
INSERT INTO dbo.Book
(
    Title
   ,Author
   ,Classification
   ,Genre
   ,Description
   ,Isbn
   ,Submitter
)
VALUES (@Title
       ,@Author
       ,@Classification
       ,@Genre
       ,'' -- Description - nvarchar(max)
       ,@Isbn
	   ,@Submitter
    )