# Car Rental Application

A full-stack car rental application built with .NET 8, SQL Server, and React.

## 🏗️ Architecture

This application follows a three-tier architecture:

- **Frontend**: React 18+ (SPA)
- **Backend**: ASP.NET Core 8 Web API
- **Database**: SQL Server 2022 (Docker)

## 📋 Documentation

- [Architecture Design](./ARCHITECTURE.md) - Detailed architecture diagrams and design decisions

## 🚀 Quick Start

### Prerequisites

- Docker Desktop
- Node.js 18+ (for local development)
- .NET 8 SDK (for local development)

### Using Docker Compose (Recommended)

```bash
# Clone the repository
git clone <repository-url>
cd car-rental-app

# Start all services
docker-compose up -d

# Check service status
docker-compose ps

# View logs
docker-compose logs -f
```

### Access the Application

- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5000
- **API Documentation**: http://localhost:5000/swagger
- **Database**: localhost:1433

### Default Credentials

- **SQL Server**:
  - User: `sa`
  - Password: `YourStrong@Password123`

## 🛠️ Development Setup

### Backend (.NET 8)

```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend (React)

```bash
cd frontend
npm install
npm start
```

### Database Setup

```bash
# Start SQL Server container
docker-compose up -d sqlserver

# Run migrations
cd backend
dotnet ef database update
```

## 📁 Project Structure

```
car-rental-app/
├── backend/                # .NET 8 Web API
│   ├── Controllers/       # API Controllers
│   ├── Services/          # Business Logic
│   ├── Models/            # Domain Models
│   ├── Data/              # EF Core Context & Repositories
│   ├── DTOs/              # Data Transfer Objects
│   └── Migrations/        # Database Migrations
├── frontend/              # React Application
│   ├── src/
│   │   ├── components/    # Reusable Components
│   │   ├── pages/         # Page Components
│   │   ├── services/      # API Services
│   │   ├── store/         # State Management
│   │   └── utils/         # Utility Functions
│   └── public/
├── docker-compose.yml     # Docker Compose Configuration
└── ARCHITECTURE.md        # Architecture Documentation
```

## 🔑 Key Features

- **User Authentication & Authorization** (JWT)
- **Car Management** (CRUD operations)
- **Rental Management** (Book, manage, complete rentals)
- **Customer Management**
- **Payment Processing**
- **Role-based Access Control**
- **Real-time Availability Tracking**
- **Responsive UI**

## 🧪 Testing

### Backend Tests

```bash
cd backend
dotnet test
```

### Frontend Tests

```bash
cd frontend
npm test
```

## 📦 Deployment

### Production Build

```bash
# Build all services
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d --build
```

## 🔒 Security

- JWT-based authentication
- Password hashing with BCrypt
- HTTPS/TLS encryption
- SQL injection prevention
- CORS configuration
- Input validation

## 📊 Database Schema

See [ARCHITECTURE.md](./ARCHITECTURE.md) for detailed database schema and ER diagrams.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📝 License

This project is licensed under the MIT License.

## 🛟 Support

For issues and questions, please create an issue in the repository.
