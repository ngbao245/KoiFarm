# 🐟 Koi Farm Frontend

## 🌟 Overview

A modern React-based frontend application for the Koi Farm e-commerce platform. This web application provides a user-friendly interface for customers to browse, purchase koi fish, and manage their orders, while also offering administrative capabilities for inventory and order management.

### Key Features

- 🔐 Secure user authentication
- 🏪 Interactive product catalog with advanced filtering
- 🛒 Shopping cart and checkout system
- 💳 Integrated VNPAY payment gateway
- 📱 Responsive design for all devices
- 🔍 Real-time search functionality
- 📊 Admin dashboard for inventory management
- 📜 Certificate verification system
- 🌐 Multi-language support (English/Vietnamese)

## 🛠 Technologies

- React 18
- TypeScript
- Redux Toolkit
- Material-UI (MUI)
- Axios
- React Router
- i18next
- Styled Components
- Vite

## 📋 Prerequisites

- Node.js (v16.0 or higher)
- npm or yarn
- Modern web browser
- Backend API service running

## 🚀 Getting Started

1. **Clone the repository**
```bash
git clone https://github.com/your-username/koi-farm-frontend.git
cd koi-farm-frontend
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
VITE_API_URL=http://localhost:5000/api
VITE_VNPAY_URL=https://sandbox.vnpayment.vn/paymentv2/vpcpay.html
```

4. **Start development server**
```bash
npm run dev
# or
yarn dev
```

The application will be available at `http://localhost:5173`

## 🏗 Build for Production

```bash
npm run build
# or
yarn build
```

## 📁 Project Structure

```
src/
├── assets/        # Static assets (images, fonts)
├── components/    # Reusable UI components
├── features/      # Feature-based modules
├── hooks/         # Custom React hooks
├── layouts/       # Page layouts
├── pages/         # Route pages
├── services/      # API services
├── store/         # Redux store configuration
├── types/         # TypeScript type definitions
└── utils/         # Utility functions
```

## 🔗 API Integration

This frontend application connects to the Koi Farm API backend. Ensure the backend service is running and the `VITE_API_URL` environment variable is correctly configured.

## 🌐 Deployment

1. Build the project
2. Deploy the contents of the `dist` folder to your web server
3. Configure your web server to handle client-side routing

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
