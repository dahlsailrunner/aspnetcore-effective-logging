CREATE PROCEDURE dbo.GetAllBooks 
AS 
SELECT b.Id
      ,b.Title
      ,b.Author
      ,b.Classification
      ,b.Genre
      ,b.Description 
	  ,b.Isbn
	  ,b.Submitter
FROM dbo.Book b