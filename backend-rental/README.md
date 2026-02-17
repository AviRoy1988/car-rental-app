# Rental Service API - Minimal API with Strategy Pattern

## Overview

This is a simplified car rental service built using .NET 8 Minimal API, implementing the Strategy Pattern for pricing calculations. It focuses on the core rental functionality without authentication/authorization.

## Architecture

### Key Features

- ✅ **Minimal API** - No controllers, route-to-code pattern
- ✅ **Strategy Pattern** - Separate pricing calculators for each car category
- ✅ **Repository Pattern** - Generic data access layer
- ✅ **Single Responsibility** - Each pricing strategy handles one formula
- ✅ **Configurable Pricing** - Base rates in appsettings.json

### Project Structure

```
backend-rental/RentalService.API/
├── Models/
│   ├── Rental.cs                    # Main entity
│   ├── CarCategory.cs               # Enum: SmallCar, Combi, Truck
│   ├── RentalStatus.cs              # Enum: Active, Completed
│   ├── PriceCalculationConfig.cs    # Configuration model
│   └── DTOs/
│       ├── PickupRentalRequest.cs
│       ├── ReturnRentalRequest.cs
│       └── RentalDto.cs
├── Data/
│   └── RentalDbContext.cs           # EF Core DbContext
├── Repositories/
│   ├── IRepository.cs               # Generic repository interface
│   └── Repository.cs                # Generic repository implementation
├── Pricing/                         # Strategy Pattern
│   ├── IPriceCalculator.cs          # Strategy interface
│   ├── SmallCarPriceCalculator.cs   # SmallCar pricing formula
│   ├── CombiPriceCalculator.cs      # Combi pricing formula
│   ├── TruckPriceCalculator.cs      # Truck pricing formula
│   └── PriceCalculatorFactory.cs    # Factory to resolve calculators
├── Services/
│   ├── IRentalService.cs
│   └── RentalService.cs             # Business logic
└── Program.cs                       # Minimal API endpoints + DI setup
```

## Database

**SQL Server**: Same container as main app (port 1433)
**Database Name**: `RentalServiceDB`
**Table**: `Rentals`

### Schema

| Column                       | Type                    | Description                              |
| ---------------------------- | ----------------------- | ---------------------------------------- |
| Id                           | int                     | Primary key                              |
| BookingNumber                | uniqueidentifier (GUID) | Auto-generated unique booking identifier |
| RegistrationNumber           | nvarchar(20)            | Car registration/license plate           |
| CustomerSocialSecurityNumber | nvarchar(20)            | Customer SSN                             |
| Category                     | int                     | 0=SmallCar, 1=Combi, 2=Truck             |
| PickupDateTime               | datetime2               | Pickup timestamp                         |
| PickupMeterReading           | int                     | Odometer at pickup (km)                  |
| ReturnDateTime               | datetime2               | Return timestamp (nullable)              |
| ReturnMeterReading           | int                     | Odometer at return (nullable)            |
| CalculatedPrice              | decimal(18,2)           | Final price (nullable)                   |
| Status                       | int                     | 0=Active, 1=Completed                    |
| CreatedAt                    | datetime2               | Record creation                          |
| UpdatedAt                    | datetime2               | Last update (nullable)                   |

## API Endpoints

**Base URL**: `http://localhost:5100`
**Swagger**: `http://localhost:5100/swagger`

### 1. Register Car Pickup

**POST** `/api/rentals/pickup`

**Request Body:**

```json
{
  "registrationNumber": "ABC123",
  "customerSocialSecurityNumber": "123456-7890",
  "category": 0,
  "pickupDateTime": "2026-02-12T10:00:00Z",
  "pickupMeterReading": 50000
}
```

**Response:** `201 Created`

```json
{
  "id": 1,
  "bookingNumber": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "registrationNumber": "ABC123",
  "customerSocialSecurityNumber": "123456-7890",
  "category": "SmallCar",
  "pickupDateTime": "2026-02-12T10:00:00Z",
  "pickupMeterReading": 50000,
  "returnDateTime": null,
  "returnMeterReading": null,
  "calculatedPrice": null,
  "status": "Active",
  "numberOfDays": null,
  "numberOfKm": null
}
```

### 2. Register Car Return

**POST** `/api/rentals/{bookingNumber}/return`

**Request Body:**

```json
{
  "returnDateTime": "2026-02-15T14:00:00Z",
  "returnMeterReading": 50450
}
```

**Response:** `200 OK`

