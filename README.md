# Procurement Plus

Procurement Plus is a sample application built using ASP.NET Core, Entity Framework Core and Angular 7. 

## Getting Started
Use these instructions to get the project up and running.

### Prerequisites
You will need to install the following tools:

* [.NET Core SDK 2.2](https://www.microsoft.com/net/download/dotnet-core/2.2)
* [Nodejs](https://nodejs.org/dist/v10.16.0/node-v10.16.0-x64.msi)
* [SQL Server Express] (https://www.microsoft.com/en-us/sql-server/sql-server-editions-express)
* [Visual Studio Code or 2017](https://www.visualstudio.com/downloads/) (optional)

### Setup
Follow these steps to get your development environment set up:

  1. Clone the repository
  2. At the root directory go to `.\Procurement.Api` , restore required packages by running:
     ```
     dotnet restore
     ```
  3. Next, build the solution by running:
     ```
     dotnet build
     ```
  4. Next launch the API by running:
     ```
	 dotnet run
	 ```
  4. go back to the root directory, within the `.\procurement-spa` directory, launch the front end by running:
     ```
     npm start
     ```
  
  5. Launch [http://localhost:4200/](http://localhost:4200/) in your browser to view the Web UI
  
  6. Launch [http://localhost:5000/swagger](http://localhost:5000/swagger) in your browser to view the API

## Technologies

* JWT
* Serilog
* MediatR
* Fluent Validation
* Swashbuckle

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/JasonGT/NorthwindTraders/blob/master/LICENSE.md) file for details.
