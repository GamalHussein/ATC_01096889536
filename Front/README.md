# Event Booking System - Backend (ASP.NET Core Web API)

This is the backend for the **Event Booking System** project. It handles authentication, event management, and booking logic.

## 🛠 Technologies Used

- ASP.NET Core 8 Web API
- Entity Framework Core
- SQL Server
- JWT Authentication

##  Project Structure

```
/backend
│
├── Controllers/
│   ├── AuthController.cs
│   ├── EventsController.cs
│   ├── BookingsController.cs
│   └── AdminController.cs
├── Models/
├── Data/
├── DTOs/
├── Services/
├── Migrations/
└── Program.cs
```

##  Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/GamalHussein/ATC_01096889536.git
cd event-booking-system/backend
```

### 2. Update Database Connection String

Open `appsettings.json` and set your SQL Server connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=EventBookingDB;Trusted_Connection=True;"
}
```

### 3. Apply Migrations

```bash
dotnet ef database update
```

### 4. Run the Application

```bash
dotnet run
```
Backend will start at `http://bokevent.runasp.net/`.

##  Authentication

Uses JWT-based authentication. Make sure to include `Authorization: Bearer <token>` in API requests after login.

##  Available Roles

- Admin
- User

## 🧪 Testing the API

Use tools like Postman or Swagger (enabled by default at `/swagger`) to test endpoints.
