
# Helsinki city bike app (Dev Academy pre-assignment solution)

A brief description of what this project does and who it's for

Implementation of a web application for displaying data from journeys made with city bikes in the Helsinki Capital area. The backend is implemented using .NET and includes importing data from CSV files to a database, validating data before importing, and filtering imported journeys based on their duration and distance, user authorization with token. The front-end is implemented using Angular and includes a journey list view that import, shows departure and return stations, covered distance, and duration, location on map, customer travel history, customer daily, weekly, monthly, and yearly travel report. Additionally, the frontend includes a station list that shows station information, and a single station view that includes station details and statistics such as the number of journeys starting and ending at the station, the average distance of starting and ending journeys, and the top 5 most popular return and departure stations. The implementation also includes user login authorization using tokens.

This is an application that displays the status of Helsinki city bikes in a web interface. The application retrieves the real-time data from the Helsinki City bike API and shows the status of each bike station on the map.
This is a web application that displays data about journeys made with city bikes in the Helsinki Capital area. The project consists of both a backend service and a frontend UI.

#### Table of Contents

* [Technologies Used](#technologies-used)
* [Features](#features)
* [Getting Started](#getting-started)
* [Running the App](#running-the-app)
* [Testing](#testing)
* [Contributing](#contributing)
* [License](#license)
### Technologies Used
<a name="technologies_used"></a>
The backend service is built using C# and .NET Core, while the frontend is built with Angular and TypeScript. The following libraries and frameworks were used:

#### Backend
* .NET Core 7.0
* Entity Framework Core
* AutoMapper
* Swagger
* Moq
* xUnit
#### Frontend
* Angular 15
* Angular Bootstrap 
### Features
<a name="features"></a>
### Getting Started
To get started with the Helsinki City Bike app, you will need to have the following software installed on your computer:
* Visual Studio 2019 (with .NET Core 7.0)
* SQL Server Management Studio (SSMS)
* Node.js 
* Angular CLI latest
<a name="getting-started"></a>
### Running the App
To run the Helsinki City Bike app, follow these steps:
1. Clone the repository to your local machine 
2. In SSMS, create a new database named "CityBike" (or any other name of your choosing)
3. In Visual Studio, open the "appsettings.json" file and update the connection string to point to your database 
4. Open a command prompt and navigate to the "Backend" folder of the repository 
5. Run the following commands to create the database schema and seed the database with data:
```bash
dotnet ef database update
dotnet run
```
6. Open a second command prompt and navigate to the "Frontend" folder of the repository 
7. Run the following commands to install the necessary dependencies and start the Angular development server:
```bash
npm install
ng serve
```
##  Prerequisites 
* Docker installed
* Azure subscription (for deployment)
## Running the application
<a name="running-the-app"></a>
### Testing
<a name="testing"></a>
### Contributing
<a name="contributing"></a>
### License
<a name="license"></a>
Clone the repository:
## Deployment

To deploy this project run

```bash
  git clone https://github.com/[username]/[repository].git

```
Build the Docker image:
```bash
  docker build -t [image-name] .

```
Run the Docker container:
```bash
 docker run -p 8080:80 [image-name]

```

### Deploying to Azure
```bash
az acr create --resource-group [resource-group] --name [registry-name] --sku Basic
az acr login --name [registry-name]
docker tag [image-name] [registry-name].azurecr.io/[image-name]:[tag]
docker push [registry-name].azurecr.io/[image-name]:[tag]
```
Create an Azure Container Instance and deploy the container:
```bash
az container create --resource-group [resource-group] --name [container-name] --image [registry-name].azurecr.io/[image-name]:[tag] --cpu 1 --memory 1 --ports 80

```
Get the IP address or domain URL of the container:
```bash
az container show --resource-group [resource-group] --name [container-name] --query ipAddress.fqdn

```
Access the application in your browser by going to http://[ip-address-or-domain]
## Installation
<a name="installation"></a>
