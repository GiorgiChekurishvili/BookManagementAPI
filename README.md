# Book Management API

**Book Management API** is a web API for managing books, including functionalities like user registration, login, and CRUD operations for books. This project is built with **ASP.NET Core** and utilizes various modern technologies like **AutoMapper**, **JWT authentication**, and more.

## Features


- **Authentication**: JWT-based registration and login for secure user authentication.
- **Book Search**: Search books by title and retrieve details by book ID.
- **Bulk Operations**: Support for adding and deleting multiple books in bulk.

## Technologies Used

- **ASP.NET Core**: The main framework used for building the web API.
- **Entity Framework Core**: For interacting with the database.
- **JWT Authentication**: For secure user authentication and authorization.
- **SQL Server**: The database management system for storing user and book data.
- **AutoMapper**: For mapping data between entities and DTOs.
- **Swagger**: For API documentation and testing.

## Getting Started

### Prerequisites

- **.NET 8 SDK**
- **SQL Server**

### Installation

1. Clone the repository:

   ```bash
   gh repo clone GiorgiChekurishvili/BookManagementAPI

2. **Navigate to the project directory:**

    ```bash
   cd BookManagementAPI
    ```

3. **Restore dependencies:**

    ```bash
    dotnet restore
    ```

4. **Update your `appsettings.json` with the correct connection strings:**

    ```json
   "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BookManagementDb;Trusted_Connection=True;"
    }
    ```

    - **SQL Server**: Ensure you have a SQL Server instance running locally or remotely.

5. **Apply database migrations:**

    ```bash
    dotnet ef database update
    ```

6. **Run the application:**

    ```bash
    dotnet run
    ```

## API Endpoints

### Authentication

- **POST  /api/auth/login**: Logs in a user and returns a JWT token.
- **POST /api/auth/register**: Registers a new user.

### Book Management

- **GET /api/book/titles:**: Retrieves all book titles by sorting based on the Popularity Score.
- **GET /api/book/by-title?title={title}**: Retrieves a book by its title.
- **GET /api/book/{id}**: Retrieves a book by its ID.
- **POST /api/book/add:**: Adds a new book.
- **POST /api/book/add/bulk**:  Adds multiple books.
- **PUT /api/book/{id}**: Updates a book by its ID.
- **DELETE /api/book/{id}**: Deletes a book by its ID.
- **DELETE /api/book**:  Deletes multiple books by their IDs.

  ### Authorization

All book-related endpoints require user authentication via JWT. Use the token received from the **login** endpoint to authorize requests.

### Example Authentication

1. **Login** to get a token:
   - `POST /api/auth/login`
2. Include the token in the **Authorization** header:
   - `Authorization: Bearer {token}`
3. Perform CRUD operations on books using the authenticated endpoints.
