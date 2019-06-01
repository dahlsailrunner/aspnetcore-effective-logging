CREATE PROCEDURE dbo.GetAllBooks 
AS 
SELECT b.Id
      ,b.Title
      ,b.Author
      ,b.Classification
      ,b.Genre
      ,b.Description 
FROM dbo.Book b