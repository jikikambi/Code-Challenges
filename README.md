

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