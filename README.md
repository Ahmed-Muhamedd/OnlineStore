# OnlineStore API

A modern e-commerce REST API built with ASP.NET Core 9, Entity Framework Core, Identity, and SQL Server. This application provides comprehensive functionality for managing products, orders, reviews, and customer authentication.

## ✨ Features

### Product Management
- Create, read, update, and delete products
- Support for multiple product images (up to 10 per product)
- Product categorization
- Pagination and filtering capabilities
- Inventory management with stock tracking

### Order Management
- Create and retrieve orders
- Automatic order total calculation
- Transactional order processing with rollback support
- Track customer orders
- Automatic inventory updates on order creation

### Review System
- Add, update, and retrieve product reviews
- Customer-specific review management
- Edit and delete reviews

### Authentication & Authorization
- JWT-based authentication
- Identity management with ASP.NET Core Identity
- Role-based access control (Admin, Customer)
- User registration and login

### Payment & Shipping
- Payment processing and tracking
- Shipping information management
- Estimated delivery date calculations

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 9
- **Language**: C# 13.0
- **ORM**: Entity Framework Core
- **Database**: SQL Server
- **Authentication**: JWT Bearer Tokens