```json
{
  "id": 1,
  "bookingNumber": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "category": "SmallCar",
  "pickupDateTime": "2026-02-12T10:00:00Z",
  "pickupMeterReading": 50000,
  "returnDateTime": "2026-02-15T14:00:00Z",
  "returnMeterReading": 50450,
  "calculatedPrice": 400.0,
  "status": "Completed",
  "numberOfDays": 4,
  "numberOfKm": 450
}
```

### 3. Get Rental by Booking Number

**GET** `/api/rentals/{bookingNumber}`

### 4. Get All Rentals

**GET** `/api/rentals`

### 5. Get Active Rentals

**GET** `/api/rentals/active`

## Pricing Formulas (Strategy Pattern)

### Configuration (appsettings.json)

```json
{
  "PriceCalculation": {
    "BaseDayRental": 100.0,
    "BaseKmPrice": 5.0
  }
}
```

### Car Categories

**SmallCar** (`SmallCarPriceCalculator`)

```
Price = baseDayRental × numberOfDays
Example: 100 × 4 = 400 SEK
```

**Combi** (`CombiPriceCalculator`)

```
Price = baseDayRental × numberOfDays × 1.3 + baseKmPrice × numberOfKm
Example: 100 × 4 × 1.3 + 5 × 450 = 520 + 2250 = 2770 SEK
```

**Truck** (`TruckPriceCalculator`)

```
Price = baseDayRental × numberOfDays × 1.5 + baseKmPrice × numberOfKm × 1.5
Example: 100 × 4 × 1.5 + 5 × 450 × 1.5 = 600 + 3375 = 3975 SEK
```

### Adding New Car Category (Strategy Pattern Benefit)

1. Add to enum:

```csharp
public enum CarCategory
{
    SmallCar = 0,
    Combi = 1,
    Truck = 2,
    SUV = 3  // New category
}
```

2. Create new calculator:

```csharp
public class SUVPriceCalculator : IPriceCalculator
{
    public CarCategory ApplicableCategory => CarCategory.SUV;

    public decimal Calculate(int numberOfDays, int numberOfKm, PriceCalculationConfig config)
    {
        return config.BaseDayRental * numberOfDays * 1.4m + config.BaseKmPrice * numberOfKm * 1.2m;
    }
}
```

3. Register in DI (Program.cs):

```csharp
builder.Services.AddScoped<IPriceCalculator, SUVPriceCalculator>();
```

**No changes to existing code required!**

## Setup & Running

### Prerequisites

- .NET 8 SDK
- SQL Server (Docker container already running on port 1433)

### Run the API

```bash
cd backend-rental/RentalService.API
dotnet run --urls "http://localhost:5100"
```

### Access Swagger UI

```
http://localhost:5100/swagger
```

## Testing Scenarios

### Scenario 1: Small Car Rental

```bash
# Pickup
curl -X POST http://localhost:5100/api/rentals/pickup \
  -H "Content-Type: application/json" \
  -d '{
    "registrationNumber": "ABC123",
    "customerSocialSecurityNumber": "123456-7890",
    "category": 0,
    "pickupDateTime": "2026-02-12T10:00:00Z",
    "pickupMeterReading": 50000
  }'
# Returns: {"id":1,"bookingNumber":"3fa85f64-5717-4562-b3fc-2c963f66afa6",...}
# Copy the bookingNumber GUID from response

# Return (3 days later, 300 km) - use the GUID from pickup response
curl -X POST http://localhost:5100/api/rentals/3fa85f64-5717-4562-b3fc-2c963f66afa6/return \
  -H "Content-Type: application/json" \
  -d '{
    "returnDateTime": "2026-02-15T10:00:00Z",
    "returnMeterReading": 50300
  }'
# Expected price: 100 × 3 = 300 SEK
```

### Scenario 2: Combi Rental

```bash
# Pickup
curl -X POST http://localhost:5100/api/rentals/pickup \
  -H "Content-Type: application/json" \
  -d '{
    "registrationNumber": "XYZ789",
    "customerSocialSecurityNumber": "987654-3210",
    "category": 1,
    "pickupDateTime": "2026-02-10T08:00:00Z",
    "pickupMeterReading": 75000
  }'
# Copy the returned bookingNumber GUID

# Return (5 days later, 800 km) - use the GUID from response
curl -X POST http://localhost:5100/api/rentals/{GUID-FROM-RESPONSE}/return \
  -H "Content-Type: application/json" \
  -d '{
    "returnDateTime": "2026-02-15T18:00:00Z",
    "returnMeterReading": 75800
  }'
# Expected price: 100 × 5 × 1.3 + 5 × 800 = 650 + 4000 = 4650 SEK
```

