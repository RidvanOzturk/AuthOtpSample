# AuthOtpSample

AuthOtpSample is a minimal sample project that demonstrates email-based OTP (one-time password) authentication using .NET and C#.

## Features

- Request OTP via email
- Confirm OTP for login / account actions
- Basic API rate limiting
- FluentValidation-based request validation
- Clean Architecture (`Api`, `Application`, `Domain`, `Infrastructure`)
- Unit tests for core application logic

## Technology Stack

- .NET 10
- C# 14
- ASP.NET Core Web API
- FluentValidation
- xUnit for tests

## Getting Started

### Prerequisites

- .NET SDK 10 installed
- An IDE or editor (Visual Studio, VS Code, Rider, etc.)
- Internet connection (for restoring NuGet packages)

### Clone the Repository

```bash
git clone https://github.com/RidvanOzturk/AuthOtpSample.git
cd AuthOtpSample
```

### Configure `appsettings.json`

The project uses an Ethereal test email account for sending OTP emails.

1. Open `AuthOtpSample.Api/appsettings.json`.
2. Locate the email configuration section (for example `Smtp` or `EmailSettings`).
3. The Ethereal email and password are already written in `appsettings.json` for demo purposes.
4. Use those exact credentials to log into Ethereal.

> Warning: These credentials are **only for testing** and may stop working at any time. Never use them in production.

### Ethereal Test Inbox

You can see all OTP emails in the Ethereal web UI:

- Ethereal: https://ethereal.email/

Log in with:

- Email: `carolyne.ward@ethereal.email`
- Password: `qRVqFBVMZXJ9fbRNjH`

Log in using the email and password stored in `AuthOtpSample.Api/appsettings.json`.

## Running the API

From the solution root, run:

```bash
cd AuthOtpSample.Api
dotnet run
```

The API will start on `https://localhost:<port>` (port is printed in the console).

### Swagger UI

Swagger is enabled for easy testing.

Once the API is running, open:

- `https://localhost:<port>/swagger`

From there you can:
- Request an OTP to be sent
- Confirm the OTP
- Inspect request/response models

## Project Structure

- `AuthOtpSample.Api` – ASP.NET Core Web API layer (controllers, middleware, validators, `Program.cs`)
- `AuthOtpSample.Application` – Application services, use cases, business logic
- `AuthOtpSample.Domain` – Domain entities and core domain rules
- `AuthOtpSample.Infrastructure` – Persistence, email sender implementation, external integrations
- `AuthOtpSample.Application.Tests` – Unit tests for application layer

## Running Tests

```bash
cd AuthOtpSample.Application.Tests
dotnet test
```

## Notes

- This project is meant for learning and demo only.
- Replace Ethereal with a real email provider and secure the configuration (user-secrets, Key Vault, environment variables) before using similar code in production.
