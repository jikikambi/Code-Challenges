# Order Processing Microservice (.NET 9, DDD, CQRS, Event Sourcing)

## Overview

This microservice handles **Order Processing** via a REST API, built with **.NET 9** and advanced **Domain-Driven Design (DDD)** principles.
It supports **full CQRS**, **Event Sourcing**, and **rich domain behavior** , enabling the system to store and replay events to reconstruct aggregate state.
It includes robust **validation**, **structured logging with correlation IDs**, and both **unit** and **integration** tests.

## Architecture

### Domain-Driven Design (DDD)

* `Order` is the **aggregate root**, implementing `IEventSourcedAggregate`.
* **Value Objects** for `CreditCard`, `Email`, and `Address` ensure immutability and self-validation.
* **Encapsulated collections** for `OrderItem`s , no direct list mutation.
* **Domain Events**:

  * `OrderCreatedDomainEvent`
  * `OrderItemAddedDomainEvent` (and other lifecycle events as needed)
* **Factories** for controlled aggregate creation.
* **Domain Services** for business logic that spans multiple aggregates.

### CQRS with MediatR

* **Commands** and **Queries** are isolated.
* Handlers implement `IRequestHandler<,>` for each request type.
* **Pipeline behaviors** for:

  * Validation
  * Tracking and correlation logging
  * Event store persistence

### Event Sourcing

* **`IEventStoreRepository<TAggregate>`** persists and retrieves domain events.
* **Event replay** rebuilds aggregates from historical events instead of loading from a traditional snapshot.
* Supports **auditability** and **debuggability** by keeping the full event history.

### Layered Structure

* **Domain** , Entities, Value Objects, Domain Events, Interfaces
* **Application** , Command/Query handlers, DTOs, Behaviors
* **Infrastructure** , Event Store persistence, EF Core, SQL storage

## Tech Stack

| Component     | Technology                                                |
| ------------- | --------------------------------------------------------- |
| Framework     | .NET 9 (ASP.NET Core API)                                 |
| Architecture  | DDD + CQRS + Event Sourcing                               |
| Event Store   | Custom `IEventStoreRepository` (JSON-based or DB-backed)  |
| Validation    | FluentValidation                                          |
| Database      | MSSQL (local)                                             |
| ORM           | Entity Framework Core                                     |
| Logging       | Serilog (structured, with correlation IDs & event chains) |
| Testing       | xUnit (unit + integration)                                |
| Documentation | Swagger (auto-generated)                                  |


## API Endpoints

### Create Order

**POST** `/orders`
Validates request, creates aggregate, raises `OrderCreatedDomainEvent`, persists to **Event Store**.

### Get Order by Order Number

**GET** `/orders/{orderNumber}`
Rebuilds the aggregate from **event history**.

## Features

* [x] **DDD Maturity**:

  * Aggregate roots
  * Value objects
  * Domain events
  * Encapsulated collections
  * Domain services
* [x] **CQRS** , Separate write/read paths with MediatR.
* [x] **Event Sourcing** , Persist and replay all domain events.
* [x] **FluentValidation** , Rich request validation.
* [x] **Serilog Structured Logging** , Includes correlation ID & request/event lifecycle tracking.
* [x] **Database Reset** (development mode).
* [x] **Swagger UI** , Live API exploration.

## Testing

| Type        | Description                                                   |
| ----------- | ------------------------------------------------------------- |
| Unit Tests  | Cover Domain logic, Event Sourcing, and Application layer     |
| Integration | Full-stack tests for API + Event Store + Database persistence |


## Running the Project

```bash
dotnet run --project src/Order.Service.Api
```

Swagger UI:

```
https://localhost:7191/swagger
```

## Logging Setup

* `logs/app-YYYYMMDD.log` , General operational logs
* `logs/tracking-YYYYMMDD.json` , Event-based and correlation logs

## Architectural Notes

* **DDD** ensures high cohesion, low coupling, and rich domain modeling.
* **Event Sourcing** provides traceability and rebuild capability for aggregates.
* **CQRS** cleanly separates command and query concerns.
* **Value Objects** prevent invalid states.
* **TrackingBehavior** ensures full observability of request and event chains.

