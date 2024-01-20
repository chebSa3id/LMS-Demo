# Introduction

- a simple API that Simulate a very simple Learning Management System (LMS) .

<br>

## Project Description

1. The API is developed using C# using .Net Core 8.0.

2. The API is documented using Swagger.

3. The Database Engine used is PostgreSQL.

4. The API is developed using Repository Pattern.

5. The API is secured using JWT Authentication.

6. This Project is developed by Code First Methodology.

<br>

# Getting Started

1. Download the project as zip or Clone it.
2. Update all appsettings files with your own values (database connection string).
3. Commands to run the project :

```
dotnet restore
dotnet run
```

<br>

# API Documentation

- Please Find the API swagger Documentation at :
  - http://localhost:5001/swagger/index.html
- Please Find the Database Creation Script at :
  - ./Scripts/DB.sql

# FOLDER STRUCTURE

### Configurations

- individual sections in appsettings.json should be mapped to a config class here.

### Controllers

- endpoints exposed by the api.

### Dtos

- dtos used by external clients to communicate with the api.
- create a separate directory for each client.
- create a directory for common Dtos used by multiple clients.

### Extensions

- custom extension methods used in the project.
- 'Setup' directory contains extension methods used during app startup.

### Middlewares

- custom middlewares used in the project.
- includes middlewares:
  - ExceptionMiddleware - catch any exception thrown during the request lifetime and handle returning a response (different for production vs development).
  - LoggerMiddleware - log request & response details (different for production vs development).

### Models

- entities (Db tables) used in the project.

### Repos

- repositories used to perform CRUD operations on the database.

### Scripts

- long scripts (eg. seeding script or stored procedures).

### Services

- business logic used in the controllers.

### Utils

- util functionalities used in the project.

### Swagger

- swagger configuration.

<br>
