
# Helsinki city bike app (Dev Academy pre-assignment solution)

A brief description of what this project does and who it's for

Implementation of a web application for displaying data from journeys made with city bikes in the Helsinki Capital area. The backend is implemented using .NET and includes importing data from CSV files to a database, validating data before importing, and filtering imported journeys based on their duration and distance, user authorization with token. The front-end is implemented using Angular and includes a journey list view that import, shows departure and return stations, covered distance, and duration, location on map, customer travel history, customer daily, weekly, monthly, and yearly travel report. Additionally, the frontend includes a station list that shows station information, and a single station view that includes station details and statistics such as the number of journeys starting and ending at the station, the average distance of starting and ending journeys, and the top 5 most popular return and departure stations. The implementation also includes user login authorization using tokens.

This is an application that displays the status of Helsinki city bikes in a web interface. The application retrieves the real-time data from the Helsinki City bike API and shows the status of each bike station on the map.

##  Prerequisites 
* Docker installed
* Azure subscription (for deployment)
## Running the application
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

