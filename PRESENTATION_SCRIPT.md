# Car Rental Service - Presentation Script

## Opening Statement - Requirement Alignment

**Opening:**
"Good [morning/afternoon], everyone. Thank you for the opportunity to present my solution to the car rental system challenge.

Before diving into the technical details, let me address how my implementation fulfills the core requirements:

**Core Requirements Met:**

1. ✅ **Three car categories** - Small car, Combi, and Truck - all implemented with correct pricing formulas
2. ✅ **Extensible design** - The Strategy Pattern allows adding new categories without modifying existing code
3. ✅ **Unique booking numbers** - Using GUIDs ensures global uniqueness
4. ✅ **Car pickup registration** - Captures all required fields: booking number, registration number, social security number, car category, pickup date/time, and odometer reading
5. ✅ **Car return registration** - Records booking number, return date/time, odometer reading, and automatically calculates the final price
6. ✅ **Correct pricing formulas** - All three formulas implemented exactly as specified
7. ✅ **Comprehensive testing** - Unit tests for all pricing calculators with multiple test cases

**Key Assumptions Made:**

- **Booking number generation**: I chose to auto-generate GUIDs rather than require manual entry, ensuring uniqueness and reducing errors
- **Days calculation**: Partial days count as full days using Math.Ceiling - aligning with industry standards
- **Status tracking**: Added 'Active' and 'Completed' statuses to track rental lifecycle
- **Validation rules**: Implemented business rules like return date must be after pickup, and return odometer must be higher than pickup

**Beyond Requirements:**
While the specification asked for business logic and tests, I've delivered a **production-ready system** including:

- RESTful API with Swagger documentation
- Persistent database storage with Entity Framework Core
- Repository Pattern for data access abstraction
- Dependency Injection for loose coupling
- Comprehensive error handling and validation
- Docker support for easy deployment

Now let me walk you through the architecture and implementation..."

**Transition:** "Let's start by looking at the overall system architecture..."

---

## Slide 1: Title Slide - Car Rental Service API

**Key Points:**
"This is the Car Rental Service API - a comprehensive backend solution built with .NET 8 that manages the complete lifecycle of car rentals, from pickup to return, including automatic price calculation.

The solution demonstrates modern software architecture patterns, clean code principles, and industry best practices."

**Transition:** "Let me show you the system architecture..."

---

## Slide 2: System Architecture

**Key Points:**
"The system follows a layered architecture approach with clear separation of concerns.

At the **Presentation Layer**, we have ASP.NET Core Web API with Controllers that provide a clean, RESTful interface for client applications.

The **Business Logic Layer** contains our RentalService and InvoiceService, which implement all the core business rules and validation logic.

For **Data Access**, we've implemented the Repository Pattern with Entity Framework Core, providing an abstraction layer over our SQL Server database.

And critically, we've applied the **Strategy Pattern** for our pricing calculations - allowing different pricing algorithms for Small Cars, Combis, and Trucks.

This architecture ensures that our code is maintainable, testable, and extensible."

**Transition:** "Now let's dive into how data flows through this system..."

---

## Slide 3: Data Flow - Car Pickup

**Key Points:**
"This sequence diagram shows the complete flow when a customer picks up a rental car.

The process starts when the **Client** makes a POST request to our pickup endpoint.

The **API layer** receives this request and delegates it to the **RentalService**, which is responsible for all business logic.

The service first **validates the input data** - ensuring all required fields are present and valid.

Then it checks with the **Repository** to ensure the booking number doesn't already exist - preventing duplicates.

Once validated, the service creates a new Rental entity, sets its status to Active, and generates a unique GUID as the BookingNumber.

The Repository then persists this to the **SQL Server database**, and the saved entity flows back up through the layers.

Finally, the client receives a 201 Created response with the complete rental details including the booking number they'll use for return."

**Transition:** "The return process is similar but involves price calculation..."

---

## Slide 4: Data Flow - Car Return

**Key Points:**
"When a customer returns a car, the flow is more complex because we need to calculate the final price.

