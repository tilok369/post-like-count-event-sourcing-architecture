# Event Sourcing Architectural Pattern
## Implementation for News/Post LIKE Count
An event-sourcing architecture to manage blog post or news like count.

## What is Event Sourcing?
Event Sourcing is an architectural pattern where the state of an application is determined by a sequence of events rather than the current state stored in a database. Instead of updating a database record directly, changes in the system are captured as a series of immutable events, which can be replayed to reconstruct the application's state at any point in time.

# Give it a STAR :star:
Loving this repository? Show your support by giving this project a star!

[![GitHub stars](https://img.shields.io/github/stars/tilok369/post-like-count-event-sourcing-architecture.svg?style=social&label=Star)](https://github.com/tilok369/post-like-count-event-sourcing-architecture)


## Patterns Used
- Mediator
- CQRS
- Generic Repository
- Transactional Outbox
- MassTransit with RabbitMQ

## Technologies Used
- .NET 8 Web API (minimal API)
- MediatR
- Entity Framework Core
- Carter
- Swagger
- SQL Server
- MassTransit
- RabbitMQ

## Project Evaluation
This implementation is evolved from basic to highly scalable

- Basic implementation involved NewsEventStore with Events
- [Then added Projection with Summary Table](https://github.com/tilok369/post-like-count-event-sourcing-architecture/tree/summary-projection)
- [After that added with Outbox Pattern with Separate Background Service for Projection](https://github.com/tilok369/post-like-count-event-sourcing-architecture/tree/summary-projection-with-outbox)
- [Next, added MassTransit for Projection Update for High Scalability](https://github.com/tilok369/post-like-count-event-sourcing-architecture/tree/summary-projection-with-outbox-and-masstransit)

# License
This project is under [MIT License](https://github.com/tilok369/post-like-count-event-sourcing-architecture/blob/summary-projection-with-outbox-and-masstransit/LICENSE)
