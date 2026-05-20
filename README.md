# Project Management System

A comprehensive project management system built with ASP.NET Core 9.0, implementing Clean Architecture principles with CQRS pattern. The system provides robust project and task management capabilities with real-time notifications, JWT authentication, and multi-language support.

## 🚀 Features

### Core Functionality
- **Project Management**: Create, update, delete, and track projects with detailed information
- **Task Management**: Complete task lifecycle management with assignments and status tracking
- **User Management**: User registration, authentication, and profile management
- **Role-Based Access Control**: Granular permission system with role management
- **Real-time Notifications**: SignalR-powered instant notifications for task and project updates
- **Email Integration**: Automated email notifications and OTP verification

### Authentication & Security
- **JWT Authentication**: Secure token-based authentication with access and refresh tokens
- **OAuth Integration**: Login with Google and LinkedIn
- **Two-Factor Authentication**: OTP-based email verification
- **Custom Token Encryption**: Enhanced security with token obfuscation

### Advanced Features
- **Multi-language Support**: Localization for English, German, French, and Arabic
- **Background Jobs**: Hangfire integration for scheduled tasks and background processing
- **File Upload**: Image and document management
- **Audit Logging**: Database interceptor for change tracking
- **Global Error Handling**: Centralized exception management middleware

## 🏗️ Architecture

The project follows Clean Architecture principles with clear separation of concerns:

```
ProjectManagement.Presentation/    # API Layer (Controllers, Middleware)
ProjectManagement.Core/           # Application Layer (CQRS Handlers, Business Logic)
ProjectManagement.Service/        # Service Layer (Business Services)
ProjectManagement.Infrastructure/ # Infrastructure Layer (Database, Repositories)
ProjectManagement.Domain/         # Domain Layer (Entities, Enums, Interfaces)
```

### Key Architectural Patterns
- **Clean Architecture**: Separation of concerns with dependency inversion
- **CQRS**: Command Query Responsibility Segregation using MediatR
- **Repository Pattern**: Data access abstraction
- **Dependency Injection**: Built-in ASP.NET Core DI container
- **Unit of Work**: Transaction management

## 🛠️ Technologies

### Backend Framework
- **.NET 9.0**: Latest .NET framework
- **ASP.NET Core Web API**: RESTful API development
- **Entity Framework Core 9.0**: ORM for database operations
- **SQL Server**: Primary database

### Libraries & Packages
- **MediatR**: CQRS implementation
- **AutoMapper**: Object-to-object mapping
- **FluentValidation**: Input validation
- **SignalR**: Real-time communication
- **Hangfire**: Background job processing
- **Swashbuckle (Swagger)**: API documentation
- **ASP.NET Core Identity**: Authentication and authorization

## 📋 Prerequisites

Before you begin, ensure you have the following installed:
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (2019 or later)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