After the API receives the return request with the booking number, the service retrieves the active rental from the database.

It then validates business rules - the return date must be after pickup, and the odometer reading must be higher than at pickup.

Here's where our **Strategy Pattern** comes into play: based on the car category, we select the appropriate pricing calculator.

The calculator computes the price based on the number of days, kilometers driven, and category-specific formulas.

We update the rental with the return details and final price, mark it as Completed, and save it back to the database.

The response includes the complete rental information with the calculated price."

**Transition:** "All of this data is stored in a streamlined database schema..."

---

## Slide 5: Database Schema

**Key Points:**
"We've adopted a **Single Table Design** for this service - everything is stored in one RENTAL table.

This design choice was intentional: it simplifies the data model while still capturing all necessary information for the rental lifecycle.

Key fields include:

- **BookingNumber** as our unique identifier - a GUID for global uniqueness
- **Status** to track whether a rental is Active or Completed
- **Pickup and Return details** including dates, times, and odometer readings
- **Car information** including category, registration number, and type
- **Pricing details** including base rates and the final calculated price

This normalized design eliminates complexity while maintaining all data integrity requirements."

**Transition:** "One of the most important architectural decisions was how we handle pricing..."

---

## Slide 6: Design Pattern - Strategy

**Key Points:**
"Different car categories have completely different pricing formulas. Small cars charge per kilometer, Combis use a base day rate, and Trucks have a mileage allowance system.

We could have used a giant if-else statement, but that would violate the Open-Closed Principle and make the code hard to maintain.

Instead, we've implemented the **Strategy Pattern**. Each pricing algorithm is encapsulated in its own strategy class that implements a common IPriceCalculator interface.

At runtime, the **PriceCalculatorFactory** selects the appropriate strategy based on the car category.

The beauty of this approach is **extensibility** - if tomorrow we need to add a new car category like 'Luxury SUV' with a different pricing model, we simply create a new strategy class. No existing code needs to change.

This is the Open-Closed Principle in action: open for extension, closed for modification."

**Transition:** "Another critical pattern we've used is the Repository Pattern..."

---

## Slide 7: Design Pattern - Repository

**Key Points:**
"The Repository Pattern provides a crucial abstraction layer between our business logic and data access.

We've implemented a **two-tier approach**:

First, a generic **IRepository<T>** that defines common CRUD operations - GetById, GetAll, Add, Update, Delete. This provides the foundation that can work with any entity type.

Then, we've created a specialized **IRentalRepository** that extends the base repository with domain-specific queries like:

- Finding rentals by booking number
- Retrieving all active rentals
- Filtering by car category
- Querying by date ranges

This separation provides several benefits:

- **Testability** - we can easily mock repositories in unit tests
- **Flexibility** - we can switch databases without changing business logic
- **Organization** - domain-specific queries are in one place

The service layer depends only on IRentalRepository through dependency injection, keeping our code loosely coupled."

**Transition:** "Of course, good architecture means nothing without solid testing..."

---

## Slide 8: Testing Strategy

**Key Points:**
"We've implemented comprehensive unit testing using industry-standard tools.

Our **testing stack** includes:

- **xUnit** - the modern .NET testing framework
- **Moq** - for creating mock objects and isolating units of code
- **InMemory Database** - for testing repository logic without a real database
- **Coverlet** - for measuring code coverage

We have extensive **test coverage** across:

- All three pricing calculator strategies with multiple test cases each
- The price calculator factory to ensure correct strategy selection
- Business rules like days calculation and validation logic
- Edge cases - what happens with zero kilometers, same-day returns, boundary conditions

The benefits are significant:

- We can **confidently refactor** knowing tests will catch regressions
- Tests **document expected behavior** better than comments
- We can validate **business logic independently** of infrastructure

In a real-world scenario, we'd also add integration tests and potentially end-to-end tests."

**Transition:** "All this functionality is exposed through a clean API..."

---

## Slide 9: API Endpoints

