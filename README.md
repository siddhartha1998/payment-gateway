# PaymentGateway: ASP.NET + Angular Application with Demo Integration

The **PaymentGateway** repository contains an ASP.NET application with an integrated Angular front-end and a demo application for testing third-party APIs. The solution utilizes **.NET 8, Angular 19, Node 22, and SQL Server** as the database.

## Technologies Used

- **Backend**: ASP.NET 8  
- **Frontend**: Angular 19  
- **Node.js**: Version 22  
- **Database**: SQL Server  
- **Third-Party API Integration**: Custom demo application to test API interactions  

## Project Structure

- **ASP.NET Application**: The backend for managing API interactions, user authentication, and database operations.  
- **Angular Application**: A modern single-page web application providing a user-friendly interface.  
- **Demo Application**: A separate test application inside the solution for interacting with third-party APIs. This helps simulate real-world API requests and responses to ensure proper integration.  

---

## Setup Instructions

### Prerequisites

Before setting up the project, ensure that you have installed the following:

1. **.NET 8 SDK**: Download and install from [here](https://dotnet.microsoft.com/download/dotnet/8.0).  
2. **Node.js (v22.x)**: Download and install from [Node.js](https://nodejs.org).  
3. **SQL Server**: Ensure SQL Server is installed and running locally or remotely.  
4. **Angular CLI**: Install globally by running the following command:  

   ```bash
   npm install -g @angular/cli
   
## Installation Steps

### Step 1: Clone the Repository  

Clone the repository to your local machine: 
``` bash
git clone https://github.com/siddhartha1998/payment-gateway.git
cd payment-gateway
```

### Step 2: Configure SQL Server Connection

Edit the appsettings.json file in the PaymentGateway.Server project and update the database connection string:
``` bash
"ConnectionStrings": {
  "DefaultConnection": "Server=your_server;Database=your_database;Trusted_Connection=True;TrustServerCertificate=True;Connection Timeout=30;Column Encryption Setting=Enabled;"
}
```


### Step 3: Restore the database

You can either:

- Restore the provided database backup, or
- Run the migrations using the .NET command line:

  ``` bash
  dotnet ef database update
  ```


### Step 4: Add Always Encrypted Certificate

  To enable Always Encrypted, add the certificate in the Windows Certificate Manager as shown below:
  
  ![image](https://github.com/user-attachments/assets/d4edea19-ac0c-4ff0-a4f5-364d3106f890)


### Step 5: Install and Configure RabbitMQ

  1. Download and install RabbitMQ on your local machine.
  2. Ensure RabbitMQ is running on the default port (5672).
  3. If you need a custom configuration, update the appsettings.json file as follows:

``` bash
"RabbitMQ": {
  "HostName": "localhost",
  "PortNo": 5672,
  "Username": "guest",
  "Password": "guest"
}

```


### Step 6: Set Multiple Startup Projects

Make both the DemoServer and Gateway Server startup projects to ensure they run concurrently.


### Step 7: Running the Application

Once the application is running, open Swagger UI at:
https://localhost:44390/swagger/index.html


### Step 8: Redirecting to Client Application

Removing /swagger/index.html from the URL will automatically redirect to the client application:
https://localhost:40075/login


### Step 9: Redirecting to Client Application

Check the default credentials in the seeding data for logged in into the application.
