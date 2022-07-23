## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)

## General info
This project is for NextJS Workshop NashTech.
	
## Technologies
### Backend
Project is created with:
* Net 6.0
* Entity Framework Core 6.0
* PostgrestSQL
* MediatR
* Serilog
* FuentValidation
* Swagger
* Ocelot 1.0
	
## Setup
### Backend
#### Prerequisites
- Visual Studio 2022
- Net 6 SDK

#### Steps
- Download and install `Postgresql` on your local
- Update `ConnectionStrings` in `appsettings.json`
- Start the `ApiGateway` and `Service Api` projects

### Kafka
#### Prerequisites
- Docker

#### Steps
- Start Docker
- In the root folder which contains the `docker-compose.yml` file run `docker-compose up` to start Kafka
