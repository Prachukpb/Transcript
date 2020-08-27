# .net core API

## Setup
This application is developed using **.Net Core 3.1** and **EF Core 2.0** to communicate with the **SQL database server**. 
No changes has been made to the provided SQL database schema.

Please make sure you have the following packages installed in your project.

- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.AspNetCore.Mvc.NewtonsoftJson

Update the connection string in the **appsettings.json** to connect to the database server you are working with.

## Start the Application
When the setup is complete and you start the application in debug mode, it will take you to a simple UI razor page with the list of challenges and their respective answers/output for all the challenges.

