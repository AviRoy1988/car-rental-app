# Car Rental App - Architecture Design

## System Architecture Overview

This document outlines the architecture for the Car Rental Application using .NET 8, SQL Server (Docker), and React.

## Architecture Diagram

```mermaid
graph TB
    subgraph "Client Layer"
        A[React Frontend<br/>SPA Application]
        A1[React Router]
        A2[State Management<br/>Redux/Context]
        A3[HTTP Client<br/>Axios]
    end

    subgraph "API Gateway/Load Balancer"
        B[NGINX/API Gateway]
    end

    subgraph "Backend Layer - .NET 8"
        C[ASP.NET Core Web API]

        subgraph "Controllers"
            C1[Cars Controller]
            C2[Rentals Controller]
            C3[Customers Controller]
            C4[Auth Controller]
        end

        subgraph "Business Logic Layer"
            D1[Car Service]
            D2[Rental Service]
            D3[Customer Service]
            D4[Auth Service]
            D5[Payment Service]
        end

        subgraph "Data Access Layer"
            E1[Entity Framework Core]
            E2[Repositories]
            E3[Unit of Work]
        end

        F[JWT Authentication<br/>Middleware]
        G[Exception Handling<br/>Middleware]
    end

    subgraph "Database Layer"
        H[(SQL Server<br/>in Docker)]

        subgraph "Database Schema"
            H1[Cars Table]
            H2[Rentals Table]
            H3[Customers Table]
            H4[Users Table]
            H5[Payments Table]
        end
    end

    subgraph "External Services"
        I[Payment Gateway<br/>Stripe/PayPal]
        J[Email Service<br/>SendGrid]
        K[Storage Service<br/>AWS S3/Azure Blob]
    end

    A --> A1
    A --> A2
    A --> A3
    A3 --> B
    B --> C
    C --> C1
    C --> C2
    C --> C3
    C --> C4
    C1 --> D1
    C2 --> D2
    C3 --> D3
    C4 --> D4
    D1 --> E1
    D2 --> E1
    D3 --> E1
    D4 --> E1
    D2 --> D5
    E1 --> E2
    E1 --> E3
    E2 --> H
    C --> F
    C --> G
    D5 --> I
    D2 --> J
    D1 --> K
    H --> H1
    H --> H2
    H --> H3
    H --> H4
    H --> H5

    style A fill:#61dafb,stroke:#333,stroke-width:2px,color:#000
    style C fill:#512bd4,stroke:#333,stroke-width:2px,color:#fff
    style H fill:#0078d4,stroke:#333,stroke-width:2px,color:#fff
```

## Component Architecture

```mermaid
graph LR
    subgraph "Frontend - React"
        A1[Pages/Views]
        A2[Components]
        A3[Services/API]
        A4[State Management]
        A5[Routing]

        A1 --> A2
        A1 --> A4
        A2 --> A3
        A5 --> A1
    end

    subgraph "Backend - .NET 8"
        B1[API Controllers]
        B2[Services]
        B3[Repositories]
        B4[Models/Entities]
        B5[DTOs]
        B6[Validators]

        B1 --> B2
        B2 --> B3
        B3 --> B4
        B1 --> B5
        B1 --> B6
    end

    subgraph "Database"
        C1[(SQL Server)]
    end

    A3 -->|HTTP/REST| B1
    B3 -->|EF Core| C1
```

## Data Flow Diagram

```mermaid
sequenceDiagram
    participant U as User
    participant F as React Frontend
    participant A as .NET 8 API
    participant D as SQL Server
    participant E as External Services

    U->>F: Browse Available Cars
    F->>A: GET /api/cars?available=true
    A->>D: Query Cars Table
    D-->>A: Return Available Cars
    A-->>F: JSON Response
    F-->>U: Display Cars

    U->>F: Select Car & Rent
    F->>A: POST /api/rentals
    A->>A: Validate Request
    A->>D: Check Car Availability
    D-->>A: Car Status
    A->>D: Create Rental Record
    A->>D: Update Car Status
    A->>E: Process Payment
    E-->>A: Payment Confirmation
    A->>E: Send Confirmation Email
    A-->>F: Rental Created
    F-->>U: Show Confirmation
```

## Database Schema

```mermaid
erDiagram
    CUSTOMERS ||--o{ RENTALS : creates
    CARS ||--o{ RENTALS : "rented in"
    RENTALS ||--o{ PAYMENTS : has
    USERS ||--o{ CUSTOMERS : "is a"

    CUSTOMERS {
        int Id PK
        string FirstName
        string LastName
        string Email
        string Phone
        string LicenseNumber
        datetime CreatedAt
        datetime UpdatedAt
    }

    CARS {
        int Id PK
        string Make
        string Model
        int Year
        string Color
        string VIN
        string LicensePlate
        decimal DailyRate
        string Status
        string Category
        int Mileage
        string ImageUrl
        datetime CreatedAt
        datetime UpdatedAt
    }

    RENTALS {
        int Id PK
        int CustomerId FK
        int CarId FK
        datetime StartDate
        datetime EndDate
        datetime ActualReturnDate
        decimal TotalCost
        string Status
        string Notes
        datetime CreatedAt
        datetime UpdatedAt
    }

    PAYMENTS {
        int Id PK
        int RentalId FK
        decimal Amount
        string PaymentMethod
        string TransactionId
        string Status
        datetime PaymentDate
        datetime CreatedAt
    }

    USERS {
        int Id PK
        string Username
        string PasswordHash
        string Email
        string Role
        bool IsActive
        datetime LastLogin
        datetime CreatedAt
        datetime UpdatedAt
    }
```