## A visual architecture diagram


```mermaid

sequenceDiagram
    participant Client
    participant API
    participant ApplicationLayer
    participant DomainLayer
    participant EventStore as EventStoreRepository
    participant InfrastructureLayer
    participant Serilog

    Client->>API: POST /orders (Command)
    API->>ApplicationLayer: Send(CreateOrderCommand) via MediatR
    ApplicationLayer->>DomainLayer: Call Order.Create(...) factory
    DomainLayer->>DomainLayer: Validate value objects & invariants
    DomainLayer->>DomainLayer: Raise OrderCreatedDomainEvent
    DomainLayer->>EventStore: Append domain events to event store
    DomainLayer-->>ApplicationLayer: Return AggregateRoot with events
    ApplicationLayer->>InfrastructureLayer: Persist aggregate (EF Core)
    ApplicationLayer->>Serilog: Log via TrackingBehavior (JSON + text)
    InfrastructureLayer-->>ApplicationLayer: DB save result
    ApplicationLayer-->>API: Command result (OrderNumber)
    API-->>Client: HTTP 201 Created

    Client->>API: GET /orders/{orderNumber} (Query)
    API->>ApplicationLayer: Send(GetOrderByNumberQuery) via MediatR
    ApplicationLayer->>InfrastructureLayer: Retrieve order read model
    InfrastructureLayer-->>ApplicationLayer: Return read data
    ApplicationLayer->>Serilog: Log query lifecycle
    ApplicationLayer-->>API: Return order DTO
    API-->>Client: HTTP 200 OK

```

##  Domain Event + Event Store persistence flow

```mermaid

sequenceDiagram
    participant ApplicationLayer
    participant DomainLayer
    participant EventStore as EventStoreRepository
    participant InfrastructureLayer
    participant Serilog

    ApplicationLayer->>DomainLayer: Execute aggregate method (e.g., Order.Create)
    DomainLayer->>DomainLayer: Validate inputs & business rules
    DomainLayer->>DomainLayer: Raise DomainEvent (OrderCreatedDomainEvent)
    DomainLayer->>EventStore: AppendEvent(domainEvent, aggregateId, version)
    EventStore->>Serilog: Log event append (correlation ID, JSON)
    EventStore-->>DomainLayer: Confirmation of event stored
    DomainLayer-->>ApplicationLayer: AggregateRoot + uncommitted events
    ApplicationLayer->>InfrastructureLayer: Persist aggregate state (EF Core)
    InfrastructureLayer->>Serilog: Log persistence action
    InfrastructureLayer-->>ApplicationLayer: DB save result
```

<img width="1536" height="1024" alt="image" src="https://github.com/user-attachments/assets/e18b9179-021b-4109-be49-eb24648d4175" />

## Flow Diagram: Tracking Request Pipeline
End-to-end flow diagram of how the TRequest interacts with MediatR, LoggingBehavior, TrackingBehavior, and TrackingService:

```mermaid

%% Define lanes
%% Each lane represents a component or responsibility
%% Client/API, MediatR Pipeline, TrackingService, Handler

flowchart TD
    subgraph CLIENT_API [Client/API]
        A[Send TRequest] 
    end

    subgraph MEDIATR ["MediatR Pipeline"]
        B["MediatR.Send(TRequest)"]
        C["IPipelineBehavior<TRequest, TResponse><br>- Behaviors executed in order"]
        D["LoggingBehavior<br>- Logs request & context"]
        E["TrackingBehavior<br>- Checks isTrackingEnabled<br>- Calls TrackingService.AddEventAsync()"]
    end

    subgraph TRACKING ["TrackingService"]
        F["TrackingService.AddEventAsync<TRequest, TInput>"]
        G["Create EventChain object"]
        H["Log technical/operational info (Serilog)"]
        I["Index event into Elasticsearch (IElasticClient)"]
    end

    subgraph HANDLER ["Request Handler"]
        J["TRequest Handler<br>- Executes business logic"]
        K["TResponse returned"]
    end

    %% Connect flow
    A --> B
    B --> C
    C --> D
    D --> E
    E --> F
    F --> G
    F --> H
    F --> I
    E --> J
    J --> K
```
    
