# KoiFarm Backend

## English

### Overview

KoiFarm Backend is a server-side application that powers the KoiFarm platform, providing essential APIs and business logic for koi fish management and trading system. Built with .NET 6.0, this backend service implements secure payment processing, real-time notifications, and comprehensive data management for koi fish trading.

### Features

- User authentication and authorization with JWT
- Koi fish inventory management
- Order processing and tracking
- VNPay payment integration
- Email notifications
- Certificate management for koi fish
- Review system
- Blog management
- Data analytics and reporting

### Technology Stack

- .NET 6.0
- Entity Framework Core 6.0
- SQL Server
- Docker
- JWT Authentication
- SMTP Email Service
- Swagger/OpenAPI
- AutoMapper

### Prerequisites

- .NET 6.0 SDK
- SQL Server
- Docker (optional)
- Visual Studio 2022 or VS Code

### Getting Started

1. Clone the repository
2. Update database connection string in appsettings.json
3. Run database migrations
4. Run the application

### API Documentation

Access the Swagger documentation at: `https://localhost:7006/swagger`

### Docker Support

Build the Docker image:

```
Run the container:
```

## Vietnamese

### Tổng quan

KoiFarm Backend là ứng dụng phía máy chủ hỗ trợ nền tảng KoiFarm, cung cấp các API thiết yếu và logic nghiệp vụ cho hệ thống quản lý và giao dịch cá Koi. Được xây dựng bằng .NET 6.0, dịch vụ backend này triển khai xử lý thanh toán an toàn, thông báo thời gian thực và quản lý dữ liệu toàn diện cho việc giao dịch cá Koi.

### Tính năng

- Xác thực và phân quyền người dùng với JWT
- Quản lý kho cá Koi
- Xử lý và theo dõi đơn hàng
- Tích hợp thanh toán VNPay
- Thông báo qua email
- Quản lý chứng chỉ cho cá Koi
- Hệ thống đánh giá
- Quản lý blog
- Phân tích và báo cáo dữ liệu

### Công nghệ sử dụng

- .NET 6.0
- Entity Framework Core 6.0
- SQL Server
- Docker
- JWT Authentication
- Dịch vụ Email SMTP
- Swagger/OpenAPI
- AutoMapper

### Yêu cầu hệ thống

- .NET 6.0 SDK
- SQL Server
- Docker (tùy chọn)
- Visual Studio 2022 hoặc VS Code

### Hướng dẫn cài đặt

1. Clone dự án

```bash
git clone https://github.com/yourusername/koi-farm-backend.git
```

2. Cập nhật chuỗi kết nối database trong appsettings.json
3. Chạy migration database

```bash
dotnet ef database update
```

4. Chạy ứng dụng

```bash
dotnet run
```

### Tài liệu API

Truy cập tài liệu Swagger tại: `https://localhost:7006/swagger`

### Hỗ trợ Docker

Xây dựng Docker image:

```bash
docker build -t koi-farm-api .
```

Chạy container:

```bash
docker run -p 7006:80 koi-farm-api

