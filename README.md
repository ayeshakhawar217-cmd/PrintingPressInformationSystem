# Printing Press Management System (WinForms + SQL Server)

## Overview
The Printing Press Management System is a desktop application built using C# Windows Forms (.NET Framework) with SQL Server and Entity Framework (Database First approach).


<img width="960" height="512" alt="PP1" src="https://github.com/user-attachments/assets/344182b3-ae91-4401-8261-b4fc38499955" />


It manages complete printing press operations including customers, orders, services, payments, and order tracking.

## Technologies Used
- C# (WinForms)
- .NET Framework
- SQL Server
- Entity Framework (EDMX)
- ADO.NET (some modules)
- Visual Studio

## Features
- Customer Management (Add, Update, Delete, View)
- Order Management System
- Service Management (Printing services with rates)
- Order Details with quantity and pricing
- Payment Tracking System
- Order Status Tracking (Pending / Processing / Completed)
- Relational database with proper relationships

## ER Diagram
<img width="364" height="360" alt="PrintingPressERD" src="https://github.com/user-attachments/assets/2be642b4-8c37-4794-bf32-9ab4b80e5427" />

## Database Structure

Customer  
- CustomerID (PK)  
- CustomerName  
- Phone  
- Address  

Order  
- OrderID (PK)  
- CustomerID (FK)  
- OrderDate  
- DeliveryDate  
- Status  

Service  
- ServiceID (PK)  
- ServiceName  
- Rate  

OrderDetail  
- OrderDetailID (PK)  
- OrderID (FK)  
- ServiceID (FK)  
- Quantity  
- Rate  

Payment  
- PaymentID (PK)  
- OrderID (FK)  
- PaidAmount  
- PaymentDate  

## Relationships
- One Customer → Many Orders  
- One Order → Many OrderDetails  
- One Service → Many OrderDetails  
- One Order → Many Payments  

## Setup Instructions

1. Clone the repository using:
git clone https://github.com/ayeshakhawar217-cmd/printing-press-system.git

2. Open SQL Server Management Studio (SSMS) and create a new database named:
PrintingPressdb

3. Run the provided database script file:
database.sql

4. Open the project in Visual Studio.

5. Ensure the connection string in App.config is set correctly:
data source=.\SQLEXPRESS;
initial catalog=PrintingPressdb;
integrated security=True;

6. Build the solution to restore all dependencies.

7. Set the project as Startup Project.

8. Run the application using F5.

