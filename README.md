# Order Processing Microservice (.NET 9, DDD, CQRS)

## Overview

This microservice handles **Order Processing** via a REST API, built using **.NET 9** and **Domain-Driven Design (DDD)** principles. 
It exposes endpoints to **create**, **retrieve**, and **delete** orders. 
The implementation follows **CQRS** using **MediatR**, and includes robust **validation**, **structured logging**, and **unit/integration** testing.

## Architecture

* **Domain-Driven Design (DDD)**:

  * `Order` is the **aggregate root**.
  * Includes **value objects** for CreditCard, Email and Address data.
* **CQRS with MediatR**:

  * Commands and Queries handled via `IRequestHandler`.
  * Each request type is wrapped in strongly-typed classes.
* **Three-layered structure**:

  * `Domain`: Entities, Value Objects
  * `Application`: Use cases, MediatR handlers
  * `Infrastructure`: EF Core, Persistence

## Tech Stack

| Component     | Technology                                                 |
| ------------- | ---------------------------------------------------------- |
| Framework     | .NET 9 (ASP.NET Core API)                                  |
| Architecture  | DDD + CQRS                                                 |
| Validation    | FluentValidation                                           |
| Database      | MSSQL (local)                                              |
| ORM           | Entity Framework Core                                      |
| Logging       | Serilog (`app-20250805_002.log`, `tracking-20250805.json`) |
| Testing       | xUnit (unit + integration)                                 |
| Documentation | Swagger (auto-generated)                                   |

## API Endpoints

### 1. **Create Order**

* **Endpoint:** `POST /orders`
* **Request:**

```json
{
  "invoiceAddress": "123 Sample Street, 90402 Berlin",
  "invoiceEmailAddress": "customer@example.com",
  "invoiceCreditCardNumber": "1234-5678-9101-1121",
  "items": [
    {
      "productId": "12345",
      "productName": "Gaming Laptop",
      "productAmount": 2,
      "productPrice": 1499.99
    }
  ]
}
```

* **Responses:**

  * `201 Created`: Order created with unique `orderNumber`
  * `400 Bad Request`: Validation errors (e.g., invalid email, out-of-stock)

### 2. **Get Order by Order Number**

* **Endpoint:** `GET /orders/{orderNumber}`

* **Response:**

```json
{
  "orderNumber": "54321",
  "invoiceAddress": "123 Sample Street, 90402 Berlin",
  "invoiceEmailAddress": "customer@example.com",
  "invoiceCreditCardNumber": "1234-5678-9101-1121",
  "items": [
    {
      "productId": "12345",
      "productName": "Gaming Laptop",
      "productAmount": 2,
      "productPrice": 1499.99
    }
  ],
  "createdAt": "2025-03-07T12:00:00Z"
}
```

## Features

* [x] **Domain-Driven Design** with aggregate root and value objects.
* [x] **CQRS** via MediatR for separation of write/read logic.
* [x] **FluentValidation** to validate all incoming requests.
* [x] **Serilog** structured logging:

  * General logs: `app-20250805_002.log`
  * Tracking logs: `tracking-20250805.json` via MediatR pipeline behavior.
* [x] **Database Reset** on startup (development mode).
* [x] **Swagger** for live API exploration.

## Testing

| Type            | Description                                           |
| --------------- | ----------------------------------------------------- |
| **Unit Tests**  | Cover Domain, Application, Infra layers using `xUnit` |
| **Integration** | End-to-end API and DB tests                           |

> Coverage includes domain rules, validation logic, and API contract compliance.

## Running the Project

```bash
dotnet run --project src/Order.Service.Api
```

3. Swagger UI available at:

```
https://localhost:7191/swagger
```

## Resetting the Database

On app startup in development mode, the database is:

* **Deleted**
* **Migrated**
* **Seeded** with default data

Errors during this process are logged using Serilog.

## Logging Setup

* `logs/app-20250805_002.log`: General operational logs
* `logs/tracking-20250805.json`: Event-based logs via MediatR tracking

## Architectural Notes

* **DDD** helps isolate domain complexity and enforce clean boundaries.
* **MediatR** simplifies application flow with separation of commands and queries.
* **Value Objects** improve immutability and self-validation.
* **TrackingBehavior** logs MediatR request lifecycle for observability.
  
