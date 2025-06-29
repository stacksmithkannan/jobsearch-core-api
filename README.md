# 🧠 JobFinder API – .NET Core Backend

A scalable job portal backend built with **.NET Core**, following **Clean Architecture** and **CQRS pattern with MediatR**, integrated with **Elasticsearch** for high-performance job search functionality.

---

## 🚀 Features

- 🧾 Job CRUD operations
- 🔍 Full-text search with **Elasticsearch**
- 👤 User authentication with **JWT**
- 🧱 Clean folder structure using **CQRS + MediatR**
- 📦 EF Core with SQL Server (or MongoDB optional)
- 🔐 Protected APIs
- 🧪 Swagger UI for testing endpoints

---

## 🛠 Tech Stack

| Layer        | Technology            |
|--------------|------------------------|
| Backend      | .NET 8 Web API         |
| Search Engine| Elasticsearch          |
| Auth         | JWT (JSON Web Token)   |
| Database     | SQL Server (default)   |
| ORM          | Entity Framework Core  |
| Messaging    | MediatR                |
| Docs         | Swagger (OpenAPI)      |
| Version Ctrl | Git + GitHub           |

---

## 🧩 Project Structue

JobFinder.API/
├── Application/ # CQRS Handlers, Commands, Queries
├── Domain/ # Entity models
├── Data/ # ApplicationDbContext (EF Core)
├── Controllers/ # API endpoints
├── Infrastructure/ # Auth, Elasticsearch setup
├── Program.cs # Startup and service configuration
└── appsettings.json # Configuration

## 🔧 Setup Instructions

### 1. Clone the Repository

 git clone https://github.com/your-username/jobfinder-api.git
 cd jobfinder-api

### **2. Configure Database Connection**

Edit appsettings.json:

"ConnectionStrings": {
 "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=JobFinderDB;Trusted_Connection=True;"
}
### **3. Apply Migrations**

dotnet ef migrations add InitialCreate
dotnet ef database update

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

### 🔮 Future Enhancements

✅ Resume upload (file storage)

✅ Admin dashboard

⏳ Email service integration

⏳ Notifications module

⏳ Role-based access (Admin/Recruiter/JobSeeker)

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




