
# CRM-API-Using-.NET6
Customer Replationship Management API Using .NET6 .

## Architecture

- User registration and Login via access token creation
- Role-based authorization
- Full Crud Operations GET | POST | DELTE | PUT | PATCH with pagination and filtration
- File Data Uploading
- Send Whatsapp Messages for customer with their request No.
- Saving Logs using Serilog
- Using User Secret Manager to save secured data

## Frameworks and Libraries
The API uses the following libraries and frameworks to deliver the functionalities described above:

[Entity Framework Core 6](https://learn.microsoft.com/en-us/ef/core/) ( for data access)
```bash
install-package Microsoft.EntityFrameworkCore 
```
[AutoMapper](https://automapper.org/) (for mapping between domain entities and resource classes)
```bash
install-package AutoMapper
install-package AutoMapper.Extensions.Microsoft.DependencyInjection
```
[Sqlite](https://www.sqlite.org/index.html) (for connecting database)
```bash
install-package Microsoft.EntityFrameworkCore.Sqlite 
```
[Serilog](https://serilog.net/) (for saving application logs in file | console | Seq)
```bash
install-package Serilog.AspNetCore
```
[ClosedXML](https://closedxml.readthedocs.io/en/latest/) (reading, manipulating and writing Excel)
```bash
install-package ClosedXML
```
[Twilio](https://www.twilio.com/) (Send Messages)
```bash
install-package Twilio
```

### Technologies

![.NET 6](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

![Swagger UI](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=Swagger&logoColor=white)

![Postman](https://img.shields.io/badge/Postman-FF6C37?style=for-the-badge&logo=Postman&logoColor=white)

![Sqlite](https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white)

![Twilio](https://img.shields.io/badge/Twilio-F22F46?style=for-the-badge&logo=Twilio&logoColor=white)

## API Reference

```http
  GET /api/Authenticate/Login
```

#### Get all customers

```http
  GET /api/customers
```

#### Get customer by id

```http
  GET /api/customers/{customerID}
```

#### Add Customer Request

```http
  POST /api/Requests/{customerID}
  
Form Body : {
  "description": "",
  "requestTypeId": 
}

```

![Whatsapp-submit-request-message](https://raw.githubusercontent.com/ayaosama05/CRM-API-Using-.NET6/b23fc9c18cd80ea85dbb172d7adefda527abfc0d/CustomerRelationshipManagementAPI/Uploads/Files/whatsapp.jpg)
