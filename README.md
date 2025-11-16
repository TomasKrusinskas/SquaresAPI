# Squares API

A RESTful API service for detecting squares from a set of points in a 2D plane. The solution enables users to import points, manage point collections, and automatically identify all squares that can be formed from those points.

## Overview

The Squares API provides endpoints for managing points and detecting geometric squares. The service uses a well-established computational geometry algorithm combined with NetTopologySuite library for accurate square validation, ensuring that only true squares are detected (not rectangles or other quadrilaterals).

## Architecture

The solution follows a clean architecture pattern with clear separation of concerns:

- **Squares.Api**: Web API layer providing REST endpoints
- **Squares.Core**: Business logic layer containing services, repositories, and domain models
- **Squares.Tests**: Unit and integration tests

## Prerequisites

Before launching the project, ensure the following are installed:

- **.NET 8.0 SDK** or later
  - Download from: https://dotnet.microsoft.com/download/dotnet/8.0
  - Verify installation: `dotnet --version` (should show 8.0.x or higher)

- **IDE or Editor** (optional but recommended):
  - Visual Studio 2022 or later
  - Visual Studio Code with C# extension
  - JetBrains Rider
  - Or any text editor with .NET CLI support

## Launch Instructions

### Method 1: Using .NET CLI (Recommended)

1. **Navigate to the project directory:**
   ```bash
   cd Squares.Api
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Build the solution:**
   ```bash
   dotnet build
   ```

4. **Run the API:**
   ```bash
   dotnet run
   ```

The API will start and be available at:
- HTTP: `http://localhost:5281`
- HTTPS: `https://localhost:7277`

### Method 2: Using Visual Studio

1. Open `Squares.sln` in Visual Studio
2. Set `Squares.Api` as the startup project
3. Press `F5` or click the "Run" button
4. The API will launch in the configured browser or can be accessed at the URLs above

### Method 3: Using Visual Studio Code

1. Open the project folder in VS Code
2. Open the terminal (Ctrl+` or Terminal > New Terminal)
3. Navigate to the API project:
   ```bash
   cd Squares.Api
   ```
4. Run the project:
   ```bash
   dotnet run
   ```

## Verifying the Launch

Once the API is running, verify it's working correctly:

1. **Check the console output** - Should display:
   ```
   Now listening on: http://localhost:5281
   ```

2. **Access Swagger UI** (Development mode only):
   Open a browser and navigate to:
   ```
   http://localhost:5281/swagger
   ```
   This provides an interactive interface to test all API endpoints.

3. **Test a simple endpoint:**
   ```bash
   curl http://localhost:5281/api/Points
   ```
   Should return an empty array `[]` if no points have been added yet.

## API Endpoints

The API provides the following endpoints:

### Points Management

- `GET /api/Points` - Retrieve all points
- `POST /api/Points` - Add a single point
- `POST /api/Points/import` - Import multiple points (replaces existing points)
- `DELETE /api/Points/{id}` - Delete a point by ID

### Squares Detection

- `GET /api/Squares` - Get all detected squares
- `GET /api/Squares/count` - Get the count of detected squares

## Example Usage

### Import Points and Detect Squares

1. **Import points that form a square:**

   **Windows PowerShell:**
   ```powershell
   curl.exe -X POST "http://localhost:5281/api/Points/import" -H "Content-Type: application/json" -d "{\"points\":[{\"x\":0,\"y\":0},{\"x\":1,\"y\":0},{\"x\":1,\"y\":1},{\"x\":0,\"y\":1}]}"
   ```

   **Or using Invoke-RestMethod (PowerShell native):**
   ```powershell
   Invoke-RestMethod -Uri "http://localhost:5281/api/Points/import" -Method Post -ContentType "application/json" -Body '{"points":[{"x":0,"y":0},{"x":1,"y":0},{"x":1,"y":1},{"x":0,"y":1}]}'
   ```

   **Linux/Mac/Git Bash:**
   ```bash
   curl -X POST "http://localhost:5281/api/Points/import" -H "Content-Type: application/json" -d '{"points":[{"x":0,"y":0},{"x":1,"y":0},{"x":1,"y":1},{"x":0,"y":1}]}'
   ```

2. **Retrieve detected squares:**
   ```bash
   curl http://localhost:5281/api/Squares
   ```

3. **Get square count:**
   ```bash
   curl http://localhost:5281/api/Squares/count
   ```

## Configuration

### Environment Variables

The application uses the following environment variables:

- `ASPNETCORE_ENVIRONMENT`: Set to `Development` for Swagger UI and detailed error pages
- Default ports can be changed in `Squares.Api/Properties/launchSettings.json`

### Port Configuration

The default ports are configured in `Squares.Api/Properties/launchSettings.json`:

```json
{
  "applicationUrl": "http://localhost:5281"
}
```

To use a different port, modify the `applicationUrl` value in the launch settings file. After changing the port, restart the application for the changes to take effect.

## Running Tests

To execute the test suite:

```bash
dotnet test
```

To run tests with detailed output:

```bash
dotnet test --verbosity normal
```

To run tests for a specific project:

```bash
dotnet test Squares.Tests/Squares.Tests.csproj
```

## Project Structure

```
SquaresAPI/
├── Squares.Api/              # Web API project
│   ├── Controllers/          # API controllers
│   ├── DTOs/                 # Data transfer objects
│   └── Program.cs            # Application entry point
├── Squares.Core/             # Core business logic
│   ├── Models/               # Domain models (Point, Square)
│   ├── Repositories/         # Data access layer
│   └── Services/             # Business logic services
├── Squares.Tests/            # Test project
│   ├── Controllers/          # Controller tests
│   ├── Models/               # Model tests
│   ├── Repositories/         # Repository tests
│   └── Services/             # Service tests
└── Squares.sln               # Solution file
```

## Dependencies

### Core Dependencies

- **.NET 8.0**: Target framework
- **ASP.NET Core**: Web framework
- **NetTopologySuite 2.5.0**: Geometric validation library
- **Swashbuckle.AspNetCore**: Swagger/OpenAPI documentation

### Development Dependencies

- **xUnit**: Testing framework
- **Moq**: Mocking library for unit tests
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing support

## Technical Details

### Square Detection Algorithm

The service uses a well-known computational geometry algorithm:

1. **Detection Phase**: Uses the "diagonal midpoint" approach to identify candidate squares by finding point pairs that share the same midpoint and diagonal length
2. **Validation Phase**: Uses NetTopologySuite library to validate that detected quadrilaterals are actual squares (not rectangles or rhombuses)

This two-phase approach ensures both efficiency and accuracy.

### Data Persistence

Currently, the application uses an in-memory repository. Data is stored in a thread-safe `ConcurrentDictionary` and persists only for the lifetime of the application process. Restarting the application will clear all data.

## Troubleshooting

### Port Already in Use

If the default port is already in use:

1. Change the port in `launchSettings.json`
2. Or stop the process using the port:
   - Windows: `netstat -ano | findstr :5281` then `taskkill /PID <pid> /F`
   - Linux/Mac: `lsof -ti:5281 | xargs kill`

### Build Errors

If encountering build errors:

1. Ensure .NET 8.0 SDK is installed: `dotnet --version`
2. Restore packages: `dotnet restore`
3. Clean and rebuild: `dotnet clean && dotnet build`

### Swagger UI Not Available

Swagger UI is only available in Development environment. Ensure:

1. `ASPNETCORE_ENVIRONMENT` is set to `Development`
2. Or modify `Program.cs` to enable Swagger in all environments
