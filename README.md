# Hotel Search API

## Overview
This project is a JSON REST web service for hotel search, developed using **.NET 8.0 (C#)**. It includes:

- **CRUD API** for managing hotel data.
- **Search API** to retrieve a list of hotels based on the user's current location.
- **Sorting mechanism** to prioritize hotels that are cheaper and closer.
- **Scalable architecture** designed for easy persistence integration.
- **Unit tests** to ensure reliability.

## Features
### 1. **CRUD API (Hotel Management)**
- Add, update, delete, and retrieve hotels.
- Data model includes:
  - `Hotel Name`
  - `Hotel Price`
  - `Hotel Geo Location (Latitude, Longitude)`

### 2. **Search API**
- Accepts **current geo-location (latitude & longitude)** as query parameters.
- Returns a list of hotels sorted by:
  - **Cheapest and closest first**
  - **Most expensive and farthest last**
- Supports **pagination**.

## Tech Stack
- **.NET 8.0 (ASP.NET Core Web API)** – Backend framework
- **.NET 8.0 (ASP.NET Core Razor pages)** – For login and simple front-end demonstration
- **Swagger** – API documentation
- **NUnit** – Unit testing framework


## Pagination
- Default page size: **5 results per page**
- Can't be adjusted

## Testing
Unit tests ensure:
- API handles edge cases (missing parameters, invalid locations, etc.).
- Test are run automatically with Github actions.

## Security Considerations
- **Input validation** to prevent invalid data.
- **Authentication/Authorization (optional future addition)**.
    - **Currently only Creating hotel requires authentication**

## Directory Structure
```
├───Controllers        # API controllers
├───Data               # Models & migrations
├───ViewData           # View Models
├───Services           # Business logic
├───Tests              # Unit tests
├───Pages              # Razor pages
└───wwwroot            # Static files (CSS, JS, etc.)
```
