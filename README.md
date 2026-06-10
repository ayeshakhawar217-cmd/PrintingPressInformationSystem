#Printing Press Management System (WinForms + SQL Server)

Overview
The Printing Press Management System is a desktop application built using C# Windows Forms (.NET Framework) and SQL Server with Entity Framework (Database First approach).
It is designed to manage complete printing press operations including customers, orders, services, payments, and order tracking.

Technologies Used
C# (WinForms)
.NET Framework
SQL Server
Entity Framework (EDMX)
ADO.NET (if used in some modules)
Visual Studio

Features
👤 Customer Management (Add, Update, Delete, View)
📦 Order Management System
🛠️ Service Management (Printing services with rates)
📄 Order Details with quantity and pricing
💳 Payment Tracking System
📊 Order Status Tracking (Pending / Completed / Processing)
🔗 Relational database with proper relationships
🗄️ Database Design (ERD)

The system is built on a fully relational database structure ensuring normalization and proper entity relationships.

📌 ER Diagram:
        <img width="364" height="360" alt="PrintingPressERD" src="https://github.com/user-attachments/assets/2be642b4-8c37-4794-bf32-9ab4b80e5427" />

🧱 Database Structure

Main Entities:

👤 Customer
CustomerID (PK)
CustomerName
Phone
Address
📦 Order
OrderID (PK)
CustomerID (FK)
OrderDate
DeliveryDate
Status
🛠️ Service
ServiceID (PK)
ServiceName
Rate
📄 OrderDetail
OrderDetailID (PK)
OrderID (FK)
ServiceID (FK)
Quantity
Rate
💳 Payment
PaymentID (PK)
OrderID (FK)
PaidAmount
PaymentDate
🔗 Relationships
One Customer → Many Orders
One Order → Many OrderDetails
One Service → Many OrderDetails
One Order → Many Payments
🛠️ Setup Instructions
1️⃣ Clone Repository
git clone https://github.com/your-username/printing-press-system.git
2️⃣ Database Setup
Open SQL Server Management Studio (SSMS)

Create a new database named:

PrintingPressdb

Run the provided file:

database.sql
3️⃣ Configure Connection String

Make sure your App.config contains:

data source=.\SQLEXPRESS;
initial catalog=PrintingPressdb;
integrated security=True;
4️⃣ Run Project
Open solution in Visual Studio
Set startup project
Press Start (F5)

Project Type: Academic / Portfolio Project
