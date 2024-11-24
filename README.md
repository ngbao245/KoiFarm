# 🐟 Koi Farm Backend

## 🌟 Overview

A robust Node.js-based backend service for the Koi Farm e-commerce platform. This RESTful API provides comprehensive functionality for managing koi fish sales, user authentication, order processing, and inventory management.

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

- Node.js
- Express.js
- TypeScript
- MongoDB
- Redis (for caching)
- JWT
- Mongoose
- Docker
- Jest (for testing)

## 📋 Prerequisites

- Node.js (v16.0 or higher)
- MongoDB (v4.4 or higher)
- Redis (v6.0 or higher)
- npm or yarn
- Docker (optional)

## 🚀 Getting Started

1. **Clone the repository**
```bash
git clone https://github.com/your-username/koi-farm-backend.git
cd koi-farm-backend
```

2. **Install dependencies**
```bash
npm install
# or
yarn install
```

3. **Configure environment variables**
Create a `.env` file in the root directory:
```env
# Server Configuration
PORT=5000
NODE_ENV=development

# Database Configuration
MONGODB_URI=mongodb://localhost:27017/koi-farm
REDIS_URL=redis://localhost:6379

# JWT Configuration
JWT_SECRET=your_jwt_secret_key
JWT_EXPIRES_IN=24h

# VNPAY Configuration
VNPAY_TMN_CODE=your_tmn_code
VNPAY_HASH_SECRET=your_hash_secret
VNPAY_URL=https://sandbox.vnpayment.vn/paymentv2/vpcpay.html

# Email Configuration (optional)
SMTP_HOST=smtp.example.com
SMTP_PORT=587
SMTP_USER=your_email@example.com
SMTP_PASS=your_email_password
```

4. **Start MongoDB and Redis**
Ensure MongoDB and Redis services are running on your machine.

5. **Start development server**
```bash
npm run dev
# or
yarn dev
```

The API will be available at `http://localhost:5000`

## 🐳 Docker Setup

1. **Build the Docker image**
```bash
docker-compose build
```

2. **Start the services**
```bash
docker-compose up -d
```

## 📝 API Documentation

API documentation is available at `/api-docs` when running the server. We use Swagger UI for API documentation.

## 📁 Project Structure

```
src/
├── config/        # Configuration files
├── controllers/   # Request handlers
├── middleware/    # Custom middleware
├── models/        # Database models
├── routes/        # API routes
├── services/      # Business logic
├── types/         # TypeScript types
├── utils/         # Utility functions
└── validators/    # Request validators
```

## 🧪 Testing

```bash
# Run tests
npm run test

# Run tests with coverage
npm run test:coverage
```

## 🏗 Build for Production

```bash
npm run build
# or
yarn build
```

## 🚀 Production Deployment

1. Build the project
2. Set environment variables for production
3. Start the server:
```bash
npm run start
# or
yarn start
```

## 🔍 Monitoring

The application includes:
- Health check endpoint at `/health`
- Basic monitoring at `/metrics`
- Error logging and tracking

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📧 Contact

For any inquiries, please reach out to [your-email@example.com](mailto:your-email@example.com)
