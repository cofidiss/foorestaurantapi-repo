

https://github.com/user-attachments/assets/152d83f1-8430-42ba-bbd0-0b292a156981

<img width="1390" height="722" alt="rest-Tablolar drawio" src="https://github.com/user-attachments/assets/494bc02f-8e80-4fdb-8498-c60ad7414479" />



# Project Workflow

## **1. User Authentication**

Users can register and log in through the application.

Passwords are securely hashed before being stored in **PostgreSQL**.

After successful authentication, the server issues an **HTTP-Only Authentication Cookie**.

For every authenticated request, **CustomCookieAuthenticationEvent** validates the user's current information. If the user's password has changed or the account has been disabled, authentication automatically becomes invalid.

Protected endpoints enforce **role-based authorization** (e.g., **USER** and **ADMIN**) using dynamically generated authorization policies.

---

## **2. Food Browsing**

Users can browse food categories and view foods available in each category.

Only enabled foods are returned, ordered according to their configured display order.

Food images are stored in **PostgreSQL** as **byte arrays (bytea)** and returned by the API as **Base64 strings**.

---

## **3. Order Creation**

Authenticated users can place orders by selecting one or more food items.

Before an order is created, the application validates:

- Food availability
- Order quantities
- Request data
- Business rules

Invalid requests return appropriate **HTTP 400 Bad Request** responses.

---

## **4. Order Processing**

Order creation is executed inside a **database transaction**.

The application stores:

- Order information
- Ordered food items

If any operation fails, the transaction is rolled back, ensuring database consistency.

---

## **5. Order Management**

Users can:

- View their own orders
- View order details

Administrators can additionally:

- View all orders
- Update order status
- Manage customer orders

---

## **6. Food & Category Management**

Administrators can manage both foods and food categories.

Available operations include:

- Create
- Update
- Delete
- Enable / Disable foods
- Upload food images

Validation ensures:

- Correct category ordering
- Valid day/category assignments
- Duplicate ordering is prevented

---

## **7. Validation & Error Handling**

Request validation is implemented using **Validation Attributes** and **Action Filters**.

Unexpected exceptions are handled by a **Global Exception Middleware**, where errors are logged and standardized error responses are returned.

---

## **8. Database**

The application uses **PostgreSQL** as its primary database.

All database operations are implemented using:

- **Entity Framework Core**
- **LINQ**

Common audit fields are automatically maintained for every entity:

- **creation_date**
- **created_by**
- **last_update_date**
- **last_updated_by**

The values of **day_id**, **role_id**, and **status_id** are represented using **enum** types within the application.

---

## **9. Application Architecture**

The project follows the **Onion Architecture** and uses **Dependency Injection** to ensure a clean separation of responsibilities.

**AutoMapper** is used for object mapping between layers.

Every service method returns a generic:

`Response<TErrorData, TSuccessData>`

to provide a consistent API response structure.

All endpoints are implemented using **async/await** for end-to-end asynchronous processing.

---

## **10. Testing**

Business logic is verified using **xUnit** unit tests.
