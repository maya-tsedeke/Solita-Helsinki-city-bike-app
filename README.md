
# Helsinki city bike app (Dev Academy pre-assignment solution)

A brief description of what this project does and who it's for

Implementation of a web application for displaying data from journeys made with city bikes in the Helsinki Capital area. The backend is implemented using .NET and includes importing data from CSV files to a database, validating data before importing, and filtering imported journeys based on their duration and distance, user authorization with token. The front-end is implemented using Angular and includes a journey list view that import, shows departure and return stations, covered distance, and duration, location on map, customer travel history, customer daily, weekly, monthly, and yearly travel report. Additionally, the frontend includes a station list that shows station information, and a single station view that includes station details and statistics such as the number of journeys starting and ending at the station, the average distance of starting and ending journeys, and the top 5 most popular return and departure stations. The implementation also includes user login authorization using tokens.
## API Reference

#### Get all items

```http
  GET /api/items
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `api_key` | `string` | **Required**. Your API key |

#### Get item

```http
  GET /api/items/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `string` | **Required**. Id of item to fetch |

#### add(num1, num2)

Takes two numbers and returns the sum.

