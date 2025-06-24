Agri-Energy Connect

Project Overview

Agri-Energy Connect is a role-based ASP.NET Core MVC web platform designed to link South African farmers with green energy providers. The system enables registered farmers to manage agricultural product data and employees to manage farmer profiles and monitor product listings. Built using SQL Server, Entity Framework Core, and Razor Pages, this project serves as a prototype for digitizing sustainable agriculture and energy collaboration.

 Prerequisites

Ensure you have the following tools installed before starting the project:

Tool

Description

Download Link

Visual Studio 2022+

IDE for developing ASP.NET apps

Download

.NET 6 or 7 SDK

Runtime and SDK for ASP.NET Core

Download

SQL Server

Database engine

Download

SSMS

SQL Server Management Studio

Download

 Tip: In Visual Studio, make sure to install the ASP.NET and Web Development workload.

 Getting Started

1. Create the Project

Open Visual Studio

Go to File → New → Project

Choose ASP.NET Core Web App (Model-View-Controller)

Name your project: Agri-EnergyConnect

Use:

.NET 6 or .NET 7

Authentication: None

Check Enable HTTPS

2. Set Up Folder Structure

Create the following folders:

/Models
/Controllers
/Views/Account
/Views/Farmer
/Views/Employee
/Data
/Services (optional)

Add models:

ApplicationUser.cs

Farmer.cs

Product.cs

3. Configure EF Core & DbContext

Create AppDbContext.cs:

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Farmer> Farmers { get; set; }
    public DbSet<Product> Products { get; set; }
}

Update Program.cs:

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();

In appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=AgriEnergyConnectDb;Trusted_Connection=True;"
}

4. Install Required Packages

Using Package Manager Console:

Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.AspNetCore.Session

5. Configure Middleware

Add this to Program.cs:

app.UseSession();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

6. Apply Migrations

Add-Migration InitialCreate
Update-Database

 Custom Authentication (Prototype)

Set up login/logout using session-based authentication:

AccountController.cs: Login, Logout actions

LoginViewModel.cs, RegisterViewModel.cs: ViewModels for forms

Session logic to store roles and access control (Farmer/Employee)

 Seeding Data (Optional)

Create a DataSeeder.cs to seed test users and products. Call it in Program.cs:

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DataSeeder.Seed(db);
}

▶ Running the Application

Open the solution in Visual Studio

Run dotnet restore or NuGet → Restore Packages

Build the project: Ctrl + Shift + B

Run the application: F5 or press Play

URL: http://localhost:<port>/

 Application Navigation

Feature

URL Path

Home Page

/Home/Index

Login Page

/Account/Login

Farmer Dashboard

/Farmer/Index

Add Product

/Farmer/Create

View Own Products

/Farmer/Index

Employee Dashboard

/Employee/Index

Add Farmer

/Employee/AddFarmer

View All Farmers

/Employee/ListFarmers

View Products

/Employee/ProductList

 Roles & Access Control

Page/Action

Access

/Farmer/Create

Farmer only

/Farmer/Index

Farmer only

/Employee/AddFarmer

Employee only

/Employee/ListFarmers

Employee only

/Employee/ProductList

Employee only

/Account/Login

Public

Access to protected pages requires an authenticated session.

Users are pre-seeded with simple credentials for demo purposes.

 Architecture & Patterns

Component

Pattern

UI Logic

MVC (Model-View-Controller)

Application Layers

Layered Architecture (Presentation → Business Logic → Data)

Data Transfer

DTOs (ViewModels)

Authentication

Custom Session-Based Auth

Backend Tasks

Dependency Injection + EF Core ORM

 Test Credentials (Prototype)

Role

Username

Password

Employee

employee1

password123

Farmer

farmer1

password123

You can add your own users directly in SQL Server or via seeding.

 User Workflows

 Farmer

Log in via /Account/Login

Access /Farmer/Index

Add product via /Farmer/Create

View added products

 Employee

Log in via /Account/Login

Access dashboard: /Employee/Index

Add new farmers via /Employee/AddFarmer

View all farmers: /Employee/ListFarmers

Filter products: /Employee/ProductList

 Notes & Recommendations

Project uses custom session-based authentication (no Identity)

For production, integrate ASP.NET Identity for secure password storage and token-based auth

This setup is ideal for learning about manual authentication and MVC pattern structure





Developed by Lwazi Zuma (ST10179039) as part of the Agri-Energy Connect prototype for sustainable agriculture and green energy alignment.

