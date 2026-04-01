# Development Setup & Configuration

## Local Development (Without Docker)

### Prerequisites

- .NET 10 SDK - https://dotnet.microsoft.com/download/dotnet
- Node.js 18+ - https://nodejs.org

### Backend

```bash
cd code/backend/TA-API
dotnet build
dotnet run
```

Runs on http://localhost:5148

Environment files:
- `appsettings.json` - Default config (SQLite at `./assessmentdb.sqlite`)
- `appsettings.Development.json` - Dev overrides

### Frontend

```bash
cd code/frontend/react
npm install
npm start
```

Dev server runs on http://localhost:1234

Available scripts:
- `npm start` - Start dev server with Parcel
- `npm run build` - Build for production
- `npm test` - Run Jest test suite
- `npm run test:watch` - Run tests in watch mode

## Configuration

### Hardcoded User ID

All tasks are scoped to a demo user. Currently hardcoded to `user123`.

To change the user ID, edit `code/frontend/react/src/app/pages/TaskItems/TaskItemsPage.jsx`:

```javascript
const HARDCODED_USER_ID = 'your-user-id-here';
```

### API Configuration

The frontend automatically detects the environment:
- Local Dev - Connects to http://localhost:5148
- Docker - Connects to http://backend:5148 (internal DNS)

This is handled in `code/frontend/react/src/app/services/TaskItemService.js`.

### Database

Location:
- Local dev - `code/backend/TA-API/assessmentdb.sqlite`
- Docker - `./data/assessmentdb.sqlite` (shared volume)

Migrations run automatically on application startup via `db.Database.Migrate()` in Program.cs.

To manually create new migrations:

```bash
cd code/backend/TA-API
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Testing

### Frontend Jest Tests

```bash
cd code/frontend/react
npm test
npm run test:watch
```

Test file: `src/app/services/TaskItemService.test.js`

Covers:
- Create task item (POST)
- Get all task items (GET)
- Get task by ID (GET)
- Get tasks by user ID (GET)
- Update task item (PUT)
- Delete task item (DELETE)
- Error handling for all operations

## Debugging

### Backend (VS Code)

Press F5 and select "Backend - Launch" to start debugging with breakpoints.

### Frontend

Open browser developer tools (F12) for console logs and network inspection.

## Troubleshooting

### Backend Issues

- "Cannot connect to database" - Ensure migrations ran (automatic on startup)
- "CORS errors" - Check CORS policy in Program.cs allows your frontend origin
- "Port 5148 already in use" - Stop conflicting process or change port in appsettings.json

### Frontend Issues

- "Cannot find module" - Run `npm install` in `code/frontend/react`
- "404 API errors" - Check that the backend is running on port 5148
- "Port 1234 already in use" - Stop conflicting Parcel process

### Docker Issues

- "Container exits immediately" - Check logs: `docker compose logs`
- "Database locked" - Delete `./data` folder and restart
- "Network connection refused" - Ensure both containers are on the same network
- "Build fails" - Try `docker compose down -v` then `docker compose up --build`

## API Reference (curl / Postman)

Base URL: `http://localhost:5148/api/taskitems`

### Create a task

```bash
curl -X POST http://localhost:5148/api/taskitems \
  -H "Content-Type: application/json" \
  -d '{
    "title": "My first task",
    "description": "Something to do",
    "userId": "user123"
  }'
```

### Get all tasks

```bash
curl http://localhost:5148/api/taskitems
```

### Get task by ID

```bash
curl http://localhost:5148/api/taskitems/1
```

### Get tasks by user ID

```bash
curl http://localhost:5148/api/taskitems/user/user123
```

### Update a task

```bash
curl -X PUT http://localhost:5148/api/taskitems/1 \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Updated title",
    "description": "Updated description",
    "isCompleted": true
  }'
```

### Delete a task

```bash
curl -X DELETE http://localhost:5148/api/taskitems/1
```

### Postman

Import these as individual requests in Postman, or use the collection URL format:

- **POST** `http://localhost:5148/api/taskitems` - Body: raw JSON
- **GET** `http://localhost:5148/api/taskitems` - No body
- **GET** `http://localhost:5148/api/taskitems/1` - No body
- **GET** `http://localhost:5148/api/taskitems/user/user123` - No body
- **PUT** `http://localhost:5148/api/taskitems/1` - Body: raw JSON
- **DELETE** `http://localhost:5148/api/taskitems/1` - No body

All responses follow this format:

```json
{
  "data": { ... },
  "error": null,
  "statusCode": 200
}
```
