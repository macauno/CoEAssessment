# Unosquare CoE .NET Technical Assessment

This repository hosts the projects used to evaluate members of the Unosquare CoE .NET team.

> **New here?** Jump to the [full setup guide (SETUP.md)](SETUP.md) for detailed configuration, API examples (curl/Postman), debugging, and troubleshooting.
>
> Just want to run it? Follow the **Quick Start** below.

## Quick Start (Recommended - Docker)

```bash
docker compose up --build
```

Then open:
- **Frontend** - http://localhost:3000
- **Backend API** - http://localhost:5148

The app will build both containers, run database migrations, and be ready in ~1-2 minutes.

## Local Development Setup (Without Docker)

### Prerequisites
- .NET 10 SDK - https://dotnet.microsoft.com/download/dotnet
- Node.js 18+ - https://nodejs.org

### 1. Start the Backend
```bash
cd code/backend/TA-API
dotnet build
dotnet run
```
Runs on http://localhost:5148

### 2. Start the Frontend (new terminal)
```bash
cd code/frontend/react
npm install
npm start
```
Runs on http://localhost:1234

### 3. Test the API
```bash
curl http://localhost:5148/api/taskitems/user/user123
```

> For all API endpoints and Postman setup, see [SETUP.md - API Reference](SETUP.md#api-reference-curl--postman)

## Architecture

- **Backend** - .NET 10 ASP.NET Core API with Entity Framework Core + SQLite
- **Frontend** - React 19 with Parcel bundler (plain JavaScript)
- **Database** - SQLite (auto-initialized via EF Core migrations on startup)
- **Containerization** - Docker Compose orchestration

## Key Features

- Full CRUD operations for task items
- User-scoped task filtering (demo user ID: `user123`)
- Automatic database migrations on startup
- CORS-enabled for cross-origin requests
- Jest test suite for frontend services
- Docker dev and production configurations

## Project Structure

```
code/
  backend/TA-API/        .NET 10 API
    Controllers/         API endpoints
    Services/            Business logic + DbContext
    Models/              Domain models + DTOs
    Migrations/          EF Core migrations
    Program.cs           App configuration

  frontend/react/        React application
    src/
      app/pages/         Page components
      app/components/    Reusable components
      app/services/      API service classes
    package.json         Dependencies
```

## Configuration

### User ID
Tasks are scoped to users. The frontend uses a hardcoded demo user ID: `user123`.
To change it, edit `code/frontend/react/src/app/pages/TaskItems/TaskItemsPage.jsx` line 6.

### Database
- **Local** - `assessmentdb.sqlite` (auto-created in backend folder)
- **Docker** - `./data/assessmentdb.sqlite` (persisted volume)

## Running Tests

### Backend (.NET xUnit)
```bash
cd code/backend
dotnet test
```

### Frontend (Jest)
```bash
cd code/frontend/react
npm test
```

## Troubleshooting

- **Docker build fails** - Ensure Docker Desktop is running and ports 5148/3000 are free
- **Frontend can't connect to API** - API URL auto-detects localhost vs Docker
- **Database errors** - Migrations run automatically on app startup
- **Port already in use** - Change ports in docker-compose.yml or stop conflicting services

> For more troubleshooting tips, see [SETUP.md - Troubleshooting](SETUP.md#troubleshooting)

## Guides

| Guide | Description |
|-------|-------------|
| [SETUP.md](SETUP.md) | Full development setup, API reference (curl/Postman), debugging, troubleshooting |
| [DOCKER-SETUP.md](DOCKER-SETUP.md) | Docker commands, container management, environment variables |

## Versions

- .NET 10 (EF Core SQLite, Serilog)
- React 19
- Angular 21 
