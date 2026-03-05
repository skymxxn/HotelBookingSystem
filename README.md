# HotelBookingSystem

**Hotel Booking System** is a robust, production-ready **Modular Monolith API** built with **ASP.NET Core 8**. The project demonstrates a deep understanding of modern backend architecture, focusing on scalability, security, and clean code principles.

---

## Key Features

* **Modular Monolith Architecture:** Clear separation of concerns using a multilayered approach.
* **CQRS Pattern:** Implemented via **MediatR** to decouple command and query operations for better scalability.
* **Advanced Security:**
    * **JWT Authentication:** Robust Access & Refresh token logic.
    * **AccessService:** A custom-built authorization layer for granular data filtering (e.g., Managers only see their hotels, Users see only their bookings).
    * **Rate Limiting:** Protection against brute-force and spam (HTTP 429).
* **Performance:** Distributed caching strategy with **Redis** for high-traffic endpoints.
* **Communication:** SMTP integration with professional **HTML-templated** emails for verification and notifications.
* **Documentation:** Fully interactive API documentation via **Swagger/OpenAPI**.

## Tech Stack

* **Backend:** .NET 8, ASP.NET Core, Entity Framework Core (PostgreSQL)
* **Caching:** Redis
* **Messaging:** MediatR (CQRS)
* **Validation & Logging:** FluentValidation, Serilog (Structured Logging)
* **DevOps:** Docker, Docker Compose
* **Security:** ASP.NET Core Identity, JWT

---

## Architecture & Structure

The solution follows **Clean Architecture** principles to ensure the system remains testable and independent of external frameworks:

```
src/
├── HotelBooking.API             // Entry point: Controllers, Middlewares, DI Configuration
├── HotelBooking.Application     // Business Logic: CQRS Handlers, DTOs, Mapping, Validators
├── HotelBooking.Infrastructure  // External Services: Email (SMTP), Redis Cache, JWT Logic
├── HotelBooking.Persistence     // Data Access: EF Core Context, Migrations, Configurations
└── HotelBooking.Domain          // Enterprise Logic: Entities, Domain Rules, Contracts
```

---

## Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/)

### 1. Clone the repository

```bash
git clone https://github.com/skymxxn/HotelBookingSystem.git
```

### 2. Configure Environment

Copy the .env.template to .env and provide your specific credentials:

* SMTP settings for Email notifications.
* JWT Secrets for authentication.
* PostgreSQL Connection String.

Укажите:
- SMTP-данные
- Секреты для JWT
- Подключение к PostgreSQL
- Admin credentials

### 3. Creating migrations

Restore local tools
```bash
dotnet tool restore
```

Creating migrations:
```bash
dotnet ef migrations add InitialCreate --project src/HotelBookingSystem.Persistence --startup-project src/HotelBookingSystem.API
```

### 4. Run with Docker

```bash
docker-compose up --build
```

### 5. Access

- API Base: [http://localhost:8080/api/hotels](http://localhost:8080)  
- Swagger UI: [http://localhost:8080/swagger](http://localhost:8080/swagger)

---

## License

### This project is licensed under the MIT License.
---
Developed by [Ruslan (skymxxn)](https://github.com/skymxxn) Backend Developer dedicated to building clean, scalable, and secure systems.

