# ERPDemo

A simple ERP demo project built with **ASP.NET Core** to practice and demonstrate two-tier architecture (Controller + Model/DTO + SQL Stored Procedures).

## 📌 Features
- Customer management (Add customers)
- Product management (Add products)
- Order management
- Fetch products sold within a date range (using SQL stored procedure)
- Demonstrates usage of DTOs and models with ASP.NET Core

## 🛠️ Tech Stack
- **ASP.NET Core MVC 8.0**
- **SQL Server** (Stored Procedures for database operations)
- **Entity Framework Core (optional for mapping DTOs)**
- **C# 12**

## ⚡ Project Structure
ERPDemo/
├── Controllers/
│ └── OrdersController.cs
├── Models/
│ └── Customer.cs
│ └── Product.cs
│├── DTOs/
││ └── CreateCustomerDto.cs
││ └── CreateProductDto.cs
││ └── CreateOrderDto.cs
││ └── CustomerDto.cs
││ └── ProductSoldDto.cs
││ └── OrderItemDto.cs
├── Views/
├── ERPDemo.csproj
└── README.md

DB query.txt contains the query to create Tables and stored Procedures