### Scenario 3: Truck Rental

```bash
# Pickup
curl -X POST http://localhost:5100/api/rentals/pickup \
  -H "Content-Type: application/json" \
  -d '{
    "registrationNumber": "TRUCK99",
    "customerSocialSecurityNumber": "555555-5555",
    "category": 2,
    "pickupDateTime": "2026-02-08T06:00:00Z",
    "pickupMeterReading": 120000
  }'
# Copy the returned bookingNumber GUID

# Return (7 days later, 1200 km) - use the GUID from response
curl -X POST http://localhost:5100/api/rentals/{GUID-FROM-RESPONSE}/return \
  -H "Content-Type: application/json" \
  -d '{
    "returnDateTime": "2026-02-15T20:00:00Z",
    "returnMeterReading": 121200
  }'
# Expected price: 100 × 7 × 1.5 + 5 × 1200 × 1.5 = 1050 + 9000 = 10050 SEK
```

## Design Patterns Used

### 1. Strategy Pattern (Pricing)

- **Interface**: `IPriceCalculator`
- **Implementations**: `SmallCarPriceCalculator`, `CombiPriceCalculator`, `TruckPriceCalculator`
- **Factory**: `PriceCalculatorFactory`
- **Benefit**: Easy to add new car categories without modifying existing code

### 2. Repository Pattern (Data Access)

- **Interface**: `IRepository<T>`
- **Implementation**: `Repository<T>`
- **Benefit**: Abstracts data access, testable, swappable data source

### 3. Dependency Injection

- All dependencies injected via constructor
- Services registered in `Program.cs`
- Scoped lifetimes for database-dependent services

### 4. DTO Pattern

- Separate DTOs for requests/responses
- Entity-to-DTO mapping in service layer
- Prevents over-posting and controls API contract

## Validation Rules

- ✅ BookingNumber auto-generated as GUID (uniqueidentifier)
- ✅ Return date must be >= pickup date
- ✅ Return meter reading must be >= pickup meter reading
- ✅ Can only return Active rentals
- ✅ numberOfDays rounded up using `Math.Ceiling`

## Business Logic

### Days Calculation

```csharp
var numberOfDays = (int)Math.Ceiling((returnDateTime - pickupDateTime).TotalDays);
```

- Partial days count as full days
- 2.1 days → 3 days charged

### Km Calculation

```csharp
var numberOfKm = returnMeterReading - pickupMeterReading;
```

## Ports

- **Rental Service API**: `5100`
- **Main Car Rental API**: `5299`
- **Frontend**: `3000`
- **SQL Server**: `1433`

## Differences from Main API

| Feature        | Main API (CarRental.API)                      | Rental Service API              |
| -------------- | --------------------------------------------- | ------------------------------- |
| Architecture   | Controller-based                              | Minimal API                     |
| Authentication | JWT Bearer                                    | None                            |
| Authorization  | Role-based                                    | None                            |
| Tables         | 5 (Users, Cars, Customers, Rentals, Payments) | 1 (Rentals)                     |
| Pricing        | Static in service                             | Strategy Pattern                |
| Patterns       | Repository + Service                          | Repository + Service + Strategy |
| Port           | 5299                                          | 5100                            |
| Database       | CarRentalDB                                   | RentalServiceDB                 |

## For Interview Discussion

**Key Points to Mention:**

1. **Strategy Pattern Benefits**:
   - Open/Closed Principle (open for extension, closed for modification)
   - Each pricing formula isolated and testable
   - Factory pattern for strategy resolution
   - Easy to add new car categories

2. **Repository Pattern**:
   - Generic `IRepository<T>` for all entities
   - Separation of concerns
   - Testability via mocking

3. **Minimal API**:
   - Less boilerplate than controllers
   - Route-to-code pattern
   - Still supports OpenAPI/Swagger

4. **Assumptions Made**:
   - Days calculation rounds up (partial day = full day)
   - Same base rates for all categories (only multipliers differ)
   - BookingNumber auto-generated as GUID (globally unique identifier)
   - Price locked when rental returned (no recalculation)

5. **Extensibility**:
   - Adding per-rental pricing (capture rates at pickup)
   - Historical pricing (price configuration versioning)
   - Additional validations (business hours, car availability)
   - Discount strategies (seasonal, loyalty, bulk)