## Technology Stack

### Frontend

- **Framework**: React 18+
- **State Management**: Redux Toolkit / React Context
- **Routing**: React Router v6
- **HTTP Client**: Axios
- **UI Components**: Material-UI / Ant Design
- **Form Handling**: React Hook Form
- **Validation**: Yup / Zod
- **Build Tool**: Vite / Create React App

### Backend

- **Framework**: ASP.NET Core 8 Web API
- **ORM**: Entity Framework Core 8
- **Authentication**: JWT Bearer Tokens
- **Validation**: FluentValidation
- **API Documentation**: Swagger/OpenAPI
- **Logging**: Serilog
- **Caching**: Redis (optional)

### Database

- **Database**: SQL Server 2022
- **Container**: Docker
- **Migrations**: EF Core Migrations

### DevOps

- **Containerization**: Docker & Docker Compose
- **Version Control**: Git
- **CI/CD**: GitHub Actions / Azure DevOps

## Deployment Architecture

```mermaid
graph TB
    subgraph "Production Environment"
        subgraph "Docker Host"
            A[NGINX Container<br/>Port 80/443]
            B[React App Container<br/>Static Files]
            C[.NET 8 API Container<br/>Port 5000]
            D[SQL Server Container<br/>Port 1433]
            E[Redis Container<br/>Port 6379]
        end

        F[Docker Network]
        G[Docker Volumes<br/>Database Persistence]
    end

    H[Internet] --> A
    A --> B
    A --> C
    C --> D
    C --> E
    D --> G
    C --> F
    D --> F
    E --> F

    style A fill:#009639,stroke:#333,stroke-width:2px
    style B fill:#61dafb,stroke:#333,stroke-width:2px
    style C fill:#512bd4,stroke:#333,stroke-width:2px
    style D fill:#0078d4,stroke:#333,stroke-width:2px
    style E fill:#dc382d,stroke:#333,stroke-width:2px
```

## API Endpoints Structure

### Authentication

- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user
- `POST /api/auth/refresh` - Refresh token
- `POST /api/auth/logout` - Logout user

### Cars

- `GET /api/cars` - Get all cars (with filters)
- `GET /api/cars/{id}` - Get car by ID
- `POST /api/cars` - Create new car (Admin)
- `PUT /api/cars/{id}` - Update car (Admin)
- `DELETE /api/cars/{id}` - Delete car (Admin)
- `GET /api/cars/available` - Get available cars

### Rentals

- `GET /api/rentals` - Get all rentals
- `GET /api/rentals/{id}` - Get rental by ID
- `POST /api/rentals` - Create new rental
- `PUT /api/rentals/{id}` - Update rental
- `DELETE /api/rentals/{id}` - Cancel rental
- `GET /api/rentals/customer/{customerId}` - Get customer rentals
- `POST /api/rentals/{id}/complete` - Complete rental

### Customers

- `GET /api/customers` - Get all customers
- `GET /api/customers/{id}` - Get customer by ID
- `POST /api/customers` - Create customer
- `PUT /api/customers/{id}` - Update customer
- `DELETE /api/customers/{id}` - Delete customer

### Payments

- `POST /api/payments` - Process payment
- `GET /api/payments/{id}` - Get payment details
- `GET /api/payments/rental/{rentalId}` - Get rental payments

## Security Considerations

1. **Authentication & Authorization**
   - JWT token-based authentication
   - Role-based access control (Admin, Customer)
   - Secure password hashing (BCrypt)

2. **Data Protection**
   - HTTPS/TLS encryption
   - SQL injection prevention (EF Core parameterized queries)
   - XSS protection
   - CSRF tokens

3. **API Security**
   - Rate limiting
   - CORS configuration
   - Input validation
   - Request size limits

## Performance Optimization

1. **Frontend**
   - Code splitting
   - Lazy loading
   - Image optimization
   - Caching strategies

2. **Backend**
   - Database indexing
   - Query optimization
   - Response caching
   - Async/await patterns

3. **Database**
   - Proper indexing
   - Query performance tuning
   - Connection pooling

## Development Workflow

1. **Local Development**

   ```bash
   # Start database
   docker-compose up -d sqlserver

   # Run backend
   cd backend
   dotnet run

   # Run frontend
   cd frontend
   npm start
   ```

2. **Docker Compose Setup**
   - All services run in containers
   - Hot reload for development
   - Volume mounting for code changes

## Monitoring & Logging

- Application logs (Serilog)
- Error tracking (Application Insights / Sentry)
- Performance monitoring
- Database query logging
- API metrics

## Future Enhancements

1. Microservices architecture
2. Message queuing (RabbitMQ/Azure Service Bus)
3. Advanced analytics dashboard
4. Mobile application (React Native)
5. Real-time notifications (SignalR)
6. Integration with third-party services
