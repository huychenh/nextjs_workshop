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
- Azure Storage Emulator or Azurite or Azure Account. I recommend to use Azure Account if possible.

#### Steps
- Download and install `Postgresql` on your local. Or, using docker.
- Update `ConnectionStrings` in `appsettings.json`
- Start Azure Storage Emulator or Azurite so that we can use the default provided Azure Storage connection string. Or create a real Azure Storage then update the `FileStorage:ConnectionString`. Remember to configure CORS to allow our FE to access the storage.
- Start the `ApiGateway` and `Service Api` projects

### Kafka
Only needed if you want to run the Notification service

#### Prerequisites
- Docker

#### Steps
- Start Docker
- In the root folder which contains the `docker-compose.yml` file run `docker-compose up` to start Kafka
