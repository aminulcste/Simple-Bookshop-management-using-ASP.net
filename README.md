# Bookshop Management System

A full-stack ASP.NET Core MVC application for managing a bookshop. The system supports admin and user roles with features such as book and category management, user authentication, cart, and orders.

---

## Features

- **Admin Panel**
  - Manage Categories (Create, Edit, Delete)
  - Manage Books (Create, Edit, Delete)
  - View Orders (AdminOrders)
  - Secure with authentication

- **User Panel**
  - View available books with details
  - User registration and login
  - Add books to cart
  - Place and view orders
  - Logout functionality

---

## Important URLs

### Admin

| Page            | URL                    | Controller | Action      |
|-----------------|------------------------|------------|-------------|
| Admin Dashboard | `/Admin/Index`          | Admin      | Index       |
| Categories      | `/Category/Index`       | Category   | Index       |
| Create Category | `/Category/Create`      | Category   | Create      |
| Edit Category   | `/Category/Edit/{id}`   | Category   | Edit        |
| Delete Category | `/Category/Delete/{id}` | Category   | Delete      |
| Books           | `/Book/Index`           | Book       | Index       |
| Create Book     | `/Book/Create`          | Book       | Create      |
| Edit Book       | `/Book/Edit/{id}`       | Book       | Edit        |
| Delete Book     | `/Book/Delete/{id}`     | Book       | Delete      |
| Admin Orders    | `/Order/AdminOrders`    | Order      | AdminOrders |

---

### User

| Page                | URL                    | Controller   | Action        |
|---------------------|------------------------|--------------|---------------|
| User Home (Books)   | `/UserBooks/Index`      | UserBooks    | Index         |
| Book Details        | `/UserBooks/Details/{id}`| UserBooks    | Details       |
| Cart Page           | `/Cart/Index`           | Cart         | Index         |
| Add to Cart (POST)  | `/Cart/AddToCart`       | Cart         | AddToCart     |
| User Orders         | `/Order/MyOrders`       | Order        | MyOrders      |
| Login Page          | `/Account/Login`        | Account      | Login         |
| Register Page       | `/Account/Register`     | Account      | Register      |
| Logout (POST)       | `/Account/Logout`       | Account      | Logout        |

---

## Setup Instructions

1. Clone the repository.
2. Update your `appsettings.json` with your SQL Server connection string.
3. Run database migrations to create the database schema.
4. Build and run the application.
5. Use the admin panel for managing books and categories.
6. Use the user panel for browsing and purchasing books.

---

## Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Bootstrap 5
- Identity for Authentication & Authorization

---

## Notes

- The first registered user will be assigned the Admin role automatically.
- User registration and login pages use a minimal layout without the navigation bar.
- Admin and user have separate layouts and navigation bars.
- Authorization is configured to allow free access by default but can be customized.
- Cart and Order functionality require authentication.

---

## Author

Aminul Islam  
Email: aminulashik19@gmail.com
GitHub: https://github.com/aminulcste

---

## License

MIT License  
