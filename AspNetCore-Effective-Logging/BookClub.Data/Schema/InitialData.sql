INSERT INTO dbo.Book
(
    Title
   ,Author
   ,Classification
   ,Genre
   ,Description
)
VALUES (N'Carrion Comfort' -- Title - nvarchar(300)
       ,N'Dan Simmons' -- Author - nvarchar(200)
       ,'Fiction' -- Classification - varchar(20)
       ,'Horror' -- Genre - varchar(20)
       ,N'Mind-vampires extert their control over others throughout the course of recent history.' -- Description - nvarchar(max)
    )

INSERT INTO dbo.Book
(
    Title
   ,Author
   ,Classification
   ,Genre
   ,Description
)
VALUES (N'American Gods' -- Title - nvarchar(300)
       ,N'Neil Gaiman' -- Author - nvarchar(200)
       ,'Fiction' -- Classification - varchar(20)
       ,'Sci-Fi/Fantasy' -- Genre - varchar(20)
       ,N'Crazy road trip across America involving a war between the old gods and new ones.' -- Description - nvarchar(max)
    )