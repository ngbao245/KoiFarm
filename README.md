# 🐟 Koi Farm Backend

## 🌟 Overview

A robust .NET Core Web API backend service for the Koi Farm e-commerce platform. This RESTful API provides comprehensive functionality for managing koi fish sales, user authentication, order processing, and inventory management.

### Key Features

- 🔐 JWT-based authentication & authorization
- 🏪 Product management system
- 🛒 Order processing & management
- 💳 VNPAY payment integration
- 📊 Analytics & reporting
- 📜 Digital certificate generation
- 🔒 Role-based access control
- 📱 Media file handling

## 🛠 Technologies

- .NET Core 6.0/7.0
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger/OpenAPI
- AutoMapper
- Docker (optional)

## 📋 Prerequisites

- Visual Studio 2022 (recommended) or Visual Studio Code
- .NET 6.0/7.0 SDK
- SQL Server
- Docker (optional)

## 🚀 Getting Started

1. **Clone the repository**
```bash
git clone https://github.com/your-username/koi-farm-backend.git
```

2. **Open the solution**
- Open `KoiFarm.sln` in Visual Studio 2022
- Or open the project folder in Visual Studio Code

3. **Configure connection string**
Update the `appsettings.json` file:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=KoiFarm;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "JWT": {
    "Secret": "your_jwt_secret_key",
    "ExpiryInHours": 24
  },
  "VNPay": {
    "TmnCode": "your_tmn_code",
    "HashSecret": "your_hash_secret",
    "PaymentUrl": "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"
  }
}
```

4. **Update Database**
```bash
# Using Package Manager Console in Visual Studio
Update-Database

# Or using .NET CLI
dotnet ef database update
```

5. **Run the Project**
- Press F5 in Visual Studio to run in debug mode
- Or use Visual Studio's "Start Without Debugging" (Ctrl+F5)
- Or via CLI:
```bash
dotnet run
```

The API will be available at `https://localhost:7000` (or your configured port)
Swagger documentation will be at `https://localhost:7000/swagger`

## 🐳 Docker Support (Optional)

1. **Build the Docker image**
```bash
docker build -t koifarm-api .
```

2. **Run the container**
```bash
docker run -p 8080:80 koifarm-api
```

## 📁 Project Structure

```
KoiFarm/
├── Controllers/    # API Controllers
├── Models/         # Domain Models
├── DTOs/           # Data Transfer Objects
├── Services/       # Business Logic
├── Repositories/   # Data Access Layer
├── Configurations/ # App configurations
├── Middlewares/    # Custom middleware
└── Utils/          # Utility classes
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a Pull Request

## 👥 Authors

This project was developed by:

- [@ngbao245](https://github.com/ngbao245) - Hoang Bao
- [@nghiantrong](https://github.com/nghiantrong) - Trong Nghia
- [@bardinGL](https://github.com/Bardingl) - Hung Hao

## 📧 Contact

For any inquiries about the project, please reach out to any of our team members:

- Hoang Bao - [Facebook](https://facebook.com/ng.bao245)
- Trong Nghia - [Facebook](https://www.facebook.com/trongnghia.nguyen.1238)
- Hung Hao - [Facebook](https://www.facebook.com/hao.nguyenhung.566)
