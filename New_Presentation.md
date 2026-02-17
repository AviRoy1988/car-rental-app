# Car Rental Service - Presentation

---

## Slide 1: Title

# Car Rental Service API

A robust and extensible rental service built with .NET 8 Minimal API, demonstrating best practices in software design and architecture.

---

## Slide 2: System Architecture

This diagram shows the overall architecture, including the frontend, backend, and database layers.

```mermaid
graph TB
    subgraph "Client Layer"
        A[React Frontend]
    end

    subgraph "Backend Layer - .NET 8"
        C[ASP.NET Core Web API]

        subgraph "Business Logic Layer"
            D2[Rental Service]
        end

        subgraph "Data Access Layer"
            E1[Entity Framework Core]
            E2[Repositories]
        end
    end

    subgraph "Database Layer"
        H[(SQL Server<br/>in Docker)]
    end

    A --> C
    C --> D2
    D2 --> E2
    E2 --> E1
    E1 --> H
```

---

## Slide 3: Data Flow (Car Return)

This sequence diagram illustrates the process of returning a car and calculating the final price.

```mermaid
sequenceDiagram
    participant Client
    participant API as Minimal API
    participant Service as RentalService
    participant Calc as PriceCalculator
    participant Repo as Repository
    participant DB as SQL Server

    Client->>API: POST /api/rentals/{bookingNumber}/return
    API->>Service: RegisterReturnAsync(request)
    Service->>Repo: GetByBookingNumberAsync(bookingNumber)
    Repo->>DB: Query by booking number
    DB-->>Repo: Rental entity
    Service->>Service: Validate rental is Active
    Service->>Service: Calculate numberOfDays, numberOfKm
    Service->>Calc: Calculate(category, days, km, config)
    Calc-->>Service: price
    Service->>Service: Update CalculatedPrice, Status=Completed
    Service->>Repo: UpdateAsync(rental)
    Repo->>DB: Update rental
    Service-->>API: RentalDto with price
    API-->>Client: 200 OK
```

---

## Slide 4: Database Schema

The core of our system is a single `RENTAL` table designed to hold all necessary information for a rental lifecycle.

```mermaid
erDiagram
    RENTAL {
        int Id PK
        string BookingNumber UK "Unique booking identifier"
        string RegistrationNumber "Car registration/license plate"
        string CustomerSocialSecurityNumber "Customer SSN"
        int Category "0=SmallCar, 1=Combi, 2=Truck"
        datetime PickupDateTime "When car was picked up"
        int PickupMeterReading "Odometer reading at pickup (km)"
        datetime ReturnDateTime "When car was returned (nullable)"
        int ReturnMeterReading "Odometer reading at return (nullable)"
        decimal CalculatedPrice "Final price (nullable until returned)"
        int Status "0=Active, 1=Completed"
    }
```

---

## Slide 5: Design Pattern: Strategy

To handle different pricing formulas for each car category, the **Strategy Pattern** was used. This makes the system extensible and easy to maintain.

- **Problem**: Different car types have unique pricing rules.
- **Solution**: Encapsulate each pricing algorithm into its own "strategy" class.
- **Benefit**: We can add new car categories and pricing rules without changing existing code.

```mermaid
classDiagram
    class RentalService {
        -PriceCalculatorFactory _factory
        +CalculatePrice(rental)
    }
    class PriceCalculatorFactory {
        +GetCalculator(category) IPriceCalculator
    }
    class IPriceCalculator {
        <<interface>>
        +Calculate()
    }
    class SmallCarPriceCalculator {
        +Calculate()
    }
    class CombiPriceCalculator {
        +Calculate()
    }
    class TruckPriceCalculator {
        +Calculate()
    }

    RentalService --> PriceCalculatorFactory
    PriceCalculatorFactory --> IPriceCalculator
    IPriceCalculator <|.. SmallCarPriceCalculator
    IPriceCalculator <|.. CombiPriceCalculator
    IPriceCalculator <|.. TruckPriceCalculator
```

---

## Slide 6: API Endpoints

The service exposes a clear set of RESTful endpoints for managing rentals.

| Method | Endpoint                              | Description                             |
| ------ | ------------------------------------- | --------------------------------------- |
| POST   | `/api/rentals/pickup`                 | Register car pickup                     |
| POST   | `/api/rentals/{bookingNumber}/return` | Register car return and calculate price |
| GET    | `/api/rentals`                        | Get all rentals                         |
| GET    | `/api/rentals/active`                 | Get active rentals only                 |
| GET    | `/api/rentals/{bookingNumber}`        | Get rental by booking number            |

---

## Slide 7: Summary

- **Clean Architecture**: A well-structured and maintainable codebase.
- **Extensible Design**: The Strategy Pattern allows for easy addition of new car categories and pricing models.
- **Complete Implementation**: All core requirements for the rental service have been met.
- **Modern Tech Stack**: Built using the latest .NET 8 features for high performance.

This concludes the presentation. Thank you.
