# POS Application

A small Point of Sale (POS) application built as a hands-on learning project to explore enterprise software architecture patterns and backend design principles.

The main goal of this project is not only building a POS system, but also understanding how scalable applications are structured by applying concepts inspired by real-world enterprise systems.

## Project Description

Developing a POS application as a hands-on exploration of Enterprise Patterns.  
I am implementing the core philosophies of Clean Architecture and Domain-Driven Design (DDD), while manually building features inspired by the ABP Framework to deeply understand how scalable systems are architected from the ground up.

## Main Features

### Sales & Invoices

- Create invoices with multiple items.
- Support different payment methods.
- Manage invoice lifecycle:
  - Draft
  - Paid
  - Cancelled
- Generate PDF receipts after successful payment.
- Download and print receipts from the application.

### Inventory Management

- Track item quantities based on transactions.
- Stock is automatically affected by sales operations.
- Prevent selling items when available quantity is insufficient.
- Maintain stock transaction history.

### Purchasing

- Support purchase operations.
- Purchases increase item stock quantities.
- Assign purchases to specific warehouses.
- Track purchased items and quantities.

### Warehouses

- Manage multiple warehouses.
- Store inventory movements by warehouse.
- Prepare the system to support stock distribution between warehouses.

### Users & Authorization

- User management.
- Role-based permissions.
- Protect application features based on user permissions.

## Architecture & Design

The project follows Clean Architecture principles with clear separation between:



### Implemented Concepts

- Domain-Driven Design (DDD)
- Clean Architecture
- Repository Pattern
- Unit Of Work
- Domain Services
- Application Services
- DTO Mapping
- Dependency Injection
- Background Processing
- Outbox Pattern
- Retry Policies
- Circuit Breaker Pattern

## Background Processing

The application uses asynchronous processing for long-running operations:

- Outbox Pattern for reliable event publishing.
- Hangfire for background jobs.
- Receipt generation running asynchronously.
- Retry mechanism with exponential backoff.
- Circuit breaker handling repeated failures.

## Technology Stack

### Backend

- ASP.NET Core
- C#
- Entity Framework Core
- SQL Server
- MediatR
- Hangfire
- Polly

### Frontend

- Angular
- TypeScript
- Angular Material

### Reporting

- MigraDoc
- PDFSharp

## Project Purpose

This project is created for learning and practicing enterprise application development concepts.

The focus is on understanding:
- How business rules are modeled.
- How different layers communicate.
- How to build reliable background processes.
- How real-world applications handle failures and scalability concerns.

## Future Improvements

Possible future enhancements:

- Advanced inventory reports.
- Barcode scanning support.
- Customer loyalty system.
- Multi-terminal POS support.
- Real-time notifications.
- More advanced warehouse operations.