## ⚙️ Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/Mohamed-334/Project-Management-System.git
cd Project-Management-System
```

### 2. Configure Database Connection

Update the connection string in `ProjectManagement.Presentation/appsettings.json`:

```json
"ConnectionStrings": {
  "ProjectManagement": "Server=YOUR_SERVER;Database=ProjectManagement;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

### 3. Configure JWT Settings

Update JWT settings in `appsettings.json`:

```json
"JwtSettings": {
  "Secret": "YOUR_STRONG_SECRET_KEY_HERE_AT_LEAST_32_CHARACTERS",
  "Issuer": "ProjectManagement",
  "Audience": "WebSite",
  "AccessTokenExpireDate": 1,
  "RefreshTokenExpireDate": 20
}
```

### 4. Configure Email Settings (Optional)

For email functionality, update the email settings:

```json
"emailSettings": {
  "port": "465",
  "host": "smtp.gmail.com",
  "FromEmail": "your-email@gmail.com",
  "FromName": "Your App Name",
  "password": "your-app-specific-password"
}
```

### 5. Configure OAuth Providers (Optional)

For Google and LinkedIn authentication:

```json
"Authentication": {
  "Google": {
    "ClientId": "YOUR_GOOGLE_CLIENT_ID",
    "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
  },
  "LinkedIn": {
    "ClientId": "YOUR_LINKEDIN_CLIENT_ID",
    "ClientSecret": "YOUR_LINKEDIN_CLIENT_SECRET"
  }
}
```

### 6. Apply Database Migrations

Navigate to the Presentation project directory and run:

```bash
cd ProjectManagement.Presentation
dotnet ef database update
```

This will create the database and apply all migrations, including seeding default roles and users.

### 7. Run the Application

```bash
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:5001/swagger`
- Hangfire Dashboard: `https://localhost:5001/dashboard`

## 📚 API Documentation

Once the application is running, access the Swagger documentation at:
```
https://localhost:5001/swagger
```

### Main API Endpoints

#### Authentication
- `POST /api/Authentication/Register` - User registration
- `POST /api/Authentication/Login` - User login
- `POST /api/Authentication/RefreshToken` - Refresh access token
- `POST /api/Authentication/SendOTP` - Send OTP for verification
- `POST /api/Authentication/ConfirmOTP` - Confirm OTP
- `GET /api/Authentication/GoogleAuthentication` - Google OAuth login
- `GET /api/Authentication/LinkedInAuthentication` - LinkedIn OAuth login

#### Projects
- `GET /api/Project` - Get all projects
- `GET /api/Project/{id}` - Get project by ID
- `POST /api/Project` - Create new project
- `PUT /api/Project` - Update project
- `DELETE /api/Project/{id}` - Delete project

#### Tasks
- `GET /api/Task` - Get all tasks
- `GET /api/Task/{id}` - Get task by ID
- `POST /api/Task` - Create new task
- `PUT /api/Task` - Update task
- `DELETE /api/Task/{id}` - Delete task

#### Users
- `GET /api/User` - Get all users
- `GET /api/User/{id}` - Get user by ID
- `PUT /api/User` - Update user profile

#### Roles
- `GET /api/Role` - Get all roles
- `POST /api/Role` - Create new role
- `PUT /api/Role` - Update role
- `DELETE /api/Role/{id}` - Delete role

## 🔐 Default Credentials

The system seeds default users during initial setup. Check the `UserSeeder.cs` and `RoleSeeder.cs` files for default credentials.

**⚠️ Important**: Change default credentials in production environments.

## 🌐 Localization

The application supports multiple languages:
- English (en-US) - Default
- German (de-DE)
- French (fr-FR)
- Arabic (ar-EG)

To change language, include the `Accept-Language` header in your requests:
```
Accept-Language: ar-EG
```

## 🔔 Real-time Notifications

The system uses SignalR for real-time notifications. Connect to the SignalR hub at:
```
/notificationHub
```

### Example SignalR Connection (JavaScript)
```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:5001/notificationHub")
    .build();

connection.on("ReceiveNotification", (notification) => {
    console.log("New notification:", notification);
});

await connection.start();
```

## 📊 Background Jobs (Hangfire)

Access the Hangfire dashboard to monitor background jobs:
```
https://localhost:5001/dashboard
```

## 🧪 Testing

### Running Tests
```bash
dotnet test
```

## 🐛 Error Handling

The application implements global error handling through custom middleware. All errors are:
- Logged to the database
- Returned with appropriate HTTP status codes
- Formatted in a consistent JSON structure

## 📁 Project Structure

```
ProjectManagement/
│
├── ProjectManagement.Presentation/    # Web API Layer
│   ├── Controllers/                   # API Controllers
│   ├── wwwroot/                       # Static files
│   ├── Program.cs                     # Application entry point
│   └── appsettings.json              # Configuration
│
├── ProjectManagement.Core/            # Application Core
│   ├── Features/                      # CQRS Handlers
│   │   ├── Authentication/
│   │   ├── Projects/
│   │   ├── Tasks/
│   │   ├── Users/
│   │   └── Roles/
│   ├── Mapping/                       # AutoMapper Profiles
│   └── Shared/                        # Shared Resources
│
├── ProjectManagement.Service/         # Business Logic
│   ├── Service/                       # Service Implementations
│   └── ServiceInterfaces/             # Service Contracts
│
├── ProjectManagement.Infrastructure/  # Infrastructure Layer
│   ├── Context/                       # Database Context
│   ├── Migrations/                    # EF Migrations
│   ├── Repository/                    # Repository Implementations
│   ├── Hubs/                          # SignalR Hubs
│   └── Seeder/                        # Data Seeders
│
└── ProjectManagement.Domain/          # Domain Layer
    ├── Entities/                      # Domain Entities
    ├── Enums/                         # Enumerations
    ├── Meta/                          # Metadata
    └── Shared/                        # Shared Domain Logic
```

## 🚀 Deployment

### Development
```bash
dotnet run --environment Development
```

### Production
```bash
dotnet publish -c Release
# Deploy the contents of bin/Release/net9.0/publish/
```

### Docker (Optional)
Create a `Dockerfile` in the root directory:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectManagement.Presentation.dll"]
```

Build and run:
```bash
docker build -t project-management-system .
docker run -p 8080:80 project-management-system
```

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Coding Standards
- Follow C# coding conventions
- Write meaningful commit messages
- Add XML documentation for public APIs
- Include unit tests for new features
- Update documentation as needed

## 📝 License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

## 👤 Author

**Mohamed-334**
- GitHub: [@Mohamed-334](https://github.com/Mohamed-334)

## 🙏 Acknowledgments

- Clean Architecture by Robert C. Martin
- ASP.NET Core team for the amazing framework
- Community contributors and supporters

## 📧 Support

For support, email mohamedaboelez334@gmail.com or open an issue in the GitHub repository.

## 🔄 Version History

- **v1.0.0** - Initial release
  - Core project and task management features
  - JWT authentication with OAuth support
  - Real-time notifications
  - Multi-language support
  - Background job processing

## 🗺️ Roadmap

- [ ] Mobile application
- [ ] Advanced reporting and analytics
- [ ] Project templates
- [ ] Time tracking
- [ ] Document management system
- [ ] Gantt chart integration
- [ ] Team collaboration features
- [ ] API rate limiting
- [ ] GraphQL support
- [ ] Docker containerization

---

Made with ❤️ by Mohamed-334
