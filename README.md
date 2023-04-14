
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
The Helsinki City Bike app allows users to view and filter data about journeys made with city bikes in the Helsinki Capital area. The app offers the following features:
* Backend
1. Import data from CSV files to a SQL Server database 
2. Validate imported data before saving 
3. Return a list of all journeys, including departure and return stations, covered distance in kilometers, and duration in minutes 
4. Return a list of all stations, including total number of journeys starting and ending at the station 
5. Return detailed information about a single station, including total number of journeys starting and ending at the station, and the average distance of journeys starting and ending at the station 
6. Allow users to update the return station and return date for a specific journey 
7. Provide Swagger documentation for all API endpoints
* Frontend
1. Import Station and Journey data from CSV files to a SQL Server database 
2. Display a list of all journeys, including departure and return stations, covered distance in kilometers, and duration in minutes 
3. Allow users to sort journeys by any column and filter by date range or station 
4. Display a list of all stations, including total number of journeys starting and ending at the station 
5. Allow users to filter stations by name or location and display station locations on a map 
6. Display detailed information about a single station, including total number of journeys starting and ending at the station, and the average distance of journeys starting and ending at the station 
7. Display charts showing the top 5 most popular departure and return stations for a specific station 
8. Allow users to update the return station and return date for a specific journey
### Getting Started 
<a name="getting-started"></a>
To get started with the Helsinki City Bike app, you will need to have the following software installed on your computer:
* Visual Studio 2019 (with .NET Core 7.0)
* SQL Server Management Studio (SSMS)
* Node.js 
* Angular CLI latest
After installing the prerequisites, you can follow these steps:

1. Clone the project repository to your machine. 
2. Open the terminal and navigate to the project directory. 
3. Run dotnet build to build the backend project. 
4. Navigate to the ClientApp directory and run npm install to install the frontend dependencies. 
5. Run npm start to start the frontend development server. 
6. In a new terminal window, run dotnet run to start the backend server.
### Docker
You can also run the project using Docker. Follow these steps:

1. Clone the project repository to your machine.
2. Open the terminal and navigate to the project directory.
3. Run docker-compose build to build the Docker containers.
4. Run docker-compose up to start the containers.
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
### Deploying to Azure
The project is deployed in an Azure Web App container. You can access the application through the following URL: https://solitawebapp.azurewebsites.net/
### Endpoints
The following endpoints are available in the project:

#### Import
1. POST api/Import/Stations: Import the bicycle station data from the dataset into the database.
2. POST api/Import/Journeys: Import the journey data from the datasets into the database.
#### Journey List
1. GET api/JourneyList: List all the journeys in the database.
2. POST api/JourneyList/Add: Add a new journey to the database.
3. PUT api/JourneyList/{journeyId}/return: Update the return info for a journey.
4. GET api/JourneyList/{journeyId}/User: Get the user information for a journey.
5. GET api/JourneyList/{journeyId}: Get the information for a journey.
#### Single View
1. GET api/Singleview/{stationId}: Get the information for a single station, including the number of journeys starting and ending at the station and the average distance of journeys starting and ending at the station.
2. GET api/Singleview/{stationId}?month={month}: Get the information for a single station for a specific month.
#### Station
1. POST api/Station/Create: Create a new station in the database.
2. PUT api/Station/Create/{stationId}: Update the information for a station in the database.
3. GET api/Station/Create/{stationId}: Get the information for a station in the database.
4. DELETE api/Station/Create/{stationId}: Delete a station from the database.
#### Station List
1. GET api/StationList: List all the stations in the database. Limited to 100 rows but can be changeable by user
2. GET api/User: Get the information for the currently authenticated user.
3. POST api/User/authenticate: Authenticate a user and return a JWT token.
4. POST api/User/register: Register a new user.
5. PUT api/User/{userId}: Update the information for a user.
6. GET api/User/{userId}: Get the information for a user.
7. DELETE api/User/{userId


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
