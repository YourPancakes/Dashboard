# Dashboard 

## About the Project

The project is an administrative panel (“Dashboard”) with client part in React (Vite) and server part in ASP.NET. It provides management of the entity “Client” with the ability to view the list, add, edit, delete and view payment history, as well as setting a single “rate” (rate) for all clients.

The server part is implemented with ASP.NET Core API, uses Entity Framework Core and PostgreSQL for data storage (migrations and initialization via DbSeeder), JWT authentication (RSA keys for token signing and validation) and Swagger .

The client side is written in React using functional components and hooks. Login page (LoginPage.tsx) performs credential submission and on successful authentication saves the JWT to localStorage and redirects to /dashboard. The main page (DashboardPage.tsx) displays a table of customers, modal forms for creating/editing a customer and viewing payment history, as well as a block for setting and updating the overall “rate”

Translated with DeepL.com (free version)

---

## Start

1. Clone the repository and run:  
   ```bash
   https://github.com/YourPancakes/Dashboard.git
   cd Dashboard
   docker-compose up --build

2. Frontend at http://localhost:5173  
3. API at http://localhost:5000  
4. Login credentials: admin@mirra.dev / admin123  

(Windows)
Obtaining a token via curl:
```bash
curl.exe -X POST "http://localhost:5000/auth/login" ^
  -H "Content-Type: application/json" ^
  -d "{\"email\":\"admin@mirra.dev\",\"pwd\":\"admin123\"}"
```
Get All Clietns:
```bash
curl.exe "http://localhost:5000/clients" ^
  -H "Authorization: Bearer <TOKEN>"
```
Create new client:
```bash
curl.exe -X POST "http://localhost:5000/clients" ^
  -H "Authorization: Bearer TOKEN" ^ 
  -H "Content-Type: application/json" ^  
  -d "{\"name\":\"Dave\",\"email\":\"dave@example.com\",\"balanceT\":123.45}"