**Key Points:**
"The service exposes a straightforward RESTful API with three main endpoints.

**POST /api/rentals/pickup** - Registers a new car pickup. The client sends customer details, car information, pickup date/time, and odometer reading. The API returns the created rental with a unique booking number.

**POST /api/rentals/{bookingNumber}/return** - Handles the return process. Using the booking number from pickup, the client sends return details. The API calculates the price and returns the completed rental with the final cost.

**GET /api/rentals/{bookingNumber}** - Retrieves rental details by booking number. Useful for checking status or retrieving information.

All endpoints follow REST conventions, use appropriate HTTP verbs, and return standard status codes. The API is documented with Swagger/OpenAPI for easy integration."

**Transition:** "Let me wrap up with the key takeaways..."

---

## Slide 10: Summary

**Key Points:**
"To summarize what we've built:

**Requirements Fulfilled:**

- ✅ All three car categories implemented with correct pricing formulas
- ✅ Both use cases: car pickup and car return registration
- ✅ Automatic price calculation based on specified formulas
- ✅ Comprehensive test coverage for business logic
- ✅ Extensible design ready for additional car categories

**Technical Excellence:**

- **Clean architecture** with clear separation of concerns
- **Design patterns** - Strategy Pattern for pricing, Repository Pattern for data access
- **Professional code quality** - following SOLID principles and .NET best practices
- **Production-ready** - includes API, database, error handling, and Docker support

**Key Differentiators:**
Rather than just implementing the business logic in a console application, I've delivered a **complete, deployable system** that demonstrates:

- How the business logic integrates into a real-world application
- Proper separation of concerns and testability
- Industry-standard patterns and practices
- Scalability and maintainability considerations

This solution doesn't just meet the requirements - it provides a **solid foundation** that can evolve with changing business needs."

**Closing:**
"Thank you for your time. I'm happy to answer any questions about the implementation, design decisions, or how this solution addresses the specific requirements."

---

## Q&A Preparation

**Common Questions and Answers:**

**Q: Why use a single table instead of multiple tables?**
A: For this domain, a single table simplifies the design without sacrificing data integrity. All rental information is cohesive and accessed together. If we needed to track rental history, car inventory, or customer information separately, we'd introduce additional tables.

**Q: How would you handle multiple concurrent returns?**
A: The database transaction isolation and the GUID-based booking number ensure safe concurrent operations. Each rental is independent, and EF Core handles concurrency conflicts through optimistic concurrency control if needed.

**Deep Dive:** Three mechanisms work together:

1. **Database Transaction Isolation** - SQL Server ensures operations don't interfere. If two agents try to return the same car, only one succeeds.
2. **GUID Booking Numbers** - Globally unique identifiers eliminate coordination needs during creation.
3. **Status Validation** - We only allow returns for 'Active' rentals. Attempting to return a 'Completed' rental fails validation before hitting concurrency logic.

**Q: How would you extend the pricing logic to support discounts, seasonal multipliers, and corporate rates?**
A: There are several architectural approaches, each with different trade-offs:

**Approach 1: Decorator Pattern (Most Flexible)**
Wrap the base price calculation with additional behaviors:

```csharp
// Base calculator returns raw price
var basePrice = priceCalculator.Calculate(days, km, config);

// Apply decorators in sequence
var priceWithSeasonal = new SeasonalPriceDecorator(basePrice, rentalDate);
var priceWithDiscount = new DiscountDecorator(priceWithSeasonal, customer);
var finalPrice = new CorporateRateDecorator(priceWithDiscount, customer.CorporateId);

// Each decorator adds/modifies the price
public class SeasonalPriceDecorator : IPriceDecorator
{
    public decimal ApplyModifier(decimal basePrice, DateTime date)
    {
        var multiplier = IsHighSeason(date) ? 1.2m : 1.0m;
        return basePrice * multiplier;
    }
}

public class DiscountDecorator : IPriceDecorator
{
    public decimal ApplyDiscount(decimal price, Customer customer)
    {
        if (customer.LoyaltyPoints > 1000)
            return price * 0.9m; // 10% loyalty discount
        return price;
    }
}
```

