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
- If you have Docker, run `docker-compose up` in the root folder. It will start following services
  - Kafka
  - Postgres server
  - Azurite
- If you don't have Docker, you will to download and install those services then update the appsettings.json correctly
- Start the `ApiGateway`, `CarStore.Authentication` and the needed Api projects
