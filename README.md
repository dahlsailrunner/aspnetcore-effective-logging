# ASP.NET Core -- Effective Logging
This repo contains code that was used (and is kept up-to-date) from the [Effective Logging in ASP.NET Core](https://app.pluralsight.com/library/courses/asp-dotnet-core-effective-logging) course on Pluralsight, which I authored.

It includes references to BOTH Serilog and NLog and various commits will change the logging framework from one to the other.

A couple of key recent updates have been made that might be of interest:

* Updated the code base from .NET Core 2.2 to .NET Core 3.1 - and this meant updating Swashbuckle and some other NuGet packages as well.  To see the changes made to support this change, review this commit: https://github.com/dahlsailrunner/aspnetcore-effective-logging/commit/238ea8035a7852335c1cf320879eecffc1f9d39e
* Updated the code to support changes within the public [Demo IdentityServer4 instance](https://demo.identityserver.io).  The changes there were made to reflect recommended practices for flows.  To see the changes required in these applications, have a look at this commit: https://github.com/dahlsailrunner/aspnetcore-effective-logging/commit/6aa84a6a1cbdd6012cbab559a81ad20bab73b237

## Approach
Since I needed to be able to switch from one logging framework to another, I took an approach with this set of applications to tap primarily into the `Microsoft.Logging.Extensions` functionality rather than use Serilog Enrichers.  

## Getting Started
There are two projects that should be set to run in this repo: 
* BookClub.UI (this is the user interface - and it makes calls to the API project)
* BookClub.API (this is the API which has all of the database interactions)

The API projects `appsettings.json` file has a connection string that looks for a `BookClub` database on a local SQLExpress instance.  If you don't have one `BookClub` database:
* **Optional**: [Download Microsoft® SQL Server® 2017 Express](https://www.microsoft.com/en-us/download/details.aspx?id=55994) and [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?redirectedfrom=MSDN&view=sql-server-ver15).
* Create a database called BookClub in some instance of SQL Server. Express is fine, so is the Docker version.
* Use the 4 files in the `BookClub.Data/Schema` folder to set up the `BookClub` database:
  * Create the `Book` table by running the SQL in `Book.sql`
  * Insert a couple of rows by running the SQL in `InitialData.sql`
  * Create stored procedures by running the SQL in both the `GetAllBooks.sql` and `InsertBook.sql` (each creates a proc)
* Update the connection string in `BookClub.API/appsettings.json` to point to the database.

Run the solution!  :)

## Exploring
There aren't that many pages / API methods here, and the best way to really explore what's going on is to simply try some of the pages and look at the code inside them, along with setting some breakpoints both in the UI and the API code.

