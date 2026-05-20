# OrderDispatchSystem

A microservices-based order dispatch platform built with .NET 10 and Clean Architecture. The system manages the full lifecycle of customer orders — from creation and verification through production plant assignment and driver dispatch.

## Architecture

The system consists of three independent microservices and a shared library:

```
OrderDispatchSystem/
├── UserService/       # Authentication, authorization, user management
├── OrderService/      # Order lifecycle and event publishing
├── DriverService/     # Driver profiles, scheduling, and availability
└── Shared/            
```

Each service follows a four-layer Clean Architecture:

| Layer | Responsibility |
|---|---|
| **API** | Minimal API endpoints, middleware, DI wiring |
| **Application** | CQRS handlers, DTOs, mappers, background workers |
| **Domain** | Entities, enums, business rules |
| **Infrastructure** | EF Core, repositories, Kafka producer, migrations |

Services communicate **synchronously** over REST and **asynchronously** via Apache Kafka using the Outbox pattern for transactional event delivery.

## Tech Stack

- **Runtime**: .NET 10 / ASP.NET Core 10
- **ORM**: Entity Framework Core 10
- **Databases**: PostgreSQL (one database per service)
- **Messaging**: Apache Kafka
- **Auth**: JWT + ASP.NET Core Identity
- **API Docs**: OpenAPI 10 / Scalar

## Services

### UserService

Handles authentication and user management.

### OrderService

Manages the order lifecycle and publishes integration events to Kafka.

### DriverService

Manages driver profiles, locations, and work schedules. Currently in early setup — endpoints forthcoming.

## Key Patterns

- **CQRS** — Custom mediator (`IMediator`) with `ICommandHandler` and `IQueryHandler` interfaces.
- **Outbox Pattern** — Events are written to an `OutboxMessages` table within the same transaction, then picked up by `OutboxPublisherWorker` and published to Kafka.
- **Repository Pattern** — Data access abstracted behind `IRepository<T>` and `IUnitOfWork`.
- **Database-per-Service** — Services share no databases; all cross-service communication goes through APIs or events.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL on `localhost:5432` (user: `postgres`, password: `postgres`)
- Apache Kafka on `localhost:9092`

## API Documentation

Each running service exposes interactive API docs via Scalar at `/scalar/v1`.
