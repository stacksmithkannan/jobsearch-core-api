# JobFinder API – .NET 8 Clean Architecture Backend
<p align="center"> <img src="https://img.shields.io/badge/.NET-8.0-5C2D91?style=for-the-badge&logo=dotnet" /> <img src="https://img.shields.io/badge/CQRS-MediatR-1f72ff?style=for-the-badge&logo=csharp" /> <img src="https://img.shields.io/badge/Database-SQLite | MySQL-brightgreen?style=for-the-badge&logo=database" /> <img src="https://img.shields.io/badge/Auth-JWT-orange?style=for-the-badge&logo=jsonwebtokens" /> <img src="https://img.shields.io/badge/Architecture-Clean_Architecture-blueviolet?style=for-the-badge" /> <img src="https://img.shields.io/badge/Logging-Serilog-0084ff?style=for-the-badge&logo=serilog" /> </p>

JobFinder API is a scalable backend developed using .NET 8, built with Clean Architecture, MediatR (CQRS), EF Core, and secure JWT Authentication.
It supports SQLite for local development and MySQL in production (Azure / Railway).
---

## 🚀 Features

- 🧾 Job CRUD operations
- 👤 User authentication with **JWT**
- 🧱 **CQRS** pattern with **MediatR** (Commands & Queries)
- 📦 Clean, modular architecture
- 💾 **SQLite** for local development
- ☁️ **MySQL** for production (**Railway / Azure**)
- 🔐 Role Based Authentication (Admin/User)
- 🧪 Swagger UI with **JWT** support
- 📂 Resume file upload support
---

## 🛠 Tech Stack


| Layer        | Technology             |
|--------------|------------------------|
| Backend      | .NET 8 Web API         |
| Architecture | Clean + CQRS (MediatR) |
| Local DB     | SQLite                 |
| Cloud DB     | MySQL (Railway / Azure)|
| ORM          | Entity Framework Core  |
| Auth         | JWT                    |
| Mapping      | AutoMapper             |
| Docs         | Swagger (OpenAPI)      |
| Version Ctrl | Git + GitHub           |
---

## 🧩 Project Structue

JobFinder.API/
├── Application/        # CQRS: Commands, Queries, Handlers
├── Domain/             # Entities
├── Data/               # EF Core DB Context
├── StartUp/            # Seed admin user
├── Controllers/        # API Endpoints
├── Migrations/         # MySQL migrations (Cloud)
├── MigrationsSQLite/   # SQLite migrations (Local)
├── Program.cs          # App setup
├── appsettings.json    # Production config (NO secrets)
└── wwwroot/resumes     # Uploaded files

## ⚙️ Environment Setup

## The API switches automatically:

  | Environment | 	Database | 	Config File                      |
  |-------------|---------- |-----------------------------------|
  | Development	| SQLite	   | appsettings.Development.json      |
  | Production  | MySQL     |	 appsettings.json + Azure Settings|

## 🔧 Setup Instructions

### 1. Clone the Repository

 git clone https://github.com/your-username/jobfinder-api.git
 
 cd jobfinder-api

### 2. Create Local Config (Important!)

Create this file manually:

appsettings.Development.json

{
  "ConnectionStrings": {
    "SqliteConnection": "Data Source=jobfinder.db"
  },
  "Jwt": {
    "Key": "THIS_IS_A_FAKE_32_CHAR_KEY_1234567890!!",
    "Issuer": "JobFinder",
    "Audience": "JobFinderUsers",
    "DurationInMinutes": 60
  },
  "AdminUser": {
    "UserName": "admin",
    "Email": "admin@jobfinder.com",
    "Password": "Admin@123"
  }
}
### **3. . Apply SQLite Migrations (Local Only)**

dotnet ef database update

This creates:  jobfinder.db

### **4. Run the API**

dotnet run

Swagger will be available at:

 https://localhost:<port>/swagger

### 📖 CQRS Pattern in Action

➕ Command Example

CreateJobCommand.cs

CreateJobHandler.cs

🔍 Query Example

GetJobByIdQuery.cs

GetJobByIdHandler.cs

This pattern separates read and write operations for cleaner and more testable code.

### 🔐 Authentication Flow 

Register/Login to get a JWT token

Use the token in requests:

Authorization: Bearer <your_token>

### 🧪 API Testing

Open Swagger UI

Try endpoints like POST /api/jobs, GET /api/jobs, etc.

Add JWT token in Swagger Authorize section for protected routes

### ☁️ Deployment (MySQL + Azure)

Production uses MySQL connection string:

"ConnectionStrings": {
  "DefaultConnection": "YOUR MYSQL URL"
}

Inside Azure App Settings, you will add:

ConnectionStrings:DefaultConnection

Jwt:Key

Jwt:Issuer

Jwt:Audience

AdminUser:Password

### 🔮 Future Enhancements

⏳ Email notifications

⏳ Recruiter dashboard

⏳ Job recommendations

⏳ Background worker service

⏳ Cloud file storage (Azure Blob

### 🤝 Contributing

Fork the project

Create your feature branch: git checkout -b feature/xyz

Commit your changes

Push to the branch: git push origin feature/xyz

Open a Pull Request

### 📄 License

This project is licensed under the [MIT License](https://github.com/stacksmithkannan/jobsearch-core-api/blob/main/LICENSE.txt).

### 👨‍💻 Author

**Kannan G**  
[GitHub](https://github.com/stacksmithkannan) | [LinkedIn](https://www.linkedin.com/in/kan98/)