**Approach 2: Enhanced Strategy with Modifiers**
Add a pipeline of price modifiers to the existing strategy:

```csharp
public interface IPriceModifier
{
    decimal Modify(decimal basePrice, RentalContext context);
    int Order { get; } // Control execution order
}

public class PriceCalculationService
{
    private readonly PriceCalculatorFactory _calculatorFactory;
    private readonly IEnumerable<IPriceModifier> _modifiers;

    public decimal CalculateFinalPrice(Rental rental, RentalContext context)
    {
        // 1. Get base price from strategy
        var calculator = _calculatorFactory.GetCalculator(rental.CarCategory);
        var basePrice = calculator.Calculate(rental.Days, rental.Km, config);

        // 2. Apply modifiers in order
        var finalPrice = basePrice;
        foreach (var modifier in _modifiers.OrderBy(m => m.Order))
        {
            finalPrice = modifier.Modify(finalPrice, context);
        }

        return finalPrice;
    }
}

// Example modifiers
public class SeasonalModifier : IPriceModifier
{
    public int Order => 1;
    public decimal Modify(decimal price, RentalContext context)
    {
        return context.IsHighSeason ? price * 1.2m : price;
    }
}

public class CorporateDiscountModifier : IPriceModifier
{
    public int Order => 2;
    public decimal Modify(decimal price, RentalContext context)
    {
        if (context.Customer?.CorporateId != null)
            return price * 0.85m; // 15% corporate discount
        return price;
    }
}

public class LoyaltyDiscountModifier : IPriceModifier
{
    public int Order => 3;
    public decimal Modify(decimal price, RentalContext context)
    {
        var discountPercent = context.Customer.LoyaltyTier switch
        {
            "Gold" => 0.10m,
            "Silver" => 0.05m,
            "Bronze" => 0.02m,
            _ => 0m
        };
        return price * (1 - discountPercent);
    }
}
```

**Approach 3: Configuration-Driven Rules Engine**
For complex business rules that change frequently:

```csharp
public class PriceRuleEngine
{
    public decimal ApplyRules(decimal basePrice, PricingContext context)
    {
        var rules = _ruleRepository.GetActiveRules(context.RentalDate);

        foreach (var rule in rules.OrderBy(r => r.Priority))
        {
            if (rule.Condition(context))
            {
                basePrice = rule.ApplyModification(basePrice);
            }
        }

        return basePrice;
    }
}

// Rules stored in database
public class PricingRule
{
    public string Name { get; set; } // "Summer Season Multiplier"
    public string RuleType { get; set; } // "Multiplier", "Discount", "FlatReduction"
    public decimal Value { get; set; } // 1.2, 0.15, 50
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public string Conditions { get; set; } // JSON: {"CustomerType": "Corporate"}
    public int Priority { get; set; }
}
```

**Recommendation for This Project:**
I'd use **Approach 2** (Enhanced Strategy with Modifiers) because:

- Maintains the existing Strategy Pattern for base pricing
- Adds extensibility through the Open-Closed Principle
- Each modifier is independently testable
- Easy to add/remove modifiers via dependency injection
- Order of application is clear and configurable
- No breaking changes to existing code

**Implementation Steps:**

1. Create `IPriceModifier` interface
2. Implement concrete modifiers (Seasonal, Corporate, Loyalty)
3. Register modifiers in DI container with order
4. Update `RentalService` to apply modifiers after base calculation
5. Add configuration for modifier parameters (seasonal dates, discount percentages)
6. Write unit tests for each modifier in isolation

**Q: What about authentication and authorization?**
A: This is a backend service focused on business logic. In production, we'd add authentication (JWT tokens, OAuth) at the API Gateway level or middleware, and implement role-based authorization for different operations.

**Q: How would you scale this service?**
A: The stateless design makes horizontal scaling straightforward. We could:

