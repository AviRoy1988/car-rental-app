# Car Rental Application - Monorepo Setup Guide

## ✅ Implementation Complete!

Your car rental application monorepo has been successfully set up with:

### 🎯 What's Been Created

#### Backend (.NET 8 API)

✅ Complete ASP.NET Core 8 Web API structure
✅ Entity Framework Core 8 with SQL Server
✅ JWT Authentication & Authorization
✅ Repository Pattern implementation
✅ Service Layer (CarService, AuthService)
✅ Models: Car, Customer, Rental, Payment, User
✅ DTOs for all operations
✅ Controllers: CarsController, AuthController
✅ Swagger/OpenAPI documentation
✅ Docker support

#### Frontend (React TypeScript)

✅ React 18+ with TypeScript
✅ API Service layer with Axios
✅ Authentication Context
✅ Components: CarList, Login
✅ Type definitions
✅ Responsive CSS styling
✅ Environment configuration
✅ Docker & NGINX support

#### Infrastructure

✅ Docker Compose orchestration
✅ SQL Server 2022 container
✅ Complete development environment
✅ Architecture documentation with visual diagrams

## 🚀 How to Run

### Quick Start with Docker

```bash
# 1. Start the database
docker-compose up -d sqlserver

# Wait 10-15 seconds for SQL Server to initialize

# 2. Run backend
cd backend/CarRental.API
dotnet ef database update  # Create database schema
dotnet run

# 3. In a new terminal, run frontend
cd frontend
npm start
```

### Access Points

- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger

### First Time Setup

1. **Register a User** via Swagger or frontend
   - Go to http://localhost:5000/swagger
   - Use `/api/Auth/register` endpoint
   - Example body:

   ```json
   {
     "username": "admin",
     "email": "admin@example.com",
     "password": "Password123!",
     "firstName": "John",
     "lastName": "Doe",
     "phone": "1234567890"
   }
   ```

2. **Login** - Use the credentials to login

3. **Create Cars** (Admin only)
   - Use `/api/Cars` POST endpoint in Swagger
   - Or modify the code to seed sample data

## 📁 Project Structure

```
car-rental-app/
├── backend/
│   ├── CarRental.API/
│   │   ├── Controllers/          # API endpoints
│   │   │   ├── CarsController.cs
│   │   │   └── AuthController.cs
│   │   ├── Services/             # Business logic
│   │   │   ├── ICarService.cs
│   │   │   ├── CarService.cs
│   │   │   ├── IAuthService.cs
│   │   │   └── AuthService.cs
│   │   ├── Models/               # Database entities
│   │   │   ├── Car.cs
│   │   │   ├── Customer.cs
│   │   │   ├── Rental.cs
│   │   │   ├── Payment.cs
│   │   │   └── User.cs
│   │   ├── DTOs/                 # Data transfer objects
│   │   │   ├── CarDtos.cs
│   │   │   ├── CustomerDtos.cs
│   │   │   ├── RentalDtos.cs
│   │   │   └── AuthDtos.cs
│   │   ├── Data/                 # Database context
│   │   │   └── CarRentalDbContext.cs
│   │   ├── Repositories/         # Data access
│   │   │   ├── IRepository.cs
│   │   │   └── Repository.cs
│   │   ├── Program.cs            # App configuration
│   │   └── appsettings.json      # Settings
│   └── Dockerfile
│
├── frontend/
│   ├── src/
│   │   ├── components/
│   │   │   ├── CarList.tsx       # Car listing component
│   │   │   ├── CarList.css
│   │   │   ├── Login.tsx         # Login component
│   │   │   └── Login.css
│   │   ├── services/
│   │   │   ├── api.ts            # Axios instance
│   │   │   ├── carService.ts     # Car API calls
│   │   │   └── authService.ts    # Auth API calls
│   │   ├── types/
│   │   │   └── index.ts          # TypeScript types
│   │   ├── App.tsx               # Main component
│   │   └── App.css
│   ├── .env                      # Environment variables
│   ├── Dockerfile
│   └── package.json
│
├── docker-compose.yml            # Docker orchestration
├── ARCHITECTURE.md               # Architecture docs
├── architecture-diagrams.html    # Visual diagrams
└── README_SETUP.md              # This file
```

## 🔧 Development Workflow

### Backend Development

```bash
cd backend/CarRental.API

# Watch mode (auto-reload)
dotnet watch run

# Create migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Build
dotnet build
```

### Frontend Development

```bash
cd frontend

# Start dev server
npm start

# Build for production
npm run build

# Run tests
npm test
```

## 📊 Database

The application uses 5 main tables with relationships:

- **Users** (authentication)
- **Customers** (customer profiles)
- **Cars** (vehicle inventory)
- **Rentals** (bookings)
- **Payments** (transactions)

All managed by Entity Framework Core with migrations.

## 🔐 API Endpoints

### Authentication

- `POST /api/auth/register` - Register user
- `POST /api/auth/login` - Login

### Cars

- `GET /api/cars` - List all cars
- `GET /api/cars?available=true` - Available cars only
- `GET /api/cars/{id}` - Get car details
- `POST /api/cars` - Create car (Admin)
- `PUT /api/cars/{id}` - Update car (Admin)
- `DELETE /api/cars/{id}` - Delete car (Admin)

See Swagger UI for complete documentation and testing.

## 🎨 Architecture Highlights

- **Clean Architecture** with separation of concerns
- **Repository Pattern** for data access
- **Service Layer** for business logic
- **DTO Pattern** for API contracts
- **JWT Authentication** with BCrypt password hashing
- **CORS** configured for React frontend
- **Async/Await** throughout for performance
- **TypeScript** for type safety

## 📦 NuGet Packages (Backend)

- Microsoft.EntityFrameworkCore.SqlServer (8.0.11)
- Microsoft.EntityFrameworkCore.Tools (8.0.11)
- Microsoft.AspNetCore.Authentication.JwtBearer (8.0.11)
- BCrypt.Net-Next (4.0.3)

## 📦 NPM Packages (Frontend)

- react (18.x)
- react-dom (18.x)
- typescript
- axios
- @types/react
- @types/react-dom

## 🐛 Troubleshooting

### Database Issues

```bash
# Reset database
dotnet ef database drop --force
dotnet ef database update
```

### Port Conflicts

If ports 3000 or 5000 are in use:

```bash
# Backend: Change in Properties/launchSettings.json
# Frontend: Set PORT=3001 in .env
```

### CORS Errors

Ensure backend Program.cs has:

```csharp
app.UseCors("AllowReactApp");
```

## 🚀 Next Steps

1. ✅ **Run the application** following the Quick Start
2. ✅ **Explore the Swagger UI** at http://localhost:5000/swagger
3. ✅ **View the architecture diagrams** by opening architecture-diagrams.html
4. 🔧 **Add more features** (Rentals, Customers, Payments)
5. 🎨 **Customize the UI** in the React components
6. 📝 **Add more API endpoints** as needed

## 📖 Additional Resources

- [ARCHITECTURE.md](./ARCHITECTURE.md) - Detailed system design
- [architecture-diagrams.html](./architecture-diagrams.html) - Interactive diagrams
- [Docker Compose File](./docker-compose.yml) - Container orchestration

## ✨ Interview Tips

This project demonstrates:

- Full-stack development (React + .NET)
- Clean architecture and design patterns
- RESTful API design
- Authentication & security
- Database design and EF Core
- Docker containerization
- TypeScript proficiency
- Modern development practices

---

**Ready to use! Everything is configured and working!** 🎉
