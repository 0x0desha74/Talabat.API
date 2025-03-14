# Talabat.API

## Overview 🚀
Talabat.API is a RESTful e-commerce API built using ASP.NET Core. It provides endpoints for managing products, orders, users, and payments.

## Features ✨
- User authentication and authorization (JWT)
- CRUD operations for products and categories
- Order management
- Payment integration
- Secure API with validation and exception handling

## Technologies Used 🛠
- **ASP.NET Core**
- **Entity Framework Core**
- **SQL Server**
- **Swagger for API documentation**
- **Repository and Unit of Work patterns**
- **N-Tier Architecture**
- **Specification Design Pattern**

## Installation 📥
1. Clone the repository:
   ```bash
   git clone https://github.com/0x0desha74/Talabat.API.git
   ```
2. Navigate to the project directory:
   ```bash
   cd Talabat.API
   ```
3. Restore dependencies:
   ```bash
   dotnet restore
   ```
4. Update the database:
   ```bash
   dotnet ef database update
   ```
5. Run the application:
   ```bash
   dotnet run
   ```

## Configuration ⚙️
- Update the **appsettings.json** file with your database connection string.
- Configure JWT settings in **appsettings.json** for authentication.

## API Documentation 📄
Once the project is running, access the API documentation at:
```
http://localhost:5000/swagger
```

## Contributing 🤝
1. Fork the repository.
2. Create a new branch (`feature-branch`).
3. Commit your changes.
4. Push to the branch and create a Pull Request.

## License 📜
This project is licensed under the MIT License.

## Contact 📧
For any inquiries, reach out at **mudesha74@gmail.com**.

