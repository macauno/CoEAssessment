# Docker Setup Guide

This project can be run entirely in Docker containers. Both the backend (.NET 10 API) and frontend (React 19) applications are containerized and orchestrated using Docker Compose.

## Prerequisites

- Docker Desktop installed and running
- Docker Compose v2+ (included with Docker Desktop)

## Quick Start

### 1. Build and Run with Docker Compose

From the project root directory, run:

**For Development (local - includes debugging, Swagger, detailed errors):**
```bash
docker-compose up --build
```

**For Production (optimized, security hardened):**
```bash
docker-compose -f docker-compose.prod.yml up --build
```

This will:
- Build the backend Docker image
- Build the frontend Docker image
- Start both services and connect them

### 2. Access the Applications

- **Frontend:** http://localhost:3000
- **Backend API:** http://localhost:5148
- **Swagger API Docs (Dev only):** http://localhost:5148/swagger

## What Happens

1. **Backend Container** (`task-api-backend`)
   - Runs .NET 10 ASP.NET Core API on port 5148
   - Uses SQLite database stored in `./data` volume
   - Runs EF Core migrations automatically on startup
   - Includes health checks at `/lbhealth`

2. **Frontend Container** (`task-app-frontend`)
   - Builds React 19 application with Parcel
   - Serves static files on port 3000
   - Waits for backend to be healthy before starting
   - Connected to backend via internal Docker network

## Commands

**Basic - First Time Setup & Run (Development):**
```bash
docker-compose up --build
```

**Production - First Time Setup & Run:**
```bash
docker-compose -f docker-compose.prod.yml up --build
```

**Start services (development, normal run):**
```bash
docker-compose up
```

**Start services (production):**
```bash
docker-compose -f docker-compose.prod.yml up
```

**Start in background - Development (detached mode):**
```bash
docker-compose up -d
```

**Start in background - Production (detached mode):**
```bash
docker-compose -f docker-compose.prod.yml up -d
```

**Stop services (development):**
```bash
docker-compose down
```

**Stop services (production):**
```bash
docker-compose -f docker-compose.prod.yml down
```

**Stop services and delete database volume (development):**
```bash
docker-compose down -v
```

**Stop services and delete database volume (production):**
```bash
docker-compose -f docker-compose.prod.yml down -v
```

**View logs (all services, follow) - Development:**
```bash
docker-compose logs -f
```

**View logs (all services, follow) - Production:**
```bash
docker-compose -f docker-compose.prod.yml logs -f
```

**View backend logs:**
```bash
docker-compose logs -f backend
```

**View frontend logs:**
```bash
docker-compose logs -f frontend
```

**Rebuild images:**
```bash
docker-compose up --build
```

**Rebuild images (production):**
```bash
docker-compose -f docker-compose.prod.yml up --build
```

**Clean up everything (containers, images, volumes):**
```bash
docker-compose down -v
docker system prune -a
```

**Restart after cleanup:**
```bash
docker-compose up --build
```

## Project Structure

```
.
├── docker-compose.yml          # Orchestration file
├── code/
│   ├── backend/
│   │   └── TA-API/
│   │       ├── Dockerfile      # Backend container definition
│   │       └── .dockerignore
│   └── frontend/
│       └── react/
│           ├── Dockerfile      # Frontend container definition
│           └── .dockerignore
└── data/                        # Persistent database volume
```

## Troubleshooting

**Port already in use - Change ports in docker-compose.yml:**
```yaml
ports:
  - "8080:3000"  # Access frontend on localhost:8080
  - "5149:5148"  # Access backend on localhost:5149
```

**Check backend logs:**
```bash
docker-compose logs backend
```

**Check frontend logs:**
```bash
docker-compose logs frontend
```

**Container fails to start - Complete reset:**
```bash
docker-compose down -v
docker system prune -a
docker-compose up --build
```

**Database not persisting:**
Ensure the `./data` directory exists and Docker has write permissions.

**Rebuild without cache:**
```bash
docker-compose build --no-cache
docker-compose up
```

**Force recreate containers:**
```bash
docker-compose up --force-recreate
```

## Environment Variables

Backend environment variables can be modified in `docker-compose.yml`:

```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Production
```

Frontend environment variables:

```yaml
environment:
  - REACT_APP_API_URL=http://backend:5148
```

## Notes

- CORS is configured in the backend to allow requests from the frontend
- Database persists in the `./data` volume between container restarts
- Backend health check ensures frontend waits for API to be ready
- Both services communicate via Docker network `task-app-network`
