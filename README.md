# EsriApp

This is an application that consumes REST Web Service for access to demographic data based on municipalities (USA_Counties) and with background processing extracts demographic data and saves the results.
The application itself provides the recorded data with REST WebService which supports a request to retrieve all recorded results (population
for each state) and filter the result by state name (STATE_NAME field).

## :hammer_and_pick: Built With
- .NET 5 Framework
- ASP.NET Web API - framework for building Web APIs with RestFul Services.
- [Hangfire](https://github.com/HangfireIO/Hangfire) - way to perform background processing recurring jobs inside ASP.NET applications.
- Microsoft SQL Server Express - relational database management system.
- Entity Framework Core 5 - object-database mapper for . NET. It supports LINQ queries, change tracking, updates, and schema migrations and works whit SQL Server.
- Blazor WebAssembly - create single-page applications (SPAs) using C# and . NET utilising a component-based architecture.

## ⏯️ To Start the App
- Download the code.
- Start .sln file in Visula Studio.
- You must have SQL Server Management Studio installed.
- Check the Connection String in Esri Api/appsettings.Development.json, if you have installed SQL Server Express, you have to change "Server =.;" to "Server=.\SQLEXPRESS;".
- In Visual Studio -> Package Manager Console run command Update-Database to create EsriDb and apply migrations.
- When you run the app in Visual Studio, you can make Api calls for [Postman](https://www.postman.com/) or built in [Swagger](https://swagger.io/) in Visual Studio.
- If you want to use Blazor UI to visualize data from EsriDb you have to go in Visual Studio, right mouse click on Solution -> properties -> select multiple startup projects  and below select Start to EsriApi and EsriClient.