- Deploy multiple instances behind a load balancer
- Use distributed caching for frequently accessed data
- Implement event sourcing for audit trails
- Use async messaging for background processing

**Q: Why did you use Controllers for the Web API?**
A: Controllers provide a structured approach for organizing API endpoints with clear separation of concerns. They offer built-in features like model binding, validation, routing attributes, and filters that make building robust APIs straightforward. This approach follows proven .NET Web API patterns and makes the codebase familiar to other developers.

**Q: How do you handle validation?**
A: We validate at multiple layers - input validation at the API level using data annotations, business rule validation in the service layer, and database constraints for data integrity.

---

## Additional Q&A - Requirements Focused

**Q: How does your solution meet the core specification requirements?**
A: The solution implements both required use cases - car pickup and return registration - with all specified data fields. The pricing formulas are exactly as specified: Small car (baseDayRental _ days), Combi (baseDayRental _ days _ 1.3 + baseKmPrice _ km), and Truck (baseDayRental _ days _ 1.5 + baseKmPrice _ km _ 1.5). The Strategy Pattern ensures extensibility for new categories, and comprehensive unit tests verify all business logic.

**Q: Why did you go beyond a console application?**
A: While a console app would satisfy the minimum requirement, I wanted to demonstrate how the business logic fits into a real-world, production-ready system. The core business logic (pricing calculators, validation rules) remains the same - I've just added the infrastructure around it. This shows architectural thinking and how the solution would actually be deployed and consumed.

**Q: What assumptions did you make about ambiguities?**
A: Key assumptions:

- **Booking number generation**: Auto-generated GUIDs rather than manual entry for uniqueness and reliability
- **Days calculation**: Partial days count as full days (Math.Ceiling) - common in rental industries
- **Status tracking**: Added Active/Completed states for lifecycle management
- **Validation**: Enforced business rules like return date > pickup date, return km > pickup km
- **Data persistence**: Implemented database storage though not explicitly required - necessary for a real system

**Q: How would you add a new car category like "Luxury SUV"?**
A: Thanks to the Strategy Pattern:

1. Create a new class `LuxurySUVPriceCalculator` implementing `IPriceCalculator`
2. Implement the `Calculate` method with the new pricing formula
3. Register it in the dependency injection container
4. Add the mapping in `PriceCalculatorFactory`
   No existing code needs modification - this is the Open-Closed Principle in action.

**Q: How do you ensure the pricing formulas are correct?**
A: We have comprehensive unit tests for each calculator with multiple test cases covering:

- Standard scenarios with typical values
- Edge cases (zero kilometers, single day rentals)
- Boundary conditions
- Different combinations of days and kilometers
  Each test verifies the exact formula with known inputs and expected outputs.

**Q: How would you handle the case where baseDayRental or baseKmPrice changes?**
A: These values are configured in appsettings.json and injected via IOptions. To change rates:

- For immediate effect: Update configuration and restart
- For historical accuracy: We'd store the rates used for each rental in the database at booking time
- For future bookings: Implement rate versioning with effective dates

**Q: What would be your next steps if given more time?**
A: Priority additions:

1. **Integration tests** - Test the full API endpoints with a test database
2. **Customer management** - Store customer details separately for history and preferences
3. **Car inventory** - Track available cars and prevent double-booking
4. **Payment processing** - Integration with payment gateway
5. **Audit logging** - Track all operations for compliance
6. **Performance monitoring** - Add Application Insights or similar

**Q: How does this demonstrate understanding of software engineering principles?**
A: The solution showcases:

- **SOLID Principles**: Single Responsibility, Open-Closed (Strategy Pattern), Liskov Substitution, Interface Segregation, Dependency Inversion
- **DRY**: Generic repository eliminates duplicate data access code
- **Separation of Concerns**: Clear boundaries between layers
- **Testability**: Dependency injection enables easy mocking
- **Maintainability**: Clear structure, meaningful names, comprehensive tests
- **Production Readiness**: Error handling, configuration, Docker, API documentation
